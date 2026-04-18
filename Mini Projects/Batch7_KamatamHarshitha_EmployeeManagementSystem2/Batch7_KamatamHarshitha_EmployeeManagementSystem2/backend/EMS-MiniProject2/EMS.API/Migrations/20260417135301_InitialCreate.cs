using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EMS.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "CreatedAt", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8258), "$2a$11$caF4aQAwgV6XAzbNu2XgVeRBSWTALoPefESzxtO58ianwOVDZ9Lmu", "Admin", "admin" },
                    { 2, new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8260), "$2a$11$gve9PCLry7K9q23jKtdcQ.hdc0UXII8BU8jwvcSFkLCrLrwZYucL6", "Viewer", "viewer" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CreatedAt", "Department", "Designation", "Email", "FirstName", "JoinDate", "LastName", "Phone", "Salary", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8394), "Engineering", "Software Engineer", "harshitha.kamatam@gmail.com", "Harshitha", new DateTime(2021, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Kamatam", "9876543289", 950000m, "Active", new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8394) },
                    { 2, new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8400), "Marketing", "Marketing Executive", "santhosh.kamatam@yahoo.com", "Santhosh", new DateTime(2020, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Kamatam", "9123456790", 680000m, "Active", new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8400) },
                    { 3, new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8404), "HR", "HR Executive", "goutham.kamatam@outlook.com", "Goutham", new DateTime(2019, 11, 20, 0, 0, 0, 0, DateTimeKind.Utc), "Kamatam", "9876512398", 620000m, "Active", new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8404) },
                    { 4, new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8408), "Finance", "Financial Analyst", "rishitha.kola@gmail.com", "Rishitha", new DateTime(2021, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Kola", "9989929004", 990000m, "Active", new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8408) },
                    { 5, new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8411), "Operations", "Supply chain", "sandy.kalisetty@gmail.com", "Sandeeep", new DateTime(2021, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Kalisetty", "8765429876", 950000m, "Active", new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8412) },
                    { 6, new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8415), "Engineering", "Software Engineer", "rana.thota@gmail.com", "Ranaveer", new DateTime(2021, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Thota", "9872347658", 850000m, "Active", new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8415) },
                    { 7, new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8418), "Marketing", "Marketing Executive", "thaswin.miriyala@gmail.com", "Thaswin", new DateTime(2021, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Miriyala", "92244637863", 950000m, "Active", new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8419) },
                    { 8, new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8422), "Hr", "Hr Executive", "parthi.miriyala@gmail.com", "Parthi", new DateTime(2021, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Miriyala", "9933452133", 750000m, "Active", new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8422) },
                    { 9, new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8426), "Finance", "Financial Analyst", "jaya.km@gmail.com", "Jaya", new DateTime(2021, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "MirKam", "8899554474", 650000m, "Active", new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8426) },
                    { 10, new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8429), "Operations", "Supply chain", "veeri.korsipati@gmail.com", "Veeri", new DateTime(2021, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Korsipati", "9888226608", 850000m, "Active", new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8430) },
                    { 11, new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8433), "Engineering", "Software Engineer", "yash.thota@gmail.com", "Yaswanth", new DateTime(2021, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Thota", "9887351437", 450000m, "Active", new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8433) },
                    { 12, new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8437), "Hr", "HR Executive", "krishna.parlapalli@gmail.com", "Krishna", new DateTime(2021, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Parlapalli", "99977553311", 950000m, "Active", new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8437) },
                    { 13, new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8440), "Engineering", "Software Engineer", "ram.garre@gmail.com", "Ram", new DateTime(2021, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Garre", "9886644645", 880000m, "Active", new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8441) },
                    { 14, new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8444), "Engineering", "Software Engineer", "lucky.kamatam@gmail.com", "Lucky", new DateTime(2021, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Kamatam", "9009865124", 990000m, "Active", new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8444) },
                    { 15, new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8447), "Hr", "HR Executive", "hanu.mara@gmail.com", "Hanu", new DateTime(2021, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Mara", "9974672431", 950000m, "Active", new DateTime(2026, 4, 17, 13, 52, 53, 603, DateTimeKind.Utc).AddTicks(8448) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_Username",
                table: "AppUsers",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
