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
using deployment_tracker.Models;
using deployment_tracker.Models.API;

using Microsoft.EntityFrameworkCore;
using deployment_tracker.Actions;

namespace deployment_tracker.Actions.Deployments
{
    class NewDeployment : IActionPerformer<Deployment> {
        private DeploymentAppContext Context { get; }

        private ApiNewDeployment Deployment { get; }

        public bool Succeeded { get; private set; }

        public String Error { get; private set; }

        public Deployment Result { get; private set; }

        public NewDeployment(DeploymentAppContext context, ApiNewDeployment deployment) {
            Context = context;
            Deployment = deployment;         
        }

        public async Task Perform() {
            if (IsValidNewDeployment()) {
                var matchingDeployment = Context.Deployments
                    .Include(d => d.DeployedEnvironment)
                    .SingleOrDefault(deployment => deployment.BranchName == Deployment.BranchName);
                Deployment newDeployment;
                
                if (matchingDeployment != null) {
                    newDeployment = matchingDeployment;
                    newDeployment.DeploymentCount = newDeployment.DeploymentCount + 1;
                } else {
                    newDeployment = new Deployment {
                        BranchName = Deployment.BranchName,
                        SiteName = Deployment.SiteName,
                        DeploymentCount = 1,
                    };
                    newDeployment.DeployedEnvironment = Context.Environments.Single(env => env.Id == Deployment.EnvironmentId);

                    Context.Deployments.Add(newDeployment);
                }

                newDeployment.PublicURL = Deployment.PublicURL;
                newDeployment.Status = DeploymentStatus.RUNNING;
                newDeployment.SiteLogin = Deployment.SiteLogin ?? new Login();
                
                await Context.SaveChangesAsync();

                Result = newDeployment;

                Succeeded = true;
            } else {
                Succeeded = false;
            }
        }

        private bool IsValidNewDeployment() {
            if (String.IsNullOrWhiteSpace(Deployment.BranchName)) {
                Error = "The deployment branch name must be specified.";
                return false;
            }

            if (String.IsNullOrWhiteSpace(Deployment.PublicURL)) {
                Error = "The deployment public URL must be specified.";
                return false;
            }

            if (!Deployment.PublicURL.StartsWith("https://")) {
                Error = "The deployment public URL is invalid.";
                return false;
            }

            if (Deployment.EnvironmentId == 0) {
                Error = "An environment must be specified for the deployment.";
                return false;
            }

            if (!Context.Environments.Any(env => env.Id == Deployment.EnvironmentId)) {
                Error = "The specified environment does not exist.";
                return false;
            }

            return true;
        }
    }
}