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
using System.Linq;
using System.Threading.Tasks;
using DeploymentTrackerCore.Models;

using Microsoft.EntityFrameworkCore;

namespace DeploymentTrackerCore.Actions.Deployments
{
    class DeploymentDestroyed : IActionPerformer<Deployment> {
        private DeploymentAppContext Context { get; }

        private string SiteName { get; }

        public bool Succeeded { get; private set; }

        public String Error { get; private set; }

        public Deployment Result { get; private set; }

        public DeploymentDestroyed(DeploymentAppContext context, string siteName) {
            Context = context;
            SiteName = siteName;
        }

        public async Task Perform() {
            if (await IsValidDeployment()) {
                var deployments = await Context.Deployments
                .Include(d => d.DeployedEnvironment)
                .Include(d => d.Type)
                .Where(d => d.SiteName == SiteName)
                .ToListAsync();

                deployments.ForEach(deployment => deployment.Status = DeploymentStatus.DESTROYED);
                
                await Context.SaveChangesAsync();

                Result = deployments.First();

                Succeeded = true;
            } else {
                Succeeded = false;
            }
        }

        private async Task<bool> IsValidDeployment() {
            var matchingDeployment = await Context.Deployments.AnyAsync(deployment => deployment.SiteName == SiteName);

            if (!matchingDeployment) {
                Error = "A deployment with the specified site name does not exist.";
                return false;
            }

            return true;
        }
    }
}