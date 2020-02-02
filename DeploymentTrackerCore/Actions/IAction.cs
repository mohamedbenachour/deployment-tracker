using System.Threading.Tasks;

namespace DeploymentTrackerCore.Actions {
    public interface IAction<I, O> {
        Task<ApplicationActionResult<O>> Perform(I newType);
    }
}