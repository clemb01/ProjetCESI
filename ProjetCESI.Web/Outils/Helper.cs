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
            var metierFactory = new MetierFactory();
            await ((ApplicationRoleMetier)metierFactory.CreateApplicationRoleMetier()).GetAll(); // Permet d'initialiser les données
            await ((CategorieMetier)metierFactory.CreateCategorieMetier()).GetAll(); // Permet d'initialiser les données
            await ((TypeRelationMetier)metierFactory.CreateTypeRelationMetier()).GetAll(); // Permet d'initialiser les données
            await ((TypeRessourceMetier)metierFactory.CreateTypeRessourceMetier()).GetAll(); // Permet d'initialiser les données


            var admins = await userManager.GetUsersInRoleAsync("admin");
            if (admins.Count == 0)
            {
                var admin = new User()
                {
                    UserName = "Admin",
                    Email = "admin.admin@admin.fr",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(admin, "Admin");
                await userManager.AddToRoleAsync(admin, Enum.GetName(TypeUtilisateur.Admin));
            }
        }
    }
}
