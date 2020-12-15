using Microsoft.AspNetCore.Identity;
using ProjetCESI.Core;
using ProjetCESI.Metier;
using ProjetCESI.Metier.Main;
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
            var roles = await ((ApplicationRoleMetier)metierFactory.CreateApplicationRoleMetier()).GetAll();

            var admins = await userManager.GetUsersInRoleAsync("admin");
            if (admins.Count == 0)
            {
                var admin = new User()
                {
                    UserName = "Admin",
                    Email = "coladaitp19-bcl@ccicampus.fr",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(admin, "Admin");
                await userManager.AddToRoleAsync(admin, Enum.GetName(TypeUtilisateur.Admin));
            }
        }
    }
}
