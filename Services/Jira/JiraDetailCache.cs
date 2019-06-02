using System.Threading.Tasks;
using System.Threading;
using System;

using Microsoft.Extensions.Caching.Memory;

using deployment_tracker.Models;

namespace deployment_tracker.Services.Jira {
    public class JiraDetailCache {
        private MemoryCache Cache { get; }

        public JiraDetailCache() {
            Cache = new MemoryCache(new MemoryCacheOptions());
        }

        public void Store(string jiraIssue, JiraIssueDetail detail) {
            Cache.Set(jiraIssue, detail, new MemoryCacheEntryOptions {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            });
        }

        public JiraIssueDetail Get(string jiraIssue) {
            if (!Cache.TryGetValue(jiraIssue, out JiraIssueDetail detail)) {
                return null;
            }

            return detail;
        }
    }
}