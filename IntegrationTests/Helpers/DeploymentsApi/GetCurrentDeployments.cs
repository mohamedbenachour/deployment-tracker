using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DeploymentTrackerCore.Models.API;

namespace IntegrationTests.Helpers.DeploymentsApi
{
    public static class GetCurrentDeployments
    {
        public static async Task<IEnumerable<ApiDeployment>> ForClient(HttpClient client)
        {
            var response = await client.GetAsync(TestEnvironment.URLs.Deployment);

            return await response.AssertSuccessfulResponseAndGetContent<IEnumerable<ApiDeployment>>();
        }
    }
}
