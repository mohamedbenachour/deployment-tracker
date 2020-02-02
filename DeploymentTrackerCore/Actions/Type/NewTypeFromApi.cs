using System.Threading.Tasks;
using DeploymentTrackerCore.Models;
using DeploymentTrackerCore.Models.API;

namespace DeploymentTrackerCore.Actions.Type {
    public class NewTypeFromApi : IAction<ApiNewType, ApiType>
    {
        public NewTypeFromApi(DeploymentAppContext context) {
            Context = context;
        }

        private DeploymentAppContext Context { get; }

        public async Task<ApplicationActionResult<ApiType>> Perform(ApiNewType newType)
        {
            var action = new InternalToApi<ApiNewType>(
                new ActionWithValidation<ApiNewType, Models.Type>(
                    new NewType(Context)
                )
                );

            return await action.Perform(newType);
        }
    }
}