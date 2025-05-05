using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL3_OnlineShop.Migrations
{
    /// <inheritdoc />
    public partial class PBL3AddSale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SalePercentage",
                table: "Products",
                type: "decimal(2,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SalePercentage",
                table: "Products");
        }
    }
}
