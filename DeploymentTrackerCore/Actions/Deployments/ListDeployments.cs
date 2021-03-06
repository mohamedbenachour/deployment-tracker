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

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DeploymentTrackerCore.Models.API;
using DeploymentTrackerCore.Models.Entities;

using Microsoft.EntityFrameworkCore;

namespace DeploymentTrackerCore.Actions.Deployments {
    class ListDeployments {
        private DeploymentAppContext Context { get; }
        private ApiDeploymentHydrator DeploymentHydrator { get; }

        public ListDeployments(DeploymentAppContext context, ApiDeploymentHydrator deploymentHydrator) {
            Context = context;
            DeploymentHydrator = deploymentHydrator;
        }

        public async Task<IEnumerable<ApiDeployment>> Fetch() {
            return (await Context.Deployments
                    .Include(d => d.DeployedEnvironment)
                    .Include(d => d.Type)
                    .Include(d => d.DeploymentNotes)
                    .ToListAsync())
                .Select(ConvertToApi).Select(t => t.Result);
        }

        private async Task<ApiDeployment> ConvertToApi(Deployment deployment) {
            var converted = ApiDeployment.FromInternal(deployment);

            await DeploymentHydrator.Hydrate(converted);

            return converted;
        }
    }
}