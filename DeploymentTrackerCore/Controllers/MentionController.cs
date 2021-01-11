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

        [HttpDelete]
        [Route("{mentionId}")]
        public async Task<ActionResult<IEnumerable<EntityLinks>>> AcknowledgeMention(int mentionId) => await Handle(() => UserEntityLinksService.MakeInactive(mentionId));
    }
}