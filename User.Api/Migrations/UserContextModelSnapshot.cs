﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.Data.EntityFrameworkCore.Storage.Internal;
using System;
using User.Api.Data;

namespace User.Api.Migrations
{
    [DbContext(typeof(UserContext))]
    partial class UserContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125");

            modelBuilder.Entity("User.Api.Models.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("Avatar");

                    b.Property<string>("City");

                    b.Property<int>("CityId");

                    b.Property<string>("Company");

                    b.Property<string>("Email");

                    b.Property<byte>("Gender");

                    b.Property<string>("Name");

                    b.Property<string>("NameCard");

                    b.Property<string>("ProvinceId");

                    b.Property<string>("Tel");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("User.Api.Models.BPFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedTime");

                    b.Property<string>("FileName");

                    b.Property<string>("FromatFilePath");

                    b.Property<string>("OriginFilePath");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.ToTable("BPFiles");
                });

            modelBuilder.Entity("User.Api.Models.UserProperty", b =>
                {
                    b.Property<string>("Key")
                        .HasMaxLength(100);

                    b.Property<int>("AppUserId");

                    b.Property<string>("Value")
                        .HasMaxLength(100);

                    b.Property<string>("Text");

                    b.HasKey("Key", "AppUserId", "Value");

                    b.HasIndex("AppUserId");

                    b.ToTable("UserProperties");
                });

            modelBuilder.Entity("User.Api.Models.UserTag", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("Tag");

                    b.Property<DateTime>("CreatedTime");

                    b.HasKey("UserId", "Tag");

                    b.ToTable("UserTags");
                });

            modelBuilder.Entity("User.Api.Models.UserProperty", b =>
                {
                    b.HasOne("User.Api.Models.AppUser")
                        .WithMany("Properties")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
