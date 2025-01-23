using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class EditFiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Extencion",
                table: "Files");

            migrationBuilder.RenameColumn(
                name: "Subfolder",
                table: "Files",
                newName: "FullFilePath");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Files",
                newName: "ViewNameWithExtencion");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ViewNameWithExtencion",
                table: "Files",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "FullFilePath",
                table: "Files",
                newName: "Subfolder");

            migrationBuilder.AddColumn<string>(
                name: "Extencion",
                table: "Files",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
