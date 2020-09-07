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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using DeploymentTrackerCore.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DeploymentTrackerCore.Models.Entities {
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

        public DbSet<DeploymentNote> Notes { get; set; }
    }
}