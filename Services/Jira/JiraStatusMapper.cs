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

using Microsoft.Extensions.Configuration;

using deployment_tracker.Models;

namespace deployment_tracker.Services.Jira {
    public class JiraStatusMapper {
        private IDictionary<int, JiraStatus> StatusMapping { get; }

        public JiraStatusMapper(IDictionary<JiraStatus, ISet<int>> statusMapping) {
            StatusMapping = InvertMapping(statusMapping);
        }

        public JiraStatus Map(int jiraStatusId) {
            if (!StatusMapping.ContainsKey(jiraStatusId)) {
                return JiraStatus.UNKNOWN;
            }

            return StatusMapping[jiraStatusId];
        }

        private IDictionary<int, JiraStatus> InvertMapping(IDictionary<JiraStatus, ISet<int>> statusMapping) {
            var invertedMapping = new Dictionary<int, JiraStatus>(); 

            var inProgressIds = statusMapping[JiraStatus.IN_PROGRESS];

            foreach(var id in inProgressIds) {
                invertedMapping[id] = JiraStatus.IN_PROGRESS;
            }

            var completedIds = statusMapping[JiraStatus.COMPLETED];

            foreach(var id in completedIds) {
                invertedMapping[id] = JiraStatus.COMPLETED;
            }

            return invertedMapping;
        }
    }

}