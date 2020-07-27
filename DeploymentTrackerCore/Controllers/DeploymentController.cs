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

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using DeploymentTrackerCore.Models;
using DeploymentTrackerCore.Models.API;

using DeploymentTrackerCore.Actions.Deployments;

using DeploymentTrackerCore.Services.DeploymentManagement;
using DeploymentTrackerCore.Services;
using DeploymentTrackerCore.Services.Identity;
using DeploymentTrackerCore.Services.Jira;

using DeploymentTrackerCore.Hubs;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.SignalR;

namespace DeploymentTrackerCore.Controllers
{
    [Route("api/deployment")]
    [ApiController]
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class DeploymentController : BaseApiController
    {
        private DeploymentAppContext Context { get; }
        private ApiDeploymentHydrator Hydrator { get; }
        private ReportDeploymentChange Reporter { get; }

        public DeploymentController(
            DeploymentAppContext context,
            IRequestState requestState,
            UserManager<ApplicationUser> userManager,
            IDeploymentManager deploymentManager,
            IJiraService jiraService,
            IHubContext<DeploymentHub, IDeploymentClient> hubContext) : base(requestState, userManager)
        {
            Context = context;
            Hydrator = new ApiDeploymentHydrator(deploymentManager, jiraService);
            Reporter = new ReportDeploymentChange(hubContext);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApiDeployment>>> Deployments()
        {
            var deployments = await new ListDeployments(Context, Hydrator).Fetch();

            return Ok(deployments);
        }

        [HttpPost]
        [Route("destroyed")]
        public async Task<ActionResult<ApiDeployment>> DeploymentDestroyed(ApiDeploymentDestroyed request)
        {
            SetUser();

            var destroyer = new DeploymentDestroyed(Context, request.SiteName);
            var apiHandler = new ApiActionHandler(destroyer, Hydrator, Reporter);

            return await Handle(apiHandler);
        }

        [HttpPost]
        public async Task<ActionResult<ApiDeployment>> CreateDeployment(ApiNewDeployment deployment)
        {
            SetUser();

            var creator = new NewDeployment(Context, deployment);
            var apiHandler = new ApiActionHandler(creator, Hydrator, Reporter);

            return await Handle(apiHandler);
        }
    }
}
