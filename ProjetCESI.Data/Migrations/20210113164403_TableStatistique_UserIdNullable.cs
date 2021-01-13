using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetCESI.Data.Migrations
{
    public partial class TableStatistique_UserIdNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Statistiques_AspNetUsers_UtilisateurId",
                table: "Statistiques");

            migrationBuilder.AlterColumn<int>(
                name: "UtilisateurId",
                table: "Statistiques",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Statistiques_AspNetUsers_UtilisateurId",
                table: "Statistiques",
                column: "UtilisateurId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Statistiques_AspNetUsers_UtilisateurId",
                table: "Statistiques");

            migrationBuilder.AlterColumn<int>(
                name: "UtilisateurId",
                table: "Statistiques",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Statistiques_AspNetUsers_UtilisateurId",
                table: "Statistiques",
                column: "UtilisateurId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
