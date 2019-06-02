using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System;

using deployment_tracker.Models;

namespace deployment_tracker.Services.Jira {
    public class JiraService : IJiraService {
        private JiraConfiguration Configuration { get; }
        private JiraStatusMapper StatusMapper { get; }
        private JiraDetailCache Cache { get; }

        public JiraService(IConfiguration configuration, ILogger<JiraService> logger) {
            Configuration = new JiraConfiguration(configuration, logger);
            StatusMapper = new JiraStatusMapper(Configuration.StatusMapping);
            Cache = new JiraDetailCache();
        }

        public string GetJiraUrl(IBranchedDeployment deployment) {
            var jiraIssue = GetJiraIssue(deployment);

            if (jiraIssue != null) {
                return $"{Configuration.BaseUrl}/browse/{GetJiraIssue(deployment)}";
            }

            return null;
        }

        public string GetJiraIssue(IBranchedDeployment deployment)
            => GetJiraKey(deployment.BranchName);

        public async Task<JiraStatus> GetJiraStatus(IBranchedDeployment deployment) {
            var jiraIssue = GetJiraIssue(deployment);

            return await new JiraStatusFetcher(GetFetcher(), StatusMapper).Fetch(jiraIssue);
        }

        private string GetJiraKey(string source)
            => new JiraIssueKeyExtractor(Configuration.SiteProjectKey).Extract(source);

        private JiraIssueFetcher GetFetcher()
            => new JiraIssueFetcher(Configuration.BaseUrl, Configuration.JiraLogin, Cache);
    }

    class JiraConfiguration {
        public JiraConfiguration(IConfiguration configuration, ILogger logger) {
            var jiraConfiguration = configuration.GetSection("Jira");

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

            var resolvedList = new HashSet<int>();
            var inProgressList = new HashSet<int>();

            jiraStatusMapping.GetSection("RESOLVED").Bind(resolvedList);
            jiraStatusMapping.GetSection("IN_PROGRESS").Bind(inProgressList);

            StatusMapping[JiraStatus.RESOLVED] = resolvedList;
            StatusMapping[JiraStatus.IN_PROGRESS] = inProgressList;
        }

        public string BaseUrl { get; private set; }
        public string SiteProjectKey { get; private set; }
        public LoginInformation JiraLogin { get; private set; }
        public IDictionary<JiraStatus, ISet<int>> StatusMapping { get; } = new Dictionary<JiraStatus, ISet<int>>();
    }
}