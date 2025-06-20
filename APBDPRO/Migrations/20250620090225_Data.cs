using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace APBDPRO.Migrations
{
    /// <inheritdoc />
    public partial class Data : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Client",
                columns: new[] { "Id", "Adres", "Email", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "123 Main St", "client1@example.com", 123456789 },
                    { 2, "456 Elm St", "client2@example.com", 987654321 }
                });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "Id", "DateFrom", "DateTo", "Name", "Value" },
                values: new object[] { 1, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2025, 3, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Spring Sale", 20 });

            migrationBuilder.InsertData(
                table: "Software_Category",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Antivirus" },
                    { 2, "Office Suite" }
                });

            migrationBuilder.InsertData(
                table: "Status_Subscription",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Active" },
                    { 2, "Cancelled" }
                });

            migrationBuilder.InsertData(
                table: "Users_Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "User" },
                    { 2, "Admin" }
                });

            migrationBuilder.InsertData(
                table: "Company",
                columns: new[] { "Id", "KRS", "Name" },
                values: new object[] { 1, "1234567890", "Tech Corp" });

            migrationBuilder.InsertData(
                table: "Person",
                columns: new[] { "Id", "Deleted", "FirstName", "LastName", "PESEL" },
                values: new object[] { 2, false, "John", "Doe", "90010112345" });

            migrationBuilder.InsertData(
                table: "Software",
                columns: new[] { "Id", "ActualVersion", "Description", "Name", "Price", "SoftwareCategoryId" },
                values: new object[,]
                {
                    { 1, "1.2.3", "Antivirus software", "SafeGuard", 49.990000000000002, 1 },
                    { 2, "2023.1", "Office productivity suite", "OfficeMax", 89.989999999999995, 2 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "IdUser", "Login", "Password", "RefreshToken", "RefreshTokenExp", "Salt", "UserRoleId" },
                values: new object[] { 1, "admin", "rrLQWg2e++4RSCGid02OYSlPuHz21DShv+H1RPrpIRk=", "OnVJ/FbGSyIUCpdceCuCxti7b7wrg+/+TnzGohsVdLM=", new DateTime(2025, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "7FPhRi75IVLn9VcF6TMfrw==", 2 });

            migrationBuilder.InsertData(
                table: "Discount_Software",
                columns: new[] { "DiscountsId", "SoftwareId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                table: "Offers",
                columns: new[] { "Id", "ClientId", "Price", "SoftwareId" },
                values: new object[] { 1, 1, 39.990000000000002, 1 });

            migrationBuilder.InsertData(
                table: "Agreements",
                columns: new[] { "OfferId", "EndDate", "IsCanceled", "IsSigned", "SoftwareVersion", "StartDate", "YearsOfAssistance" },
                values: new object[] { 1, new DateTime(2027, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, true, "1.2.3", new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2 });

            migrationBuilder.InsertData(
                table: "Payment",
                columns: new[] { "Id", "Amount", "OfferId", "PaymentDate", "Refunded" },
                values: new object[] { 1, 39.990000000000002, 1, new DateTime(2025, 4, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), false });

            migrationBuilder.InsertData(
                table: "Subscription",
                columns: new[] { "OfferId", "Name", "PriceForFirstInstallment", "RenewalPeriodDurationInMonths", "StatusSubscriptionId" },
                values: new object[] { 1, "Annual Plan", 29.989999999999998, 12, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Agreements",
                keyColumn: "OfferId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Company",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Discount_Software",
                keyColumns: new[] { "DiscountsId", "SoftwareId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "Payment",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Person",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Software",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Status_Subscription",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Subscription",
                keyColumn: "OfferId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "IdUser",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users_Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Discounts",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Offers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Software_Category",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Status_Subscription",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users_Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Client",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Software",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Software_Category",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
