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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DeploymentTrackerCore.Models;
using DeploymentTrackerCore.Models.API;
using FluentAssertions;
using FluentAssertions.Execution;
using IntegrationTests.Helpers;
using IntegrationTests.Helpers.DeploymentsApi;
using IntegrationTests.Helpers.TestSetup;

using NUnit.Framework;

namespace IntegrationTests
{
    [TestFixture]
    public class Deployments
    {
        private int EnvironmentId { get; set; }

        [Test]
        public async Task ADeploymentCanBeCreated()
        {
            var deploymentToCreate = CreateDeploymentForDefaultType();

            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();

            var response = await client.PostJsonAsync(TestEnvironment.URLs.Deployment, deploymentToCreate);

            await response.AssertSuccessfulResponseAndGetContent<ApiNewDeployment>();
        }

        [Test]
        public async Task DeploymentsForDifferentTypesUsingDifferentSiteNamesAndTheSameBranchNameCanBeCreated()
        {

            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();
            var branchName = TestNames.BranchName;

            var firstTypeId = await Types.AddUniqueTypeAndGetId();
            var secondTypeId = await Types.AddUniqueTypeAndGetId();

            var firstDeploymentToCreate = CreateDeployment(firstTypeId);
            var secondDeploymentToCreate = CreateDeployment(secondTypeId);

            await client.PostJsonAsync(TestEnvironment.URLs.Deployment, firstDeploymentToCreate);
            await client.PostJsonAsync(TestEnvironment.URLs.Deployment, secondDeploymentToCreate);

            var currentDeployments = await GetCurrentDeployments.ForClient(client);

            using (new AssertionScope())
            {
                AssertThatDeploymentExists(currentDeployments, firstDeploymentToCreate);
                AssertThatDeploymentExists(currentDeployments, secondDeploymentToCreate);
            }
        }

        private void AssertThatDeploymentExists(IEnumerable<ApiDeployment> existingDeployments, ApiNewDeployment toFind) =>
            existingDeployments.Should().ContainEquivalentOf(
                toFind,
                (options) => options
            .Including(deployment => deployment.SiteName)
            .Including(deployment => deployment.BranchName)
            .Including(deployment => deployment.Type.Id));

        [Test]
        public async Task DeploymentsCreatedCanBeListed()
        {
            var deploymentToCreate = CreateDeploymentForDefaultType();

            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();

            (await client.PostJsonAsync(TestEnvironment.URLs.Deployment, deploymentToCreate))
            .AssertSuccessfulResponse();

            var response = await client.GetAsync(TestEnvironment.URLs.Deployment);

            await response.AssertSuccessfulResponseAndGetContent<IEnumerable<ApiDeployment>>();
        }

        [OneTimeSetUp]
        public async Task OneTimeSetup() => EnvironmentId = (await UniqueEnvironment.Create()).Id;

        private ApiNewDeployment CreateDeployment(int typeId) => new ApiNewDeployment
        {
            BranchName = TestNames.BranchName,
            SiteName = TestNames.SiteName,
            EnvironmentId = EnvironmentId,
            PublicURL = TestNames.PublicUrl,
            SiteLogin = new Login
            {
                UserName = "user-name-here",
                Password = "passwordzz"
            },
            Type = new ApiType
            {
                Id = typeId
            }
        };

        private ApiNewDeployment CreateDeploymentForDefaultType() => CreateDeployment(1);
    }
}