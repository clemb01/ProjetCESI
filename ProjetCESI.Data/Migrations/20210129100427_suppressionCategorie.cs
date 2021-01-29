using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetCESI.Data.Migrations
{
    public partial class suppressionCategorie : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ressources_Categories_CategorieId",
                table: "Ressources");

            migrationBuilder.AddForeignKey(
                name: "FK_Ressources_Categories_CategorieId",
                table: "Ressources",
                column: "CategorieId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ressources_Categories_CategorieId",
                table: "Ressources");

            migrationBuilder.AddForeignKey(
                name: "FK_Ressources_Categories_CategorieId",
                table: "Ressources",
                column: "CategorieId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
