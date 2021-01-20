using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjetCESI.Core;
using ProjetCESI.Web.Models;
using System;
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
                var CheckEmail = await UserManager.IsEmailConfirmedAsync(user);

                if (!CheckEmail)
                {
                    ModelState.AddModelError("", "Veuillez vérifier votre boite mail pour valider votre Email.");
                    ViewBag.RenvoieMail = $"<p>Vous n'avez pas reçu le mail ? <a href='/Account/RenvoyerEmailConfirm?Username={model.Username}' >Renvoyer le mail</a></p>";
                    return View(model);
                }

                var result = await SignInManager.PasswordSignInAsync(user, model.Password, true, false);

                if (await UserManager.IsLockedOutAsync(user))
                {
                    ViewData["Message"] = "Votre Compte est bloqué, veuillez contacter l'administrateur";
                    return View();
                }

                if (result.Succeeded)
                {
                    return RedirectToAction("Accueil", "Accueil");
                }
                else
                {
                    ModelState.AddModelError("", "Identifiant ou mot de passe invalide");
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
        public async Task<IActionResult> RenvoyerEmailConfirm(string Username)
        {
            var user = await UserManager.FindByNameAsync(Username);
            var token = await UserManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, Request.Scheme);
            await MetierFactory.EmailMetier().SendEmailAsync(user.Email, "Email de confirmation", confirmationLink);
            return View();
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
                    result = await UserManager.AddToRoleAsync(user, Enum.GetName(TypeUtilisateur.Citoyen));
                    return RedirectToAction("SuccessRegistration");
                }

            }
            else
            {
                return View(model);
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

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordModel)
        {
            if (!ModelState.IsValid)
                return View(forgotPasswordModel);
            var user = await UserManager.FindByEmailAsync(forgotPasswordModel.Email);
            if (user == null)
                return RedirectToAction(nameof(ForgotPasswordConfirm));
            var token = await UserManager.GeneratePasswordResetTokenAsync(user);
            var callback = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);
            await MetierFactory.EmailMetier().SendEmailAsync(user.Email, "Réinitialisation du mot de passe", callback);
            return RedirectToAction(nameof(ForgotPasswordConfirm));
        }

        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirm()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordModel);
            var user = await UserManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null)
                RedirectToAction(nameof(ResetPasswordConfirm));
            var resetPassResult = await UserManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return View();
            }
            return RedirectToAction(nameof(ResetPasswordConfirm));
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult ResetPasswordConfirm()
        {
            return View();
        }
    }
}
