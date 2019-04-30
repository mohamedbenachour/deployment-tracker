using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using deployment_tracker.Models;
using deployment_tracker.Models.API;

using Microsoft.EntityFrameworkCore;

namespace deployment_tracker.Actions.Environment
{
    class NewEnvironment {
        private DeploymentAppContext Context { get; }

        private ApiNewEnvironment Environment { get; }

        public bool Succeeded { get; private set; }

        public String Error { get; private set; }

        public DeploymentEnvironment CreatedEnvironment { get; private set; }

        public NewEnvironment(DeploymentAppContext context, ApiNewEnvironment environment) {
            Context = context;
            Environment = environment;         
        }

        public async Task Create() {
            if (IsValidNewEnvironment()) {
                var newEnvironment = new DeploymentEnvironment {
                    Name = Environment.Name,
                    HostName = Environment.HostName
                };

                Context.Environments.Add(newEnvironment);
                
                await Context.SaveChangesAsync();

                CreatedEnvironment = newEnvironment;

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