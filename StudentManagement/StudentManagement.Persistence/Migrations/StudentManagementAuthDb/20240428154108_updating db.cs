using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentManagement.Persistence.Migrations.StudentManagementAuthDb
{
    /// <inheritdoc />
    public partial class updatingdb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "474017db-ea4c-4f89-9889-f152dc3608b1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "98073817-0015-45be-b602-9cdfce289440");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0de3d955-c52f-471a-a1a3-5c73cf97e945", "0de3d955-c52f-471a-a1a3-5c73cf97e945", "Reader", "READER" },
                    { "b6771761-ff52-4f56-ac6a-2733feb1eaf5", "b6771761-ff52-4f56-ac6a-2733feb1eaf5", "Writer", "WRITER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0de3d955-c52f-471a-a1a3-5c73cf97e945");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b6771761-ff52-4f56-ac6a-2733feb1eaf5");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "474017db-ea4c-4f89-9889-f152dc3608b1", "474017db-ea4c-4f89-9889-f152dc3608b1", "Reader", "READER" },
                    { "98073817-0015-45be-b602-9cdfce289440", "98073817-0015-45be-b602-9cdfce289440", "Writer", "WRITER" }
                });
        }
    }
}
