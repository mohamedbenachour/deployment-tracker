using DeploymentTrackerCore.Models.Entities;

namespace DeploymentTrackerCore.Models.API.DeploymentNotes {
    public class ApiNote {
        public int Id { get; set; }
        public int DeploymentId { get; set; }
        public string Content { get; set; }

        public AuditDetail CreatedBy {get; set;}

    }
}