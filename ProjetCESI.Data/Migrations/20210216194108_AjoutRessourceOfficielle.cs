using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetCESI.Data.Migrations
{
    public partial class AjoutRessourceOfficielle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RessourceOfficielle",
                table: "Ressources",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RessourceOfficielle",
                table: "Ressources");
        }
    }
}
