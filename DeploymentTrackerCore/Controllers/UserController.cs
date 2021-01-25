using System.Collections.Generic;
using System.Linq;

using DeploymentTrackerCore.Models.API;
using DeploymentTrackerCore.Services.Identity;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeploymentTrackerCore.Controllers {
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController {
        public UserController(IUserCollection userCollection) {
            this.UserCollection = userCollection;
        }

        private IUserCollection UserCollection { get; }

        [HttpGet]
        public IEnumerable<ApiUser> ListUsers() {
            return UserCollection.ListUsers().Select(user => new ApiUser {
                Name = user.Name,
                    Username = user.UserName
            });
        }
    }
}