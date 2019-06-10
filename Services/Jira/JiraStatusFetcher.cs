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

    public class JiraStatusFetcher {
        private JiraIssueFetcher IssueFetcher { get; }
        private JiraStatusMapper StatusMapper { get; }

        public JiraStatusFetcher(JiraIssueFetcher issueFetcher, JiraStatusMapper statusMapper) {
            IssueFetcher = issueFetcher;
            StatusMapper = statusMapper;
        }

        public async Task<JiraStatus> Fetch(string jiraIssue) {
            if (jiraIssue == null) {
                return JiraStatus.UNKNOWN;
            }

            var jiraIssueDetail = await IssueFetcher.Fetch(jiraIssue);

            if (jiraIssueDetail == null) {
                return JiraStatus.UNKNOWN;
            }

            return StatusMapper.Map(jiraIssueDetail.Fields?.Status?.Id ?? -1);
        }
    }
}