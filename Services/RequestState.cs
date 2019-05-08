using System;

namespace deployment_tracker.Services {
    public class RequestState : IRequestState {
        private User CurrentUser { get; set; } = null;

        public void SetUser(User user) {
            CurrentUser = user;
        }

        public User GetUser() {
            if (CurrentUser == null) {
                throw new Exception("No user has been set");
            }

            return CurrentUser;
        }
    }
}