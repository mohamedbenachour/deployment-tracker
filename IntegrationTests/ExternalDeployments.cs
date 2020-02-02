/*
* This file is part of Deployment Tracker.
* 
* Deployment Tracker is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
*
* Deployment Tracker is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License
* along with Deployment Tracker. If not, see <https://www.gnu.org/licenses/>.
*/

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

            var destroyedResponse = await client.PostJsonAsync(
                TestEnvironment.URLs.DeploymentExternalRoot.WithPart("destroyed").ToString(),
                deploymentToDestroy
                );

            var destroyedDeployment = await destroyedResponse.AssertSuccessfulResponseAndGetContent<ApiDeployment>();

            destroyedDeployment.Status.Should().Be(DeploymentStatus.DESTROYED.ToString());
        }

        [OneTimeSetUp]
        public async Task OneTimeSetup() {
            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();
            var environmentToCreate = new ApiNewEnvironment {
                HostName = "external-testing",
                Name = "externals-go-here"
            };

            var createResponse = await client.PostJsonAsync(TestEnvironment.URLs.Environment, environmentToCreate);

            var createdEnvironment = await createResponse.AssertSuccessfulResponseAndGetContent<ApiEnvironment>();
        
            EnvironmentId = createdEnvironment.Id;
        }
    }
}