using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class InitialVer3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carrier",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Vat = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    InnKpp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Phones = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Emails = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carrier", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<short>(type: "smallint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    InnKpp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Phones = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Emails = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ViewNameWithExtencion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullFilePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntityType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordState = table.Column<short>(type: "smallint", nullable: false),
                    IsExpired = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TruckModel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TruckNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TrailerModel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TrailerNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Carrier_CarrierId",
                        column: x => x.CarrierId,
                        principalTable: "Carrier",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdditionalCondition",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdditionalCondition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdditionalCondition_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FamilyName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PassportSerial = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PassportDateOfIssue = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PassportIssuer = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Phones = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Drivers_Carrier_CarrierId",
                        column: x => x.CarrierId,
                        principalTable: "Carrier",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Drivers_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<short>(type: "smallint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<short>(type: "smallint", nullable: false),
                    LoadingPointId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Weight = table.Column<float>(type: "real", nullable: false),
                    Volume = table.Column<float>(type: "real", nullable: false),
                    CarrierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VehicleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LogistId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarrierPayment = table.Column<float>(type: "real", nullable: false),
                    CarrierPrepayment = table.Column<float>(type: "real", nullable: false),
                    CarrierPayPriority = table.Column<short>(type: "smallint", nullable: false),
                    CarrierPaymentCondition = table.Column<short>(type: "smallint", nullable: false),
                    ClientPayment = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_Carrier_CarrierId",
                        column: x => x.CarrierId,
                        principalTable: "Carrier",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_Company_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_Users_LogistId",
                        column: x => x.LogistId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Document",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentType = table.Column<short>(type: "smallint", nullable: false),
                    RecieveType = table.Column<short>(type: "smallint", nullable: false),
                    RecievingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentDirection = table.Column<short>(type: "smallint", nullable: false),
                    Summ = table.Column<float>(type: "real", nullable: false),
                    ContractId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Document", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Document_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DocumentDirection = table.Column<short>(type: "smallint", nullable: false),
                    Summ = table.Column<float>(type: "real", nullable: false),
                    ContractId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payment_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoutePoint",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Route = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DateAndTime = table.Column<DateTime>(type: "datetime2", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Side = table.Column<short>(type: "smallint", nullable: false),
                    Type = table.Column<short>(type: "smallint", nullable: false),
                    Phones = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContractId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutePoint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoutePoint_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalCondition_TemplateId",
                table: "AdditionalCondition",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CarrierId",
                table: "Contracts",
                column: "CarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ClientId",
                table: "Contracts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_DriverId",
                table: "Contracts",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_LoadingPointId",
                table: "Contracts",
                column: "LoadingPointId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_LogistId",
                table: "Contracts",
                column: "LogistId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_Number_CreationDate",
                table: "Contracts",
                columns: new[] { "Number", "CreationDate" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_TemplateId",
                table: "Contracts",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_VehicleId",
                table: "Contracts",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Document_ContractId",
                table: "Document",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_CarrierId",
                table: "Drivers",
                column: "CarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_Drivers_VehicleId",
                table: "Drivers",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_ContractId",
                table: "Payment",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_RoutePoint_ContractId",
                table: "RoutePoint",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Templates_Name",
                table: "Templates",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_CarrierId",
                table: "Vehicles",
                column: "CarrierId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_TruckModel_TruckNumber_TrailerModel_TrailerNumber",
                table: "Vehicles",
                columns: new[] { "TruckModel", "TruckNumber", "TrailerModel", "TrailerNumber" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_RoutePoint_LoadingPointId",
                table: "Contracts",
                column: "LoadingPointId",
                principalTable: "RoutePoint",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Templates_TemplateId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Carrier_CarrierId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Drivers_Carrier_CarrierId",
                table: "Drivers");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Carrier_CarrierId",
                table: "Vehicles");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Company_ClientId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Drivers_DriverId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_RoutePoint_LoadingPointId",
                table: "Contracts");

            migrationBuilder.DropTable(
                name: "AdditionalCondition");

            migrationBuilder.DropTable(
                name: "Document");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "Carrier");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "RoutePoint");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
