namespace deployment_tracker.Services.Token {
    public interface ITokenVerifier {
        bool IsValid(string token);
    }
}