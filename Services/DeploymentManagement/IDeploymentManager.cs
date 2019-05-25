using deployment_tracker.Models;

namespace deployment_tracker.Services.DeploymentManagement {
    public interface IDeploymentManager {
        string GetTeardownUrl(IDeployedSite deployment);
    }
}