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
 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using System.Threading.Tasks;

using DeploymentTrackerCore.Services;
using DeploymentTrackerCore.Services.Identity;
using DeploymentTrackerCore.Actions;

namespace DeploymentTrackerCore.Controllers {
    public abstract class BaseApiController : Controller {
        private IRequestState CurrentRequestState { get; }
        private UserManager<ApplicationUser> Users { get; }

        public BaseApiController(IRequestState requestState, UserManager<ApplicationUser> userManager) {
            CurrentRequestState = requestState;
            Users = userManager;
        }

        protected async Task<ActionResult<T>> Handle<T>(IActionPerformer<T> performer) {
            await performer.Perform();

            if (performer.Succeeded) {
                return Ok(performer.Result);
            }

            return BadRequest(performer.Error);
        }

        protected async Task SetUser() {
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