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

    public class JiraIssueClient {
        private string BaseUrl { get; }
        private LoginInformation Login { get; }

        public JiraIssueClient(string baseUrl, LoginInformation login) {
            BaseUrl = baseUrl;
            Login = login;
        }

        public async Task<JiraIssueDetail> FetchIssueDetail(string jiraIssue) {
            return await GetJiraIssue(jiraIssue);
        }

        private async Task<JiraIssueDetail> GetJiraIssue(string jiraIssue) {
            using (HttpClient client = new HttpClient()) {
                SetAuthentication(client);

                try {
                    var response = await client.GetAsync(GetJiraUrl(jiraIssue));

                    if (!response.IsSuccessStatusCode) {
                        return null;
                    }

                    DefaultContractResolver contractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new CamelCaseNamingStrategy()
                    };

                    var responseBody = await response.Content.ReadAsStringAsync();

                    var jiraDetail = JsonConvert.DeserializeObject<JiraIssueDetail>(responseBody, new JsonSerializerSettings
                    {
                        ContractResolver = contractResolver
                    });

                    return jiraDetail;
                } catch (Exception) {
                    return null;
                }
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