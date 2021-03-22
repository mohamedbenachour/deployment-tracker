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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using DeploymentTrackerCore.Models.Entities;

namespace DeploymentTrackerCore.Models.API {
    public class ApiDeployment : IBranchedDeployment, IDeployedSite {
        public int Id { get; set; }
        public string BranchName { get; set; }
        public string PublicURL { get; set; }

        public string SiteName { get; set; }
        public int EnvironmentId { get; set; }
        public string Status { get; set; }
        public string TeardownUrl { get; set; }

        public ApiType Type { get; set; }

        public JiraInformation Jira { get; set; }

        public Login SiteLogin { get; set; }

        public AuditDetail ModifiedBy { get; set; }
        public AuditDetail CreatedBy { get; set; }
        public ApiDeploymentManagementUrls ManagementUrls { get; set; }

        public IDictionary<string, string> Properties { get; set; }

        public bool HasNotes { get; set; }

        IIdentifiable IDeployedSite.Type => Type;

        public static ApiDeployment FromInternal(Deployment toConvert) {
            return new ApiDeployment {
                Id = toConvert.Id,
                    BranchName = toConvert.BranchName,
                    PublicURL = toConvert.PublicURL,
                    EnvironmentId = toConvert.DeployedEnvironment.Id,
                    SiteName = toConvert.SiteName,
                    Status = toConvert.Status.ToString(),
                    CreatedBy = toConvert.CreatedBy,
                    ModifiedBy = toConvert.ModifiedBy,
                    SiteLogin = toConvert.SiteLogin,
                    Type = ApiType.FromInternal(toConvert.Type),
                    HasNotes = toConvert.DeploymentNotes?.Any(note => note.IsActive) ?? false,
                    Properties = String.IsNullOrEmpty(toConvert.Properties) ?
                    new Dictionary<string, string>() :
                    JsonSerializer.Deserialize<Dictionary<string, string>>(toConvert.Properties)
            };
        }
    }
}