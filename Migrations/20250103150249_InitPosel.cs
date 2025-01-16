using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DawidKrolikiewiczProj.Migrations
{
    /// <inheritdoc />
    public partial class InitPosel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SecondName",
                table: "Posel",
                newName: "Profession");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Profession",
                table: "Posel",
                newName: "SecondName");
        }
    }
}
