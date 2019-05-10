using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using deployment_tracker.Models.API;

namespace deployment_tracker.Hubs {
    public class DeploymentHub : Hub<IDeploymentClient> {
        public async Task ReportDeploymentChange(ApiDeployment deployment) {
            await Clients.Others.DeploymentChange(deployment);
        }
    }
}