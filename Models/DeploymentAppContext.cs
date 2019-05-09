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

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

using deployment_tracker.Services;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace deployment_tracker.Models
{
    public class DeploymentAppContext : DbContext
    {
        private IRequestState CurrentRequestState { get; }

        public DeploymentAppContext(DbContextOptions<DeploymentAppContext> options, IRequestState requestState)
            : base(options)
        {
            CurrentRequestState = requestState;
         }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var converter = new EnumToStringConverter<DeploymentStatus>();

            modelBuilder.Entity<Deployment>()
                .Property(d => d.Status)
                .HasConversion(converter);
        }

        private void SetAuditDetails() {
            var applicableEntries = from e in ChangeTracker.Entries()
                           where typeof(IAuditable).IsAssignableFrom(e.Entity.GetType())
                            && (e.State == EntityState.Added
                               || e.State == EntityState.Modified) 
                           select e;

                var auditDetail = new AuditDetail {
                    Timestamp = DateTime.UtcNow,
                    Name = CurrentRequestState.GetUser().Name,
                    UserName = CurrentRequestState.GetUser().Username
                };

            foreach (var entry in applicableEntries)
            {
                var actualEntity = (IAuditable) entry.Entity;
                if (entry.State == EntityState.Added) {
                    actualEntity.SetCreatedBy(auditDetail);
                }

                actualEntity.SetModifiedBy(auditDetail);
            }
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken() )
        {
            SetAuditDetails();
            return await base.SaveChangesAsync( cancellationToken );
        }

        public override int SaveChanges()
        {
            SetAuditDetails();

            return base.SaveChanges();
        }

        public DbSet<DeploymentEnvironment> Environments { get; set; }
        public DbSet<Deployment> Deployments { get; set; }
    }

    public enum DeploymentStatus {
        RUNNING,
        DESTROYED
    }

    public class Deployment : IAuditable {
        [Key]
        public int Id { get; set; }

        public virtual DeploymentEnvironment DeployedEnvironment { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 3)]
        public string BranchName {get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string SiteName { get; set; }

        [Required]
        [StringLength(150, MinimumLength = 3)]
        public string PublicURL { get; set; }

        public DeploymentStatus Status { get; set; }

        public int DeploymentCount { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        [Required]
        public AuditDetail CreatedBy { get; set; }

        public void SetCreatedBy(AuditDetail detail) { CreatedBy = detail; }

        public void SetModifiedBy(AuditDetail detail) { ModifiedBy = detail; }

        [Required]
        public AuditDetail ModifiedBy { get; set; }
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

    public class DeploymentEnvironment {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string HostName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name {get; set; }

        public virtual IList<Deployment> Deployments {get; set;} = new List<Deployment>();

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }

    public interface IAuditable {
        void SetCreatedBy(AuditDetail detail);

        void SetModifiedBy(AuditDetail detail);
    }
}