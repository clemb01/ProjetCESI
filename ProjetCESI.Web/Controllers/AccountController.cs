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
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            User user = await UserManager.FindByNameAsync(model.Username);

            if(user != null)
            {
                var result = await SignInManager.PasswordSignInAsync(user, model.Password, true, false);
                //var result = await UserManager.IsEmailConfirmedAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Accueil", "Accueil");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid Login Attempt");
                    return View(model);
                }
            }
            else
            {
                ModelState.AddModelError("", "Veuillez créer un compte avant de vous connecter");
                return View(model);
            }

            //return View(model);
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
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            if (user == null)
                return View("Error");
            var result = await UserManager.ConfirmEmailAsync(user, token);
            return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult SuccessRegistration()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email
                };

                IdentityResult result = null;

                result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var token = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, Request.Scheme);
                    await MetierFactory.EmailMetier().SendEmailAsync(user.Email, "Email de confirmation", confirmationLink);
                    result = await UserManager.AddToRoleAsync(user, Enum.GetName(TypeUtilisateur.Aucun));
                    return RedirectToAction("SuccessRegistration");
                }

            }
            else
            {
                return View(model);
            }

            return View(model);
        }
    }
}
