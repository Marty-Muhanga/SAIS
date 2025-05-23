using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SAIS.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApplicationStatusLookups",
                columns: table => new
                {
                    StatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationStatusLookups", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "Counties",
                columns: table => new
                {
                    CountyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountyCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CountyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Counties", x => x.CountyID);
                });

            migrationBuilder.CreateTable(
                name: "MaritalStatusLookups",
                columns: table => new
                {
                    MaritalStatusID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaritalStatusLookups", x => x.MaritalStatusID);
                });

            migrationBuilder.CreateTable(
                name: "Programs",
                columns: table => new
                {
                    ProgramID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ProgramName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.ProgramID);
                });

            migrationBuilder.CreateTable(
                name: "SearchViewModel",
                columns: table => new
                {
                    SearchTerm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusID = table.Column<int>(type: "int", nullable: true),
                    FromDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CountyID = table.Column<int>(type: "int", nullable: true),
                    SubCountyID = table.Column<int>(type: "int", nullable: true),
                    LocationID = table.Column<int>(type: "int", nullable: true),
                    SubLocationID = table.Column<int>(type: "int", nullable: true),
                    VillageID = table.Column<int>(type: "int", nullable: true),
                    CurrentPage = table.Column<int>(type: "int", nullable: false),
                    PageSize = table.Column<int>(type: "int", nullable: false),
                    TotalCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "SexLookups",
                columns: table => new
                {
                    SexID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SexName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SexCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SexLookups", x => x.SexID);
                });

            migrationBuilder.CreateTable(
                name: "SubCounties",
                columns: table => new
                {
                    SubCountyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubCountyCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SubCountyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CountyID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCounties", x => x.SubCountyID);
                    table.ForeignKey(
                        name: "FK_SubCounties_Counties_CountyID",
                        column: x => x.CountyID,
                        principalTable: "Counties",
                        principalColumn: "CountyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    LocationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LocationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SubCountyID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.LocationID);
                    table.ForeignKey(
                        name: "FK_Locations_SubCounties_SubCountyID",
                        column: x => x.SubCountyID,
                        principalTable: "SubCounties",
                        principalColumn: "SubCountyID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubLocations",
                columns: table => new
                {
                    SubLocationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubLocationCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SubLocationName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LocationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubLocations", x => x.SubLocationID);
                    table.ForeignKey(
                        name: "FK_SubLocations_Locations_LocationID",
                        column: x => x.LocationID,
                        principalTable: "Locations",
                        principalColumn: "LocationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Villages",
                columns: table => new
                {
                    VillageID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VillageCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    VillageName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SubLocationID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Villages", x => x.VillageID);
                    table.ForeignKey(
                        name: "FK_Villages_SubLocations_SubLocationID",
                        column: x => x.SubLocationID,
                        principalTable: "SubLocations",
                        principalColumn: "SubLocationID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Applicants",
                columns: table => new
                {
                    ApplicantID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MiddleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SexID = table.Column<int>(type: "int", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    MaritalStatusID = table.Column<int>(type: "int", nullable: false),
                    IDNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PostalAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PhysicalAddress = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    VillageID = table.Column<int>(type: "int", nullable: false),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    SignatureImage = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applicants", x => x.ApplicantID);
                    table.ForeignKey(
                        name: "FK_Applicants_ApplicationStatusLookups_StatusID",
                        column: x => x.StatusID,
                        principalTable: "ApplicationStatusLookups",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Applicants_MaritalStatusLookups_MaritalStatusID",
                        column: x => x.MaritalStatusID,
                        principalTable: "MaritalStatusLookups",
                        principalColumn: "MaritalStatusID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Applicants_SexLookups_SexID",
                        column: x => x.SexID,
                        principalTable: "SexLookups",
                        principalColumn: "SexID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Applicants_Villages_VillageID",
                        column: x => x.VillageID,
                        principalTable: "Villages",
                        principalColumn: "VillageID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicantPrograms",
                columns: table => new
                {
                    ApplicantProgramID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicantID = table.Column<int>(type: "int", nullable: false),
                    ProgramID = table.Column<int>(type: "int", nullable: false),
                    StatusID = table.Column<int>(type: "int", nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicantPrograms", x => x.ApplicantProgramID);
                    table.ForeignKey(
                        name: "FK_ApplicantPrograms_Applicants_ApplicantID",
                        column: x => x.ApplicantID,
                        principalTable: "Applicants",
                        principalColumn: "ApplicantID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicantPrograms_ApplicationStatusLookups_StatusID",
                        column: x => x.StatusID,
                        principalTable: "ApplicationStatusLookups",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicantPrograms_Programs_ProgramID",
                        column: x => x.ProgramID,
                        principalTable: "Programs",
                        principalColumn: "ProgramID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantPrograms_ApplicantID",
                table: "ApplicantPrograms",
                column: "ApplicantID");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantPrograms_ProgramID",
                table: "ApplicantPrograms",
                column: "ProgramID");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantPrograms_StatusID",
                table: "ApplicantPrograms",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_MaritalStatusID",
                table: "Applicants",
                column: "MaritalStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_SexID",
                table: "Applicants",
                column: "SexID");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_StatusID",
                table: "Applicants",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_Applicants_VillageID",
                table: "Applicants",
                column: "VillageID");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_SubCountyID",
                table: "Locations",
                column: "SubCountyID");

            migrationBuilder.CreateIndex(
                name: "IX_SubCounties_CountyID",
                table: "SubCounties",
                column: "CountyID");

            migrationBuilder.CreateIndex(
                name: "IX_SubLocations_LocationID",
                table: "SubLocations",
                column: "LocationID");

            migrationBuilder.CreateIndex(
                name: "IX_Villages_SubLocationID",
                table: "Villages",
                column: "SubLocationID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicantPrograms");

            migrationBuilder.DropTable(
                name: "SearchViewModel");

            migrationBuilder.DropTable(
                name: "Applicants");

            migrationBuilder.DropTable(
                name: "Programs");

            migrationBuilder.DropTable(
                name: "ApplicationStatusLookups");

            migrationBuilder.DropTable(
                name: "MaritalStatusLookups");

            migrationBuilder.DropTable(
                name: "SexLookups");

            migrationBuilder.DropTable(
                name: "Villages");

            migrationBuilder.DropTable(
                name: "SubLocations");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "SubCounties");

            migrationBuilder.DropTable(
                name: "Counties");
        }
    }
}
