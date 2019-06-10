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
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace deployment_tracker.Services.Jira {

    public class JiraIssueFetcher {
        private string BaseUrl { get; }
        private JiraDetailCache Cache { get; }
        private LoginInformation Login { get; }

        public JiraIssueFetcher(string baseUrl, LoginInformation login, JiraDetailCache cache) {
            BaseUrl = baseUrl;
            Cache = cache;
            Login = login;
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
                SetAuthentication(client);

                string responseBody = await client.GetStringAsync(GetJiraUrl(jiraIssue));

                DefaultContractResolver contractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };

                var jiraDetail = JsonConvert.DeserializeObject<JiraIssueDetail>(responseBody, new JsonSerializerSettings
                {
                    ContractResolver = contractResolver
                });

                return jiraDetail;
            }
        }


        private void SetAuthentication(HttpClient client) {
            if (Login == null) {
                return;
            }

            var byteArray = Encoding.ASCII.GetBytes($"{Login.Username}:{Login.Password}");

            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        private string GetJiraUrl(string jiraIssue)
            => $"{BaseUrl}/rest/api/2/issue/{jiraIssue}";
    }
}