using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetCESI.Data.Migrations
{
    public partial class RessourceToutEnNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ressources_Categories_CategorieId",
                table: "Ressources");

            migrationBuilder.DropForeignKey(
                name: "FK_Ressources_TypeRessources_TypeRessourceId",
                table: "Ressources");

            migrationBuilder.AlterColumn<int>(
                name: "TypeRessourceId",
                table: "Ressources",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CategorieId",
                table: "Ressources",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Ressources_Categories_CategorieId",
                table: "Ressources",
                column: "CategorieId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ressources_TypeRessources_TypeRessourceId",
                table: "Ressources",
                column: "TypeRessourceId",
                principalTable: "TypeRessources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ressources_Categories_CategorieId",
                table: "Ressources");

            migrationBuilder.DropForeignKey(
                name: "FK_Ressources_TypeRessources_TypeRessourceId",
                table: "Ressources");

            migrationBuilder.AlterColumn<int>(
                name: "TypeRessourceId",
                table: "Ressources",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategorieId",
                table: "Ressources",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
    }
}
