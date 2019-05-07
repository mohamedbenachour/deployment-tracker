using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using deployment_tracker.Models;
using deployment_tracker.Models.API;
using deployment_tracker.Services.DeploymentManagement;

using Microsoft.EntityFrameworkCore;

namespace deployment_tracker.Actions.Environment
{
    class ListEnvironments {
        private DeploymentAppContext Context { get; }
        private IDeploymentManager DeploymentManager { get; }

        public ListEnvironments(DeploymentAppContext context, IDeploymentManager deploymentManager) {
            Context = context;
            DeploymentManager = deploymentManager;
        }

        public IEnumerable<ApiEnvironment> Fetch() {
            return Context.Environments
                .Include(env => env.Deployments)
                .Select(ConvertToApi).ToList();
        }

        private ApiEnvironment ConvertToApi(DeploymentEnvironment environment) {
            var converted = ApiEnvironment.FromInternal(environment);

            foreach (var deployment in converted.Deployments) {
                HydrateTeardownUrl(deployment);
            }

            return converted;
        }


        private void HydrateTeardownUrl(ApiDeployment deployment) {
            deployment.TeardownUrl = DeploymentManager.GetTeardownUrl(deployment);
        }
    }
}