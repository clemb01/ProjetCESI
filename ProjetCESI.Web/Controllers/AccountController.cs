using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjetCESI.Core;
using ProjetCESI.Data.Metier;
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

                if (await UserManager.IsLockedOutAsync(user))
                {
                    ViewData["Message"] = "Votre Compte est bloqué, veuillez contacter l'administrateur";
                    return View();
                    //return Content("Compte bloqué");
                }

                if (result.Succeeded)
                {
                    return RedirectToAction("Accueil", "Accueil");
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
                    result = await UserManager.AddToRoleAsync(user, Enum.GetName(TypeUtilisateur.Citoyen));
                }

                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, true);

                    return Redirect("/Accueil/Accueil");
                }
            }

            return View(model);
        }


        public async Task<IActionResult> ProfilUser()
        {
            var id = User.Claims.SingleOrDefault(c => c.Type.Contains("nameidentifier"))?.Value;
            var user = await UserManager.FindByIdAsync(id);
            if(user != null)
            {
                var model = new UserViewModel();
                model.Utilisateur = user;

                return View(model);
            }
            return View();

        }

        public async Task<IActionResult> ConfirmationAnonyme(string id)
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
        public async Task<IActionResult> AnonymiseMyAccount(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            bool result = await MetierFactory.CreateUtilisateurMetier().AnonymiseUser(user);
            await SignInManager.SignOutAsync();
            return Redirect("/Accueil/Accueil");
        }
    }
}
