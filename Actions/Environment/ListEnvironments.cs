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
    class ListEnvironments {
        private DeploymentAppContext Context { get; }

        public ListEnvironments(DeploymentAppContext context) {
            Context = context;                
        }

        public IEnumerable<ApiEnvironment> Fetch() {
            return Context.Environments
                .Include(env => env.Deployments)
                .Select(ApiEnvironment.FromInternal).ToList();
        }

    }
}