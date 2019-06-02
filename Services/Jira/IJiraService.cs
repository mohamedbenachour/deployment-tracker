using System.Threading.Tasks;
using System.Threading;
using deployment_tracker.Models;

namespace deployment_tracker.Services.Jira {
    public enum JiraStatus {
        IN_PROGRESS,
        RESOLVED,
        UNKNOWN
    }

    public interface IJiraService {
        string GetJiraUrl(IBranchedDeployment deployment);

        string GetJiraIssue(IBranchedDeployment deployment);

        Task<JiraStatus> GetJiraStatus(IBranchedDeployment deployment);
    }
}