﻿// <auto-generated />
using System;

using DeploymentTrackerCore.Models.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DeploymentTrackerCore.Migrations {
    [DbContext(typeof(DeploymentAppContext))]
    [Migration("20200720051227_TypeTeardownTemplate")]
    partial class TypeTeardownTemplate {
        protected override void BuildTargetModel(ModelBuilder modelBuilder) {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5");

            modelBuilder.Entity("DeploymentTrackerCore.Models.Deployment", b => {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<string>("BranchName")
                    .IsRequired()
                    .HasColumnType("TEXT")
                    .HasMaxLength(200);

                b.Property<int?>("DeployedEnvironmentId")
                    .HasColumnType("INTEGER");

                b.Property<int>("DeploymentCount")
                    .HasColumnType("INTEGER");

                b.Property<string>("PublicURL")
                    .IsRequired()
                    .HasColumnType("TEXT")
                    .HasMaxLength(150);

                b.Property<byte[]>("RowVersion")
                    .IsConcurrencyToken()
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnType("BLOB");

                b.Property<string>("SiteName")
                    .IsRequired()
                    .HasColumnType("TEXT")
                    .HasMaxLength(100);

                b.Property<string>("Status")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<int?>("TypeId")
                    .HasColumnType("INTEGER");

                b.HasKey("Id");

                b.HasIndex("DeployedEnvironmentId");

                b.HasIndex("TypeId");

                b.ToTable("Deployments");
            });

            modelBuilder.Entity("DeploymentTrackerCore.Models.DeploymentEnvironment", b => {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<string>("HostName")
                    .IsRequired()
                    .HasColumnType("TEXT")
                    .HasMaxLength(50);

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("TEXT")
                    .HasMaxLength(50);

                b.Property<byte[]>("RowVersion")
                    .IsConcurrencyToken()
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnType("BLOB");

                b.HasKey("Id");

                b.ToTable("Environments");
            });

            modelBuilder.Entity("DeploymentTrackerCore.Models.Type", b => {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("INTEGER");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("TEXT")
                    .HasMaxLength(200);

                b.Property<byte[]>("RowVersion")
                    .IsConcurrencyToken()
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnType("BLOB");

                b.Property<string>("TeardownTemplate")
                    .HasColumnType("TEXT")
                    .HasMaxLength(500);

                b.HasKey("Id");

                b.ToTable("Types");

                b.HasData(
                    new {
                        Id = 1,
                            Name = "Default"
                    });
            });

            modelBuilder.Entity("DeploymentTrackerCore.Models.Deployment", b => {
                b.HasOne("DeploymentTrackerCore.Models.DeploymentEnvironment", "DeployedEnvironment")
                    .WithMany("Deployments")
                    .HasForeignKey("DeployedEnvironmentId");

                b.HasOne("DeploymentTrackerCore.Models.Type", "Type")
                    .WithMany("Deployments")
                    .HasForeignKey("TypeId");

                b.OwnsOne("DeploymentTrackerCore.Models.AuditDetail", "CreatedBy", b1 => {
                    b1.Property<int>("DeploymentId")
                        .HasColumnType("INTEGER");

                    b1.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(150);

                    b1.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b1.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b1.HasKey("DeploymentId");

                    b1.ToTable("Deployments");

                    b1.WithOwner()
                        .HasForeignKey("DeploymentId");
                });

                b.OwnsOne("DeploymentTrackerCore.Models.AuditDetail", "ModifiedBy", b1 => {
                    b1.Property<int>("DeploymentId")
                        .HasColumnType("INTEGER");

                    b1.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(150);

                    b1.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b1.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b1.HasKey("DeploymentId");

                    b1.ToTable("Deployments");

                    b1.WithOwner()
                        .HasForeignKey("DeploymentId");
                });

                b.OwnsOne("DeploymentTrackerCore.Models.Login", "SiteLogin", b1 => {
                    b1.Property<int>("DeploymentId")
                        .HasColumnType("INTEGER");

                    b1.Property<string>("Password")
                        .HasColumnType("TEXT");

                    b1.Property<string>("UserName")
                        .HasColumnType("TEXT");

                    b1.HasKey("DeploymentId");

                    b1.ToTable("Deployments");

                    b1.WithOwner()
                        .HasForeignKey("DeploymentId");
                });
            });
#pragma warning restore 612, 618
        }
    }
}