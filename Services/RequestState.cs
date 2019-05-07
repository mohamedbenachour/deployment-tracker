namespace deployment_tracker.Services {
    public class RequestState : IRequestState {
        private User CurrentUser { get; set; } = new User {
            Name = "Unknown",
            Username = "unknown-user"
        };

        public void SetUser(User user) {
            CurrentUser = user;
        }

        public User GetUser() => CurrentUser;
    }
}