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
using System.Threading;
using System;

using System.Collections.Concurrent;
using System.Collections.Generic;

using deployment_tracker.Actions.Jira;

using deployment_tracker.Models;

namespace deployment_tracker.Services.Jira {
    public class JiraIssueUpdater : IDisposable {
        private JiraDetailCache Cache { get; }

        private JiraIssueClient Client { get; }

        private BlockingCollection<string> JiraIssuesToUpdate { get; }

        private ISet<string> JiraIssuesToManage { get; }

        private ReportJiraStatusChange JiraStatusChangeReporter { get; }

        private JiraStatusExtractor StatusExtractor { get; }

        private bool HasStarted { get; set; } = false;
        private Timer QueueTimer { get; set; }

        public JiraIssueUpdater(JiraIssueClient client, JiraDetailCache cache, JiraStatusExtractor statusExtractor, ReportJiraStatusChange reporter) {
            Cache = cache;
            Client = client;
            JiraStatusChangeReporter = reporter;
            StatusExtractor = statusExtractor;
            JiraIssuesToUpdate = new BlockingCollection<string>();
            JiraIssuesToManage = new HashSet<string>();
        }

        public void AddIssueToManage(string jiraIssue) {
            JiraIssuesToManage.Add(jiraIssue);
        }

        public void Start() {
            if (HasStarted) {
                return;
            }

            HasStarted = true;

            QueueTimer = new Timer(InternalQueueCycle, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
            ThreadPool.QueueUserWorkItem(InternalUpdateCycle);
        }

        public void Dispose() {
            QueueTimer.Dispose();
        }

        private void InternalQueueCycle(object _) {
            foreach (var jiraIssue in JiraIssuesToManage) {
                JiraIssuesToUpdate.Add(jiraIssue);
            }
        }

        private void InternalUpdateCycle(object _) {
            while(true) {
                var jiraIssue = JiraIssuesToUpdate.Take();

                var issueDetail = Client.FetchIssueDetail(jiraIssue).Result;

                ProcessNewIssueDetail(jiraIssue, issueDetail);
            }
        }

        private void ProcessNewIssueDetail(string jiraIssue, JiraIssueDetail issueDetail) {
            var newStatus = StatusExtractor.GetStatus(issueDetail);
            var currentStatus = JiraStatus.UNKNOWN;
            var currentIssueDetail = Cache.Get(jiraIssue);

            if (currentIssueDetail != null) {
                currentStatus = StatusExtractor.GetStatus(currentIssueDetail);
            }

            if (newStatus != currentStatus) {
                JiraStatusChangeReporter.Report(jiraIssue, newStatus);
            }

            Cache.Store(jiraIssue, issueDetail);
        }
    }
}