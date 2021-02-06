using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetCESI.Data.Migrations
{
    public partial class AjoutStatutActiviteTableUtilisateurRessource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatutActivite",
                table: "UtilisateurRessources",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatutActivite",
                table: "UtilisateurRessources");
        }
    }
}
