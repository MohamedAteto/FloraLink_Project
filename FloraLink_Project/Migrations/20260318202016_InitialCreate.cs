using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FloraLink.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlantTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MinMoisture = table.Column<double>(type: "float", nullable: false),
                    MaxMoisture = table.Column<double>(type: "float", nullable: false),
                    MinTemperature = table.Column<double>(type: "float", nullable: false),
                    MaxTemperature = table.Column<double>(type: "float", nullable: false),
                    CriticalMoistureThreshold = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SensorId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PlantTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plants_PlantTypes_PlantTypeId",
                        column: x => x.PlantTypeId,
                        principalTable: "PlantTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Plants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantId = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alerts_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlantDiaryEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlantDiaryEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlantDiaryEntries_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SensorReadings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantId = table.Column<int>(type: "int", nullable: false),
                    SoilMoisture = table.Column<double>(type: "float", nullable: false),
                    Temperature = table.Column<double>(type: "float", nullable: false),
                    RecordedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HealthScore = table.Column<double>(type: "float", nullable: false),
                    PlantStatus = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorReadings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SensorReadings_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WateringEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlantId = table.Column<int>(type: "int", nullable: false),
                    WateredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WaterAmountMl = table.Column<double>(type: "float", nullable: false),
                    IsAutomatic = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WateringEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WateringEvents_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PlantTypes",
                columns: new[] { "Id", "CriticalMoistureThreshold", "Description", "MaxMoisture", "MaxTemperature", "MinMoisture", "MinTemperature", "Name" },
                values: new object[,]
                {
                    { 1, 10.0, "Desert succulent, very drought tolerant", 40.0, 35.0, 20.0, 20.0, "Cactus" },
                    { 2, 40.0, "Tropical plants needing high humidity", 80.0, 30.0, 60.0, 18.0, "Tropical" },
                    { 3, 30.0, "Kitchen herbs like basil and mint", 70.0, 28.0, 50.0, 15.0, "Herb" },
                    { 4, 50.0, "Shade-loving moisture-hungry ferns", 90.0, 25.0, 70.0, 15.0, "Fern" },
                    { 5, 15.0, "Drought-tolerant succulent plants", 50.0, 32.0, 25.0, 15.0, "Succulent" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_PlantId",
                table: "Alerts",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_PlantDiaryEntries_PlantId",
                table: "PlantDiaryEntries",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_Plants_PlantTypeId",
                table: "Plants",
                column: "PlantTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Plants_UserId",
                table: "Plants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SensorReadings_PlantId",
                table: "SensorReadings",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WateringEvents_PlantId",
                table: "WateringEvents",
                column: "PlantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alerts");

            migrationBuilder.DropTable(
                name: "PlantDiaryEntries");

            migrationBuilder.DropTable(
                name: "SensorReadings");

            migrationBuilder.DropTable(
                name: "WateringEvents");

            migrationBuilder.DropTable(
                name: "Plants");

            migrationBuilder.DropTable(
                name: "PlantTypes");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
