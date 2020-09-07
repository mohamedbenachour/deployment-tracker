namespace DeploymentTrackerCore.Actions {
    public interface IAction<Input, Output> {
        Output Perform(Input input);
    }
}