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
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;

using System;

using deployment_tracker.Models;
using deployment_tracker.Hubs;
using deployment_tracker.Actions.Jira;

namespace deployment_tracker.Services.Jira {
    public class JiraService : IJiraService {
        private JiraConfiguration Configuration { get; }
        private JiraStatusMapper StatusMapper { get; }
        private JiraDetailCache Cache { get; }

        private JiraIssueUpdater Updater { get; }

        public JiraService(IConfiguration configuration, ILogger<JiraService> logger, IHubContext<JiraHub, IJiraClient> hubContext) {
            Configuration = new JiraConfiguration(configuration, logger);

            if (Configuration.Enabled) {
                StatusMapper = new JiraStatusMapper(Configuration.StatusMapping);
                Cache = new JiraDetailCache();
                Updater = new JiraIssueUpdater(GetClient(), Cache, GetStatusExtractor(), new ReportJiraStatusChange(hubContext));

                Updater.Start();
            }
        }

        public string GetJiraUrl(IBranchedDeployment deployment) {
            if (!Configuration.Enabled) {
                return null;
            }

            var jiraIssue = GetJiraIssue(deployment);

            if (jiraIssue != null) {
                return $"{Configuration.BaseUrl}/browse/{GetJiraIssue(deployment)}";
            }

            return null;
        }

        public string GetJiraIssue(IBranchedDeployment deployment) {
            if (!Configuration.Enabled) {
                return null;
            }

            return GetJiraKey(deployment.BranchName);
        }

        public async Task<JiraStatus> GetJiraStatus(IBranchedDeployment deployment) {
            if (!Configuration.Enabled) {
                return JiraStatus.UNKNOWN;
            }

            var jiraIssue = GetJiraIssue(deployment);

            return await new JiraStatusFetcher(GetFetcher(), GetStatusExtractor()).Fetch(jiraIssue);
        }

        private string GetJiraKey(string source)
            => new JiraIssueKeyExtractor(Configuration.SiteProjectKey).Extract(source);

        private JiraIssueFetcher GetFetcher()
            => new JiraIssueFetcher(GetClient(), Cache, Updater);

        private JiraIssueClient GetClient()
            => new JiraIssueClient(Configuration.BaseUrl, Configuration.JiraLogin);

        private JiraStatusExtractor GetStatusExtractor()
            => new JiraStatusExtractor(StatusMapper);
    }

    class JiraConfiguration {
        public JiraConfiguration(IConfiguration configuration, ILogger logger) {
            var jiraConfiguration = configuration.GetSection("Jira");

            Enabled = Boolean.Parse(jiraConfiguration[nameof(Enabled)]);

            if (!Enabled) {
                return;
            }

            logger.LogInformation("Jira integration is enabled");

            BaseUrl = jiraConfiguration[nameof(BaseUrl)];

            if (!BaseUrl.StartsWith("https")) {
                logger.LogError("[SECURITY-ISSUE] Jira URL does not use HTTPS");
            }

            SiteProjectKey = jiraConfiguration[nameof(SiteProjectKey)];

            var userConfiguration = jiraConfiguration.GetSection("User");

            var username = userConfiguration["UserName"];
            var password = userConfiguration["Password"];

            if (!String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(password)) {
                logger.LogInformation($"Using user '{username}' for Jira integration");

                JiraLogin = new LoginInformation {
                    Username = username,
                    Password = password
                };
            }

            var jiraStatusMapping = jiraConfiguration.GetSection("StatusIdMapping");

            var completedList = new HashSet<int>();
            var inProgressList = new HashSet<int>();

            jiraStatusMapping.GetSection("COMPLETED").Bind(completedList);
            jiraStatusMapping.GetSection("IN_PROGRESS").Bind(inProgressList);

            StatusMapping[JiraStatus.COMPLETED] = completedList;
            StatusMapping[JiraStatus.IN_PROGRESS] = inProgressList;
        }

        public bool Enabled { get; private set; }
        public string BaseUrl { get; private set; }
        public string SiteProjectKey { get; private set; }
        public LoginInformation JiraLogin { get; private set; }
        public IDictionary<JiraStatus, ISet<int>> StatusMapping { get; } = new Dictionary<JiraStatus, ISet<int>>();
    }
}