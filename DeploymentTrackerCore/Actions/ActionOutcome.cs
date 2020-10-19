namespace DeploymentTrackerCore.Actions {
    public class ActionOutcome<T> {
        public bool Succeeded { get; private set; }

        public T Result { get; private set; }

        public object Error { get; private set; }

        public static ActionOutcome<T> WithError(object error) => new ActionOutcome<T> {
            Succeeded = false,
            Error = error
        };

        public static ActionOutcome<T> WithResult(T result) => new ActionOutcome<T> {
            Succeeded = true,
            Result = result
        };
    }
}