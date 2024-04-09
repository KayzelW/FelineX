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
    [Migration("20240409073925_TruthfulField")]
    partial class TruthfulField
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Shared.DB.Classes.Task.Task", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("InteractionType")
                        .HasColumnType("int");

                    b.Property<string>("Question")
                        .HasMaxLength(1000)
                        .HasColumnType("varchar(1000)");

                    b.Property<Guid?>("TestId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("User")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("TestId");

                    b.HasIndex("User");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("Shared.DB.Classes.Task.ThemeTask", b =>
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

            modelBuilder.Entity("Shared.DB.Classes.Task.VariableAnswer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("StringAnswer")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<Guid?>("TaskId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("TaskId");

                    b.ToTable("VariableAnswers");
                });

            modelBuilder.Entity("Shared.DB.Classes.Test", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CreatorId")
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

            modelBuilder.Entity("Shared.DB.Classes.Task.Task", b =>
                {
                    b.HasOne("Shared.DB.Classes.Test", null)
                        .WithMany("Tasks")
                        .HasForeignKey("TestId");

                    b.HasOne("Shared.DB.Classes.User.User", "Creator")
                        .WithMany()
                        .HasForeignKey("User");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("Shared.DB.Classes.Task.VariableAnswer", b =>
                {
                    b.HasOne("Shared.DB.Classes.Task.Task", "Task")
                        .WithMany("VariableAnswers")
                        .HasForeignKey("TaskId");

                    b.Navigation("Task");
                });

            modelBuilder.Entity("Shared.DB.Classes.Test", b =>
                {
                    b.HasOne("Shared.DB.Classes.User.User", "Creator")
                        .WithMany("CreatedTests")
                        .HasForeignKey("CreatorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("TaskThemeTask", b =>
                {
                    b.HasOne("Shared.DB.Classes.Task.ThemeTask", null)
                        .WithMany()
                        .HasForeignKey("ThematicsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shared.DB.Classes.Task.Task", null)
                        .WithMany()
                        .HasForeignKey("ThemeTask")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shared.DB.Classes.Task.Task", b =>
                {
                    b.Navigation("VariableAnswers");
                });

            modelBuilder.Entity("Shared.DB.Classes.Test", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("Shared.DB.Classes.User.User", b =>
                {
                    b.Navigation("CreatedTests");
                });
#pragma warning restore 612, 618
        }
    }
}
