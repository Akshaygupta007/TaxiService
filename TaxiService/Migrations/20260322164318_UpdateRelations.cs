using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxiService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VehicleId",
                table: "Vehicles",
                newName: "VehicleID");

            migrationBuilder.RenameColumn(
                name: "DriverID",
                table: "Vehicles",
                newName: "ManufactureYear");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DriverVehicles",
                columns: table => new
                {
                    DriverID = table.Column<int>(type: "int", nullable: false),
                    VehicleID = table.Column<int>(type: "int", nullable: false),
                    IsPrimaryDriver = table.Column<bool>(type: "bit", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverVehicles", x => new { x.DriverID, x.VehicleID });
                    table.ForeignKey(
                        name: "FK_DriverVehicles_Drivers_DriverID",
                        column: x => x.DriverID,
                        principalTable: "Drivers",
                        principalColumn: "DriverID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DriverVehicles_Vehicles_VehicleID",
                        column: x => x.VehicleID,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CabTypeID",
                table: "Vehicles",
                column: "CabTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_DriverVehicles_VehicleID",
                table: "DriverVehicles",
                column: "VehicleID");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_CabTypes_CabTypeID",
                table: "Vehicles",
                column: "CabTypeID",
                principalTable: "CabTypes",
                principalColumn: "CabTypeID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_CabTypes_CabTypeID",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "DriverVehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_CabTypeID",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Vehicles");

            migrationBuilder.RenameColumn(
                name: "VehicleID",
                table: "Vehicles",
                newName: "VehicleId");

            migrationBuilder.RenameColumn(
                name: "ManufactureYear",
                table: "Vehicles",
                newName: "DriverID");
        }
    }
}
