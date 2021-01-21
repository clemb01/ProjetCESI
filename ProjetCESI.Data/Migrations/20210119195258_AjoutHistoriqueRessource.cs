using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjetCESI.Data.Migrations
{
    public partial class AjoutHistoriqueRessource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EstValide",
                table: "Ressources",
                newName: "RessourceSupprime");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DateSuppression",
                table: "Ressources",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<int>(
                name: "Statut",
                table: "Ressources",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "HistoriqueRessources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateCreation = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    RessourceId = table.Column<int>(type: "int", nullable: true),
                    Titre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Contenu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeRessourceId = table.Column<int>(type: "int", nullable: false),
                    CategorieId = table.Column<int>(type: "int", nullable: false),
                    TypeRelationSerializer = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HistoriqueRessources");

            migrationBuilder.DropColumn(
                name: "DateSuppression",
                table: "Ressources");

            migrationBuilder.DropColumn(
                name: "Statut",
                table: "Ressources");

            migrationBuilder.RenameColumn(
                name: "RessourceSupprime",
                table: "Ressources",
                newName: "EstValide");
        }
    }
}
