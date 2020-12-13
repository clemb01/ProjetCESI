using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjetCESI.Core;
using ProjetCESI.Data.Metier;
using ProjetCESI.Metier.Main;
using ProjetCESI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager) : base(userManager, signInManager)
        { }

        [AllowAnonymous]
        public IActionResult Login()
        {
            LoginViewModel model = new LoginViewModel();

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginViewModel model, string returnUrl)
        {
            return View();
        }

        [HttpPost]
        public IActionResult LogOff()
        {
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            RegisterViewModel model = new RegisterViewModel();

            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.FirstName))
            {
                ModelState.AddModelError("EmptyPhone", "Le prénom est requis");
            }
            else if (string.IsNullOrWhiteSpace(model.LastName))
            {
                ModelState.AddModelError("EmptyPhone", "Le nom est requis");
            }

            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email
                };

                IdentityResult result = null;

                result = await UserManager.CreateAsync(user, model.Password);

                if(result.Succeeded)
                {
                    result = await UserManager.AddToRoleAsync(user, Enum.GetName(TypeUtilisateur.Client));
                }

                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, true);

                    return Redirect("/Accueil/Index");
                }
            }

            return View(model);
        }
    }
}
