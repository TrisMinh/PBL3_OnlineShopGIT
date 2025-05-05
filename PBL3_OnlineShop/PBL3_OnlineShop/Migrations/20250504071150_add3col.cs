using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL3_OnlineShop.Migrations
{
    /// <inheritdoc />
    public partial class add3col : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Collections",
                table: "Products",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Colors",
                table: "Products",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "Products",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Collections",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Colors",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Products");
        }
    }
}
