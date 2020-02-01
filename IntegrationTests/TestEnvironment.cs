using IntegrationTests.EnvironmentSetup;

public class TestEnvironment {
    public const string ExternalToken = "foobar";

    public struct User {
        public const string UserName = "jenkins-user";
        public const string Password = "test-password";
    }

    public struct URLs {
        public const string Environment = "/api/environment";
        public const string Deployment = "/api/deployment";

        public const string DeploymentExternal = "/api/deployment/external";
    }

    public static TestApplicationFactory<DeploymentTrackerCore.Startup> ClientFactory = new TestApplicationFactory<DeploymentTrackerCore.Startup>();
}