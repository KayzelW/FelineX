﻿// <auto-generated />
using System;
using System.Collections.Generic;
using APIServer.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace APIServer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Shared.DB.Test.Answers.TaskAnswer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("AnsweredTaskId")
                        .HasColumnType("uuid");

                    b.Property<bool>("IsFailedCheck")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsSuccess")
                        .HasColumnType("boolean");

                    b.Property<string>("Result")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("StringAnswer")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<Guid?>("StudentId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("TestAnswerId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AnsweredTaskId");

                    b.HasIndex("StudentId");

                    b.HasIndex("TestAnswerId");

                    b.ToTable("TaskAnswers");
                });

            modelBuilder.Entity("Shared.DB.Test.Answers.TestAnswer", b =>
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

                    b.Property<Guid?>("StudentId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("AnsweredTestId");

                    b.HasIndex("StudentId");

                    b.ToTable("TestAnswers");
                });

            modelBuilder.Entity("Shared.DB.Test.Task.Task", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<List<string>>("DataRows")
                        .HasColumnType("text[]");

                    b.Property<int?>("DatabaseType")
                        .HasColumnType("integer");

                    b.Property<int>("InteractionType")
                        .HasColumnType("integer");

                    b.Property<string>("Question")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<Guid>("SettingsId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("SettingsId")
                        .IsUnique();

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("Shared.DB.Test.Task.TaskSettings", b =>
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

            modelBuilder.Entity("Shared.DB.Test.Task.ThemeTask", b =>
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

            modelBuilder.Entity("Shared.DB.Test.Task.VariableAnswer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("StringAnswer")
                        .HasColumnType("text");

                    b.Property<Guid?>("TaskId")
                        .HasColumnType("uuid");

                    b.Property<bool?>("Truthful")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("TaskId");

                    b.ToTable("VariableAnswers");
                });

            modelBuilder.Entity("Shared.DB.Test.Test", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("CreationTime")
                        .HasColumnType("timestamp(6)");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("SettingsId")
                        .HasColumnType("uuid");

                    b.Property<string>("TestName")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.HasIndex("SettingsId");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("Shared.DB.Test.TestSettings", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("TestSettings");
                });

            modelBuilder.Entity("Shared.DB.User.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<long>("AccessFlags")
                        .HasColumnType("bigint");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Shared.DB.User.UserGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid?>("GroupCreatorId")
                        .HasColumnType("uuid");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid?>("TestSettingsId")
                        .HasColumnType("uuid");
                    
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

            modelBuilder.Entity("TaskTest", b =>
                {
                    b.Property<Guid>("TasksId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("TestsId")
                        .HasColumnType("uuid");

                    b.HasKey("TasksId", "TestsId");

                    b.HasIndex("TestsId");

                    b.ToTable("TaskTest");
                });

            modelBuilder.Entity("TaskThemeTask", b =>
                {
                    b.Property<Guid>("ThematicsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("ThemeTask")
                        .HasColumnType("uuid");

                    b.HasKey("ThematicsId", "ThemeTask");

                    b.HasIndex("ThemeTask");

                    b.ToTable("TaskThemeTask");
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

            modelBuilder.Entity("UserUserGroup", b =>
                {
                    b.Property<Guid>("StudentsId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserGroupsId")
                        .HasColumnType("uuid");

                    b.HasKey("StudentsId", "UserGroupsId");

                    b.HasIndex("UserGroupsId");

                    b.ToTable("UserUserGroup");
                });

            modelBuilder.Entity("Shared.DB.Test.Answers.TaskAnswer", b =>
                {
                    b.HasOne("Shared.DB.Test.Task.Task", "AnsweredTask")
                        .WithMany()
                        .HasForeignKey("AnsweredTaskId");

                    b.HasOne("Shared.DB.User.User", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId");

                    b.HasOne("Shared.DB.Test.Answers.TestAnswer", null)
                        .WithMany("TaskAnswers")
                        .HasForeignKey("TestAnswerId");

                    b.Navigation("AnsweredTask");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Shared.DB.Test.Answers.TestAnswer", b =>
                {
                    b.HasOne("Shared.DB.Test.Test", "AnsweredTest")
                        .WithMany()
                        .HasForeignKey("AnsweredTestId");

                    b.HasOne("Shared.DB.User.User", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId");

                    b.Navigation("AnsweredTest");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Shared.DB.Test.Task.Task", b =>
                {
                    b.HasOne("Shared.DB.User.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId");

                    b.HasOne("Shared.DB.Test.Task.TaskSettings", "Settings")
                        .WithOne()
                        .HasForeignKey("Shared.DB.Test.Task.Task", "SettingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Settings");
                });

            modelBuilder.Entity("Shared.DB.Test.Task.VariableAnswer", b =>
                {
                    b.HasOne("Shared.DB.Test.Task.Task", null)
                        .WithMany("VariableAnswers")
                        .HasForeignKey("TaskId");
                });

            modelBuilder.Entity("Shared.DB.Test.Test", b =>
                {
                    b.HasOne("Shared.DB.User.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId");

                    b.HasOne("Shared.DB.Test.TestSettings", "Settings")
                        .WithMany()
                        .HasForeignKey("SettingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");

                    b.Navigation("Settings");
                });

            modelBuilder.Entity("Shared.DB.User.UserGroup", b =>
                {
                    b.HasOne("Shared.DB.User.User", "GroupCreator")
                        .WithMany()
                        .HasForeignKey("GroupCreatorId");

                    b.Navigation("GroupCreator");
                });

            modelBuilder.Entity("TaskAnswerVariableAnswer", b =>
                {
                    b.HasOne("Shared.DB.Test.Task.VariableAnswer", null)
                        .WithMany()
                        .HasForeignKey("MarkedVariablesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shared.DB.Test.Answers.TaskAnswer", null)
                        .WithMany()
                        .HasForeignKey("TaskAnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskTest", b =>
                {
                    b.HasOne("Shared.DB.Test.Task.Task", null)
                        .WithMany()
                        .HasForeignKey("TasksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shared.DB.Test.Test", null)
                        .WithMany()
                        .HasForeignKey("TestsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskThemeTask", b =>
                {
                    b.HasOne("Shared.DB.Test.Task.ThemeTask", null)
                        .WithMany()
                        .HasForeignKey("ThematicsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shared.DB.Test.Task.Task", null)
                        .WithMany()
                        .HasForeignKey("ThemeTask")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TestSettingsThemeTask", b =>
                {
                    b.HasOne("Shared.DB.Test.Task.ThemeTask", null)
                        .WithMany()
                        .HasForeignKey("TasksThemesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shared.DB.Test.TestSettings", null)
                        .WithMany()
                        .HasForeignKey("TestSettingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TestSettingsUserGroup", b =>
                {
                    b.HasOne("Shared.DB.User.UserGroup", null)
                        .WithMany()
                        .HasForeignKey("TestGroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shared.DB.Test.TestSettings", null)
                        .WithMany()
                        .HasForeignKey("TestSettingsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UserUserGroup", b =>
                {
                    b.HasOne("Shared.DB.User.User", null)
                        .WithMany()
                        .HasForeignKey("StudentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shared.DB.User.UserGroup", null)
                        .WithMany()
                        .HasForeignKey("UserGroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shared.DB.Test.Answers.TestAnswer", b =>
                {
                    b.Navigation("TaskAnswers");
                });

            modelBuilder.Entity("Shared.DB.Test.Task.Task", b =>
                {
                    b.Navigation("VariableAnswers");
                });
#pragma warning restore 612, 618
        }
    }
}
