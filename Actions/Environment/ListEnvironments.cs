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

        public IEnumerable<ApiEnvironment> Fetch() {
            return Context.Environments
                .Include(env => env.Deployments)
                .Select(ConvertToApi).ToList();
        }

        private ApiEnvironment ConvertToApi(DeploymentEnvironment environment) {
            var converted = ApiEnvironment.FromInternal(environment);

            foreach (var deployment in converted.Deployments) {
                DeploymentHydrator.Hydrate(deployment);
            }

            return converted;
        }
    }
}