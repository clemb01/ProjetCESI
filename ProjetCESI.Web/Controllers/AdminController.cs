using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjetCESI.Core;
using ProjetCESI.Metier.Main;
using ProjetCESI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IAdminMetier _adminMetier;

        public AdminController(UserManager<User> userManager, SignInManager<User> signInManager, IAdminMetier adminMetier) : base(userManager, signInManager)
        {
            _adminMetier = adminMetier;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserList()
        {
            List<UserViewModel> userList = new List<UserViewModel>();
            var users = MetierFactory.CreateUtilisateurMetier().GetUser();

            foreach (var user in users)
            {
                userList.Add(new UserViewModel { Utilisateur = user });
            }

            return View(userList);
        }
    }
}
