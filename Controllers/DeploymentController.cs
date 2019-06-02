using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using deployment_tracker.Models;
using deployment_tracker.Models.API;

using deployment_tracker.Actions;

using deployment_tracker.Actions.Deployments;

using deployment_tracker.Services.DeploymentManagement;
using deployment_tracker.Services;
using deployment_tracker.Services.Identity;
using deployment_tracker.Services.Jira;

using deployment_tracker.Hubs;

using Microsoft.AspNetCore.Authorization;

using Microsoft.AspNetCore.SignalR;

using Microsoft.EntityFrameworkCore;

namespace deployment_tracker.Controllers
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
            IHubContext<DeploymentHub, IDeploymentClient> hubContext) : base(requestState, userManager) {
            Context = context;
            Hydrator = new ApiDeploymentHydrator(deploymentManager, jiraService);
            Reporter = new ReportDeploymentChange(hubContext);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Deployment>>> Deployments()
        {
            var deployments = await new ListDeployments(Context, Hydrator).Fetch();

            return Ok(deployments);
        }

        [HttpPost]
        [Route("destroyed")]
        public async Task<ActionResult<ApiDeployment>> DeploymentDestroyed(ApiDeploymentDestroyed request) {
            await SetUser();
            
            var destroyer = new DeploymentDestroyed(Context, request.SiteName);
            var apiHandler = new ApiActionHandler(destroyer, Hydrator, Reporter);

            return await Handle(apiHandler);
        }

        [HttpPost]
        public async Task<ActionResult<ApiDeployment>> CreateDeployment(ApiNewDeployment deployment)
        {
            await SetUser();

            var creator = new NewDeployment(Context, deployment);
            var apiHandler = new ApiActionHandler(creator, Hydrator, Reporter);

            return await Handle(apiHandler);
        }
    }
}
