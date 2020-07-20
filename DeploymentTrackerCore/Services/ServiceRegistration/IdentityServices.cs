using System;
using DeploymentTrackerCore.Services.Identity;
using DeploymentTrackerCore.Services.Identity.LDAP;
using DeploymentTrackerCore.Services.Identity.Mock;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeploymentTrackerCore.Services.ServiceRegistration
{
    public static class IdentityServices
    {
        public static void Configure(IConfiguration configuration, IServiceCollection services)
        {
            var idSourceType = configuration.GetSection("IdentitySource")["Type"];

            if (idSourceType == "MockStore")
            {
                services.AddIdentityCore<ApplicationUser>()
                    .AddUserStore<MockUserStore>()
                    .AddSignInManager<SignInManager<ApplicationUser>>();

            }
            else if (idSourceType == "LDAP")
            {
                services.AddSingleton<LDAPConfiguration>();
                services.AddSingleton<LDAPClient>();

                services.AddIdentityCore<ApplicationUser>()
                    .AddUserStore<LDAPUserStore>()
                    .AddUserManager<LDAPUserManager>()
                    .AddSignInManager<SignInManager<ApplicationUser>>();
            }
            else
            {
                throw new Exception("No auth services configured.");
            }
        }
    }
}