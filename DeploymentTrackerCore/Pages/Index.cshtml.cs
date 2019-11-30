using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

using DeploymentTrackerCore.Services.Configuration;

namespace DeploymentTrackerCore.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {

        private ConfigurationService ConfigurationService { get; }

        public IndexModel(ConfigurationService configurationService) {
            ConfigurationService = configurationService;
        }

        public void OnGet()
        {
            ViewData["PageData"] = new {
                AllowManualDeploymentsToBeAdded = ConfigurationService.GetAllowManualDeploymentsToBeAdded()
            };
        }
    }
}
