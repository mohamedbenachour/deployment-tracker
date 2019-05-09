using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using deployment_tracker.Models;

using Microsoft.EntityFrameworkCore;

using deployment_tracker.Actions;

namespace deployment_tracker.Actions.Environment
{
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