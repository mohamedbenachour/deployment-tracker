using System.Collections.Generic;
using System.Linq;
using deployment_tracker.Models;
using deployment_tracker.Models.API;

using Microsoft.EntityFrameworkCore;

namespace deployment_tracker.Actions.Deployments
{
    class ListDeployments {
        private DeploymentAppContext Context { get; }
        private ApiDeploymentHydrator DeploymentHydrator { get; }

        public ListDeployments(DeploymentAppContext context, ApiDeploymentHydrator deploymentHydrator) {
            Context = context;
            DeploymentHydrator = deploymentHydrator;
        }

        public IEnumerable<ApiDeployment> Fetch() {
            return Context.Deployments
                .Select(ConvertToApi).ToList();
        }

        private ApiDeployment ConvertToApi(Deployment deployment) {
            var converted = ApiDeployment.FromInternal(deployment);

            DeploymentHydrator.Hydrate(converted);

            return converted;
        }
    }
}