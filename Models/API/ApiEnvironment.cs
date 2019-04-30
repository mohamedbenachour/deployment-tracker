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
using System.Linq;

using deployment_tracker.Models;

namespace deployment_tracker.Models.API {
    public class ApiEnvironment {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HostName { get; set; }

        public IList<ApiDeployment> Deployments { get; set; }

        public static ApiEnvironment FromInternal(DeploymentEnvironment toConvert) {
            var rval = new ApiEnvironment {
                Id = toConvert.Id,
                Name = toConvert.Name,
                HostName = toConvert.HostName,
                Deployments = toConvert.Deployments != null ? toConvert.Deployments.Select(ApiDeployment.FromInternal).ToList() : new List<ApiDeployment>()
            };

            return rval;
        }
    }
}