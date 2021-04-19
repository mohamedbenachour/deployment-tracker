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

using DeploymentTrackerCore.Services.Identity;
using DeploymentTrackerCore.Services.Identity.LDAP;
using DeploymentTrackerCore.Services.Identity.LDAP.DirectoryServices;
using DeploymentTrackerCore.Services.Identity.Mock;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeploymentTrackerCore.Services.ServiceRegistration {
    public static class IdentityServices {
        public static void Configure(IConfiguration configuration, IServiceCollection services) {
            var idSourceType = configuration.GetSection("IdentitySource")["Type"];

            if (idSourceType == "MockStore") {

                services.AddTransient<IUserCollection, MockUserStore>();

                services.AddIdentityCore<ApplicationUser>()
                    .AddUserStore<MockUserStore>()
                    .AddSignInManager<SignInManager<ApplicationUser>>();

            } else if (idSourceType == "LDAP") {
                services.AddSingleton<LDAPConfiguration>();
                services.AddSingleton<ILDAPClient, LDAPClient>();

                services.AddTransient<IUserCollection, LDAPUserStore>();

                services.AddIdentityCore<ApplicationUser>()
                    .AddUserStore<LDAPUserStore>()
                    .AddUserManager<LDAPUserManager>()
                    .AddSignInManager<SignInManager<ApplicationUser>>();
            } else {
                throw new Exception("No auth services configured.");
            }
        }
    }
}