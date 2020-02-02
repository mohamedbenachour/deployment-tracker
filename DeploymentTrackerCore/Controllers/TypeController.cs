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
using Microsoft.AspNetCore.Mvc;
using DeploymentTrackerCore.Models;
using DeploymentTrackerCore.Models.API;
using Microsoft.AspNetCore.Authorization;

using DeploymentTrackerCore.Actions.Environment;
using Microsoft.AspNetCore.Identity;
using DeploymentTrackerCore.Services.Identity;
using DeploymentTrackerCore.Services;
using DeploymentTrackerCore.Actions.Type;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DeploymentTrackerCore.Controllers
{
    [Route("api/type")]
    [ApiController]
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class TypeController : BaseApiController
    {
        private DeploymentAppContext Context { get; }
        private NewTypeFromApi NewTypeFromApi { get; }

        public TypeController(
            DeploymentAppContext context,
            IRequestState requestState,
            UserManager<ApplicationUser> userManager,
            NewTypeFromApi newTypeFromApi
        ) : base(requestState, userManager) {
            Context = context;
            NewTypeFromApi = newTypeFromApi;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApiType>>> Types()
        {
            var types = await Context.Types.ToListAsync();

            return Ok(types.Select(ApiType.FromInternal));
        }

        [HttpPost]
        public async Task<ActionResult<ApiType>> CreateType(ApiNewType type)
        {
            SetUser();

            return await Handle(NewTypeFromApi, type);
        }
    }
}
