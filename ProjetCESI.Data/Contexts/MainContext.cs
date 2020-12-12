using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjetCESI.Data.Context
{
    public class MainContext : IdentityDbContext<User>
    {
        public static MainContext Create()
        {
            return new MainContext();
        }

        protected string ConnectionString { get; set; }

        public MainContext() : base()
        {
            ConnectionString = "Main";
            //Database.EnsureCreated();
        }

        public MainContext(DbContextOptions<MainContext> options)
            : base(options)
        {
            ConnectionString = "Main";
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory).AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            IConfigurationRoot configurationRoot = configurationBuilder.Build();

            optionsBuilder.UseSqlServer(configurationRoot.GetConnectionString(ConnectionString));
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<User> Utilisateurs { get; set; }
    }
}
