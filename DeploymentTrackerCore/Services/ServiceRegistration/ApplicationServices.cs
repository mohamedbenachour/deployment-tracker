using DeploymentTrackerCore.Services.DeploymentNotes;
using DeploymentTrackerCore.Services.UserEntityLinks;

using Microsoft.Extensions.DependencyInjection;

namespace DeploymentTrackerCore.Services.ServiceRegistration {
    public class ApplicationServices {
        public static void Configure(IServiceCollection services) {

            services.AddScoped<IDeploymentNotesService, DeploymentNotesService>();
            services.AddScoped<IUserEntityLinksService, UserEntityLinksService>();
        }
    }
}