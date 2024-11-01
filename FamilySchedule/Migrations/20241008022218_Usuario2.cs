using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilySchedule.Migrations
{
    /// <inheritdoc />
    public partial class Usuario2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "contraseña",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "contraseña",
                table: "Usuarios");
        }
    }
}
