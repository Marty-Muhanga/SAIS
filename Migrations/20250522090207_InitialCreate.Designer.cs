﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SAIS.Data;

#nullable disable

namespace SAIS.Migrations
{
    [DbContext(typeof(SAISContext))]
    [Migration("20250522090207_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SAIS.Models.Applicant", b =>
                {
                    b.Property<int>("ApplicantID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ApplicantID"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<DateTime>("ApplicationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("IDNumber")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("MaritalStatusID")
                        .HasColumnType("int");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ModifiedBy")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("ModifiedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PhysicalAddress")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("PostalAddress")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("SexID")
                        .HasColumnType("int");

                    b.Property<byte[]>("SignatureImage")
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("StatusID")
                        .HasColumnType("int");

                    b.Property<string>("Telephone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("VillageID")
                        .HasColumnType("int");

                    b.HasKey("ApplicantID");

                    b.HasIndex("MaritalStatusID");

                    b.HasIndex("SexID");

                    b.HasIndex("StatusID");

                    b.HasIndex("VillageID");

                    b.ToTable("Applicants");
                });

            modelBuilder.Entity("SAIS.Models.ApplicantProgram", b =>
                {
                    b.Property<int>("ApplicantProgramID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ApplicantProgramID"));

                    b.Property<int>("ApplicantID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ApprovalDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ApprovedBy")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("ProgramID")
                        .HasColumnType("int");

                    b.Property<int>("StatusID")
                        .HasColumnType("int");

                    b.HasKey("ApplicantProgramID");

                    b.HasIndex("ApplicantID");

                    b.HasIndex("ProgramID");

                    b.HasIndex("StatusID");

                    b.ToTable("ApplicantPrograms");
                });

            modelBuilder.Entity("SAIS.Models.ApplicationStatusLookup", b =>
                {
                    b.Property<int>("StatusID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StatusID"));

                    b.Property<string>("StatusCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("StatusName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("StatusID");

                    b.ToTable("ApplicationStatusLookups");
                });

            modelBuilder.Entity("SAIS.Models.County", b =>
                {
                    b.Property<int>("CountyID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CountyID"));

                    b.Property<string>("CountyCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("CountyName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("CountyID");

                    b.ToTable("Counties");
                });

            modelBuilder.Entity("SAIS.Models.Location", b =>
                {
                    b.Property<int>("LocationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LocationID"));

                    b.Property<string>("LocationCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("LocationName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("SubCountyID")
                        .HasColumnType("int");

                    b.HasKey("LocationID");

                    b.HasIndex("SubCountyID");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("SAIS.Models.MaritalStatusLookup", b =>
                {
                    b.Property<int>("MaritalStatusID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaritalStatusID"));

                    b.Property<string>("StatusCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("StatusName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("MaritalStatusID");

                    b.ToTable("MaritalStatusLookups");
                });

            modelBuilder.Entity("SAIS.Models.Program_", b =>
                {
                    b.Property<int>("ProgramID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProgramID"));

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("ProgramCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("ProgramName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ProgramID");

                    b.ToTable("Programs");
                });

            modelBuilder.Entity("SAIS.Models.SexLookup", b =>
                {
                    b.Property<int>("SexID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SexID"));

                    b.Property<string>("SexCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("SexName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("SexID");

                    b.ToTable("SexLookups");
                });

            modelBuilder.Entity("SAIS.Models.SubCounty", b =>
                {
                    b.Property<int>("SubCountyID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubCountyID"));

                    b.Property<int>("CountyID")
                        .HasColumnType("int");

                    b.Property<string>("SubCountyCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("SubCountyName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("SubCountyID");

                    b.HasIndex("CountyID");

                    b.ToTable("SubCounties");
                });

            modelBuilder.Entity("SAIS.Models.SubLocation", b =>
                {
                    b.Property<int>("SubLocationID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubLocationID"));

                    b.Property<int>("LocationID")
                        .HasColumnType("int");

                    b.Property<string>("SubLocationCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("SubLocationName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("SubLocationID");

                    b.HasIndex("LocationID");

                    b.ToTable("SubLocations");
                });

            modelBuilder.Entity("SAIS.Models.Village", b =>
                {
                    b.Property<int>("VillageID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("VillageID"));

                    b.Property<int>("SubLocationID")
                        .HasColumnType("int");

                    b.Property<string>("VillageCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("VillageName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("VillageID");

                    b.HasIndex("SubLocationID");

                    b.ToTable("Villages");
                });

            modelBuilder.Entity("SAIS.ViewModels.SearchViewModel", b =>
                {
                    b.Property<int?>("CountyID")
                        .HasColumnType("int");

                    b.Property<int>("CurrentPage")
                        .HasColumnType("int");

                    b.Property<DateTime?>("FromDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("LocationID")
                        .HasColumnType("int");

                    b.Property<int>("PageSize")
                        .HasColumnType("int");

                    b.Property<string>("SearchTerm")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StatusID")
                        .HasColumnType("int");

                    b.Property<int?>("SubCountyID")
                        .HasColumnType("int");

                    b.Property<int?>("SubLocationID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ToDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TotalCount")
                        .HasColumnType("int");

                    b.Property<int?>("VillageID")
                        .HasColumnType("int");

                    b.ToTable("SearchViewModel");
                });

            modelBuilder.Entity("SAIS.Models.Applicant", b =>
                {
                    b.HasOne("SAIS.Models.MaritalStatusLookup", "MaritalStatus")
                        .WithMany()
                        .HasForeignKey("MaritalStatusID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SAIS.Models.SexLookup", "Sex")
                        .WithMany()
                        .HasForeignKey("SexID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SAIS.Models.ApplicationStatusLookup", "Status")
                        .WithMany()
                        .HasForeignKey("StatusID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SAIS.Models.Village", "Village")
                        .WithMany("Applicants")
                        .HasForeignKey("VillageID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MaritalStatus");

                    b.Navigation("Sex");

                    b.Navigation("Status");

                    b.Navigation("Village");
                });

            modelBuilder.Entity("SAIS.Models.ApplicantProgram", b =>
                {
                    b.HasOne("SAIS.Models.Applicant", "Applicant")
                        .WithMany("ApplicantPrograms")
                        .HasForeignKey("ApplicantID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SAIS.Models.Program_", "Program")
                        .WithMany("ApplicantPrograms")
                        .HasForeignKey("ProgramID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SAIS.Models.ApplicationStatusLookup", "Status")
                        .WithMany()
                        .HasForeignKey("StatusID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Applicant");

                    b.Navigation("Program");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("SAIS.Models.Location", b =>
                {
                    b.HasOne("SAIS.Models.SubCounty", "SubCounty")
                        .WithMany("Locations")
                        .HasForeignKey("SubCountyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SubCounty");
                });

            modelBuilder.Entity("SAIS.Models.SubCounty", b =>
                {
                    b.HasOne("SAIS.Models.County", "County")
                        .WithMany("SubCounties")
                        .HasForeignKey("CountyID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("County");
                });

            modelBuilder.Entity("SAIS.Models.SubLocation", b =>
                {
                    b.HasOne("SAIS.Models.Location", "Location")
                        .WithMany("SubLocations")
                        .HasForeignKey("LocationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Location");
                });

            modelBuilder.Entity("SAIS.Models.Village", b =>
                {
                    b.HasOne("SAIS.Models.SubLocation", "SubLocation")
                        .WithMany("Villages")
                        .HasForeignKey("SubLocationID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SubLocation");
                });

            modelBuilder.Entity("SAIS.Models.Applicant", b =>
                {
                    b.Navigation("ApplicantPrograms");
                });

            modelBuilder.Entity("SAIS.Models.County", b =>
                {
                    b.Navigation("SubCounties");
                });

            modelBuilder.Entity("SAIS.Models.Location", b =>
                {
                    b.Navigation("SubLocations");
                });

            modelBuilder.Entity("SAIS.Models.Program_", b =>
                {
                    b.Navigation("ApplicantPrograms");
                });

            modelBuilder.Entity("SAIS.Models.SubCounty", b =>
                {
                    b.Navigation("Locations");
                });

            modelBuilder.Entity("SAIS.Models.SubLocation", b =>
                {
                    b.Navigation("Villages");
                });

            modelBuilder.Entity("SAIS.Models.Village", b =>
                {
                    b.Navigation("Applicants");
                });
#pragma warning restore 612, 618
        }
    }
}
