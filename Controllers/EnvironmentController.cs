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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using deployment_tracker.Models;
using deployment_tracker.Models.API;
using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using deployment_tracker.Actions.Environment;
using deployment_tracker.Actions.Deployments;
using Microsoft.EntityFrameworkCore;
using deployment_tracker.Services.DeploymentManagement;
using Microsoft.AspNetCore.Identity;
using deployment_tracker.Services.Identity;
using deployment_tracker.Services.Jira;
using deployment_tracker.Services;

namespace deployment_tracker.Controllers
{
    [Route("api/environment")]
    [ApiController]
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class EnvironmentController : BaseApiController
    {
        private DeploymentAppContext Context { get; }
        private IDeploymentManager DeploymentManager { get; }
        private IJiraService JiraService { get; }

        public EnvironmentController(DeploymentAppContext context,
        IDeploymentManager deploymentManager,
        IRequestState requestState,
        UserManager<ApplicationUser> userManager,
        IJiraService jiraService,
        IUserStore<ApplicationUser> userStore) : base(requestState, userManager) {
            Context = context;
            DeploymentManager = deploymentManager;
            JiraService = jiraService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApiEnvironment>>> Environments()
        {
            var environments = await new ListEnvironments(Context, new ApiDeploymentHydrator(DeploymentManager, JiraService)).Fetch();

            return Ok(environments);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteEnvironment(int id) {
            await SetUser();

            var deletor = new DeleteEnvironment(Context, id);

            await deletor.Perform();

            if (deletor.Succeeded) {
                return Ok();
            }       

            return BadRequest(deletor.Error);
        }

        [HttpPost]
        public async Task<ActionResult<ApiEnvironment>> CreateEnvironment(ApiNewEnvironment environment)
        {
            await SetUser();

            var creator = new NewEnvironment(Context, environment);

            await creator.Perform();

            if (creator.Succeeded) {
                return Ok(ApiEnvironment.FromInternal(creator.Result));
            }

            return BadRequest(creator.Error);
        }
    }
}
