﻿/*
* This file is part of Deployment Tracker.
* 
* Deployment Tracker is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*
* Deployment Tracker is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with Deployment Tracker. If not, see <https://www.gnu.org/licenses/>.
*/

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;

using DeploymentTrackerCore.Models;
using DeploymentTrackerCore.Services;
using DeploymentTrackerCore.Services.Configuration;
using DeploymentTrackerCore.Services.Identity;
using DeploymentTrackerCore.Services.Identity.Mock;
using DeploymentTrackerCore.Services.DeploymentManagement;
using DeploymentTrackerCore.Services.Token;
using DeploymentTrackerCore.Services.Jira;
using DeploymentTrackerCore.Hubs;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DeploymentTrackerCore
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

            // Identity Services
            if (Configuration.GetSection("IdentitySource")["Type"] == "MockStore") {
                services.AddIdentityCore<ApplicationUser>()
                    .AddUserStore<MockUserStore>()
                    .AddSignInManager<SignInManager<ApplicationUser>>();
            } else {
                throw new Exception("No auth services configured.");
            }

            services.AddSingleton<ConfigurationService>();
            services.AddSingleton<IJiraService, JiraService>();
            services.AddSingleton<IDeploymentManager, JenkinsDeploymentManager>();
            services.AddSingleton<ITokenVerifier, TokenVerifier>();

            services.AddScoped<IRequestState, RequestState>();

            services.AddDbContext<DeploymentAppContext>
                (options => options.UseSqlite(Configuration.GetSection("ConnectionStrings")["Application"]));

            services.AddRazorPages();

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseCookiePolicy(new CookiePolicyOptions {
                MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Strict
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapHub<DeploymentHub>("/deploymentHub");
                endpoints.MapHub<JiraHub>("/jiraHub");
            });
        }
    }
}