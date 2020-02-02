using System.Threading.Tasks;

namespace DeploymentTrackerCore.Actions {
    public class ActionWithValidation<I, O> : IAction<I, O>
    {
        public ActionWithValidation(IAction<I, O> action) {
            InternalAction = action;
        }

        public IAction<I, O> InternalAction { get; }

        public async Task<ApplicationActionResult<O>> Perform(I input)
        {
            try {
                return await InternalAction.Perform(input);
            } catch (ActionNotValidException exc) {
                return new ApplicationActionResult<O> {
                    Succeeded = false,
                    Error = exc.Message
                };
            }
        }
    }
}