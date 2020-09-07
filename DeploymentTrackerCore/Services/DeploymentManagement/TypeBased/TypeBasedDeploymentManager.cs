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

using System;
using System.Threading.Tasks;

using DeploymentTrackerCore.Models;

using Microsoft.Extensions.Logging;

namespace DeploymentTrackerCore.Services.DeploymentManagement.TypeBased {
    public class TypeBasedDeploymentManager : IDeploymentManager {
        public TypeBasedDeploymentManager(DeploymentAppContext context, ILogger<TypeBasedDeploymentManager> logger) {
            DBContext = context;
            Logger = logger;
        }

        private DeploymentAppContext DBContext { get; }
        private ILogger<TypeBasedDeploymentManager> Logger { get; }

        public async Task<string> GetDeploymentTriggerUrl(IDeployedSite deployment) {
            var typeForDeployment = await GetTypeForDeployment(deployment);
            var deploymentTemplate = typeForDeployment.DeploymentTemplate;

            if (String.IsNullOrWhiteSpace(deploymentTemplate)) {
                Logger.LogInformation($"Type '{typeForDeployment.Id}' has no deployment template defined for it.");

                return null;
            }

            return new DeployedSiteStringTemplater().Template(deployment, deploymentTemplate);
        }

        public async Task<string> GetTeardownUrl(IDeployedSite deployment) {
            var typeForDeployment = await GetTypeForDeployment(deployment);
            var teardownTemplate = typeForDeployment.TeardownTemplate;

            if (String.IsNullOrWhiteSpace(teardownTemplate)) {
                Logger.LogError($"Type '{typeForDeployment.Id}' has no teardown template defined for it.");

                return null;
            }

            return new DeployedSiteStringTemplater().Template(deployment, teardownTemplate);
        }

        private async Task<Models.Type> GetTypeForDeployment(IDeployedSite deployment) => await DBContext.Types.FindAsync(deployment.Type.Id);
    }
}