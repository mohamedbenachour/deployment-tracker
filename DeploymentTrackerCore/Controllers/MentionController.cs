using System.Collections.Generic;
using System.Threading.Tasks;

using DeploymentTrackerCore.Actions;
using DeploymentTrackerCore.Services;
using DeploymentTrackerCore.Services.Identity;
using DeploymentTrackerCore.Services.UserEntityLinks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DeploymentTrackerCore.Controllers {
    [Route("api/mention")]
    [ApiController]
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class MentionController : BaseApiController {
        public MentionController(
            IUserEntityLinksService userEntityLinksService,
            IRequestState requestState,
            UserManager<ApplicationUser> userManager) : base(requestState, userManager) {
            UserEntityLinksService = userEntityLinksService;
        }

        private IUserEntityLinksService UserEntityLinksService { get; }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EntityLinks>>> GetCurrentMentions() => await Handle(() => UserEntityLinksService.FetchEntityLinksForCurrentUser());
    }
}