using System;

namespace DeploymentTrackerCore.Actions {
    public class ActionNotValidException : Exception {
        public ActionNotValidException(string message) : base(message) {}
    }
}