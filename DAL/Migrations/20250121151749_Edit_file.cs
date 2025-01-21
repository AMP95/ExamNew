using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Edit_file : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_File_FileId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Templates_File_FileId",
                table: "Templates");

            migrationBuilder.DropTable(
                name: "File");

            migrationBuilder.DropIndex(
                name: "IX_Templates_FileId",
                table: "Templates");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_FileId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Templates");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Contracts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FileId",
                table: "Templates",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "FileId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "File",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SaveName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_File", x => x.Id);
                    table.ForeignKey(
                        name: "FK_File_Carrier_CarrierId",
                        column: x => x.CarrierId,
                        principalTable: "Carrier",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_File_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_File_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Templates_FileId",
                table: "Templates",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_FileId",
                table: "Contracts",
                column: "FileId");

            migrationBuilder.CreateIndex(
                name: "IX_File_CarrierId",
                table: "File",
                column: "CarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_File_DriverId",
                table: "File",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_File_SaveName",
                table: "File",
                column: "SaveName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_File_VehicleId",
                table: "File",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_File_FileId",
                table: "Contracts",
                column: "FileId",
                principalTable: "File",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Templates_File_FileId",
                table: "Templates",
                column: "FileId",
                principalTable: "File",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
