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

using System.Collections.Generic;
using System.Threading.Tasks;
using DeploymentTrackerCore.Models.API;
using FluentAssertions;
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
        public async Task AnEnvironmentCanBeCreated()
        {
            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();
            var environmentToCreate = new ApiNewEnvironment
            {
                HostName = "the-cloud",
                Name = "The Cloud, Not The real one Obvs"
            };

            var response = await client.PostJsonAsync(EnvironmentURL, environmentToCreate);

            var createdEnvironment = await response.AssertSuccessfulResponseAndGetContent<ApiEnvironment>();

            createdEnvironment.Should().BeEquivalentTo(environmentToCreate, (options) => options
                .ExcludingMissingMembers());
        }

        [Test]
        public async Task AnEnvironmentWithADuplicateHostNameAndNameCannotBeCreated()
        {
            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();
            var environmentToCreate = new ApiNewEnvironment
            {
                HostName = "duplicate-me",
                Name = "The copy"
            };

            await client.PostJsonAsync(EnvironmentURL, environmentToCreate);

            var response = await client.PostJsonAsync(EnvironmentURL, environmentToCreate);

            response.AssertBadRequest();
        }

        [Test]
        public async Task AnEnvironmentWithNoDeploymentsCanBeDeleted()
        {
            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();
            var environmentToCreate = new ApiNewEnvironment
            {
                HostName = "one-to-delete",
                Name = "The one I want to delete"
            };

            var createResponse = await client.PostJsonAsync(EnvironmentURL, environmentToCreate);

            var createdEnvironment = await createResponse.AssertSuccessfulResponseAndGetContent<ApiEnvironment>();

            var deleteResponse = await client.DeleteAsync($"{EnvironmentURL}/{createdEnvironment.Id}");

            deleteResponse.AssertSuccessfulResponse();
        }

        [Test]
        public async Task AnEnvironmentWithDeploymentsCannotBeDeleted()
        {
            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();
            var environmentToCreate = new ApiNewEnvironment
            {
                HostName = "another-un",
                Name = "Another Environment I like"
            };

            var response = await client.PostJsonAsync(EnvironmentURL, environmentToCreate);

            var createdEnvironment = await response.AssertSuccessfulResponseAndGetContent<ApiEnvironment>();

            var deploymentToCreate = new ApiNewDeployment
            {
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