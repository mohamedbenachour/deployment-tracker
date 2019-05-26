using deployment_tracker.Models;

using Microsoft.Extensions.Configuration;

namespace deployment_tracker.Services.Jira {
    public class JiraService : IJiraService {
        private JiraConfiguration Configuration { get; }

        public JiraService(IConfiguration configuration) {
            Configuration = new JiraConfiguration(configuration);
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

        private string GetJiraKey(string source)
            => new JiraIssueKeyExtractor(Configuration.SiteProjectKey).Extract(source);
    }

    class JiraConfiguration {
        public JiraConfiguration(IConfiguration configuration) {
            var jiraConfiguration = configuration.GetSection("Jira");

            BaseUrl = jiraConfiguration[nameof(BaseUrl)];
            SiteProjectKey = jiraConfiguration[nameof(SiteProjectKey)];
        }

        public string BaseUrl { get; private set; }
        public string SiteProjectKey { get; private set; }
    }
}