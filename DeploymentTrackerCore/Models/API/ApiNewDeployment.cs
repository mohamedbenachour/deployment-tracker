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
using System.ComponentModel.DataAnnotations;

using DeploymentTrackerCore.Models.Entities;

namespace DeploymentTrackerCore.Models.API {
    public class ApiNewDeployment : IBranchedDeployment, IDeployedSite {

        [Required]
        public string BranchName { get; set; }

        [Required]
        public string SiteName { get; set; }

        [Required]
        public string PublicURL { get; set; }

        public int EnvironmentId { get; set; }

        public Login SiteLogin { get; set; }

        public ApiType Type { get; set; }

        public IDictionary<string, string> Properties { get; set; }

        IIdentifiable IDeployedSite.Type => Type;
    }
}