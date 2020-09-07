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

namespace DeploymentTrackerCore.Models.Entities {
    public class Type : IIdentifiable {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(500, MinimumLength = 3)]
        public string TeardownTemplate { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual IList<Deployment> Deployments { get; set; } = new List<Deployment>();

        [StringLength(500, MinimumLength = 3)]
        public string DeploymentTemplate { get; set; }
    }
}