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
    class DeploymentDestroyed {
        private DeploymentAppContext Context { get; }

        private string SiteName { get; }

        public bool Succeeded { get; private set; }

        public String Error { get; private set; }

        public Deployment DestroyedDeployment { get; private set; }

        public DeploymentDestroyed(DeploymentAppContext context, string siteName) {
            Context = context;
            SiteName = siteName;
        }

        public async Task Destroy() {
            if (IsValidDeployment()) {
                var deployment = Context.Deployments
                .Include(d => d.DeployedEnvironment)
                .Single(d => d.SiteName == SiteName);

                deployment.Status = DeploymentStatus.DESTROYED;
                
                await Context.SaveChangesAsync();

                DestroyedDeployment = deployment;

                Succeeded = true;
            } else {
                Succeeded = false;
            }
        }

        private bool IsValidDeployment() {
            var matchingDeployment = Context.Deployments.Any(deployment => deployment.SiteName == SiteName);

            if (!matchingDeployment) {
                Error = "A deployment with the specified site name does not exist.";
                return false;
            }

            return true;
        }
    }
}