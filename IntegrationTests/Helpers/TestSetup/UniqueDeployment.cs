using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using DeploymentTrackerCore.Models.API;

namespace IntegrationTests.Helpers.TestSetup {
    public static class UniqueDeployment {
        public static async Task<ApiDeployment> ForTypeAndEnvironment(int environmentId, int typeId) {
            var deploymentToCreate = new ApiNewDeployment {
                SiteName = TestNames.SiteName,
                BranchName = TestNames.BranchName,
                EnvironmentId = environmentId,
                Type = new ApiType {
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
    }
}