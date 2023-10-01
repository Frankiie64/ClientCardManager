using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClientCardManager.Infrastructure.Persistence.Migrations
{
    public partial class AgregarColumnaActivo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Clientes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Clientes");
        }
    }
}
