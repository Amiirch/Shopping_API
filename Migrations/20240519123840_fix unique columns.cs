using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shopping_API.Migrations
{
    /// <inheritdoc />
    public partial class fixuniquecolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_email_userName_phoneNumber",
                table: "Users",
                columns: new[] { "email", "userName", "phoneNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_name",
                table: "Products",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_email_userName_phoneNumber",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Products_name",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");
        }
    }
}
