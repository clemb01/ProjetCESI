using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Data.Context
{
    public class MainContext : IdentityDbContext<User, ApplicationRole, int>
    {
        public static MainContext Create()
        {
            return new MainContext();
        }

        protected string ConnectionString { get; set; }

        public MainContext() : base()
        {
            ConnectionString = "Cn1";
            ChangeTracker.LazyLoadingEnabled = false;
        }

        public MainContext(DbContextOptions<MainContext> options)
            : base(options)
        {
            ConnectionString = "Cn1";
            ChangeTracker.LazyLoadingEnabled = false;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connUrl = Environment.GetEnvironmentVariable("DATABASE_URL");

            // Parse connection URL to connection string for Npgsql
            connUrl = connUrl.Replace("postgres://", string.Empty);

            var pgUserPass = connUrl.Split("@")[0];
            var pgHostPortDb = connUrl.Split("@")[1];
            var pgHostPort = pgHostPortDb.Split("/")[0];

            var pgDb = pgHostPortDb.Split("/")[1];
            var pgUser = pgUserPass.Split(":")[0];
            var pgPass = pgUserPass.Split(":")[1];
            var pgHost = pgHostPort.Split(":")[0];
            var pgPort = pgHostPort.Split(":")[1];

            var connStr = $"Server={pgHost};Port={pgPort};User Id={pgUser};Password={pgPass};Database={pgDb};SSL Mode=Require;Trust Server Certificate=true";

            optionsBuilder.UseNpgsql(connStr);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>().HasMany(c => c.Commentaires).WithOne(c => c.Utilisateur).OnDelete(DeleteBehavior.SetNull);
            builder.Entity<User>().HasMany(c => c.RessourcesCree).WithOne(c => c.UtilisateurCreateur).OnDelete(DeleteBehavior.SetNull);
            builder.Entity<User>().HasMany(c => c.UtilisateurRessources).WithOne(c => c.Utilisateur).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Ressource>().HasMany(c => c.UtilisateurRessources).WithOne(c => c.Ressource).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Ressource>().HasMany(c => c.Commentaires).WithOne(c => c.Ressource).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Ressource>().HasMany(c => c.TypeRelationsRessources).WithOne(c => c.Ressource).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Ressource>().HasMany(c => c.HistoriqueRessource).WithOne(c => c.RessourceParent).OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Categorie>().HasMany(c => c.Ressources).WithOne(c => c.Categorie).OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Commentaire>().HasOne(c => c.CommentaireParent).WithMany(c => c.CommentairesEnfant).OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);
        }

        public DbSet<UtilisateurRessource> UtilisateurRessources { get; set; }
        public DbSet<Ressource> Ressources { get; set; }
        public DbSet<Commentaire> Commentaires { get; set; }
        public DbSet<TypeRelationRessource> TypeRelationRessources { get; set; }
        public DbSet<TypeRelation> TypeRelations { get; set; }
        public DbSet<TypeRessource> TypeRessources { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Statistique> Statistiques { get; set; }
    }
}
