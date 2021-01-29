using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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
    public class AccountController : BaseController
    {
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager) : base(userManager, signInManager)
        { }

        [AllowAnonymous]
        public LoginViewModel Login()
        {
            LoginViewModel model = new LoginViewModel();

            return model;
        }

        //[AllowAnonymous]
        //[HttpPost]
        //public async Task<LoginViewModel> Login(LoginViewModel model, string returnUrl)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return model;
        //    }
        //    User user = await UserManager.FindByNameAsync(model.Username);

        //    if (user != null)
        //    {
        //        var CheckEmail = await UserManager.IsEmailConfirmedAsync(user);

        //        if (!CheckEmail)
        //        {
        //            ModelState.AddModelError("", "Veuillez vérifier votre boite mail pour valider votre Email.");
        //            ViewBag.RenvoieMail = $"<p>Vous n'avez pas reçu le mail ? <a href='/Account/RenvoyerEmailConfirm?Username={model.Username}' >Renvoyer le mail</a></p>";
        //            return model;
        //        }

        //        var result = await SignInManager.PasswordSignInAsync(user, model.Password, true, false);

        //        if (await UserManager.IsLockedOutAsync(user))
        //        {
        //            return View();
        //        }

        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("Accueil", "Accueil");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Identifiant ou mot de passe invalide");
        //            return View(model);
        //        }
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "Veuillez créer un compte avant de vous connecter");
        //        return View(model);
        //    }

        //}

        //public async Task<IActionResult> LogOff()
        //{
        //    await SignInManager.SignOutAsync();

        //    return RedirectToAction("Login");
        //}

        [AllowAnonymous]
        public RegisterViewModel Register()
        {
            RegisterViewModel model = new RegisterViewModel();

            return model;
        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> RenvoyerEmailConfirm(string Username)
        {
            var user = await UserManager.FindByNameAsync(Username);
            var token = await UserManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, Request.Scheme);
            await MetierFactory.EmailMetier().SendEmailAsync(user.Email, "Email de confirmation", confirmationLink);
            return StatusCode(StatusCodes.Status200OK);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError);
            var result = await UserManager.ConfirmEmailAsync(user, token);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<RegisterViewModel> RegisterAsync(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {
                var CheckUser = new User();
                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email
                };

                IdentityResult result = null;

                CheckUser = await UserManager.FindByNameAsync(model.Username);
                if (CheckUser != null)
                {
                    ModelState.AddModelError("", "Ce nom d'utilisateur existe déjà");
                    return model;
                }

                CheckUser = await UserManager.FindByEmailAsync(model.Email);
                if (CheckUser != null)
                {
                    ModelState.AddModelError("", "Email déjà utilisé");
                    return model;
                }

                result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var token = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, Request.Scheme);
                    await MetierFactory.EmailMetier().SendEmailAsync(user.Email, "Email de confirmation", confirmationLink);
                    result = await UserManager.AddToRoleAsync(user, Enum.GetName(TypeUtilisateur.Citoyen));
                    return model;
                }

            }
            else
            {
                return model;
            }
            return model;
        }

        public async Task<UserViewModel> ProfilUser()
        {
            var id = User.Claims.SingleOrDefault(c => c.Type.Contains("nameidentifier"))?.Value;
            var user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                var model = new UserViewModel();
                model.Utilisateur = user;

                return model;
            }
            return null;

        }

        public async Task<UserViewModel> ConfirmationAnonyme(string id)
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
        public async Task<IActionResult> AnonymiseMyAccount(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            bool result = await MetierFactory.CreateUtilisateurMetier().AnonymiseUser(user);
            await SignInManager.SignOutAsync();
            return Redirect("/Accueil/Accueil");
        }


        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordModel)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status500InternalServerError);
            var user = await UserManager.FindByEmailAsync(forgotPasswordModel.Email);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError);
            var token = await UserManager.GeneratePasswordResetTokenAsync(user);
            var callback = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);
            await MetierFactory.EmailMetier().SendEmailAsync(user.Email, "Réinitialisation du mot de passe", callback);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }


        [AllowAnonymous]
        [HttpGet]
        public ResetPasswordViewModel ResetPassword(string token, string email)
        {
            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return model;
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status500InternalServerError);
            var user = await UserManager.FindByEmailAsync(resetPasswordModel.Email);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError);
            var resetPassResult = await UserManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return null;
            }
            return StatusCode(StatusCodes.Status500InternalServerError);
        }



        [HttpPost]
        public async Task<IActionResult> UpdateProfilUser(string id, string newUsername)
        {
            var user = await UserManager.FindByIdAsync(id);
            var result = await MetierFactory.CreateUtilisateurMetier().UpdateInfoUser(user, newUsername);
            await SignInManager.RefreshSignInAsync(user);
            return RedirectToAction("ProfilUser");
        }


        [HttpPost]
        public async Task<IActionResult> UpdateEmail(string id, string newEmail)
        {
            var user = await UserManager.FindByIdAsync(id);
            var result = await UserManager.GenerateChangeEmailTokenAsync(user, newEmail);

            var confirmationLink = Url.Action(nameof(ConfirmChangeEmail), "Account", new { token = result, id = user.Id, newEmail }, Request.Scheme);

            var htmlContent = String.Format(
                    @"Thank you for updating your email. Please confirm the email by clicking this link: 
        <br><a href='{0}'>Confirm new email</a>",
                    confirmationLink);


            // send email to the user with the confirmation link
            await MetierFactory.EmailMetier().SendEmailAsync(user.Email, "Email de confirmation", confirmationLink);

            return RedirectToAction("SuccessRegistration");



        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ConfirmChangeEmail(string token, string id, string newEmail)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError);
            var result = await UserManager.ChangeEmailAsync(user, newEmail, token);
            return StatusCode(StatusCodes.Status500InternalServerError);
        }


        [HttpPost]
        public async Task<UpdatePasswordViewModel> UpdatePassword(UpdatePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.ChangePasswordAsync(Utilisateur, model.Password, model.NewPassword);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("Password", "Mot de passe incorrect");
                    return model;
                }
                return null;
            }
            return model;
        }
    }
}

