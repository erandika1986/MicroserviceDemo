﻿// <auto-generated />
using System;
using Department.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Department.API.Migrations
{
    [DbContext(typeof(DepartmentContext))]
    [Migration("20181203025540_DEPT0000001")]
    partial class DEPT0000001
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Department.API.Models.DepartmentEmployeeModel", b =>
                {
                    b.Property<int>("DepartmentId");

                    b.Property<int>("EmployeeId");

                    b.Property<DateTime>("AssignedDate");

                    b.Property<int?>("DepartmentModelId");

                    b.Property<bool>("IsActive");

                    b.HasKey("DepartmentId", "EmployeeId");

                    b.HasIndex("DepartmentModelId");

                    b.ToTable("DepartmentEmployee");
                });

            modelBuilder.Entity("Department.API.Models.DepartmentModel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("IsActive");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Department");
                });

            modelBuilder.Entity("Department.API.Models.DepartmentEmployeeModel", b =>
                {
                    b.HasOne("Department.API.Models.DepartmentModel", "Department")
                        .WithMany()
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Department.API.Models.DepartmentModel")
                        .WithMany("DepartmentEmployees")
                        .HasForeignKey("DepartmentModelId");
                });
#pragma warning restore 612, 618
        }
    }
}
