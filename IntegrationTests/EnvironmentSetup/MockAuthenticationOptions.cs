using Microsoft.AspNetCore.Authentication;

namespace IntegrationTests.EnvironmentSetup {
    public class MockAuthenticationOptions : AuthenticationSchemeOptions {
        public string MockUserName { get; set; }
    }
}