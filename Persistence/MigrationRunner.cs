
using System;
using deployment_tracker.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace deployment_tracker.Persistence {
    public class MigrationRunner {

        public static void Run(IWebHost host) {
            using (var scope = host.Services.CreateScope()) {
                var services = scope.ServiceProvider;

                try {
                    var environment = services.GetRequiredService<IHostingEnvironment>();

                    if (!environment.IsDevelopment()) {
                        var context = services.GetRequiredService<DeploymentAppContext>();

                        context.Database.Migrate();
                    }
                } catch (Exception ex) {
                    var logger = services.GetRequiredService<ILogger<MigrationRunner>>();

                    logger.LogError("Unable to run migrations for database", ex);
                }
            }
        }
    }
}