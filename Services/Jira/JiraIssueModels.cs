namespace deployment_tracker.Services.Jira {
    public class JiraIssueDetail {
        public JiraIssueFields Fields { get; set; }
    }

    public class JiraIssueFields {
        public JiraIssueStatus Status { get; set; }
    }

    public class JiraIssueStatus {
        public string Name { get; set; }
        public int Id { get; set; }
    }
}