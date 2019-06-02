using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace deployment_tracker.Services.Jira {

    public class JiraIssueFetcher {
        private string BaseUrl { get; }
        private JiraDetailCache Cache { get; }

        public JiraIssueFetcher(string baseUrl, JiraDetailCache cache) {
            BaseUrl = baseUrl;
            Cache = cache;
        }

        public async Task<JiraIssueDetail> Fetch(string jiraIssue) {
            var jiraDetail = Cache.Get(jiraIssue);

            if (jiraDetail == null) {
                jiraDetail = await GetJiraIssue(jiraIssue);

                Cache.Store(jiraIssue, jiraDetail);
            }

            return jiraDetail;
        }

        private async Task<JiraIssueDetail> GetJiraIssue(string jiraIssue) {
            using (HttpClient client = new HttpClient()) {
                string responseBody = await client.GetStringAsync(GetJiraUrl(jiraIssue));

                DefaultContractResolver contractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };

                var jiraDetail = JsonConvert.DeserializeObject<JiraIssueDetail>(responseBody, new JsonSerializerSettings
                {
                    ContractResolver = contractResolver
                });

                //Console.WriteLine($"{jiraDetail.Fields.Status.Name}{jiraDetail.Fields.Status.Id}");

                return jiraDetail;
            }
        }

        private string GetJiraUrl(string jiraIssue)
            => $"{BaseUrl}/rest/api/2/issue/{jiraIssue}";
    }
}