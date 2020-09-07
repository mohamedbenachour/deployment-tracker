using System.Threading.Tasks;

using DeploymentTrackerCore.Models.API;

namespace IntegrationTests.Helpers.TestSetup {
    public class UniqueDeploymentProvider {
        private int TypeId { get; }
        private int EnvironmentId { get; }

        public UniqueDeploymentProvider() {
            EnvironmentId = UniqueEnvironment.Create().Result.Id;
            TypeId = Types.AddUniqueTypeAndGetId().Result;
        }

        public async Task<ApiDeployment> Create() => await UniqueDeployment.ForTypeAndEnvironment(EnvironmentId, TypeId);
    }
}