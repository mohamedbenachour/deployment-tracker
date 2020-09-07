using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using DeploymentTrackerCore.Models;
using DeploymentTrackerCore.Models.API;

using FluentAssertions;

using IntegrationTests.Helpers;
using IntegrationTests.Helpers.TestSetup;

using NUnit.Framework;

namespace IntegrationTests {
    [TestFixture]
    public class DeploymentTriggerURLsForTypes {

        private int EnvironmentId { get; set; }

        [Test]
        public async Task ADeploymentCreatedForATypeWithNoTeardownUrlTemplateHasANullTeardownUrl() {

            var createdDeployment = await CreateDeploymentUsingTypeWithSpecifiedDeploymentUrlTemplate(null);

            createdDeployment.ManagementUrls.DeploymentTriggerUrl.Should().BeNull();
        }

        [Test]
        public async Task ADeploymentCreatedForATypeWithATeardownUrlTemplateHasTheCorrectUrlReturned() {
            var urlTemplate = "https://deployment-deployer.xyz/moar-deployments/{{BranchName}}build?moreStuff=true";

            var createdDeployment = await CreateDeploymentUsingTypeWithSpecifiedDeploymentUrlTemplate(urlTemplate);

            createdDeployment.ManagementUrls.DeploymentTriggerUrl.Should().Be(urlTemplate.Replace("{{BranchName}}", HttpUtility.UrlEncode(createdDeployment.BranchName)));
        }

        [OneTimeSetUp]
        public async Task OneTimeSetup() => EnvironmentId = (await UniqueEnvironment.Create()).Id;

        private async Task<ApiDeployment> CreateDeploymentUsingTypeWithSpecifiedDeploymentUrlTemplate(string deploymentUrlTemplate) {
            var type = new Type {
                Name = TestNames.TypeName,
                DeploymentTemplate = deploymentUrlTemplate
            };

            var typeId = await Types.AddTypeAndGetId(type);

            return await UniqueDeployment.ForTypeAndEnvironment(EnvironmentId, typeId);
        }

    }
}