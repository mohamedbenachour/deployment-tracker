using System.Collections.Generic;

using Microsoft.Extensions.Configuration;

using deployment_tracker.Models;

namespace deployment_tracker.Services.Jira {
    public class JiraStatusMapper {
        private IDictionary<int, JiraStatus> StatusMapping { get; }

        public JiraStatusMapper(IDictionary<JiraStatus, ISet<int>> statusMapping) {
            StatusMapping = InvertMapping(statusMapping);
        }

        public JiraStatus Map(int jiraStatusId) {
            if (!StatusMapping.ContainsKey(jiraStatusId)) {
                return JiraStatus.UNKNOWN;
            }

            return StatusMapping[jiraStatusId];
        }

        private IDictionary<int, JiraStatus> InvertMapping(IDictionary<JiraStatus, ISet<int>> statusMapping) {
            var invertedMapping = new Dictionary<int, JiraStatus>(); 

            var inProgressIds = statusMapping[JiraStatus.IN_PROGRESS];

            foreach(var id in inProgressIds) {
                invertedMapping[id] = JiraStatus.IN_PROGRESS;
            }

            var resolvedIds = statusMapping[JiraStatus.RESOLVED];

            foreach(var id in resolvedIds) {
                invertedMapping[id] = JiraStatus.RESOLVED;
            }

            return invertedMapping;
        }
    }

}