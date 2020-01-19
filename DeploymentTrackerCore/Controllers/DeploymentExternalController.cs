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
using Microsoft.AspNetCore.Mvc;
using DeploymentTrackerCore.Models;
using DeploymentTrackerCore.Models.API;

using DeploymentTrackerCore.Actions.Deployments;
using DeploymentTrackerCore.Services;
using DeploymentTrackerCore.Services.DeploymentManagement;
using DeploymentTrackerCore.Services.Token;
using DeploymentTrackerCore.Services.Jira;

using DeploymentTrackerCore.Hubs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace DeploymentTrackerCore.Controllers
{
    [Route("api/deployment/external")]
    [ApiController]
    [AllowAnonymous]
    public class DeploymentExternalController : BaseApiController
    {
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
            VerifyValidToken(request.Token.Value);
            SetUser(request.User);
            
            var destroyer = new DeploymentDestroyed(Context, request.SiteName);
            var apiHandler = new ApiActionHandler(destroyer, Hydrator, Reporter);

            return await Handle(apiHandler);
        }

        [HttpPost]
        public async Task<ActionResult<ApiDeployment>> CreateDeployment(ApiExternalNewDeployment deployment)
        {
            VerifyValidToken(deployment.Token.Value);
            SetUser(deployment.User);

            var creator = new NewDeployment(Context, deployment);
            var apiHandler = new ApiActionHandler(creator, Hydrator, Reporter);

            return await Handle(apiHandler);
        }

        private void SetUser(ApiUser user) {
            if (user != null) {
                CurrentRequestState.SetUser(new User {
                    Name = user.Name,
                    Username = user.Username
                });
            }
        }

        private void VerifyValidToken(string token) {
            if (!ExternalTokenVerifier.IsValid(token)) {
                throw new Exception("Invalid token");
            }
        }
    }
}
