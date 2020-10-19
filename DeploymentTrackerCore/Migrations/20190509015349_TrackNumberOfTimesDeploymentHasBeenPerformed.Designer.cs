﻿// <auto-generated />
using System;

using DeploymentTrackerCore.Models.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DeploymentTrackerCore.Migrations {
    [DbContext(typeof(DeploymentAppContext))]
    [Migration("20190509015349_TrackNumberOfTimesDeploymentHasBeenPerformed")]
    partial class TrackNumberOfTimesDeploymentHasBeenPerformed {
        protected override void BuildTargetModel(ModelBuilder modelBuilder) {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("DeploymentTrackerCore.Models.Deployment", b => {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("BranchName")
                    .IsRequired()
                    .HasMaxLength(200);

                b.Property<int?>("DeployedEnvironmentId");

                b.Property<int>("DeploymentCount");

                b.Property<string>("PublicURL")
                    .IsRequired()
                    .HasMaxLength(150);

                b.Property<byte[]>("RowVersion")
                    .IsConcurrencyToken()
                    .ValueGeneratedOnAddOrUpdate();

                b.Property<string>("SiteName")
                    .IsRequired()
                    .HasMaxLength(100);

                b.Property<string>("Status")
                    .IsRequired();

                b.HasKey("Id");

                b.HasIndex("DeployedEnvironmentId");

                b.ToTable("Deployments");
            });

            modelBuilder.Entity("DeploymentTrackerCore.Models.DeploymentEnvironment", b => {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd();

                b.Property<string>("HostName")
                    .IsRequired()
                    .HasMaxLength(50);

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(50);

                b.Property<byte[]>("RowVersion")
                    .IsConcurrencyToken()
                    .ValueGeneratedOnAddOrUpdate();

                b.HasKey("Id");

                b.ToTable("Environments");
            });

            modelBuilder.Entity("DeploymentTrackerCore.Models.Deployment", b => {
                b.HasOne("DeploymentTrackerCore.Models.DeploymentEnvironment", "DeployedEnvironment")
                    .WithMany("Deployments")
                    .HasForeignKey("DeployedEnvironmentId");

                b.OwnsOne("DeploymentTrackerCore.Models.AuditDetail", "CreatedBy", b1 => {
                    b1.Property<int>("DeploymentId");

                    b1.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150);

                    b1.Property<DateTime>("Timestamp");

                    b1.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b1.HasKey("DeploymentId");

                    b1.ToTable("Deployments");

                    b1.HasOne("DeploymentTrackerCore.Models.Deployment")
                        .WithOne("CreatedBy")
                        .HasForeignKey("DeploymentTrackerCore.Models.AuditDetail", "DeploymentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

                b.OwnsOne("DeploymentTrackerCore.Models.AuditDetail", "ModifiedBy", b1 => {
                    b1.Property<int>("DeploymentId");

                    b1.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(150);

                    b1.Property<DateTime>("Timestamp");

                    b1.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b1.HasKey("DeploymentId");

                    b1.ToTable("Deployments");

                    b1.HasOne("DeploymentTrackerCore.Models.Deployment")
                        .WithOne("ModifiedBy")
                        .HasForeignKey("DeploymentTrackerCore.Models.AuditDetail", "DeploymentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
            });
#pragma warning restore 612, 618
        }
    }
}