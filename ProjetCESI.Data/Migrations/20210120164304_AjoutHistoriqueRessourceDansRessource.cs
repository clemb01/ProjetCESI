using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetCESI.Data.Migrations
{
    public partial class AjoutHistoriqueRessourceDansRessource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commentaires_Commentaires_CommentaireParentId",
                table: "Commentaires");

            migrationBuilder.DropTable(
                name: "HistoriqueRessources");

            migrationBuilder.AddColumn<int>(
                name: "RessourceParentId",
                table: "Ressources",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ressources_RessourceParentId",
                table: "Ressources",
                column: "RessourceParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commentaires_Commentaires_CommentaireParentId",
                table: "Commentaires",
                column: "CommentaireParentId",
                principalTable: "Commentaires",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ressources_Ressources_RessourceParentId",
                table: "Ressources",
                column: "RessourceParentId",
                principalTable: "Ressources",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commentaires_Commentaires_CommentaireParentId",
                table: "Commentaires");

            migrationBuilder.DropForeignKey(
                name: "FK_Ressources_Ressources_RessourceParentId",
                table: "Ressources");

            migrationBuilder.DropIndex(
                name: "IX_Ressources_RessourceParentId",
                table: "Ressources");

            migrationBuilder.DropColumn(
                name: "RessourceParentId",
                table: "Ressources");

            migrationBuilder.CreateTable(
                name: "HistoriqueRessources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategorieId = table.Column<int>(type: "int", nullable: false),
                    Contenu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreation = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    RessourceId = table.Column<int>(type: "int", nullable: true),
                    Titre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeRelationSerializer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeRessourceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoriqueRessources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HistoriqueRessources_Categories_CategorieId",
                        column: x => x.CategorieId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HistoriqueRessources_Ressources_RessourceId",
                        column: x => x.RessourceId,
                        principalTable: "Ressources",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_HistoriqueRessources_TypeRessources_TypeRessourceId",
                        column: x => x.TypeRessourceId,
                        principalTable: "TypeRessources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HistoriqueRessources_CategorieId",
                table: "HistoriqueRessources",
                column: "CategorieId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoriqueRessources_RessourceId",
                table: "HistoriqueRessources",
                column: "RessourceId");

            migrationBuilder.CreateIndex(
                name: "IX_HistoriqueRessources_TypeRessourceId",
                table: "HistoriqueRessources",
                column: "TypeRessourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commentaires_Commentaires_CommentaireParentId",
                table: "Commentaires",
                column: "CommentaireParentId",
                principalTable: "Commentaires",
                principalColumn: "Id");
        }
    }
}
