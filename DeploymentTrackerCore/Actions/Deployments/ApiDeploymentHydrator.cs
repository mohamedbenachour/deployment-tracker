/*
 * This file is part of Deployment Tracker.
 * 
 * Deployment Tracker is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * Deployment Tracker is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with Deployment Tracker. If not, see <https://www.gnu.org/licenses/>.
 */

using System.Threading.Tasks;

using DeploymentTrackerCore.Models;
using DeploymentTrackerCore.Models.API;
using DeploymentTrackerCore.Services.DeploymentManagement;
using DeploymentTrackerCore.Services.Jira;

namespace DeploymentTrackerCore.Actions.Deployments {
    public class ApiDeploymentHydrator {
        private IDeploymentManager DeploymentManager { get; }
        private IJiraService JiraService { get; }

        public ApiDeploymentHydrator(IDeploymentManager deploymentManager, IJiraService jiraService) {
            DeploymentManager = deploymentManager;
            JiraService = jiraService;
        }

        public async Task Hydrate(ApiDeployment deployment) {
            var jiraInformation = new JiraInformation {
                Url = JiraService.GetJiraUrl(deployment),
                Issue = JiraService.GetJiraIssue(deployment)
            };

            deployment.ManagementUrls = new ApiDeploymentManagementUrls {
                DeploymentTriggerUrl = await DeploymentManager.GetDeploymentTriggerUrl(deployment)
            };

            if (SiteIsRunning(deployment)) {
                deployment.TeardownUrl = await DeploymentManager.GetTeardownUrl(deployment);

                var jiraUrl = JiraService.GetJiraUrl(deployment);

                if (jiraInformation.Url != null) {
                    jiraInformation.Status = (await JiraService.GetJiraStatus(deployment)).ToString();
                }
            }

            deployment.Jira = jiraInformation;
        }

        private bool SiteIsRunning(ApiDeployment deployment) => deployment.Status == DeploymentStatus.RUNNING.ToString();
    }
}