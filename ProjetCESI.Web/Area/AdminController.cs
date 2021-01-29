using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjetCESI.Core;
using ProjetCESI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Area
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : BaseController
    {
        public async Task<List<UserViewModel>> UserList()
        {
            List<UserViewModel> userList = new List<UserViewModel>();
            var users = await MetierFactory.CreateUtilisateurMetier().GetUser();

            foreach (var user in users)
            {
                userList.Add(new UserViewModel { Utilisateur = user });
            }

            return userList;
        }

        public async Task<UserViewModel> AnonymeUser(string id)
        {
            if (id != null)
            {
                var user = await UserManager.FindByIdAsync(id);
                if (user != null)
                {
                    var model = new UserViewModel();
                    model.Utilisateur = user;

                    return model;
                }

            }
            return null;
        }

        [HttpPost]
        public async Task<GestionViewModel> Anonymise(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            bool result = await MetierFactory.CreateUtilisateurMetier().AnonymiseUser(user);

            var model = new GestionViewModel();
            model.Users = (await MetierFactory.CreateUtilisateurMetier().GetUser()).ToList();
            model.NomVue = "UserList";
            return model;
        }

        public async Task<UserViewModel> BanTempo(string id)
        {
            if (id != null)
            {
                var user = await UserManager.FindByIdAsync(id);
                if (user != null)
                {
                    var model = new UserViewModel();
                    model.Utilisateur = user;
                    return model;
                }
            }
            return null;
        }

        [HttpPost]
        public async Task<GestionViewModel> BanTemporary(string id, int time)
        {
            var user = await UserManager.FindByIdAsync(id);
            bool result = await MetierFactory.CreateUtilisateurMetier().BanUserTemporary(user, time);

            var model = new GestionViewModel();
            model.Users = (await MetierFactory.CreateUtilisateurMetier().GetUser()).ToList();
            model.NomVue = "UserList";
            return model;
        }

        [HttpPost]
        public async Task<GestionViewModel> DebanUser(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            bool result = await MetierFactory.CreateUtilisateurMetier().DeBan(user);

            var model = new GestionViewModel();
            model.Users = (await MetierFactory.CreateUtilisateurMetier().GetUser()).ToList();
            model.NomVue = "UserList";
            return model;
        }

        [HttpPost]
        public async Task<GestionViewModel> BanPermanent(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            bool result = await MetierFactory.CreateUtilisateurMetier().BanUserPermanent(user);

            var model = new GestionViewModel();
            model.Users = (await MetierFactory.CreateUtilisateurMetier().GetUser()).ToList();
            model.NomVue = "UserList";
            return model;
        }

        //public async Task<UserViewModel> UpdateRole(string id)
        //{
        //    if (id != null)
        //    {
        //        var user = await UserManager.FindByIdAsync(id);
        //        if (user != null)
        //        {
        //            var model = new UserViewModel();
        //            model.Utilisateur = user;
        //            model.Role = (await UserManager.GetRolesAsync(user)).FirstOrDefault();
        //            ViewBag.Roles = new SelectList(await MetierFactory.CreateApplicationRoleMetier().GetAll(), "Id", "Name");
        //            return model;
        //        }
        //    }
        //    return null;
        //}

        //[HttpPost]
        //public async Task<IActionResult> UpdateRole(string id, int roleid)
        //{
        //    var user = await UserManager.FindByIdAsync(id);
        //    if (user != null)
        //    {
        //        var model = new UserViewModel();
        //        model.Utilisateur = user;
        //        var result = await UserManager.RemoveFromRolesAsync(user, await UserManager.GetRolesAsync(user));
        //        var result1 = await UserManager.AddToRoleAsync(user, Enum.GetName((TypeUtilisateur)roleid));
        //        model.Role = (await UserManager.GetRolesAsync(user)).FirstOrDefault();
        //        ViewBag.Roles = new SelectList(await MetierFactory.CreateApplicationRoleMetier().GetAll(), "Id", "Name");

        //        return View(model);
        //    }
        //    return View(null);
        //}

    }  
}
