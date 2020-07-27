using System.Web;
using DeploymentTrackerCore.Models;
using DeploymentTrackerCore.Services.DeploymentManagement.TypeBased;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace UnitTests.Services.DeploymentManagement.TypeBased
{
    public class DeployedSiteStringTemplaterTests
    {
        private const string StaticUrl = "https://thing-to-template.com";

        [Test]
        public void ADeployedSiteWithAStaticValueAsATemplateReturnsTheStaticValue()
        {
            var templatedResult = new DeployedSiteStringTemplater()
                .Template(CreateMockDeployedSite("test"), StaticUrl);

            templatedResult.Should().Be(StaticUrl);
        }

        [Test]
        public void ADeployedSiteWithATemplateForTheSiteNameHasTheSiteNameVariableReplaced()
        {
            var siteName = "site-name-1-2-3";
            var template = "https://thing-to-template.com/destroy?siteName={{SiteName}}";

            var templatedResult = new DeployedSiteStringTemplater()
                .Template(CreateMockDeployedSite(siteName), template);

            templatedResult.Should().Be($"https://thing-to-template.com/destroy?siteName={siteName}");
        }

        [Test]
        public void TheSiteNameShouldBeUrlEncodedWhenGeneratingTheUrl()
        {
            var siteName = "site1&test%12";
            var template = "https://thing-to-template.com/destroy?siteName={{SiteName}}";

            var templatedResult = new DeployedSiteStringTemplater()
                .Template(CreateMockDeployedSite(siteName), template);

            templatedResult.Should().Be($"https://thing-to-template.com/destroy?siteName={HttpUtility.UrlEncode(siteName)}");
        }

        private IDeployedSite CreateMockDeployedSite(string siteName)
        {
            var mockSite = new Mock<IDeployedSite>();

            mockSite.Setup(ms => ms.SiteName).Returns(siteName);

            return mockSite.Object;
        }
    }
}