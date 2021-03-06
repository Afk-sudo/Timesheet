﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Timesheet.DataAccess;

namespace Timesheet.DataAccess.Npgsql.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210321144248_edit timelog remove employee fk")]
    partial class edittimelogremoveemployeefk
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("Timesheet.Domain.Entities.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Login")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<decimal>("Salary")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.ToTable("Employees");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Employee");
                });

            modelBuilder.Entity("Timesheet.Domain.Entities.TimeLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Comment")
                        .HasColumnType("text");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("EmployeeLogin")
                        .HasColumnType("text");

                    b.Property<int>("WorkingHours")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("TimeLogs");
                });

            modelBuilder.Entity("Timesheet.Domain.Entities.ChiefEmployee", b =>
                {
                    b.HasBaseType("Timesheet.Domain.Entities.Employee");

                    b.Property<decimal>("Bonus")
                        .HasColumnType("numeric");

                    b.HasDiscriminator().HasValue("ChiefEmployee");
                });

            modelBuilder.Entity("Timesheet.Domain.Entities.FreelancerEmployee", b =>
                {
                    b.HasBaseType("Timesheet.Domain.Entities.Employee");

                    b.HasDiscriminator().HasValue("FreelancerEmployee");
                });

            modelBuilder.Entity("Timesheet.Domain.Entities.StaffEmployee", b =>
                {
                    b.HasBaseType("Timesheet.Domain.Entities.Employee");

                    b.HasDiscriminator().HasValue("StaffEmployee");
                });
#pragma warning restore 612, 618
        }
    }
}
