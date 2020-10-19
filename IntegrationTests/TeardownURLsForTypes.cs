using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DeploymentTrackerCore.Models.API;
using DeploymentTrackerCore.Models.Entities;
using DeploymentTrackerCore.Services;

using FluentAssertions;

using IntegrationTests.EnvironmentSetup;
using IntegrationTests.Helpers;
using IntegrationTests.Helpers.TestSetup;

using Microsoft.EntityFrameworkCore;

using NUnit.Framework;

namespace IntegrationTests {
    [TestFixture]
    public class TeardownURLSForTypes {
        private int EnvironmentId { get; set; }

        [Test]
        public async Task ADeploymentCreatedForATypeWithNoTeardownUrlTemplateHasANullTeardownUrl() {

            var createdDeployment = await CreateDeploymentUsingTypeWithSpecifiedTeardownUrlTemplate(null);

            createdDeployment.TeardownUrl.Should().BeNull();
        }

        [Test]
        public async Task ADeploymentCreatedForATypeWithATeardownUrlTemplateHasTheCorrectUrlReturned() {
            var urlTemplate = "https://deployment-destroyer.xyz/teardown?siteName={{SiteName}}&moreStuff=true";

            var createdDeployment = await CreateDeploymentUsingTypeWithSpecifiedTeardownUrlTemplate(urlTemplate);

            createdDeployment.TeardownUrl.Should().Be(urlTemplate.Replace("{{SiteName}}", createdDeployment.SiteName));
        }

        [OneTimeSetUp]
        public async Task OneTimeSetup() => EnvironmentId = (await UniqueEnvironment.Create()).Id;

        private async Task<ApiDeployment> CreateDeploymentUsingTypeWithSpecifiedTeardownUrlTemplate(string teardownUrlTemplate) {
            var type = new Type {
                Name = TestNames.TypeName,
                TeardownTemplate = teardownUrlTemplate
            };

            var typeId = await Types.AddTypeAndGetId(type);

            return await UniqueDeployment.ForTypeAndEnvironment(EnvironmentId, typeId);
        }

    }
}