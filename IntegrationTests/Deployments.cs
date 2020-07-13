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
using IntegrationTests.Helpers;
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
            var deploymentToCreate = new ApiNewDeployment
            {
                BranchName =TestNames.BranchName,
                SiteName = TestNames.SiteName,
                EnvironmentId = EnvironmentId,
                PublicURL = "https://internals.com.au",
                SiteLogin = new Login
                {
                    UserName = "user-name-here",
                    Password = "passwordzz"
                },
                Type = new ApiType
                {
                    Id = 1
                }
            };

            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();

            var response = await client.PostJsonAsync(TestEnvironment.URLs.Deployment, deploymentToCreate);

            await response.AssertSuccessfulResponseAndGetContent<ApiNewDeployment>();
        }

        [Test]
        public async Task DeploymentsCreatedCanBeListed()
        {
            var deploymentToCreate = new ApiNewDeployment
            {
                BranchName = TestNames.BranchName,
                SiteName = TestNames.SiteName,
                EnvironmentId = EnvironmentId,
                PublicURL = "https://internals.com.au",
                SiteLogin = new Login
                {
                    UserName = "user-name-here",
                    Password = "passwordzz"
                },
                Type = new ApiType
                {
                    Id = 1
                }
            };

            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();
            
            (await client.PostJsonAsync(TestEnvironment.URLs.Deployment, deploymentToCreate))
                .AssertSuccessfulResponse();

            var response = await client.GetAsync(TestEnvironment.URLs.Deployment);

            await response.AssertSuccessfulResponseAndGetContent<IEnumerable<ApiDeployment>>();
        }

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();
            var environmentToCreate = new ApiNewEnvironment
            {
                HostName = TestNames.HostName,
                Name = TestNames.Environment
            };

            var createResponse = await client.PostJsonAsync(TestEnvironment.URLs.Environment, environmentToCreate);

            var createdEnvironment = await createResponse.AssertSuccessfulResponseAndGetContent<ApiEnvironment>();

            EnvironmentId = createdEnvironment.Id;
        }
    }
}
