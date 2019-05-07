using deployment_tracker.Models.API;

namespace deployment_tracker.Services.DeploymentManagement {
    public interface IDeploymentManager {
        string GetTeardownUrl(ApiDeployment deployment);
    }
}