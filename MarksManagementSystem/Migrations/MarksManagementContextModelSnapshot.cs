﻿// <auto-generated />
using System;
using MarksManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MarksManagementSystem.Migrations
{
    [DbContext(typeof(MarksManagementContext))]
    partial class MarksManagementContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MarksManagementSystem.Data.Models.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CourseId"));

                    b.Property<int>("CourseCredits")
                        .HasColumnType("int");

                    b.Property<string>("CourseName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("CourseId");

                    b.HasIndex("CourseName")
                        .IsUnique();

                    b.ToTable("Course");
                });

            modelBuilder.Entity("MarksManagementSystem.Data.Models.CourseTutor", b =>
                {
                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("TutorId")
                        .HasColumnType("int");

                    b.Property<bool>("IsUnitLeader")
                        .HasColumnType("bit");

                    b.HasKey("CourseId", "TutorId");

                    b.HasIndex("TutorId");

                    b.ToTable("CourseTutor");
                });

            modelBuilder.Entity("MarksManagementSystem.Data.Models.Tutor", b =>
                {
                    b.Property<int>("TutorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TutorId"));

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.Property<DateTime>("TutorDateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("TutorEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("TutorFirstName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("TutorLastName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("TutorPassword")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TutorId");

                    b.HasIndex("TutorEmail")
                        .IsUnique();

                    b.ToTable("Tutor");
                });

            modelBuilder.Entity("MarksManagementSystem.Data.Models.CourseTutor", b =>
                {
                    b.HasOne("MarksManagementSystem.Data.Models.Course", "Course")
                        .WithMany("CourseTutors")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MarksManagementSystem.Data.Models.Tutor", "Tutor")
                        .WithMany("CourseTutors")
                        .HasForeignKey("TutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Tutor");
                });

            modelBuilder.Entity("MarksManagementSystem.Data.Models.Course", b =>
                {
                    b.Navigation("CourseTutors");
                });

            modelBuilder.Entity("MarksManagementSystem.Data.Models.Tutor", b =>
                {
                    b.Navigation("CourseTutors");
                });
#pragma warning restore 612, 618
        }
    }
}
