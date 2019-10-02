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
 
using deployment_tracker.Actions;
using deployment_tracker.Models;
using deployment_tracker.Models.API;

using System;
using System.Threading.Tasks;

using deployment_tracker.Hubs;
using deployment_tracker.Services.Jira;

using Microsoft.AspNetCore.SignalR;

namespace deployment_tracker.Actions.Jira {
    public class ReportJiraStatusChange {
        private IHubContext<JiraHub, IJiraClient> Hub { get; }

        public ReportJiraStatusChange(IHubContext<JiraHub, IJiraClient> hub) {
            Hub = hub;
        }

        public async Task Report(string jiraIssue, JiraStatus jiraStatus) {
            await Hub.Clients.All.JiraStatusChange(jiraIssue, jiraStatus.ToString());
        }
    }
}