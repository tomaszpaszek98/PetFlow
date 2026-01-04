using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PetFlow.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    DateOfEvent = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Reminder = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    StatusId = table.Column<int>(type: "INTEGER", nullable: false),
                    InactivatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Inactivated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 1),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Species = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    Breed = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PhotoUrl = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    StatusId = table.Column<int>(type: "INTEGER", nullable: false),
                    InactivatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Inactivated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicalNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    PetId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    StatusId = table.Column<int>(type: "INTEGER", nullable: false),
                    InactivatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Inactivated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalNotes_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Content = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    Type = table.Column<int>(type: "INTEGER", maxLength: 50, nullable: false),
                    PetId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    StatusId = table.Column<int>(type: "INTEGER", nullable: false),
                    InactivatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Inactivated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PetEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PetId = table.Column<int>(type: "INTEGER", nullable: false),
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Modified = table.Column<DateTime>(type: "TEXT", nullable: true),
                    StatusId = table.Column<int>(type: "INTEGER", nullable: false),
                    InactivatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    Inactivated = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PetEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PetEvents_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PetEvents_Pets_PetId",
                        column: x => x.PetId,
                        principalTable: "Pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Created", "CreatedBy", "DateOfEvent", "Description", "Inactivated", "InactivatedBy", "Modified", "ModifiedBy", "Reminder", "StatusId", "Title" },
                values: new object[] { 1, new DateTime(2026, 1, 3, 22, 44, 57, 25, DateTimeKind.Utc).AddTicks(4953), "3.01.2026 22:44:57", new DateTime(2026, 1, 10, 22, 44, 57, 25, DateTimeKind.Utc).AddTicks(5747), "Annual checkup for Max", null, null, null, null, true, 1, "Vet Appointment" });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Created", "CreatedBy", "DateOfEvent", "Description", "Inactivated", "InactivatedBy", "Modified", "ModifiedBy", "StatusId", "Title" },
                values: new object[] { 2, new DateTime(2026, 1, 3, 22, 44, 57, 25, DateTimeKind.Utc).AddTicks(4953), "3.01.2026 22:44:57", new DateTime(2026, 1, 6, 22, 44, 57, 25, DateTimeKind.Utc).AddTicks(6078), "Bath and nail trimming for Whiskers", null, null, null, null, 1, "Grooming Session" });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "Created", "CreatedBy", "DateOfEvent", "Description", "Inactivated", "InactivatedBy", "Modified", "ModifiedBy", "Reminder", "StatusId", "Title" },
                values: new object[] { 3, new DateTime(2026, 1, 3, 22, 44, 57, 25, DateTimeKind.Utc).AddTicks(4953), "3.01.2026 22:44:57", new DateTime(2026, 1, 17, 22, 44, 57, 25, DateTimeKind.Utc).AddTicks(6096), "Rabies vaccination for Buddy", null, null, null, null, true, 1, "Vaccination" });

            migrationBuilder.InsertData(
                table: "Pets",
                columns: new[] { "Id", "Breed", "Created", "CreatedBy", "DateOfBirth", "Inactivated", "InactivatedBy", "Modified", "ModifiedBy", "Name", "PhotoUrl", "Species", "StatusId", "UserId" },
                values: new object[,]
                {
                    { 1, "Golden Retriever", new DateTime(2026, 1, 3, 22, 44, 57, 20, DateTimeKind.Utc).AddTicks(4271), "3.01.2026 22:44:57", new DateTime(2020, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, "Max", "https://example.com/max.jpg", "Dog", 1, 1 },
                    { 2, "British Shorthair", new DateTime(2026, 1, 3, 22, 44, 57, 20, DateTimeKind.Utc).AddTicks(4271), "3.01.2026 22:44:57", new DateTime(2019, 7, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, "Whiskers", "https://example.com/whiskers.jpg", "Cat", 1, 1 },
                    { 3, "Labrador Retriever", new DateTime(2026, 1, 3, 22, 44, 57, 20, DateTimeKind.Utc).AddTicks(4271), "3.01.2026 22:44:57", new DateTime(2021, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, "Buddy", null, "Dog", 1, 1 }
                });

            migrationBuilder.InsertData(
                table: "MedicalNotes",
                columns: new[] { "Id", "Created", "CreatedBy", "Description", "Inactivated", "InactivatedBy", "Modified", "ModifiedBy", "PetId", "StatusId", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 3, 22, 44, 57, 25, DateTimeKind.Utc).AddTicks(8684), "3.01.2026 22:44:57", "Started antibiotic treatment for Max's ear infection", null, null, null, null, 1, 1, "Ear Infection Treatment" },
                    { 2, new DateTime(2026, 1, 3, 22, 44, 57, 25, DateTimeKind.Utc).AddTicks(8684), "3.01.2026 22:44:57", "Professional dental cleaning performed for Whiskers", null, null, null, null, 2, 1, "Dental Cleaning" },
                    { 3, new DateTime(2026, 1, 3, 22, 44, 57, 25, DateTimeKind.Utc).AddTicks(8684), "3.01.2026 22:44:57", "Buddy needs to reduce weight by 2kg - started new diet plan", null, null, null, null, 3, 1, "Weight Management" }
                });

            migrationBuilder.InsertData(
                table: "Notes",
                columns: new[] { "Id", "Content", "Created", "CreatedBy", "Inactivated", "InactivatedBy", "Modified", "ModifiedBy", "PetId", "StatusId", "Type" },
                values: new object[,]
                {
                    { 1, "Max seems to have good energy today", new DateTime(2026, 1, 3, 22, 44, 57, 25, DateTimeKind.Utc).AddTicks(7245), "3.01.2026 22:44:57", null, null, null, null, 1, 1, 1 },
                    { 2, "Whiskers was very playful during the afternoon", new DateTime(2026, 1, 3, 22, 44, 57, 25, DateTimeKind.Utc).AddTicks(7245), "3.01.2026 22:44:57", null, null, null, null, 2, 1, 0 },
                    { 3, "Buddy ate all his meals today", new DateTime(2026, 1, 3, 22, 44, 57, 25, DateTimeKind.Utc).AddTicks(7245), "3.01.2026 22:44:57", null, null, null, null, 3, 1, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MedicalNotes_PetId",
                table: "MedicalNotes",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_PetId",
                table: "Notes",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_PetEvents_EventId",
                table: "PetEvents",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_PetEvents_PetId",
                table: "PetEvents",
                column: "PetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MedicalNotes");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "PetEvents");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Pets");
        }
    }
}
