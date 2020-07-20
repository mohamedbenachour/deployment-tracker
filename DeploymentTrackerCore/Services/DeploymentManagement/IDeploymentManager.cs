using System.Threading.Tasks;
using DeploymentTrackerCore.Models;

namespace DeploymentTrackerCore.Services.DeploymentManagement
{
    public interface IDeploymentManager
    {
        Task<string> GetTeardownUrl(IDeployedSite deployment);
    }
}