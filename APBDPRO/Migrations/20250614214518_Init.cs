using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace APBDPRO.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Client",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Adres = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Client", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    DateFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateTo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SellTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Software_Category",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Software_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users_Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    KRS = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Company_Client_Id",
                        column: x => x.Id,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Person",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PESEL = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Person", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Person_Client_Id",
                        column: x => x.Id,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Software",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ActualVersion = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SoftwareCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Software", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Software_Software_Category_SoftwareCategoryId",
                        column: x => x.SoftwareCategoryId,
                        principalTable: "Software_Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    IdUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RefreshTokenExp = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserRoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.IdUser);
                    table.ForeignKey(
                        name: "FK_Users_Users_Roles_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "Users_Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Discount_Software",
                columns: table => new
                {
                    SoftwareId = table.Column<int>(type: "int", nullable: false),
                    DiscountsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discount_Software", x => new { x.SoftwareId, x.DiscountsId });
                    table.ForeignKey(
                        name: "FK_Discount_Software_Discounts_DiscountsId",
                        column: x => x.DiscountsId,
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Discount_Software_Software_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "Software",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sells",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    SoftwareId = table.Column<int>(type: "int", nullable: false),
                    SellTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sells", x => new { x.ClientId, x.SoftwareId });
                    table.ForeignKey(
                        name: "FK_Sells_Client_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Client",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sells_SellTypes_SellTypeId",
                        column: x => x.SellTypeId,
                        principalTable: "SellTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Sells_Software_SoftwareId",
                        column: x => x.SoftwareId,
                        principalTable: "Software",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "Id", "Adres", "Email", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "Street 1", "client1@example.com", 111111111 },
                    { 2, "Street 2", "client2@example.com", 222222222 },
                    { 3, "Street 3", "client3@example.com", 333333333 }
                });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "Id", "DateFrom", "DateTo", "Name", "Value" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "New Year", 10 },
                    { 2, new DateTime(2025, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Summer Sale", 15 },
                    { 3, new DateTime(2025, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Black Friday", 25 }
                });

            migrationBuilder.InsertData(
                table: "SellTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Online" },
                    { 2, "Retail" },
                    { 3, "Wholesale" }
                });

            migrationBuilder.InsertData(
                table: "Software_Category",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Security" },
                    { 2, "Office" },
                    { 3, "Development" }
                });

            migrationBuilder.InsertData(
                table: "Users_Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "User" },
                    { 2, "Admin" },
                    { 3, "Moderator" }
                });

            migrationBuilder.InsertData(
                table: "Company",
                columns: new[] { "Id", "KRS", "Name" },
                values: new object[,]
                {
                    { 1, "1234567891", "Company A" },
                    { 2, "4323552342", "Company B" },
                    { 3, "2754633906", "Company C" }
                });

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "Deleted", "FirstName", "LastName", "PESEL" },
                values: new object[,]
                {
                    { 1, false, "John", "Doe", "12345678901" },
                    { 2, false, "Jane", "Smith", "23456789012" },
                    { 3, false, "Alice", "Brown", "34567890123" }
                });

            migrationBuilder.InsertData(
                table: "Software",
                columns: new[] { "Id", "ActualVersion", "Description", "Name", "SoftwareCategoryId" },
                values: new object[,]
                {
                    { 1, "1.0", "Protects from viruses", "Antivirus", 1 },
                    { 2, "2.1", "For documents", "Word Processor", 2 },
                    { 3, "3.3", "For coding", "IDE", 3 }
                });

            migrationBuilder.InsertData(
                table: "Discount_Software",
                columns: new[] { "DiscountsId", "SoftwareId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 2 },
                    { 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "Sells",
                columns: new[] { "ClientId", "SoftwareId", "SellTypeId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 2, 2 },
                    { 3, 3, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Discount_Software_DiscountsId",
                table: "Discount_Software",
                column: "DiscountsId");

            migrationBuilder.CreateIndex(
                name: "IX_Sells_SellTypeId",
                table: "Sells",
                column: "SellTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sells_SoftwareId",
                table: "Sells",
                column: "SoftwareId");

            migrationBuilder.CreateIndex(
                name: "IX_Software_SoftwareCategoryId",
                table: "Software",
                column: "SoftwareCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserRoleId",
                table: "Users",
                column: "UserRoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "Discount_Software");

            migrationBuilder.DropTable(
                name: "Person");

            migrationBuilder.DropTable(
                name: "Sells");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "Client");

            migrationBuilder.DropTable(
                name: "SellTypes");

            migrationBuilder.DropTable(
                name: "Software");

            migrationBuilder.DropTable(
                name: "Users_Roles");

            migrationBuilder.DropTable(
                name: "Software_Category");
        }
    }
}
