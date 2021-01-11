using Microsoft.EntityFrameworkCore;

namespace DeploymentTrackerCore.Models.Entities {
    public class TableIndexes {
        internal static void DefineIndexes(ModelBuilder modelBuilder) {
            modelBuilder.Entity<UserEntityLink>()
                .HasIndex(userEntityLink => userEntityLink.TargetUserName);

            modelBuilder.Entity<UserEntityLink>()
                .HasIndex(userEntityLink => userEntityLink.IsActive);
        }
    }
}