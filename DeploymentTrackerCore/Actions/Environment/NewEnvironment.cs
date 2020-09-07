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

using DeploymentTrackerCore.Models.API;
using DeploymentTrackerCore.Models.Entities;

namespace DeploymentTrackerCore.Actions.Environment {
    public class NewEnvironment {
        private DeploymentAppContext Context { get; }

        private ApiNewEnvironment Environment { get; }

        public bool Succeeded { get; private set; }

        public String Error { get; private set; }

        public DeploymentEnvironment Result { get; private set; }

        public NewEnvironment(DeploymentAppContext context, ApiNewEnvironment environment) {
            Context = context;
            Environment = environment;
        }

        public async Task Perform() {
            if (IsValidNewEnvironment()) {
                var newEnvironment = new DeploymentEnvironment {
                    Name = Environment.Name,
                    HostName = Environment.HostName
                };

                Context.Environments.Add(newEnvironment);

                await Context.SaveChangesAsync();

                Result = newEnvironment;

                Succeeded = true;
            } else {
                Succeeded = false;
            }
        }

        private bool IsValidNewEnvironment() {
            if (String.IsNullOrWhiteSpace(Environment.Name)) {
                Error = "An environment name must be specified.";
                return false;
            }

            if (String.IsNullOrWhiteSpace(Environment.HostName)) {
                Error = "The environment host name must be specified.";
                return false;
            }

            var matchingEnvironment = Context.Environments.Any(env => env.Name == Environment.Name || env.HostName == Environment.HostName);

            if (matchingEnvironment) {
                Error = "An environment with the same name already exists.";
                return false;
            }

            return true;
        }
    }
}