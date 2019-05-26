using deployment_tracker.Models;

namespace deployment_tracker.Services.Jira {
    public enum JiraStatus {
        IN_PROGRESS,
        RESOLVED
    }

    public interface IJiraService {
        string GetJiraUrl(IBranchedDeployment deployment);

        string GetJiraIssue(IBranchedDeployment deployment);
    }
}