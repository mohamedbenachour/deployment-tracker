using deployment_tracker.Actions;
using deployment_tracker.Models;
using deployment_tracker.Models.API;

using System;
using System.Threading.Tasks;

namespace deployment_tracker.Actions.Deployments {
    public class ApiActionHandler : IActionPerformer<ApiDeployment> {
        private IActionPerformer<Deployment> InternalAction { get; }
        private ApiDeploymentHydrator Hydrator { get; }
        public IPostAction<ApiDeployment> PostAction { get; }

        public bool Succeeded { get; private set; }

        public String Error { get; private set; }

        public ApiDeployment Result { get; private set; }

        public ApiActionHandler(IActionPerformer<Deployment> internalAction, ApiDeploymentHydrator hydrator, IPostAction<ApiDeployment> postAction = null) {
            InternalAction = internalAction;
            Hydrator = hydrator;
            PostAction = postAction;
        }

        public async Task Perform() {
            await InternalAction.Perform();

            if (InternalAction.Succeeded) {
                Succeeded = true;
                Result = ApiDeployment.FromInternal(InternalAction.Result);
                Hydrator.Hydrate(Result);

                if (PostAction != null) {
                    PostAction.Perform(Result);
                }
            } else {
                Succeeded = false;
                Error = InternalAction.Error;
            }
        }
    }
}