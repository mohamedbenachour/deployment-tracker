using deployment_tracker.Models.API;
using deployment_tracker.Services.DeploymentManagement;

using deployment_tracker.Models;

namespace deployment_tracker.Actions.Deployments {
    public class ApiDeploymentHydrator {
        private IDeploymentManager DeploymentManager { get; }

        public ApiDeploymentHydrator(IDeploymentManager deploymentManager) {
            DeploymentManager = deploymentManager;
        }

        public void Hydrate(ApiDeployment deployment) {
            if (deployment.Status == DeploymentStatus.RUNNING.ToString()) {
                deployment.TeardownUrl = DeploymentManager.GetTeardownUrl(deployment);
            }
        }
    }
}