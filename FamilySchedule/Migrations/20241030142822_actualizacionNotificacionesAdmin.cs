using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilySchedule.Migrations
{
    /// <inheritdoc />
    public partial class actualizacionNotificacionesAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Admin",
                table: "Notificaciones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Admin",
                table: "Notificaciones");
        }
    }
}
