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

using System.Threading.Tasks;

namespace DeploymentTrackerCore.Services.Jira
{

    public class JiraIssueFetcher {
        private JiraDetailCache Cache { get; }
        private JiraIssueClient Client { get; }

        private JiraIssueUpdater Updater { get; }

        public JiraIssueFetcher(JiraIssueClient client, JiraDetailCache cache, JiraIssueUpdater updater) {
            Client = client;
            Cache = cache;
            Updater = updater;
        }

        public async Task<JiraIssueDetail> Fetch(string jiraIssue) {
            var jiraDetail = Cache.Get(jiraIssue);

            if (jiraDetail == null) {
                jiraDetail = await Client.FetchIssueDetail(jiraIssue);

                Cache.Store(jiraIssue, jiraDetail);
            }

            Updater.AddIssueToManage(jiraIssue);

            return jiraDetail;
        }
    }
}