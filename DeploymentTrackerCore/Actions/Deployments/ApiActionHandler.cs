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
 
using DeploymentTrackerCore.Actions;
using DeploymentTrackerCore.Models;
using DeploymentTrackerCore.Models.API;

using System;
using System.Threading.Tasks;

namespace DeploymentTrackerCore.Actions.Deployments {
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
                await Hydrator.Hydrate(Result);

                if (PostAction != null) {
                    await PostAction.Perform(Result);
                }
            } else {
                Succeeded = false;
                Error = InternalAction.Error;
            }
        }
    }
}