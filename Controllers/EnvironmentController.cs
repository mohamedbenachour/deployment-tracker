using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using deployment_tracker.Models;
using deployment_tracker.Models.API;

using Microsoft.AspNetCore.Mvc.ModelBinding;

using deployment_tracker.Actions.Environment;
using Microsoft.EntityFrameworkCore;
using deployment_tracker.Services.DeploymentManagement;


namespace deployment_tracker.Controllers
{
    [Route("api/environment")]
    [ApiController]
    public class EnvironmentController : Controller
    {
        private DeploymentAppContext Context { get; }
        private IDeploymentManager DeploymentManager { get; }

        public EnvironmentController(DeploymentAppContext context, IDeploymentManager deploymentManager) {
            Context = context;
            DeploymentManager = deploymentManager;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ApiEnvironment>> Environments()
        {
            var environments = new ListEnvironments(Context, DeploymentManager).Fetch();

            return Ok(environments);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteEnvironment(int id) {
            var deletor = new DeleteEnvironment(Context, id);

            await deletor.Delete();

            if (deletor.Succeeded) {
                return Ok();
            }       

            return BadRequest(deletor.Error);
        }

        [HttpPost]
        public async Task<ActionResult<ApiEnvironment>> CreateEnvironment(ApiNewEnvironment environment)
        {
            var creator = new NewEnvironment(Context, environment);

            Console.WriteLine("Testttxs");


            await creator.Create();

            if (creator.Succeeded) {
                return Ok(ApiEnvironment.FromInternal(creator.CreatedEnvironment));
            }

            return BadRequest(creator.Error);
        }
    }
}
