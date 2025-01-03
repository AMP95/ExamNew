using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Edit_route_points : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_RoutePoint_LoadingPointId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_RoutePoint_Contracts_ContractId",
                table: "RoutePoint");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoutePoint",
                table: "RoutePoint");

            migrationBuilder.DropColumn(
                name: "Loading",
                table: "RoutePoint");

            migrationBuilder.RenameTable(
                name: "RoutePoint",
                newName: "RoutePoints");

            migrationBuilder.RenameIndex(
                name: "IX_RoutePoint_ContractId",
                table: "RoutePoints",
                newName: "IX_RoutePoints_ContractId");

            migrationBuilder.AlterColumn<short>(
                name: "Status",
                table: "Contracts",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<short>(
                name: "PaymentCondition",
                table: "Contracts",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<short>(
                name: "PayPriority",
                table: "Contracts",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<short>(
                name: "Vat",
                table: "Carrier",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<short>(
                name: "Side",
                table: "RoutePoints",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "Type",
                table: "RoutePoints",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoutePoints",
                table: "RoutePoints",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_RoutePoints_LoadingPointId",
                table: "Contracts",
                column: "LoadingPointId",
                principalTable: "RoutePoints",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RoutePoints_Contracts_ContractId",
                table: "RoutePoints",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_RoutePoints_LoadingPointId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_RoutePoints_Contracts_ContractId",
                table: "RoutePoints");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoutePoints",
                table: "RoutePoints");

            migrationBuilder.DropColumn(
                name: "Side",
                table: "RoutePoints");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "RoutePoints");

            migrationBuilder.RenameTable(
                name: "RoutePoints",
                newName: "RoutePoint");

            migrationBuilder.RenameIndex(
                name: "IX_RoutePoints_ContractId",
                table: "RoutePoint",
                newName: "IX_RoutePoint_ContractId");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Contracts",
                type: "int",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<int>(
                name: "PaymentCondition",
                table: "Contracts",
                type: "int",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<int>(
                name: "PayPriority",
                table: "Contracts",
                type: "int",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<int>(
                name: "Vat",
                table: "Carrier",
                type: "int",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AddColumn<int>(
                name: "Loading",
                table: "RoutePoint",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoutePoint",
                table: "RoutePoint",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_RoutePoint_LoadingPointId",
                table: "Contracts",
                column: "LoadingPointId",
                principalTable: "RoutePoint",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RoutePoint_Contracts_ContractId",
                table: "RoutePoint",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id");
        }
    }
}
