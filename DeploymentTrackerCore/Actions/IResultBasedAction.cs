using System.Threading.Tasks;

namespace DeploymentTrackerCore.Actions {
    public interface IResultBasedAction<Input, Output> : IAction<Input, Task<ActionOutcome<Output>>> { }
}