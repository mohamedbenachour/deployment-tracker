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

using System;
using System.Threading.Tasks;

using DeploymentTrackerCore.Actions;
using DeploymentTrackerCore.Actions.Deployments;
using DeploymentTrackerCore.Hubs;
using DeploymentTrackerCore.Models.API;
using DeploymentTrackerCore.Models.Entities;
using DeploymentTrackerCore.Services;
using DeploymentTrackerCore.Services.DeploymentManagement;
using DeploymentTrackerCore.Services.Jira;
using DeploymentTrackerCore.Services.Token;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace DeploymentTrackerCore.Controllers {
    [Route("api/deployment/external")]
    [ApiController]
    [AllowAnonymous]
    public class DeploymentExternalController : BaseApiController {
        private DeploymentAppContext Context { get; }
        private IRequestState CurrentRequestState { get; }
        private ITokenVerifier ExternalTokenVerifier { get; }
        private ApiDeploymentHydrator Hydrator { get; }
        private ReportDeploymentChange Reporter { get; }

        public DeploymentExternalController(
            DeploymentAppContext context,
            IRequestState requestState,
            ITokenVerifier tokenVerifier,
            IDeploymentManager deploymentManager,
            IJiraService jiraService,
            IHubContext<DeploymentHub, IDeploymentClient> hubContext) : base(requestState, null) {
            Context = context;
            CurrentRequestState = requestState;
            ExternalTokenVerifier = tokenVerifier;
            Hydrator = new ApiDeploymentHydrator(deploymentManager, jiraService);
            Reporter = new ReportDeploymentChange(hubContext);
        }

        [HttpPost]
        [Route("destroyed")]
        public async Task<ActionResult<ApiDeployment>> DeploymentDestroyed(ApiExternalDeploymentDestroyed request) {
            var destroyer = new DeploymentDestroyed(Context, request.SiteName);
            var apiHandler = new ApiActionHandler(destroyer, Hydrator, Reporter);

            return await Handle(apiHandler, request);
        }

        [HttpPost]
        public async Task<ActionResult<ApiDeployment>> CreateDeployment(ApiExternalNewDeployment deployment) {
            var creator = new NewDeployment(Context, deployment);
            var apiHandler = new ApiActionHandler(creator, Hydrator, Reporter);

            return await Handle(apiHandler, deployment);
        }

        protected async Task<ActionResult<T>> Handle<T>(IActionPerformer<T> actionHandler, IExternalRequest request) {
            if (!IsValidToken(request.Token.Value)) {
                return BadRequest();
            }

            SetUser(request.User);

            return await base.Handle(actionHandler);
        }

        private void SetUser(ApiUser user) {
            if (user != null) {
                CurrentRequestState.SetUser(new User {
                    Name = user.Name,
                        Username = user.Username
                });
            }
        }

        private bool IsValidToken(string token) {
            return ExternalTokenVerifier.IsValid(token);
        }
    }
}