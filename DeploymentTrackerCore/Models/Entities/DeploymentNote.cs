using System.ComponentModel.DataAnnotations;

namespace DeploymentTrackerCore.Models.Entities {
    public class DeploymentNote : IAuditable {

        [Key]
        public int Id { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Required]
        public virtual Deployment Deployment { get; set; }

        [Required]
        [StringLength(500)]
        public string Content { get; set; }

        public bool IsActive { get; set; }

        [Required]
        public AuditDetail CreatedBy { get; set; }

        public void SetCreatedBy(AuditDetail detail) { CreatedBy = detail; }

        public void SetModifiedBy(AuditDetail detail) { ModifiedBy = detail; }

        [Required]
        public AuditDetail ModifiedBy { get; set; }
    }
}