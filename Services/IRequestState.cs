namespace deployment_tracker.Services {
    public interface IRequestState {
        User GetUser();
        void SetUser(User user);
    }

    public class User {
        public string Name { get; set; }
        public string Username { get; set; }
    }
}