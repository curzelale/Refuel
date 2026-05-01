using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Refuel.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class VehiclesPlateNickname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "licences_plate",
                table: "vehicles",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nickname",
                table: "vehicles",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "licences_plate",
                table: "vehicles");

            migrationBuilder.DropColumn(
                name: "nickname",
                table: "vehicles");
        }
    }
}
