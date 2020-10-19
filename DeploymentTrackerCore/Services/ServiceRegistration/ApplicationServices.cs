using DeploymentTrackerCore.Services.DeploymentNotes;

using Microsoft.Extensions.DependencyInjection;

namespace DeploymentTrackerCore.Services.ServiceRegistration {
    public class ApplicationServices {
        public static void Configure(IServiceCollection services) {

            services.AddScoped<IDeploymentNotesService, DeploymentNotesService>();
        }
    }
}