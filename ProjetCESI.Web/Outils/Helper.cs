using Microsoft.AspNetCore.Identity;
using ProjetCESI.Core;
using ProjetCESI.Metier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Outils
{
    public class Helper
    {
        public static async Task Seed(UserManager<User> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            var metierFactory = new MetierFactory(null);
            await ((ApplicationRoleMetier)metierFactory.CreateApplicationRoleMetier()).GetAll(); // Permet d'initialiser les données
            await ((CategorieMetier)metierFactory.CreateCategorieMetier()).GetAll(); // Permet d'initialiser les données
            await ((TypeRelationMetier)metierFactory.CreateTypeRelationMetier()).GetAll(); // Permet d'initialiser les données
            await ((TypeRessourceMetier)metierFactory.CreateTypeRessourceMetier()).GetAll(); // Permet d'initialiser les données

            var admin = await userManager.FindByNameAsync("Admin");
            var superAdmin = await userManager.FindByNameAsync("SuperAdmin");
            var modo = await userManager.FindByNameAsync("Moderateur");
            var user = await userManager.FindByNameAsync("Citoyen");

            if (superAdmin == null)
            {
                superAdmin = new User()
                {
                    UserName = "superAdmin",
                    Email = "super.admin@superadmin.fr",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(superAdmin, "Azerty@153!");
                await userManager.AddToRoleAsync(superAdmin, Enum.GetName(TypeUtilisateur.SuperAdmin));
            }

            if (admin == null)
            {
                admin = new User()
                {
                    UserName = "Admin",
                    Email = "admin.admin@admin.fr",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(admin, "Azerty@153!");
                await userManager.AddToRoleAsync(admin, Enum.GetName(TypeUtilisateur.Admin));
            }

            if (modo == null)
            {
                modo = new User()
                {
                    UserName = "Moderateur",
                    Email = "modo@modo.fr",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(modo, "Azerty@153!");
                await userManager.AddToRoleAsync(modo, Enum.GetName(TypeUtilisateur.Moderateur));
            }

            if (user == null)
            {
                user = new User()
                {
                    UserName = "Citoyen",
                    Email = "citoyen@citoyen.fr",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(user, "Azerty@153!");
                await userManager.AddToRoleAsync(user, Enum.GetName(TypeUtilisateur.Citoyen));
            }
        }
    }
}
