using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PBL3_OnlineShop.Migrations
{
    /// <inheritdoc />
    public partial class addavatar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UrlAvatar",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UrlAvatar",
                table: "Users");
        }
    }
}
