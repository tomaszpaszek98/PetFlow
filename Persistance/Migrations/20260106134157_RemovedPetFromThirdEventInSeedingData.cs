using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PetFlow.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemovedPetFromThirdEventInSeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PetEvents",
                keyColumn: "Id",
                keyValue: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PetEvents",
                columns: new[] { "Id", "Created", "CreatedBy", "EventId", "Inactivated", "InactivatedBy", "Modified", "ModifiedBy", "PetId", "StatusId" },
                values: new object[] { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "system", 3, null, null, null, null, 3, 1 });
        }
    }
}
