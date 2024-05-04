﻿// <auto-generated />
using System;
using ElectronicJournal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ElectronicJournal.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230409085118_Create")]
    partial class Create
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("ElectronicJournal.Models.Grade", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<int?>("SubjectId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("grade")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("SubjectId");

                    b.HasIndex("UserId");

                    b.ToTable("Grades");
                });

            modelBuilder.Entity("ElectronicJournal.Models.GradesDate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("Date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("GradesDates");
                });

            modelBuilder.Entity("ElectronicJournal.Models.Group", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("GroupName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("ElectronicJournal.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("RoleName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ElectronicJournal.Models.Subject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<string>("SubjectName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("UserId");

                    b.ToTable("Subjects");
                });

            modelBuilder.Entity("ElectronicJournal.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("GroupId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ElectronicJournal.Models.Grade", b =>
                {
                    b.HasOne("ElectronicJournal.Models.Group", "Groups")
                        .WithMany("GradeList")
                        .HasForeignKey("GroupId");

                    b.HasOne("ElectronicJournal.Models.Subject", "Subject")
                        .WithMany("GradeList")
                        .HasForeignKey("SubjectId");

                    b.HasOne("ElectronicJournal.Models.User", "User")
                        .WithMany("GradeList")
                        .HasForeignKey("UserId");

                    b.Navigation("Groups");

                    b.Navigation("Subject");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ElectronicJournal.Models.GradesDate", b =>
                {
                    b.HasOne("ElectronicJournal.Models.Group", "Group")
                        .WithMany("DateList")
                        .HasForeignKey("GroupId");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("ElectronicJournal.Models.Subject", b =>
                {
                    b.HasOne("ElectronicJournal.Models.Group", "Group")
                        .WithMany("SubjectList")
                        .HasForeignKey("GroupId");

                    b.HasOne("ElectronicJournal.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("Group");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ElectronicJournal.Models.User", b =>
                {
                    b.HasOne("ElectronicJournal.Models.Group", "Groups")
                        .WithMany("UserList")
                        .HasForeignKey("GroupId");

                    b.HasOne("ElectronicJournal.Models.Role", "Roles")
                        .WithMany("UserList")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Groups");

                    b.Navigation("Roles");
                });

            modelBuilder.Entity("ElectronicJournal.Models.Group", b =>
                {
                    b.Navigation("DateList");

                    b.Navigation("GradeList");

                    b.Navigation("SubjectList");

                    b.Navigation("UserList");
                });

            modelBuilder.Entity("ElectronicJournal.Models.Role", b =>
                {
                    b.Navigation("UserList");
                });

            modelBuilder.Entity("ElectronicJournal.Models.Subject", b =>
                {
                    b.Navigation("GradeList");
                });

            modelBuilder.Entity("ElectronicJournal.Models.User", b =>
                {
                    b.Navigation("GradeList");
                });
#pragma warning restore 612, 618
        }
    }
}
