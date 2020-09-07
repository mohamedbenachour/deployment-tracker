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

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DeploymentTrackerCore.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DeploymentTrackerCore.Models {
    public class DeploymentAppContext : DbContext {
        private IRequestState CurrentRequestState { get; }

        public DeploymentAppContext(DbContextOptions<DeploymentAppContext> options, IRequestState requestState) : base(options) {
            CurrentRequestState = requestState;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            var converter = new EnumToStringConverter<DeploymentStatus>();

            modelBuilder.Entity<Deployment>()
                .Property(d => d.Status)
                .HasConversion(converter);

            modelBuilder.Entity<Type>()
                .HasData(new { Id = 1, Name = "Default" });
        }

        private void SetAuditDetails() {
            var auditTime = DateTime.UtcNow;
            var currentUser = CurrentRequestState.GetUser();
            var applicableEntries = from e in ChangeTracker.Entries()
            where typeof(IAuditable).IsAssignableFrom(e.Entity.GetType()) &&
                (e.State == EntityState.Added ||
                    e.State == EntityState.Modified)
            select e;

            foreach (var entry in applicableEntries) {
                var auditDetail = new AuditDetail {
                    Timestamp = DateTime.UtcNow,
                    Name = CurrentRequestState.GetUser().Name,
                    UserName = CurrentRequestState.GetUser().Username
                };
                var actualEntity = (IAuditable)entry.Entity;
                if (entry.State == EntityState.Added) {
                    actualEntity.SetCreatedBy(auditDetail);
                }

                actualEntity.SetModifiedBy(auditDetail);
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken()) {
            SetAuditDetails();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges() {
            SetAuditDetails();

            return base.SaveChanges();
        }

        public DbSet<DeploymentEnvironment> Environments { get; set; }
        public DbSet<Deployment> Deployments { get; set; }
        public DbSet<Type> Types { get; set; }
    }

    public enum DeploymentStatus {
        RUNNING,
        DESTROYED
    }

    public class Type : IIdentifiable {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(500, MinimumLength = 3)]
        public string TeardownTemplate { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual IList<Deployment> Deployments { get; set; } = new List<Deployment>();

        [StringLength(500, MinimumLength = 3)]
        public string DeploymentTemplate { get; set; }
    }

    public class Deployment : IAuditable, IBranchedDeployment, IDeployedSite {
        [Key]
        public int Id { get; set; }

        public virtual DeploymentEnvironment DeployedEnvironment { get; set; }

        public virtual Type Type { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string BranchName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string SiteName { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string PublicURL { get; set; }

        public DeploymentStatus Status { get; set; }

        public int DeploymentCount { get; set; }

        public Login SiteLogin { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Required]
        public AuditDetail CreatedBy { get; set; }

        public void SetCreatedBy(AuditDetail detail) { CreatedBy = detail; }

        public void SetModifiedBy(AuditDetail detail) { ModifiedBy = detail; }

        [Required]
        public AuditDetail ModifiedBy { get; set; }

        IIdentifiable IDeployedSite.Type => Type;
    }

    [Owned]
    public class AuditDetail {
        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string Name { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; }

        public DateTime Timestamp { get; set; }
    }

    [Owned]
    public class Login {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class DeploymentEnvironment {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string HostName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }

        public virtual IList<Deployment> Deployments { get; set; } = new List<Deployment>();

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

    public interface IAuditable {
        void SetCreatedBy(AuditDetail detail);

        void SetModifiedBy(AuditDetail detail);
    }
}