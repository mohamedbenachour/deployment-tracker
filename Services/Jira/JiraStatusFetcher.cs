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