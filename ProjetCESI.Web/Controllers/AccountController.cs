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
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            User user = await UserManager.FindByNameAsync(model.Email);

            if(user != null)
            {
                var result = await SignInManager.PasswordSignInAsync(user, model.Password, true, false);

                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Accueil");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> LogOff()
        {
            await SignInManager.SignOutAsync();

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
