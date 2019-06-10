/*
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

using deployment_tracker.Models;
using deployment_tracker.Services;
using deployment_tracker.Services.Configuration;
using deployment_tracker.Services.Identity;
using deployment_tracker.Services.Identity.Mock;
using deployment_tracker.Services.DeploymentManagement;
using deployment_tracker.Services.Token;
using deployment_tracker.Services.Jira;
using deployment_tracker.Hubs;

using Microsoft.EntityFrameworkCore;

namespace deployment_tracker
{
    public class Startup
    {
        public Startup(IConfiguration configuration, ILogger<Startup> logger)
        {
            Configuration = configuration;
            Logger = logger;
        }

        public IConfiguration Configuration { get; }
        private ILogger Logger { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });


           // Add identity types
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddDefaultTokenProviders();

            // Identity Services
            if (Configuration.GetSection("IdentitySource")["Type"] == "MockStore") {
                Logger.LogInformation("Using the MockStore identity source");

                services.AddSingleton<IUserStore<ApplicationUser>, MockUserStore>();
                services.AddTransient<IRoleStore<ApplicationRole>, MockRoleStore>();
            } else {
                Logger.LogError("No identity source has been configured");
            }

            services.AddSingleton<ConfigurationService>();
            services.AddSingleton<IJiraService, JiraService>();
            services.AddSingleton<IDeploymentManager, JenkinsDeploymentManager>();
            services.AddSingleton<ITokenVerifier, TokenVerifier>();

            services.AddScoped<IRequestState, RequestState>();

            services.AddDbContext<DeploymentAppContext>
                (options => options.UseSqlite(Configuration.GetSection("ConnectionStrings")["Application"]));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<DeploymentHub>("/deploymentHub");
            });
            app.UseMvc();
        }
    }
}
