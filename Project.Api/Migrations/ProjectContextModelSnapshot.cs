﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Storage.Internal;
using Project.Infrastructure;
using System;

namespace Project.Api.Migrations
{
    [DbContext(typeof(ProjectContext))]
    partial class ProjectContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026");

            modelBuilder.Entity("Project.Domain.AggregatesModel.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AreaId");

                    b.Property<string>("AreaName");

                    b.Property<string>("Avatar");

                    b.Property<int>("BrokerageOpints");

                    b.Property<string>("City");

                    b.Property<int>("CityId");

                    b.Property<string>("Company");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<int>("FinMoney");

                    b.Property<string>("FinPercentage");

                    b.Property<string>("FinStage");

                    b.Property<string>("FormatBPFile");

                    b.Property<int>("Incom");

                    b.Property<string>("Introduction");

                    b.Property<bool>("OnPlatform");

                    b.Property<string>("OriginBPFile");

                    b.Property<string>("Province");

                    b.Property<int>("ProvinceId");

                    b.Property<int>("ReferenceId");

                    b.Property<DateTime>("RegisterTime");

                    b.Property<int>("Revenue");

                    b.Property<bool>("ShowSecurityInfo");

                    b.Property<int>("SourceId");

                    b.Property<string>("Tags");

                    b.Property<DateTime>("UpdateTime");

                    b.Property<int>("UserId");

                    b.Property<int>("valuation");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("Project.Domain.AggregatesModel.ProjectContributor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avatar");

                    b.Property<int>("ContributorType");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<bool>("IsCloser");

                    b.Property<int>("ProjectId");

                    b.Property<int>("UserId");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectContributors");
                });

            modelBuilder.Entity("Project.Domain.AggregatesModel.ProjectProperty", b =>
                {
                    b.Property<int>("ProjectId");

                    b.Property<string>("Key")
                        .HasMaxLength(100);

                    b.Property<string>("Value")
                        .HasMaxLength(100);

                    b.Property<string>("Text");

                    b.HasKey("ProjectId", "Key", "Value");

                    b.ToTable("ProjectProperties");
                });

            modelBuilder.Entity("Project.Domain.AggregatesModel.ProjectViewer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Avatar");

                    b.Property<DateTime>("CreateTime");

                    b.Property<int>("ProjectId");

                    b.Property<int>("UserId");

                    b.Property<string>("UserName");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("ProjectViewers");
                });

            modelBuilder.Entity("Project.Domain.AggregatesModel.ProjectVisibleRule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ProjectId");

                    b.Property<string>("Tags");

                    b.Property<bool>("Visible");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId")
                        .IsUnique();

                    b.ToTable("ProjectsVisibleRules");
                });

            modelBuilder.Entity("Project.Domain.AggregatesModel.ProjectContributor", b =>
                {
                    b.HasOne("Project.Domain.AggregatesModel.Project")
                        .WithMany("Contributors")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Project.Domain.AggregatesModel.ProjectProperty", b =>
                {
                    b.HasOne("Project.Domain.AggregatesModel.Project")
                        .WithMany("Properties")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Project.Domain.AggregatesModel.ProjectViewer", b =>
                {
                    b.HasOne("Project.Domain.AggregatesModel.Project")
                        .WithMany("Viewers")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Project.Domain.AggregatesModel.ProjectVisibleRule", b =>
                {
                    b.HasOne("Project.Domain.AggregatesModel.Project")
                        .WithOne("VisibleRule")
                        .HasForeignKey("Project.Domain.AggregatesModel.ProjectVisibleRule", "ProjectId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
