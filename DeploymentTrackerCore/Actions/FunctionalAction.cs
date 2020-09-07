using System;

namespace DeploymentTrackerCore.Actions {
    public abstract class FunctionalAction<Input, Output> : IAction<Input, Output> {
        public abstract Output Perform(Input input);

        public Func<Input, Output> Function => (input) => Perform(input);
    }
}