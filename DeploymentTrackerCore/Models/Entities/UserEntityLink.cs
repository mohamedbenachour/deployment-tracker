using System;
using System.ComponentModel.DataAnnotations;

namespace DeploymentTrackerCore.Models.Entities {
    public class UserEntityLink : IIdentifiable, IAuditable {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        [Required]
        public string TargetUserName { get; set; }

        [StringLength(200)]
        [Required]
        public string ReferencedEntity { get; set; }

        public bool IsActive { get; set; }

        public AuditDetail CreatedBy { get; set; }
        public AuditDetail ModifiedBy { get; set; }

        public void SetCreatedBy(AuditDetail detail) {
            CreatedBy = detail;
        }

        public void SetModifiedBy(AuditDetail detail) {
            ModifiedBy = detail;
        }
    }
}