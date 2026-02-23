using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AuctionService.Migrations
{
    /// <inheritdoc />
    public partial class addeddbmigrator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "imageUrl",
                table: "Items",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "Milege",
                table: "Items",
                newName: "Mileage");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Auctions",
                newName: "AuctionEnd");

            migrationBuilder.RenameColumn(
                name: "ReservedPrice",
                table: "Auctions",
                newName: "ReservePrice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Items",
                newName: "imageUrl");

            migrationBuilder.RenameColumn(
                name: "Mileage",
                table: "Items",
                newName: "Milege");

            migrationBuilder.RenameColumn(
                name: "ReservePrice",
                table: "Auctions",
                newName: "ReservedPrice");

            migrationBuilder.RenameColumn(
                name: "AuctionEnd",
                table: "Auctions",
                newName: "UpdatedAt");
        }
    }
}
