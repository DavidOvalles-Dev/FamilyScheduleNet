using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilySchedule.Migrations
{
    /// <inheritdoc />
    public partial class AddingTipoNotificacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
      name: "TiposDeNotificaciones",
      columns: table => new
      {
          Id = table.Column<int>(type: "int", nullable: false)
              .Annotation("SqlServer:Identity", "1, 1"),
          TipoDeNotificacion = table.Column<string>(type: "nvarchar(max)", nullable: false)
      },
      constraints: table =>
      {
          table.PrimaryKey("PK_TiposDeNotificaciones", x => x.Id);
      });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
        name: "TiposDeNotificaciones");
        }
    }
}
