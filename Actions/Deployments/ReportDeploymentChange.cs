using deployment_tracker.Actions;
using deployment_tracker.Models;
using deployment_tracker.Models.API;

using System;
using System.Threading.Tasks;

using deployment_tracker.Hubs;
using Microsoft.AspNetCore.SignalR;


namespace deployment_tracker.Actions.Deployments {
    public class ReportDeploymentChange : IPostAction<ApiDeployment> {
        private IHubContext<DeploymentHub, IDeploymentClient> Hub { get; }

        public ReportDeploymentChange(IHubContext<DeploymentHub, IDeploymentClient> hub) {
            Hub = hub;
        }

        public async Task Perform(ApiDeployment changedDeployment) {
            await Hub.Clients.All.DeploymentChange(changedDeployment);
        }
    }
}