using DeploymentTrackerCore.Models;

namespace DeploymentTrackerCore.Services.DeploymentManagement {
    public interface IDeploymentManager {
        string GetTeardownUrl(IDeployedSite deployment);
    }
}