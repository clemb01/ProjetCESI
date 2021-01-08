using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetCESI.Data.Migrations
{
    public partial class ModifNomTypeRelationcategorieTypeRessource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NomRessource",
                table: "TypeRessources",
                newName: "Nom");

            migrationBuilder.RenameColumn(
                name: "NomRelation",
                table: "TypeRelations",
                newName: "Nom");

            migrationBuilder.RenameColumn(
                name: "NomCategorie",
                table: "Categories",
                newName: "Nom");

            migrationBuilder.CreateIndex(
                name: "IX_Ressources_CategorieId",
                table: "Ressources",
                column: "CategorieId");

            migrationBuilder.CreateIndex(
                name: "IX_Ressources_TypeRessourceId",
                table: "Ressources",
                column: "TypeRessourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ressources_Categories_CategorieId",
                table: "Ressources",
                column: "CategorieId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ressources_TypeRessources_TypeRessourceId",
                table: "Ressources",
                column: "TypeRessourceId",
                principalTable: "TypeRessources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ressources_Categories_CategorieId",
                table: "Ressources");

            migrationBuilder.DropForeignKey(
                name: "FK_Ressources_TypeRessources_TypeRessourceId",
                table: "Ressources");

            migrationBuilder.DropIndex(
                name: "IX_Ressources_CategorieId",
                table: "Ressources");

            migrationBuilder.DropIndex(
                name: "IX_Ressources_TypeRessourceId",
                table: "Ressources");

            migrationBuilder.RenameColumn(
                name: "Nom",
                table: "TypeRessources",
                newName: "NomRessource");

            migrationBuilder.RenameColumn(
                name: "Nom",
                table: "TypeRelations",
                newName: "NomRelation");

            migrationBuilder.RenameColumn(
                name: "Nom",
                table: "Categories",
                newName: "NomCategorie");
        }
    }
}
