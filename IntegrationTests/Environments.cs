using System.Collections.Generic;
using System.Threading.Tasks;
using DeploymentTrackerCore.Models.API;
using FluentAssertions;
using IntegrationTests.EnvironmentSetup;
using IntegrationTests.Helpers;
using NUnit.Framework;

namespace IntegrationTests
{
    [TestFixture]
    public class Environments
    {
        private const string EnvironmentURL = TestEnvironment.URLs.Environment;

        [Test]
        public async Task EnvironmentsCanBeRetrievedForAnEmptySite()
        {
            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();

            var response = await client.GetAsync(EnvironmentURL);

            var environments = await response.AssertSuccessfulResponseAndGetContent<IEnumerable<ApiEnvironment>>();

            environments.Should().NotBeNull();
        }

        [Test]
        public async Task AnEnvironmentCanBeCreated() {
            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();
            var environmentToCreate = new ApiNewEnvironment {
                HostName = "the-cloud",
                Name = "The Cloud, Not The real one Obvs"
            };

            var response = await client.PostJsonAsync(EnvironmentURL, environmentToCreate);

            var createdEnvironment = await response.AssertSuccessfulResponseAndGetContent<ApiEnvironment>();

            createdEnvironment.Should().BeEquivalentTo(environmentToCreate, (options) => options
                .ExcludingMissingMembers());
        }

        [Test]
        public async Task AnEnvironmentWithADuplicateHostNameAndNameCannotBeCreated() {
            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();
            var environmentToCreate = new ApiNewEnvironment {
                HostName = "duplicate-me",
                Name = "The copy"
            };

            await client.PostJsonAsync(EnvironmentURL, environmentToCreate);

            var response = await client.PostJsonAsync(EnvironmentURL, environmentToCreate);

            response.AssertBadRequest();
        }

        [Test]
        public async Task AnEnvironmentWithNoDeploymentsCanBeDeleted() {
            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();
            var environmentToCreate = new ApiNewEnvironment {
                HostName = "one-to-delete",
                Name = "The one I want to delete"
            };

            var createResponse = await client.PostJsonAsync(EnvironmentURL, environmentToCreate);

            var createdEnvironment = await createResponse.AssertSuccessfulResponseAndGetContent<ApiEnvironment>();

            var deleteResponse = await client.DeleteAsync($"{EnvironmentURL}/{createdEnvironment.Id}");

            deleteResponse.AssertSuccessfulResponse();
        }

        [Test]
        public async Task AnEnvironmentWithDeploymentsCannotBeDeleted() {
            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();
            var environmentToCreate = new ApiNewEnvironment {
                HostName = "another-un",
                Name = "Another Environment I like"
            };

            var response = await client.PostJsonAsync(EnvironmentURL, environmentToCreate);

            var createdEnvironment = await response.AssertSuccessfulResponseAndGetContent<ApiEnvironment>();

            var deploymentToCreate = new ApiNewDeployment {
                BranchName = "i-cannot-be-deleted",
                EnvironmentId = createdEnvironment.Id,
                PublicURL = "https://yoyo.co",
                SiteName = "delete-me"
            };

            var deploymentResponse = await client.PostJsonAsync(TestEnvironment.URLs.Deployment, deploymentToCreate);

            deploymentResponse.AssertSuccessfulResponse();

            var deleteResponse = await client.DeleteAsync($"{EnvironmentURL}/{createdEnvironment.Id}");

            deleteResponse.AssertBadRequest();
        }
    }
}