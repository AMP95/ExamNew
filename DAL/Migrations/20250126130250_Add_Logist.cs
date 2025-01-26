using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class Add_Logist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LogistId",
                table: "Contracts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Logists",
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
                    table.PrimaryKey("PK_Logists", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_LogistId",
                table: "Contracts",
                column: "LogistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Logists_LogistId",
                table: "Contracts",
                column: "LogistId",
                principalTable: "Logists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Logists_LogistId",
                table: "Contracts");

            migrationBuilder.DropTable(
                name: "Logists");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_LogistId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "LogistId",
                table: "Contracts");
        }
    }
}
