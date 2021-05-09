﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ProjetCESI.Data.Context;

namespace ProjetCESI.Data.Migrations
{
    [DbContext(typeof(MainContext))]
    partial class MainContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ProjetCESI.Core.ApplicationRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("ProjetCESI.Core.Categorie", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Nom")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("ProjetCESI.Core.Commentaire", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int?>("CommentaireParentId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("DateCreation")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("DateModification")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset?>("DateSuppression")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("RessourceId")
                        .HasColumnType("integer");

                    b.Property<int>("Statut")
                        .HasColumnType("integer");

                    b.Property<string>("Texte")
                        .HasColumnType("text");

                    b.Property<int?>("UtilisateurId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CommentaireParentId");

                    b.HasIndex("RessourceId");

                    b.HasIndex("UtilisateurId");

                    b.ToTable("Commentaires");
                });

            modelBuilder.Entity("ProjetCESI.Core.Ressource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int?>("CategorieId")
                        .HasColumnType("integer");

                    b.Property<string>("Contenu")
                        .HasColumnType("text");

                    b.Property<string>("ContenuOriginal")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("DateCreation")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("DateModification")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTimeOffset>("DateSuppression")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("KeyLink")
                        .HasColumnType("text");

                    b.Property<int>("NombreConsultation")
                        .HasColumnType("integer");

                    b.Property<bool>("RessourceOfficielle")
                        .HasColumnType("boolean");

                    b.Property<int?>("RessourceParentId")
                        .HasColumnType("integer");

                    b.Property<bool>("RessourceSupprime")
                        .HasColumnType("boolean");

                    b.Property<int>("Statut")
                        .HasColumnType("integer");

                    b.Property<string>("Titre")
                        .HasColumnType("text");

                    b.Property<int>("TypePartage")
                        .HasColumnType("integer");

                    b.Property<int?>("TypeRessourceId")
                        .HasColumnType("integer");

                    b.Property<int?>("UtilisateurCreateurId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CategorieId");

                    b.HasIndex("RessourceParentId");

                    b.HasIndex("TypeRessourceId");

                    b.HasIndex("UtilisateurCreateurId");

                    b.ToTable("Ressources");
                });

            modelBuilder.Entity("ProjetCESI.Core.Statistique", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Action")
                        .HasColumnType("text");

                    b.Property<string>("Controller")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("DateRecherche")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Parametre")
                        .HasColumnType("text");

                    b.Property<int?>("UtilisateurId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UtilisateurId");

                    b.ToTable("Statistiques");
                });

            modelBuilder.Entity("ProjetCESI.Core.TypeRelation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Nom")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TypeRelations");
                });

            modelBuilder.Entity("ProjetCESI.Core.TypeRelationRessource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("RessourceId")
                        .HasColumnType("integer");

                    b.Property<int>("TypeRelationId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RessourceId");

                    b.HasIndex("TypeRelationId");

                    b.ToTable("TypeRelationRessources");
                });

            modelBuilder.Entity("ProjetCESI.Core.TypeRessource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Nom")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TypeRessources");
                });

            modelBuilder.Entity("ProjetCESI.Core.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("UtilisateurSupprime")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("ProjetCESI.Core.UtilisateurRessource", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<bool>("EstExploite")
                        .HasColumnType("boolean");

                    b.Property<bool>("EstFavoris")
                        .HasColumnType("boolean");

                    b.Property<bool>("EstMisDeCote")
                        .HasColumnType("boolean");

                    b.Property<int>("RessourceId")
                        .HasColumnType("integer");

                    b.Property<int?>("StatutActivite")
                        .HasColumnType("integer");

                    b.Property<int>("UtilisateurId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RessourceId");

                    b.HasIndex("UtilisateurId");

                    b.ToTable("UtilisateurRessources");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("ProjetCESI.Core.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("ProjetCESI.Core.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("ProjetCESI.Core.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("ProjetCESI.Core.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjetCESI.Core.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("ProjetCESI.Core.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ProjetCESI.Core.Commentaire", b =>
                {
                    b.HasOne("ProjetCESI.Core.Commentaire", "CommentaireParent")
                        .WithMany("CommentairesEnfant")
                        .HasForeignKey("CommentaireParentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ProjetCESI.Core.Ressource", "Ressource")
                        .WithMany("Commentaires")
                        .HasForeignKey("RessourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjetCESI.Core.User", "Utilisateur")
                        .WithMany("Commentaires")
                        .HasForeignKey("UtilisateurId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("CommentaireParent");

                    b.Navigation("Ressource");

                    b.Navigation("Utilisateur");
                });

            modelBuilder.Entity("ProjetCESI.Core.Ressource", b =>
                {
                    b.HasOne("ProjetCESI.Core.Categorie", "Categorie")
                        .WithMany("Ressources")
                        .HasForeignKey("CategorieId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("ProjetCESI.Core.Ressource", "RessourceParent")
                        .WithMany("HistoriqueRessource")
                        .HasForeignKey("RessourceParentId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ProjetCESI.Core.TypeRessource", "TypeRessource")
                        .WithMany()
                        .HasForeignKey("TypeRessourceId");

                    b.HasOne("ProjetCESI.Core.User", "UtilisateurCreateur")
                        .WithMany("RessourcesCree")
                        .HasForeignKey("UtilisateurCreateurId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Categorie");

                    b.Navigation("RessourceParent");

                    b.Navigation("TypeRessource");

                    b.Navigation("UtilisateurCreateur");
                });

            modelBuilder.Entity("ProjetCESI.Core.Statistique", b =>
                {
                    b.HasOne("ProjetCESI.Core.User", "Utilisateur")
                        .WithMany()
                        .HasForeignKey("UtilisateurId");

                    b.Navigation("Utilisateur");
                });

            modelBuilder.Entity("ProjetCESI.Core.TypeRelationRessource", b =>
                {
                    b.HasOne("ProjetCESI.Core.Ressource", "Ressource")
                        .WithMany("TypeRelationsRessources")
                        .HasForeignKey("RessourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjetCESI.Core.TypeRelation", "TypeRelation")
                        .WithMany("TypeRelationsRessource")
                        .HasForeignKey("TypeRelationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ressource");

                    b.Navigation("TypeRelation");
                });

            modelBuilder.Entity("ProjetCESI.Core.UtilisateurRessource", b =>
                {
                    b.HasOne("ProjetCESI.Core.Ressource", "Ressource")
                        .WithMany("UtilisateurRessources")
                        .HasForeignKey("RessourceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ProjetCESI.Core.User", "Utilisateur")
                        .WithMany("UtilisateurRessources")
                        .HasForeignKey("UtilisateurId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ressource");

                    b.Navigation("Utilisateur");
                });

            modelBuilder.Entity("ProjetCESI.Core.Categorie", b =>
                {
                    b.Navigation("Ressources");
                });

            modelBuilder.Entity("ProjetCESI.Core.Commentaire", b =>
                {
                    b.Navigation("CommentairesEnfant");
                });

            modelBuilder.Entity("ProjetCESI.Core.Ressource", b =>
                {
                    b.Navigation("Commentaires");

                    b.Navigation("HistoriqueRessource");

                    b.Navigation("TypeRelationsRessources");

                    b.Navigation("UtilisateurRessources");
                });

            modelBuilder.Entity("ProjetCESI.Core.TypeRelation", b =>
                {
                    b.Navigation("TypeRelationsRessource");
                });

            modelBuilder.Entity("ProjetCESI.Core.User", b =>
                {
                    b.Navigation("Commentaires");

                    b.Navigation("RessourcesCree");

                    b.Navigation("UtilisateurRessources");
                });
#pragma warning restore 612, 618
        }
    }
}
