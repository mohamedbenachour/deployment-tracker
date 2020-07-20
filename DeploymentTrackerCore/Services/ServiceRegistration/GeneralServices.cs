using DeploymentTrackerCore.Services.Configuration;
using DeploymentTrackerCore.Services.DeploymentManagement;
using DeploymentTrackerCore.Services.Jira;
using DeploymentTrackerCore.Services.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DeploymentTrackerCore.Services.ServiceRegistration
{
    public static class GeneralServices
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddSingleton<ConfigurationService>();
            services.AddSingleton<IJiraService, JiraService>();
            services.AddSingleton<IDeploymentManager, JenkinsDeploymentManager>();
            services.AddSingleton<ITokenVerifier, TokenVerifier>();

            services.AddScoped<IRequestState, RequestState>();
        }
    }
}