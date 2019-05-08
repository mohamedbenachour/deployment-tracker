using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using deployment_tracker.Models;
using deployment_tracker.Models.API;

using deployment_tracker.Actions.Deployments;
using deployment_tracker.Services;
using deployment_tracker.Services.Identity;

using Microsoft.AspNetCore.Authorization;


using Microsoft.EntityFrameworkCore;

namespace deployment_tracker.Controllers
{
    [Route("api/deployment")]
    [ApiController]
    [Authorize]
    public class DeploymentController : Controller
    {
        private DeploymentAppContext Context { get; }
        private IRequestState CurrentRequestState { get; }
        private UserManager<ApplicationUser> Users { get; }

        public DeploymentController(DeploymentAppContext context, IRequestState requestState, UserManager<ApplicationUser> userManager) {
            Context = context;
            CurrentRequestState = requestState;
            Users = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Deployment>>> Deployments()
        {
            return await Context.Deployments.ToListAsync();
        }

        [HttpPost]
        [Route("destroyed")]
        public async Task<ActionResult<ApiDeployment>> DeploymentDestroyed(ApiDeploymentDestroyed request) {
            await SetUser();
            
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
            await SetUser();

            var creator = new NewDeployment(Context, deployment);

            await creator.Create();

            if (creator.Succeeded) {
                return Ok(ApiDeployment.FromInternal(creator.CreatedDeployment));
            }

            return BadRequest(creator.Error);
        }

        private async Task SetUser() {
            var user = HttpContext.User;
            if (user != null) {
                var resolvedUser = await Users.GetUserAsync(user);
                CurrentRequestState.SetUser(new User {
                    Name = resolvedUser.Name,
                    Username = resolvedUser.UserName
                });
            }
        }
    }
}
