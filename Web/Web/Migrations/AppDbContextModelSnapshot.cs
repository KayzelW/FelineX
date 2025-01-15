﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Web.Services;

#nullable disable

namespace Web.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ApplicationUserTestSettings", b =>
                {
                    b.Property<Guid>("TestSettingsId")
                        .HasColumnType("uuid");

                    b.Property<string>("TestUsersId")
                        .HasColumnType("text");

                    b.HasKey("TestSettingsId", "TestUsersId");

                    b.HasIndex("TestUsersId");

                    b.ToTable("ApplicationUserTestSettings");
                });

            modelBuilder.Entity("ApplicationUserUserGroup", b =>
                {
                    b.Property<string>("StudentsId")
                        .HasColumnType("text");

                    b.Property<Guid>("UserGroupsId")
                        .HasColumnType("uuid");

                    b.HasKey("StudentsId", "UserGroupsId");

                    b.HasIndex("UserGroupsId");

                    b.ToTable("ApplicationUserUserGroup");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Shared.Data.ApplicationRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Shared.Data.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Shared.Data.Test.Answers.TaskAnswer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AnsweredTaskId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsCheckEnded")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsFailedCheck")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsSuccess")
                        .HasColumnType("boolean");

                    b.Property<string>("Result")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<string>("StringAnswer")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<string>("StudentId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("TestAnswerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AnsweredTaskId");

                    b.HasIndex("StudentId");

                    b.HasIndex("TestAnswerId");

                    b.ToTable("TaskAnswers");
                });

            modelBuilder.Entity("Shared.Data.Test.Answers.TestAnswer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AnsweredTestId")
                        .HasColumnType("uuid");

                    b.Property<string>("ClientConnectionLog")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FantomName")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("PassingDate")
                        .HasColumnType("timestamp(6)");

                    b.Property<double>("Score")
                        .HasColumnType("double precision");

                    b.Property<string>("StudentId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AnsweredTestId");

                    b.HasIndex("StudentId");

                    b.ToTable("TestAnswers");
                });

            modelBuilder.Entity("Shared.Data.Test.Task.TaskSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("SqlQueryCheck")
                        .HasColumnType("text");

                    b.Property<string>("SqlQueryInstall")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TaskSettings");
                });

            modelBuilder.Entity("Shared.Data.Test.Task.ThemeTask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Theme")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("ThemeTasks");
                });

            modelBuilder.Entity("Shared.Data.Test.Task.UniqueTask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CreatorId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.PrimitiveCollection<List<string>>("DataRows")
                        .HasColumnType("text[]");

                    b.Property<int?>("DatabaseType")
                        .HasColumnType("integer");

                    b.Property<int>("InteractionType")
                        .HasColumnType("integer");

                    b.Property<string>("Question")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("Shared.Data.Test.Task.VariableAnswer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("StringAnswer")
                        .HasColumnType("text");

                    b.Property<bool?>("Truthful")
                        .HasColumnType("boolean");

                    b.Property<Guid?>("UniqueTaskId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UniqueTaskId");

                    b.ToTable("VariableAnswers");
                });

            modelBuilder.Entity("Shared.Data.Test.TestLink", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("Expiration")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid>("TestId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TestSettingsId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("TestId");

                    b.HasIndex("TestSettingsId");

                    b.ToTable("TestLinks");
                });

            modelBuilder.Entity("Shared.Data.Test.TestSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("UniqueTestId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UniqueTestId");

                    b.ToTable("TestSettings");
                });

            modelBuilder.Entity("Shared.Data.Test.UniqueTest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("CreationTime")
                        .HasColumnType("timestamp(6)");

                    b.Property<string>("CreatorId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TestName")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("Shared.Data.UserGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("GroupCreatorId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("GroupCreatorId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("TaskAnswerVariableAnswer", b =>
                {
                    b.Property<Guid>("MarkedVariablesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TaskAnswerId")
                        .HasColumnType("uuid");

                    b.HasKey("MarkedVariablesId", "TaskAnswerId");

                    b.HasIndex("TaskAnswerId");

                    b.ToTable("TaskAnswerVariableAnswer");
                });

            modelBuilder.Entity("TestSettingsThemeTask", b =>
                {
                    b.Property<Guid>("TasksThemesId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TestSettingsId")
                        .HasColumnType("uuid");

                    b.HasKey("TasksThemesId", "TestSettingsId");

                    b.HasIndex("TestSettingsId");

                    b.ToTable("TestSettingsThemeTask");
                });

            modelBuilder.Entity("TestSettingsUserGroup", b =>
                {
                    b.Property<Guid>("TestGroupsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TestSettingsId")
                        .HasColumnType("uuid");

                    b.HasKey("TestGroupsId", "TestSettingsId");

                    b.HasIndex("TestSettingsId");

                    b.ToTable("TestSettingsUserGroup");
                });

            modelBuilder.Entity("ThemeTaskUniqueTask", b =>
                {
                    b.Property<Guid>("ThematicsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ThemeTask")
                        .HasColumnType("uuid");

                    b.HasKey("ThematicsId", "ThemeTask");

                    b.HasIndex("ThemeTask");

                    b.ToTable("ThemeTaskUniqueTask");
                });

            modelBuilder.Entity("UniqueTaskUniqueTest", b =>
                {
                    b.Property<Guid>("TasksId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TestsId")
                        .HasColumnType("uuid");

                    b.HasKey("TasksId", "TestsId");

                    b.HasIndex("TestsId");

                    b.ToTable("UniqueTaskUniqueTest");
                });

            modelBuilder.Entity("ApplicationUserTestSettings", b =>
                {
                    b.HasOne("Shared.Data.Test.TestSettings", null)
                        .WithMany()
                        .HasForeignKey("TestSettingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shared.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("TestUsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ApplicationUserUserGroup", b =>
                {
                    b.HasOne("Shared.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("StudentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shared.Data.UserGroup", null)
                        .WithMany()
                        .HasForeignKey("UserGroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Shared.Data.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Shared.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Shared.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Shared.Data.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shared.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Shared.Data.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shared.Data.Test.Answers.TaskAnswer", b =>
                {
                    b.HasOne("Shared.Data.Test.Task.UniqueTask", "AnsweredTask")
                        .WithMany()
                        .HasForeignKey("AnsweredTaskId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Shared.Data.ApplicationUser", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shared.Data.Test.Answers.TestAnswer", "TestAnswer")
                        .WithMany("TaskAnswers")
                        .HasForeignKey("TestAnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AnsweredTask");

                    b.Navigation("Student");

                    b.Navigation("TestAnswer");
                });

            modelBuilder.Entity("Shared.Data.Test.Answers.TestAnswer", b =>
                {
                    b.HasOne("Shared.Data.Test.UniqueTest", "AnsweredTest")
                        .WithMany()
                        .HasForeignKey("AnsweredTestId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Shared.Data.ApplicationUser", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AnsweredTest");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Shared.Data.Test.Task.TaskSettings", b =>
                {
                    b.HasOne("Shared.Data.Test.Task.UniqueTask", "AssignedTask")
                        .WithOne("Settings")
                        .HasForeignKey("Shared.Data.Test.Task.TaskSettings", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AssignedTask");
                });

            modelBuilder.Entity("Shared.Data.Test.Task.UniqueTask", b =>
                {
                    b.HasOne("Shared.Data.ApplicationUser", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("Shared.Data.Test.Task.VariableAnswer", b =>
                {
                    b.HasOne("Shared.Data.Test.Task.UniqueTask", null)
                        .WithMany("VariableAnswers")
                        .HasForeignKey("UniqueTaskId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Shared.Data.Test.TestLink", b =>
                {
                    b.HasOne("Shared.Data.Test.UniqueTest", "Test")
                        .WithMany()
                        .HasForeignKey("TestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shared.Data.Test.TestSettings", "TestSettings")
                        .WithMany()
                        .HasForeignKey("TestSettingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Test");

                    b.Navigation("TestSettings");
                });

            modelBuilder.Entity("Shared.Data.Test.TestSettings", b =>
                {
                    b.HasOne("Shared.Data.Test.UniqueTest", null)
                        .WithMany("Settings")
                        .HasForeignKey("UniqueTestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Shared.Data.Test.UniqueTest", b =>
                {
                    b.HasOne("Shared.Data.ApplicationUser", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("Shared.Data.UserGroup", b =>
                {
                    b.HasOne("Shared.Data.ApplicationUser", "GroupCreator")
                        .WithMany()
                        .HasForeignKey("GroupCreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GroupCreator");
                });

            modelBuilder.Entity("TaskAnswerVariableAnswer", b =>
                {
                    b.HasOne("Shared.Data.Test.Task.VariableAnswer", null)
                        .WithMany()
                        .HasForeignKey("MarkedVariablesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shared.Data.Test.Answers.TaskAnswer", null)
                        .WithMany()
                        .HasForeignKey("TaskAnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TestSettingsThemeTask", b =>
                {
                    b.HasOne("Shared.Data.Test.Task.ThemeTask", null)
                        .WithMany()
                        .HasForeignKey("TasksThemesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shared.Data.Test.TestSettings", null)
                        .WithMany()
                        .HasForeignKey("TestSettingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TestSettingsUserGroup", b =>
                {
                    b.HasOne("Shared.Data.UserGroup", null)
                        .WithMany()
                        .HasForeignKey("TestGroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shared.Data.Test.TestSettings", null)
                        .WithMany()
                        .HasForeignKey("TestSettingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ThemeTaskUniqueTask", b =>
                {
                    b.HasOne("Shared.Data.Test.Task.ThemeTask", null)
                        .WithMany()
                        .HasForeignKey("ThematicsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shared.Data.Test.Task.UniqueTask", null)
                        .WithMany()
                        .HasForeignKey("ThemeTask")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UniqueTaskUniqueTest", b =>
                {
                    b.HasOne("Shared.Data.Test.Task.UniqueTask", null)
                        .WithMany()
                        .HasForeignKey("TasksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shared.Data.Test.UniqueTest", null)
                        .WithMany()
                        .HasForeignKey("TestsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shared.Data.Test.Answers.TestAnswer", b =>
                {
                    b.Navigation("TaskAnswers");
                });

            modelBuilder.Entity("Shared.Data.Test.Task.UniqueTask", b =>
                {
                    b.Navigation("Settings")
                        .IsRequired();

                    b.Navigation("VariableAnswers");
                });

            modelBuilder.Entity("Shared.Data.Test.UniqueTest", b =>
                {
                    b.Navigation("Settings");
                });
#pragma warning restore 612, 618
        }
    }
}
