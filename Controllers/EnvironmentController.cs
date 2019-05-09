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

namespace deployment_tracker.Controllers
{
    [Route("api/environment")]
    [ApiController]
    [Authorize]
    public class EnvironmentController : Controller
    {
        private DeploymentAppContext Context { get; }
        private IDeploymentManager DeploymentManager { get; }

        public EnvironmentController(DeploymentAppContext context, IDeploymentManager deploymentManager, IUserStore<ApplicationUser> userStore) {
            Context = context;
            DeploymentManager = deploymentManager;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ApiEnvironment>> Environments()
        {
            var environments = new ListEnvironments(Context, new ApiDeploymentHydrator(DeploymentManager)).Fetch();

            return Ok(environments);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteEnvironment(int id) {
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
            var creator = new NewEnvironment(Context, environment);

            await creator.Perform();

            if (creator.Succeeded) {
                return Ok(ApiEnvironment.FromInternal(creator.Result));
            }

            return BadRequest(creator.Error);
        }
    }
}
