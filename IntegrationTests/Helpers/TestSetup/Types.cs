using System.Threading.Tasks;
using DeploymentTrackerCore.Models;
using DeploymentTrackerCore.Services;
using IntegrationTests.EnvironmentSetup;
using Microsoft.EntityFrameworkCore;

namespace IntegrationTests.Helpers.TestSetup
{
    public static class Types
    {
        public static async Task<int> AddTypeAndGetId(Type type)
        {
            await AddType(type);

            return await GetTypeId(type);
        }

        public static async Task<int> AddUniqueTypeAndGetId()
        {
            var type = new Type
            {
                Name = TestNames.TypeName,
                TeardownTemplate = null
            };

            return await AddTypeAndGetId(type);
        }

        private static async Task AddType(Type type)
        {
            var context = GetTestContext();

            context.Types.Add(type);

            await context.SaveChangesAsync();
        }

        private static async Task<int> GetTypeId(Type type)
        {
            var context = GetTestContext();

            return (await context.Types
            .SingleAsync(t => t.Name == type.Name))
            .Id;
        }

        private static DeploymentAppContext GetTestContext()
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