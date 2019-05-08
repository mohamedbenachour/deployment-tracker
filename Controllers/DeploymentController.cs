using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using deployment_tracker.Models;
using deployment_tracker.Models.API;

using deployment_tracker.Actions.Deployments;
using deployment_tracker.Services;

using Microsoft.EntityFrameworkCore;

namespace deployment_tracker.Controllers
{
    [Route("api/deployment")]
    [ApiController]
    public class DeploymentController : Controller
    {
        private DeploymentAppContext Context { get; }
        private IRequestState CurrentRequestState { get; }

        public DeploymentController(DeploymentAppContext context, IRequestState requestState) {
            Context = context;
            CurrentRequestState = requestState;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Deployment>>> Deployments()
        {
            return await Context.Deployments.ToListAsync();
        }

        [HttpPost]
        [Route("destroyed")]
        public async Task<ActionResult<ApiDeployment>> DeploymentDestroyed(ApiDeploymentDestroyed request) {
            SetUser(request.User);
            
            var destroyer = new DeploymentDestroyed(Context, request.SiteName);

            await destroyer.Destroy();

            if (destroyer.Succeeded) {
                return Ok(ApiDeployment.FromInternal(destroyer.DestroyedDeployment));
            }

            return BadRequest(destroyer.Error);
        }

        [HttpPost]
        public async Task<ActionResult<Deployment>> CreateDeployment(ApiNewDeployment deployment)
        {
            SetUser(deployment.User);

            var creator = new NewDeployment(Context, deployment);

            await creator.Create();

            if (creator.Succeeded) {
                return Ok(ApiDeployment.FromInternal(creator.CreatedDeployment));
            }

            return BadRequest(creator.Error);
        }

        private void SetUser(ApiUser user) {
            if (user != null) {
                CurrentRequestState.SetUser(new User {
                    Name = user.Name,
                    Username = user.Username
                });
            }
        }
    }
}
