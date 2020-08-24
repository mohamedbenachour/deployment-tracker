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

using DeploymentTrackerCore.Services.Configuration;
using DeploymentTrackerCore.Services.Identity;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeploymentTrackerCore.Pages {
    [Authorize]
    public class IndexModel : PageModel {

        private ConfigurationService ConfigurationService { get; }

        public IndexModel (ConfigurationService configurationService) {
            ConfigurationService = configurationService;
        }

        public void OnGet () {
            var user = ApplicationUser.FromClaimsPrincipal (HttpContext.User);

            ViewData["PageData"] = new {
                AllowManualDeploymentsToBeAdded = ConfigurationService.GetAllowManualDeploymentsToBeAdded (),
                User = new {
                UserName = user.UserName,
                Email = user.Email
                }
            };
        }
    }
}