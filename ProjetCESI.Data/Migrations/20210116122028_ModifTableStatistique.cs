using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetCESI.Data.Migrations
{
    public partial class ModifTableStatistique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RechercheEffectue",
                table: "Statistiques",
                newName: "Parametre");

            migrationBuilder.RenameColumn(
                name: "ParametreRecherche",
                table: "Statistiques",
                newName: "Controller");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "Statistiques",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Action",
                table: "Statistiques");

            migrationBuilder.RenameColumn(
                name: "Parametre",
                table: "Statistiques",
                newName: "RechercheEffectue");

            migrationBuilder.RenameColumn(
                name: "Controller",
                table: "Statistiques",
                newName: "ParametreRecherche");
        }
    }
}
