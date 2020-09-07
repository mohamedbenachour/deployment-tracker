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

using DeploymentTrackerCore.Actions;
using DeploymentTrackerCore.Models.Entities;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeploymentTrackerCore.Actions.Environment {
    class DeleteEnvironment {
        private DeploymentAppContext Context { get; }

        private int EnvironmentID { get; }

        public bool Succeeded { get; private set; }

        public String Error { get; private set; }

        public DeleteEnvironment(DeploymentAppContext context, int environmentID) {
            Context = context;
            EnvironmentID = environmentID;
        }

        public async Task Perform() {
            if (IsValidToDelete()) {
                var environmentReference = Context.Environments.Single(env => env.Id == EnvironmentID);

                Context.Environments.Remove(environmentReference);

                await Context.SaveChangesAsync();

                Succeeded = true;
            } else {
                Succeeded = false;
            }
        }

        private bool IsValidToDelete() {
            if (!Context.Environments.Any(env => env.Id == EnvironmentID)) {
                Error = "No matching environment";
                return false;
            }

            if (!Context.Environments.Any(env => env.Id == EnvironmentID && (env.Deployments == null || env.Deployments.Count == 0))) {
                Error = "An environment with deployments cannot be deleted";
                return false;
            }

            return true;
        }
    }
}