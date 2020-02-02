namespace DeploymentTrackerCore.Actions {
    public class ApplicationActionResult<T> {
        public bool Succeeded { get; set; }

        public string Error { get; set; }

        public T Result { get; set; }

        public ApplicationActionResult<O> CopyError<O>()
            => new ApplicationActionResult<O> {
                Succeeded = false,
                Error = Error
            };
    }
}