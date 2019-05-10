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
using deployment_tracker.Services.Identity;
using deployment_tracker.Services.DeploymentManagement;
using deployment_tracker.Services.Token;
using deployment_tracker.Hubs;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

using Microsoft.EntityFrameworkCore;

namespace deployment_tracker.Controllers
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
            IHubContext<DeploymentHub, IDeploymentClient> hubContext) : base(requestState, null) {
            Context = context;
            CurrentRequestState = requestState;
            ExternalTokenVerifier = tokenVerifier;
            Hydrator = new ApiDeploymentHydrator(deploymentManager);
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
