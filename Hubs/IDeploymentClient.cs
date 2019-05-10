using System.Threading.Tasks;

using deployment_tracker.Models.API;

namespace deployment_tracker.Hubs {
    public interface IDeploymentClient {
        Task DeploymentChange(ApiDeployment deployment);
    }
}