using System;

using DeploymentTrackerCore.Models.Entities;

namespace DeploymentTrackerCore.Services.UserEntityLinks {
    public class EntityLinks {
        public int Id { get; set; }

        public string ReferencedEntity { get; set; }

        public AuditDetail CreatedBy { get; set; }

        public AuditDetail ModifiedBy { get; set; }
    }
}