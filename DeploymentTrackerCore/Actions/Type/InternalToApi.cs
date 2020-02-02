using System.Threading.Tasks;
using DeploymentTrackerCore.Models.API;

namespace DeploymentTrackerCore.Actions.Type {
    public class InternalToApi<I> : IAction<I, ApiType>
    {
        public InternalToApi(IAction<I, Models.Type> internalAction) {
            InternalAction = internalAction;
        }

        public IAction<I, Models.Type> InternalAction { get; }

        public async Task<ApplicationActionResult<ApiType>> Perform(I input)
        {
            var actionResult = await InternalAction.Perform(input);

            if (actionResult.Succeeded) {
                return new ApplicationActionResult<ApiType> {
                    Succeeded = true,
                    Result = ApiType.FromInternal(actionResult.Result)
                };
            }

            return actionResult.CopyError<ApiType>();
        }
    }
}