
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeploymentTrackerCore.Models;
using DeploymentTrackerCore.Models.API;
using DeploymentTrackerCore.Services;
using FluentAssertions;
using IntegrationTests.EnvironmentSetup;
using IntegrationTests.Helpers;
using IntegrationTests.Helpers.TestSetup;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace IntegrationTests
{
    [TestFixture]
    public class TeardownURLsForTypes
    {
        private int EnvironmentId { get; set; }

        [Test]
        public async Task ADeploymentCreatedForATypeWithNoTeardownUrlTemplateHasANullTeardownUrl()
        {

            var createdDeployment = await CreateDeploymentUsingTypeWithSpecifiedTeardownUrlTemplate(null);

            createdDeployment.TeardownUrl.Should().BeNull();
        }

        [Test]
        public async Task ADeploymentCreatedForATypeWithATeardownUrlTemplateHasTheCorrectUrlReturned()
        {
            var urlTemplate = "https://deployment-destroyer.xyz/teardown?siteName={{SiteName}}&moreStuff=true";

            var createdDeployment = await CreateDeploymentUsingTypeWithSpecifiedTeardownUrlTemplate(urlTemplate);


            createdDeployment.TeardownUrl.Should().Be(urlTemplate.Replace("{{SiteName}}", createdDeployment.SiteName));
        }

        [OneTimeSetUp]
        public async Task OneTimeSetup() => EnvironmentId = (await UniqueEnvironment.Create()).Id;

        private async Task<ApiDeployment> CreateDeploymentUsingTypeWithSpecifiedTeardownUrlTemplate(string teardownUrlTemplate)
        {
            var type = new Type
            {
                Name = TestNames.TypeName,
                TeardownTemplate = teardownUrlTemplate
            };

            await AddType(type);

            var typeId = await GetTypeId(type);

            var deploymentToCreate = new ApiNewDeployment
            {
                SiteName = TestNames.SiteName,
                BranchName = TestNames.BranchName,
                EnvironmentId = EnvironmentId,
                Type = new ApiType
                {
                    Id = typeId
                },
                PublicURL = TestNames.PublicUrl
            };

            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();

            (await client.PostJsonAsync(TestEnvironment.URLs.Deployment, deploymentToCreate))
                .AssertSuccessfulResponse();

            var deployments = await (await client.GetAsync(TestEnvironment.URLs.Deployment))
            .AssertSuccessfulResponseAndGetContent<IEnumerable<ApiDeployment>>();

            var createdDeployment = deployments.Single(deployment => deployment.SiteName == deploymentToCreate.SiteName);

            return createdDeployment;
        }

        private async Task AddType(Type type)
        {
            var context = GetTestContext();

            context.Types.Add(type);

            await context.SaveChangesAsync();
        }

        private async Task<int> GetTypeId(Type type)
        {
            var context = GetTestContext();

            return (await context.Types
            .SingleAsync(t => t.Name == type.Name))
            .Id;
        }

        private DeploymentAppContext GetTestContext()
        {
            var contextOptions = new DbContextOptionsBuilder<DeploymentAppContext>();

            TestApplicationFactory<System.String>.SetupDatabaseOptions(contextOptions);

            return new DeploymentAppContext(contextOptions.Options, new TestRequestState());
        }

        private class TestRequestState : IRequestState
        {
            public User GetUser()
            {
                return new User
                {
                    Name = "Test User",
                    Username = "test-user"
                };
            }

            public void SetUser(User user)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}