using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Refuel.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "fuels",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_fuels", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "gas_stations",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    address = table.Column<string>(type: "TEXT", nullable: false),
                    latitude = table.Column<double>(type: "REAL", nullable: false),
                    longitude = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gas_stations", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "vehicles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    brand = table.Column<string>(type: "TEXT", nullable: false),
                    model = table.Column<string>(type: "TEXT", nullable: false),
                    owner = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "gas_station_fuels",
                columns: table => new
                {
                    fuel_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    gas_station_id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gas_station_fuels", x => new { x.fuel_id, x.gas_station_id });
                    table.ForeignKey(
                        name: "FK_gas_station_fuels_fuels_fuel_id",
                        column: x => x.fuel_id,
                        principalTable: "fuels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_gas_station_fuels_gas_stations_gas_station_id",
                        column: x => x.gas_station_id,
                        principalTable: "gas_stations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "refuels",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    vehicle_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    gas_station_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    fuel_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    quantity = table.Column<double>(type: "REAL", nullable: false),
                    total_price = table.Column<double>(type: "REAL", nullable: false),
                    date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    odometer_km = table.Column<float>(type: "REAL", nullable: false),
                    note = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refuels", x => x.id);
                    table.ForeignKey(
                        name: "FK_refuels_fuels_fuel_id",
                        column: x => x.fuel_id,
                        principalTable: "fuels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_refuels_gas_stations_gas_station_id",
                        column: x => x.gas_station_id,
                        principalTable: "gas_stations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_refuels_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "vehicle_fuels",
                columns: table => new
                {
                    fuel_id = table.Column<Guid>(type: "TEXT", nullable: false),
                    vehicle_id = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicle_fuels", x => new { x.fuel_id, x.vehicle_id });
                    table.ForeignKey(
                        name: "FK_vehicle_fuels_fuels_fuel_id",
                        column: x => x.fuel_id,
                        principalTable: "fuels",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_vehicle_fuels_vehicles_vehicle_id",
                        column: x => x.vehicle_id,
                        principalTable: "vehicles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_gas_station_fuels_gas_station_id",
                table: "gas_station_fuels",
                column: "gas_station_id");

            migrationBuilder.CreateIndex(
                name: "IX_refuels_fuel_id",
                table: "refuels",
                column: "fuel_id");

            migrationBuilder.CreateIndex(
                name: "IX_refuels_gas_station_id",
                table: "refuels",
                column: "gas_station_id");

            migrationBuilder.CreateIndex(
                name: "IX_refuels_vehicle_id",
                table: "refuels",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "IX_vehicle_fuels_vehicle_id",
                table: "vehicle_fuels",
                column: "vehicle_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "gas_station_fuels");

            migrationBuilder.DropTable(
                name: "refuels");

            migrationBuilder.DropTable(
                name: "vehicle_fuels");

            migrationBuilder.DropTable(
                name: "gas_stations");

            migrationBuilder.DropTable(
                name: "fuels");

            migrationBuilder.DropTable(
                name: "vehicles");
        }
    }
}
