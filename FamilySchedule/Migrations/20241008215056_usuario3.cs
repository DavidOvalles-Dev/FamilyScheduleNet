using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilySchedule.Migrations
{
    /// <inheritdoc />
    public partial class usuario3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "contraseña",
                table: "Usuarios",
                newName: "Contraseña");

            migrationBuilder.AddColumn<string>(
                name: "ConfirmarContraseña",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreDeUsuario",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmarContraseña",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "NombreDeUsuario",
                table: "Usuarios");

            migrationBuilder.RenameColumn(
                name: "Contraseña",
                table: "Usuarios",
                newName: "contraseña");
        }
    }
}
