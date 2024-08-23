using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foodota.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateRestaurantModelColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Logo",
                table: "Restaurants",
                newName: "LogoPath");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Restaurants",
                newName: "ImagePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LogoPath",
                table: "Restaurants",
                newName: "Logo");

            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Restaurants",
                newName: "ImageUrl");
        }
    }
}
