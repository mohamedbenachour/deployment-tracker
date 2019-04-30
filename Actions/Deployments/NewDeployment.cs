using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using deployment_tracker.Models;
using deployment_tracker.Models.API;

using Microsoft.EntityFrameworkCore;

namespace deployment_tracker.Actions.Deployments
{
    class NewDeployment {
        private DeploymentAppContext Context { get; }

        private ApiNewDeployment Deployment { get; }

        public bool Succeeded { get; private set; }

        public String Error { get; private set; }

        public Deployment CreatedDeployment { get; private set; }

        public NewDeployment(DeploymentAppContext context, ApiNewDeployment deployment) {
            Context = context;
            Deployment = deployment;         
        }

        public async Task Create() {
            if (IsValidNewDeployment()) {
                var newDeployment = new Deployment {
                    BranchName = Deployment.BranchName,
                    PublicURL = Deployment.PublicURL
                };

                newDeployment.Status = DeploymentStatus.RUNNING;
                newDeployment.DeployedEnvironment = Context.Environments.Single(env => env.Id == Deployment.EnvironmentId);

                Context.Deployments.Add(newDeployment);
                
                await Context.SaveChangesAsync();

                CreatedDeployment = newDeployment;

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

            var matchingDeployment = Context.Deployments.Any(deployment => deployment.BranchName == Deployment.BranchName || deployment.PublicURL == Deployment.PublicURL);

            if (matchingDeployment) {
                Error = "A deployment with the same details already exists.";
                return false;
            }

            return true;
        }
    }
}