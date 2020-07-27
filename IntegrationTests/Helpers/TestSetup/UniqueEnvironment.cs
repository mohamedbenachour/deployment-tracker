using System.Threading.Tasks;
using DeploymentTrackerCore.Models.API;

namespace IntegrationTests.Helpers.TestSetup
{
    public static class UniqueEnvironment
    {
        public static async Task<ApiEnvironment> Create()
        {
            var client = await TestEnvironment.ClientFactory.GetAuthenticatedClient();

            var environmentToCreate = new ApiNewEnvironment
            {
                HostName = TestNames.HostName,
                Name = TestNames.Environment
            };

            var createResponse = await client.PostJsonAsync(TestEnvironment.URLs.Environment, environmentToCreate);

            var createdEnvironment = await createResponse.AssertSuccessfulResponseAndGetContent<ApiEnvironment>();

            return createdEnvironment;
        }
    }
}