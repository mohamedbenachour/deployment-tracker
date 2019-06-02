using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        public async Task<IEnumerable<ApiDeployment>> Fetch() {
            return (await Context.Deployments.ToListAsync())
                .Select(ConvertToApi).Select(t => t.Result);
        }

        private async Task<ApiDeployment> ConvertToApi(Deployment deployment) {
            var converted = ApiDeployment.FromInternal(deployment);

            await DeploymentHydrator.Hydrate(converted);

            return converted;
        }
    }
}