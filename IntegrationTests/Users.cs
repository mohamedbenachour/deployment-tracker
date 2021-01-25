using System.Collections.Generic;
using System.Threading.Tasks;

using DeploymentTrackerCore.Models.API;

using FluentAssertions;

using IntegrationTests.Helpers;

using NUnit.Framework;

namespace IntegrationTests {
    [TestFixture]
    public class Users {
        [Test]
        public async Task TheListOfUsersCanBeRetrieved() {
            var user = TestNames.UserName;
            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient(user);

            var response = await client.GetAsync(TestEnvironment.URLs.Users);

            (await response.AssertSuccessfulResponseAndGetContent<IEnumerable<ApiUser>>())
            .Should().NotBeEmpty();
        }
    }
}