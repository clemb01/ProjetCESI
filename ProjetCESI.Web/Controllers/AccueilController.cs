using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjetCESI.Models;
using ProjetCESI.Web.Controllers;
using ProjetCESI.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Controllers
{
    [Authorize]
    public class AccueilController : BaseController
    {
        public AccueilController()
        {
        }

        public IActionResult Accueil()
        {
            List<UserViewModel> userList = new List<UserViewModel>();
            var users = MetierFactory.CreateUtilisateurMetier().GetUser();

            foreach (var user in users)
            {
                userList.Add(new UserViewModel { Utilisateur = user });
            }

            return View(userList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
