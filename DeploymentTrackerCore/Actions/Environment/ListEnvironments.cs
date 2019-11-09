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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DeploymentTrackerCore.Models;
using DeploymentTrackerCore.Models.API;
using DeploymentTrackerCore.Services.DeploymentManagement;
using DeploymentTrackerCore.Actions.Deployments;

using Microsoft.EntityFrameworkCore;

namespace DeploymentTrackerCore.Actions.Environment
{
    class ListEnvironments {
        private DeploymentAppContext Context { get; }
        private ApiDeploymentHydrator DeploymentHydrator { get; }

        public ListEnvironments(DeploymentAppContext context, ApiDeploymentHydrator deploymentHydrator) {
            Context = context;
            DeploymentHydrator = deploymentHydrator;
        }

        public async Task<IEnumerable<ApiEnvironment>> Fetch() {
            return (await Context.Environments
                .Include(env => env.Deployments)
                .ThenInclude(d => d.Type)
                .ToListAsync())
                .Select(ConvertToApi)
                .Select(t => t.Result);
        }

        private async Task<ApiEnvironment> ConvertToApi(DeploymentEnvironment environment) {
            var converted = ApiEnvironment.FromInternal(environment);

            foreach (var deployment in converted.Deployments) {
                await DeploymentHydrator.Hydrate(deployment);
            }

            return converted;
        }
    }
}