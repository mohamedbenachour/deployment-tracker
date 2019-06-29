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
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace deployment_tracker.Services.Jira {

    public class JiraStatusExtractor {
        private JiraStatusMapper StatusMapper { get; }

        public JiraStatusExtractor(JiraStatusMapper statusMapper) {
            StatusMapper = statusMapper;
        }

        public JiraStatus GetStatus(JiraIssueDetail issueDetail) {
            if (issueDetail == null) {
                return JiraStatus.UNKNOWN;
            }

            return StatusMapper.Map(issueDetail.Fields?.Status?.Id ?? -1);
        }
    }
}