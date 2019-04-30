using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using deployment_tracker.Models;
using deployment_tracker.Models.API;

using deployment_tracker.Actions.Deployments;

using Microsoft.EntityFrameworkCore;

namespace deployment_tracker.Controllers
{
    [Route("api/deployment")]
    [ApiController]
    public class DeploymentController : Controller
    {
        private DeploymentAppContext Context { get; }

        public DeploymentController(DeploymentAppContext context) {
            Context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Deployment>>> Deployments()
        {
            return await Context.Deployments.ToListAsync();
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> UpdateStatus([FromQuery] DeploymentStatus newStatus) {
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<Deployment>> CreateDeployment(ApiNewDeployment deployment)
        {
            var creator = new NewDeployment(Context, deployment);

            await creator.Create();

            if (creator.Succeeded) {
                return Ok(ApiDeployment.FromInternal(creator.CreatedDeployment));
            }

            return BadRequest(creator.Error);
        }
    }
}
