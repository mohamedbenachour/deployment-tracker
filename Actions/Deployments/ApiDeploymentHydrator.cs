using deployment_tracker.Models.API;
using deployment_tracker.Services.DeploymentManagement;
using deployment_tracker.Services.Jira;

using deployment_tracker.Models;

namespace deployment_tracker.Actions.Deployments {
    public class ApiDeploymentHydrator {
        private IDeploymentManager DeploymentManager { get; }
        private IJiraService JiraService { get; }

        public ApiDeploymentHydrator(IDeploymentManager deploymentManager, IJiraService jiraService) {
            DeploymentManager = deploymentManager;
            JiraService = jiraService;
        }

        public void Hydrate(ApiDeployment deployment) {
            if (deployment.Status == DeploymentStatus.RUNNING.ToString()) {
                deployment.TeardownUrl = DeploymentManager.GetTeardownUrl(deployment);
            }
            deployment.JiraUrl = JiraService.GetJiraUrl(deployment);
        }
    }
}