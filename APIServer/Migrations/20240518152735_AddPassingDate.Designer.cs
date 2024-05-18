﻿// <auto-generated />
using System;
using APIServer.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace APIServer.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240518152735_AddPassingDate")]
    partial class AddPassingDate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Shared.DB.Classes.Test.Task.Task", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("char(36)");

                    b.Property<int>("InteractionType")
                        .HasColumnType("int");

                    b.Property<string>("Question")
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("Shared.DB.Classes.Test.Task.TaskAnswer.TaskAnswer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("AnsweredTaskId")
                        .HasColumnType("char(36)");

                    b.Property<string>("StringAnswer")
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)");

                    b.Property<Guid?>("StudentId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("AnsweredTaskId");

                    b.HasIndex("StudentId");

                    b.ToTable("TaskAnswers");
                });

            modelBuilder.Entity("Shared.DB.Classes.Test.Task.TaskAnswer.TestAnswer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("AnsweredTestId")
                        .HasColumnType("char(36)");

                    b.Property<string>("FantomName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<DateTime>("PassingDate")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("StudentId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("AnsweredTestId");

                    b.HasIndex("StudentId");

                    b.ToTable("TestAnswers");
                });

            modelBuilder.Entity("Shared.DB.Classes.Test.Task.ThemeTask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Theme")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.ToTable("ThemeTasks");
                });

            modelBuilder.Entity("Shared.DB.Classes.Test.Task.VariableAnswer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("StringAnswer")
                        .HasColumnType("longtext");

                    b.Property<Guid?>("TaskId")
                        .HasColumnType("char(36)");

                    b.Property<bool?>("Truthful")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.HasIndex("TaskId");

                    b.ToTable("VariableAnswers");
                });

            modelBuilder.Entity("Shared.DB.Classes.Test.Test", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("CreationTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("CreatorId")
                        .HasColumnType("char(36)");

                    b.Property<string>("TestName")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("Shared.DB.Classes.User.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<uint>("AccessFlags")
                        .HasColumnType("int unsigned");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("UserName")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Shared.DB.Classes.User.UserGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("GroupCreatorId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("GroupCreatorId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("TaskAnswerTestAnswer", b =>
                {
                    b.Property<Guid>("TaskAnswersId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("TestAnswerId")
                        .HasColumnType("char(36)");

                    b.HasKey("TaskAnswersId", "TestAnswerId");

                    b.HasIndex("TestAnswerId");

                    b.ToTable("TaskAnswerTestAnswer");
                });

            modelBuilder.Entity("TaskAnswerVariableAnswer", b =>
                {
                    b.Property<Guid>("MarkedVariablesId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("TaskAnswerId")
                        .HasColumnType("char(36)");

                    b.HasKey("MarkedVariablesId", "TaskAnswerId");

                    b.HasIndex("TaskAnswerId");

                    b.ToTable("TaskAnswerVariableAnswer");
                });

            modelBuilder.Entity("TaskTest", b =>
                {
                    b.Property<Guid>("TasksId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("TestsId")
                        .HasColumnType("char(36)");

                    b.HasKey("TasksId", "TestsId");

                    b.HasIndex("TestsId");

                    b.ToTable("TaskTest");
                });

            modelBuilder.Entity("TaskThemeTask", b =>
                {
                    b.Property<Guid>("ThematicsId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("ThemeTask")
                        .HasColumnType("char(36)");

                    b.HasKey("ThematicsId", "ThemeTask");

                    b.HasIndex("ThemeTask");

                    b.ToTable("TaskThemeTask");
                });

            modelBuilder.Entity("UserUserGroup", b =>
                {
                    b.Property<Guid>("StudentsId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("UserGroupsId")
                        .HasColumnType("char(36)");

                    b.HasKey("StudentsId", "UserGroupsId");

                    b.HasIndex("UserGroupsId");

                    b.ToTable("UserUserGroup");
                });

            modelBuilder.Entity("Shared.DB.Classes.Test.Task.Task", b =>
                {
                    b.HasOne("Shared.DB.Classes.User.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("Shared.DB.Classes.Test.Task.TaskAnswer.TaskAnswer", b =>
                {
                    b.HasOne("Shared.DB.Classes.Test.Task.Task", "AnsweredTask")
                        .WithMany()
                        .HasForeignKey("AnsweredTaskId");

                    b.HasOne("Shared.DB.Classes.User.User", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId");

                    b.Navigation("AnsweredTask");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Shared.DB.Classes.Test.Task.TaskAnswer.TestAnswer", b =>
                {
                    b.HasOne("Shared.DB.Classes.Test.Test", "AnsweredTest")
                        .WithMany()
                        .HasForeignKey("AnsweredTestId");

                    b.HasOne("Shared.DB.Classes.User.User", "Student")
                        .WithMany()
                        .HasForeignKey("StudentId");

                    b.Navigation("AnsweredTest");

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Shared.DB.Classes.Test.Task.VariableAnswer", b =>
                {
                    b.HasOne("Shared.DB.Classes.Test.Task.Task", null)
                        .WithMany("VariableAnswers")
                        .HasForeignKey("TaskId");
                });

            modelBuilder.Entity("Shared.DB.Classes.Test.Test", b =>
                {
                    b.HasOne("Shared.DB.Classes.User.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("Shared.DB.Classes.User.UserGroup", b =>
                {
                    b.HasOne("Shared.DB.Classes.User.User", "GroupCreator")
                        .WithMany()
                        .HasForeignKey("GroupCreatorId");

                    b.Navigation("GroupCreator");
                });

            modelBuilder.Entity("TaskAnswerTestAnswer", b =>
                {
                    b.HasOne("Shared.DB.Classes.Test.Task.TaskAnswer.TaskAnswer", null)
                        .WithMany()
                        .HasForeignKey("TaskAnswersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shared.DB.Classes.Test.Task.TaskAnswer.TestAnswer", null)
                        .WithMany()
                        .HasForeignKey("TestAnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskAnswerVariableAnswer", b =>
                {
                    b.HasOne("Shared.DB.Classes.Test.Task.VariableAnswer", null)
                        .WithMany()
                        .HasForeignKey("MarkedVariablesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shared.DB.Classes.Test.Task.TaskAnswer.TaskAnswer", null)
                        .WithMany()
                        .HasForeignKey("TaskAnswerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskTest", b =>
                {
                    b.HasOne("Shared.DB.Classes.Test.Task.Task", null)
                        .WithMany()
                        .HasForeignKey("TasksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shared.DB.Classes.Test.Test", null)
                        .WithMany()
                        .HasForeignKey("TestsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TaskThemeTask", b =>
                {
                    b.HasOne("Shared.DB.Classes.Test.Task.ThemeTask", null)
                        .WithMany()
                        .HasForeignKey("ThematicsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shared.DB.Classes.Test.Task.Task", null)
                        .WithMany()
                        .HasForeignKey("ThemeTask")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("UserUserGroup", b =>
                {
                    b.HasOne("Shared.DB.Classes.User.User", null)
                        .WithMany()
                        .HasForeignKey("StudentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shared.DB.Classes.User.UserGroup", null)
                        .WithMany()
                        .HasForeignKey("UserGroupsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shared.DB.Classes.Test.Task.Task", b =>
                {
                    b.Navigation("VariableAnswers");
                });
#pragma warning restore 612, 618
        }
    }
}
