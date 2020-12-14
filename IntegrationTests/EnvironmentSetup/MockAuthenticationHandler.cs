using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using DeploymentTrackerCore.Services.Identity;

using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IntegrationTests.EnvironmentSetup {
    public class MockAuthenticationHandler : AuthenticationHandler<MockAuthenticationOptions> {
        public MockAuthenticationHandler(IOptionsMonitor<MockAuthenticationOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync() {
            var principal = new ClaimsPrincipal(GetMockUser().GetClaimIdentity("Test"));
            var ticket = new AuthenticationTicket(principal, "Test");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }

        private ApplicationUser GetMockUser() => new ApplicationUser {
            Name = "Test User",
            UserName = Options.MockUserName
        };
    }
}