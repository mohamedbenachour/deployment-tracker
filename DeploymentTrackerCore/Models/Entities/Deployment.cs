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
using System.ComponentModel.DataAnnotations;

namespace DeploymentTrackerCore.Models.Entities {
    public class Deployment : IAuditable, IBranchedDeployment, IDeployedSite {
        [Key]
        public int Id { get; set; }

        public virtual DeploymentEnvironment DeployedEnvironment { get; set; }

        public virtual Type Type { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string BranchName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string SiteName { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string PublicURL { get; set; }

        public DeploymentStatus Status { get; set; }

        public int DeploymentCount { get; set; }

        public Login SiteLogin { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Required]
        public AuditDetail CreatedBy { get; set; }

        public void SetCreatedBy(AuditDetail detail) { CreatedBy = detail; }

        public void SetModifiedBy(AuditDetail detail) { ModifiedBy = detail; }

        [Required]
        public AuditDetail ModifiedBy { get; set; }

        IIdentifiable IDeployedSite.Type => Type;
    }
}