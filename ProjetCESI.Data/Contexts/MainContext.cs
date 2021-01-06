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
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configurationRoot = configurationBuilder.Build();

            optionsBuilder.UseSqlServer(configurationRoot.GetConnectionString(ConnectionString));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Ressource>().HasMany(c => c.UtilisateurRessources).WithOne(c => c.Ressource).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Ressource>().HasMany(c => c.Commentaires).WithOne(c => c.Ressource).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Ressource>().HasMany(c => c.TypeRelationsRessources).WithOne(c => c.Ressource).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<TypeRelation>().HasMany(c => c.TypeRelationsRessource).WithOne(c => c.TypeRelation).OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Commentaire>().HasMany(c => c.CommentairesEnfant).WithOne(c => c.CommentaireParent).OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(builder);
        }

        public DbSet<UtilisateurRessource> UtilisateurRessources { get; set; }
        public DbSet<Ressource> Ressources { get; set; }
        public DbSet<Commentaire> Commentaires { get; set; }
        public DbSet<TypeRelationRessource> TypeRelationRessources { get; set; }
        public DbSet<TypeRelation> TypeRelations { get; set; }
        public DbSet<TypeRessource> TypeRessources { get; set; }
        public DbSet<Categorie> Categories { get; set; }
    }
}
