using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using System.Threading.Tasks;

using deployment_tracker.Services;
using deployment_tracker.Services.Identity;
using deployment_tracker.Actions;

namespace deployment_tracker.Controllers {
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