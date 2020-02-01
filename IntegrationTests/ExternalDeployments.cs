using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using DeploymentTrackerCore.Models;
using DeploymentTrackerCore.Models.API;
using FluentAssertions;
using IntegrationTests.Helpers;
using NUnit.Framework;

namespace IntegrationTests
{
    [TestFixture]
    public class ExternalDeployments {
        private int EnvironmentId { get; set; }

        [Test]
        public async Task ADeploymentCanBeAddedUsingSomewhatQuestionableJson() {
            var sampleJsonWithTrailingCommas = $@"{{
                ""branchName"": ""test-123-external"",
                ""siteName"": ""external-test"",
                ""environmentId"": {EnvironmentId},
                ""publicURL"": ""https://local.co/external"",
                ""user"": {{
                    ""userName"": ""external-user"",
                    ""name"": ""External User"",
                }},
                ""token"": {{
                    ""value"": ""{TestEnvironment.ExternalToken}"",
                }},
            }}";

            var client = TestEnvironment.ClientFactory.CreateClient();

            var response = await client.PostAsync(TestEnvironment.URLs.DeploymentExternal, new StringContent(sampleJsonWithTrailingCommas, Encoding.UTF8, MediaTypeNames.Application.Json));

            var createdDeployment = await response.AssertSuccessfulResponseAndGetContent<ApiNewDeployment>();
        
            createdDeployment.Should().NotBeNull();
        }

        [Test]
        public async Task ADeploymentCanBeAdded() {
            var deploymentToCreate = new ApiExternalNewDeployment {
                BranchName = "external-one-being-added",
                SiteName = "some-name",
                EnvironmentId = EnvironmentId,
                PublicURL = "https://externals.com.au",
                User = new ApiUser {
                    Name = "External Userz",
                    Username = "external-user-here"
                },
                SiteLogin = new Login {
                    UserName = "user-name-here",
                    Password = "passwordzz"
                },
                Token = new ApiExternalTokenContainer {
                    Value = TestEnvironment.ExternalToken
                },
                Type = new ApiType {
                    Id = 1
                }
            };

            var client = TestEnvironment.ClientFactory.CreateClient();

            var response = await client.PostJsonAsync(TestEnvironment.URLs.DeploymentExternal, deploymentToCreate);

            var createdDeployment = await response.AssertSuccessfulResponseAndGetContent<ApiNewDeployment>();

            createdDeployment.Should().BeEquivalentTo(deploymentToCreate, (options) => 
                options.ExcludingMissingMembers()
                .Excluding(d => d.Type.Name)
                );
        }

        [Test]
        public async Task ADeploymentWithATypeNotSpecifiedHasTheDefaultTypeSet() {
            var deploymentToCreate = new ApiExternalNewDeployment {
                BranchName = "external-one-being-added-v2",
                SiteName = "some-name-2",
                EnvironmentId = EnvironmentId,
                PublicURL = "https://externals.com.au",
                User = new ApiUser {
                    Name = "External Userz",
                    Username = "external-user-here"
                },
                SiteLogin = new Login {
                    UserName = "user-name-here",
                    Password = "passwordzz"
                },
                Token = new ApiExternalTokenContainer {
                    Value = TestEnvironment.ExternalToken
                }
            };

            var client = TestEnvironment.ClientFactory.CreateClient();

            var response = await client.PostJsonAsync(TestEnvironment.URLs.DeploymentExternal, deploymentToCreate);

            var createdDeployment = await response.AssertSuccessfulResponseAndGetContent<ApiNewDeployment>();

            createdDeployment.Type.Id.Should().Be(1);
        }

        [Test]
        public async Task AnInvalidTokenCannotBeUsedToCreateANewDeployment() {
            var deploymentToCreate = new ApiExternalNewDeployment {
                BranchName = "external-one-being-added-and-torndown",
                SiteName = "some-name-2-torndown",
                EnvironmentId = EnvironmentId,
                PublicURL = "https://externals.com.au/torndown",
                User = new ApiUser {
                    Name = "External Userz",
                    Username = "external-user-here"
                },
                SiteLogin = new Login {
                    UserName = "user-name-here",
                    Password = "passwordzz"
                },
                Token = new ApiExternalTokenContainer {
                    Value = "definitely-aint-valid"
                }
            };

            var client = TestEnvironment.ClientFactory.CreateClient();

            var response = await client.PostJsonAsync(TestEnvironment.URLs.DeploymentExternal, deploymentToCreate);

            response.AssertBadRequest();
        }

        [Test]
        public async Task ADeploymentCanBeTorndown() {
            var deploymentToCreate = new ApiExternalNewDeployment {
                BranchName = "external-one-being-added-and-torndown",
                SiteName = "some-name-2-torndown",
                EnvironmentId = EnvironmentId,
                PublicURL = "https://externals.com.au/torndown",
                User = new ApiUser {
                    Name = "External Userz",
                    Username = "external-user-here"
                },
                SiteLogin = new Login {
                    UserName = "user-name-here",
                    Password = "passwordzz"
                },
                Token = new ApiExternalTokenContainer {
                    Value = TestEnvironment.ExternalToken
                }
            };

            var client = TestEnvironment.ClientFactory.CreateClient();

            var response = await client.PostJsonAsync(TestEnvironment.URLs.DeploymentExternal, deploymentToCreate);

            response.AssertSuccessfulResponse();

            var deploymentToDestroy = new ApiExternalDeploymentDestroyed {
                SiteName = deploymentToCreate.SiteName,
                User = new ApiUser {
                    Name = "External Userz",
                    Username = "external-user-here"
                },
                Token = new ApiExternalTokenContainer {
                    Value = TestEnvironment.ExternalToken
                }
            };

            var destroyedResponse = await client.PostJsonAsync($"{TestEnvironment.URLs.DeploymentExternal}/destroyed", deploymentToDestroy);

            var destroyedDeployment = await destroyedResponse.AssertSuccessfulResponseAndGetContent<ApiDeployment>();

            destroyedDeployment.Status.Should().Be(DeploymentStatus.DESTROYED.ToString());
        }

        [OneTimeSetUp]
        public async Task OneTimeSetup() {
            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();
            var environmentToCreate = new ApiNewEnvironment {
                HostName = "one-to-delete",
                Name = "The one I want to delete"
            };

            var createResponse = await client.PostJsonAsync(TestEnvironment.URLs.Environment, environmentToCreate);

            var createdEnvironment = await createResponse.AssertSuccessfulResponseAndGetContent<ApiEnvironment>();
        
            EnvironmentId = createdEnvironment.Id;
        }
    }
}