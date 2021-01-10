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

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> UserList()
        {
            List<UserViewModel> userList = new List<UserViewModel>();
            var users = await MetierFactory.CreateUtilisateurMetier().GetUser();

            foreach (var user in users)
            {
                userList.Add(new UserViewModel { Utilisateur = user });
            }

            return View(userList);
        }

        public async Task<IActionResult> AnonymeUser(string id)
        {
            if(id != null)
            {
                var user = await UserManager.FindByIdAsync(id);
                if (user != null)
                {
                    var model = new UserViewModel();
                    model.Utilisateur = user;

                    return View(model);
                }
                    
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Anonymise(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            bool result = await MetierFactory.CreateUtilisateurMetier().AnonymiseUser(user);
            return RedirectToAction("UserList");
        }

        public async Task<IActionResult> BanTempo(string id)
        {
            if (id != null)
            {
                var user = await UserManager.FindByIdAsync(id);
                if (user != null)
                {
                    var model = new UserViewModel();
                    model.Utilisateur = user;
                    return View(model);
                }
            }
            return View();
        }
    

        [HttpPost]
        public async Task<IActionResult> BanTemporary(string id, int time)
        {
            var user = await UserManager.FindByIdAsync(id);
            bool result = await MetierFactory.CreateUtilisateurMetier().BanUserTemporary(user, time);
            TempData["Utilisateur"] = true;
                

            return RedirectToAction("UserList");
        }

        [HttpPost]
        public async Task<IActionResult> DebanUser(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            bool result = await MetierFactory.CreateUtilisateurMetier().DeBan(user);

            return RedirectToAction("UserList");
        }

        [HttpPost]
        public async Task<IActionResult> BanPermanent(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            bool result = await MetierFactory.CreateUtilisateurMetier().BanUserPermanent(user);

            return RedirectToAction("UserList");
        }

    }
}
