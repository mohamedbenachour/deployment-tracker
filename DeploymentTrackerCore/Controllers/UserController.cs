using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            UserCollection = userCollection;
        }

        private IUserCollection UserCollection { get; }

        [HttpGet]
        public async Task<IEnumerable<ApiUser>> ListUsers() {
            return (await UserCollection.ListUsers()).Select(user => new ApiUser {
                Name = user.Name,
                    Username = user.UserName
            });
        }
    }
}