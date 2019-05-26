using System.Text.RegularExpressions;

namespace deployment_tracker.Services.Jira {

    public class JiraIssueKeyExtractor {
        private string ProjectKey { get; }
        private string IssueKeyRegex { get; }

        public JiraIssueKeyExtractor(string projectKey) {
            ProjectKey = projectKey.ToUpper();
            IssueKeyRegex = $"{ProjectKey}-[0-9]+";
        }

        public string Extract(string source) {
            var match = Regex.Match(source.ToUpper(), IssueKeyRegex);

            if (match.Success) {
                return match.Value;
            }

            return null;
        }
    }

}