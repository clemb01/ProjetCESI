using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AdminAPIController : BaseAPIController
    {
        [HttpGet("UserList")]
        public async Task<ResponseAPI> UserList()
        {
            var response = new ResponseAPI();

            List<UserViewModel> userList = new List<UserViewModel>();
            var users = await MetierFactory.CreateUtilisateurMetier().GetUser();

            foreach (var user in users)
            {
                userList.Add(new UserViewModel { Utilisateur = user });
            }

            response.StatusCode = "200";
            response.Data = userList;

            return response;
        }

        [HttpGet("AnonymeUser")]
        public async Task<ResponseAPI> AnonymeUser(string id)
        {
            var response = new ResponseAPI();

            if (id != null)
            {
                var user = await UserManager.FindByIdAsync(id);
                if (user != null)
                {
                    var model = new UserViewModel();
                    model.Utilisateur = user;

                    response.StatusCode = "200";
                    response.Data = model;
                }
                else
                {
                    response.StatusCode = "500";
                    response.IsError = true;
                    response.Message = "Une erreur est survenue";
                }
            }
            else
            {
                response.StatusCode = "500";
                response.IsError = true;
                response.Message = "Une erreur est survenue";
            }

            return response;
        }

        [HttpPost("Anonymise")]
        public async Task<ResponseAPI> Anonymise(string id)
        {
            var response = new ResponseAPI();

            var user = await UserManager.FindByIdAsync(id);
            bool result = await MetierFactory.CreateUtilisateurMetier().AnonymiseUser(user);

            var model = new GestionViewModel();
            model.Users = (await MetierFactory.CreateUtilisateurMetier().GetUser()).ToList();
            model.NomVue = "UserList";

            response.StatusCode = "200";
            response.Data = model;

            return response;
        }

        [HttpGet("BanTempo")]
        public async Task<ResponseAPI> BanTempo(string id)
        {
            var response = new ResponseAPI();

            if (id != null)
            {
                var user = await UserManager.FindByIdAsync(id);
                if (user != null)
                {
                    var model = new UserViewModel();
                    model.Utilisateur = user;

                    response.StatusCode = "200";
                    response.Data = model;
                }
                else
                {
                    response.StatusCode = "500";
                    response.IsError = true;
                    response.Message = "Une erreur est survenue";
                }
            }
            else
            {
                response.StatusCode = "500";
                response.IsError = true;
                response.Message = "Une erreur est survenue";
            }

            return response;
        }

        [HttpPost]
        public async Task<ResponseAPI> BanTemporary(string id, int time)
        {
            var response = new ResponseAPI();

            var user = await UserManager.FindByIdAsync(id);
            bool result = await MetierFactory.CreateUtilisateurMetier().BanUserTemporary(user, time);

            var model = new GestionViewModel();
            model.Users = (await MetierFactory.CreateUtilisateurMetier().GetUser()).ToList();
            model.NomVue = "UserList";

            response.StatusCode = "200";
            response.Data = model;

            return response;
        }

        [HttpPost]
        public async Task<ResponseAPI> DebanUser(string id)
        {
            var response = new ResponseAPI();

            var user = await UserManager.FindByIdAsync(id);
            bool result = await MetierFactory.CreateUtilisateurMetier().DeBan(user);

            var model = new GestionViewModel();
            model.Users = (await MetierFactory.CreateUtilisateurMetier().GetUser()).ToList();
            model.NomVue = "UserList";

            response.StatusCode = "200";
            response.Data = model;

            return response;
        }

        [HttpPost]
        public async Task<ResponseAPI> BanPermanent(string id)
        {
            var response = new ResponseAPI();

            var user = await UserManager.FindByIdAsync(id);
            bool result = await MetierFactory.CreateUtilisateurMetier().BanUserPermanent(user);

            var model = new GestionViewModel();
            model.Users = (await MetierFactory.CreateUtilisateurMetier().GetUser()).ToList();
            model.NomVue = "UserList";

            response.StatusCode = "200";
            response.Data = model;

            return response;
        }

        //[HttpGet("UpdateRole")]
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

        [HttpPost("UpdateRole")]
        public async Task<ResponseAPI> UpdateRole(string id, int roleid)
        {
            var response = new ResponseAPI();

            var user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                var model = new UserViewModel();
                model.Utilisateur = user;
                var result = await UserManager.RemoveFromRolesAsync(user, await UserManager.GetRolesAsync(user));
                var result1 = await UserManager.AddToRoleAsync(user, Enum.GetName((TypeUtilisateur)roleid));
                model.Role = (await UserManager.GetRolesAsync(user)).FirstOrDefault();

                response.StatusCode = "200";
            }
            else
            {
                response.StatusCode = "500";
                response.IsError = true;
                response.Message = "Une erreur est survenue";
            }

            return response;
        }

    }  
}
