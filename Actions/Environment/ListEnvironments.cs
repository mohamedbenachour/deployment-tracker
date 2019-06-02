using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using deployment_tracker.Models;
using deployment_tracker.Models.API;
using deployment_tracker.Services.DeploymentManagement;
using deployment_tracker.Actions.Deployments;

using Microsoft.EntityFrameworkCore;

namespace deployment_tracker.Actions.Environment
{
    class ListEnvironments {
        private DeploymentAppContext Context { get; }
        private ApiDeploymentHydrator DeploymentHydrator { get; }

        public ListEnvironments(DeploymentAppContext context, ApiDeploymentHydrator deploymentHydrator) {
            Context = context;
            DeploymentHydrator = deploymentHydrator;
        }

        public async Task<IEnumerable<ApiEnvironment>> Fetch() {
            return (await Context.Environments
                .Include(env => env.Deployments)
                .ToListAsync())
                .Select(ConvertToApi)
                .Select(t => t.Result);
        }

        private async Task<ApiEnvironment> ConvertToApi(DeploymentEnvironment environment) {
            var converted = ApiEnvironment.FromInternal(environment);

            foreach (var deployment in converted.Deployments) {
                await DeploymentHydrator.Hydrate(deployment);
            }

            return converted;
        }
    }
}