using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjetCESI.Core;
using ProjetCESI.Web.Models;
using ProjetCESI.Web.Outils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Area
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]
    public class AccountAPIController : BaseAPIController
    {
        public AccountAPIController(UserManager<User> userManager) : base(userManager)
        { }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            string message = string.Empty;

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
                    message = "Ce nom d'utilisateur existe déjà";
                    return BadRequest(new { message });
                }

                CheckUser = await UserManager.FindByEmailAsync(model.Email);
                if (CheckUser != null)
                {
                    message = "Email déjà utilisé";
                    return BadRequest(new { message });
                }

                result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var token = await UserManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, Request.Scheme);
                    await MetierFactory.EmailMetier().SendEmailAsync(user.Email, "Email de confirmation", confirmationLink);
                    result = await UserManager.AddToRoleAsync(user, Enum.GetName(TypeUtilisateur.Citoyen));

                    message = "Compte créé, veuillez consulter vos mails";
                    return StatusCode(201, new { message });
                }
            }

            message = "Une erreur c'est produite.";
            return BadRequest(new { message });
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Une erreur interne c'est produite" });

            var result = await UserManager.ConfirmEmailAsync(user, token);

            return StatusCode(200, new { message = "Mail renvoyé" });
        }        

        [HttpGet("Profil")]
        public async Task<IActionResult> Profil()
        {
            var id = UserId.Value.ToString();

            if (!string.IsNullOrEmpty(id))
            {
                var user = await UserManager.FindByIdAsync(id);
                if (user != null)
                {
                    var model = new UserViewModel();
                    model.Utilisateur = user;

                    return Ok(model);
                }
            }

            return StatusCode(500, new { message = "Une erreur c'est produite" });
        }

        [HttpPost]
        public async Task<IActionResult> AnonymiseMyAccount(string id)
        {
            var user = await UserManager.FindByIdAsync(id);
            bool result = await MetierFactory.CreateUtilisateurMetier().AnonymiseUser(user);

            if (result)
                return Ok(new { message = "Compte supprimé" });
            else
                return StatusCode(500, new { message = "Une erreur c'est produite" });
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordModel)
        {
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "Le mail est incorrect" });

            var user = await UserManager.FindByEmailAsync(forgotPasswordModel.Email);

            if (user == null)
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "Aucun compte associé à cet email trouvé" });

            var token = await UserManager.GeneratePasswordResetTokenAsync(user);
            var callback = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);
            await MetierFactory.EmailMetier().SendEmailAsync(user.Email, "Réinitialisation du mot de passe", callback);

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Les données sont incorrectes" });

            var user = await UserManager.FindByEmailAsync(resetPasswordModel.Email);

            if (user == null)
                return BadRequest(new { message = "L'utilisateur n'a pas été trouvé" });

            var resetPassResult = await UserManager.ResetPasswordAsync(user, resetPasswordModel.Token, resetPasswordModel.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return StatusCode(500, new { message = "Une erreur inconnue c'est produite" });
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfilUser(string id, string newUsername)
        {
            var user = await UserManager.FindByIdAsync(id);
            var result = await MetierFactory.CreateUtilisateurMetier().UpdateInfoUser(user, newUsername);

            var signingCredentials = JwtUtils.GetSigningCredentials(Configuration);
            var claims = JwtUtils.GetClaims(user, UserManager);
            var tokenOptions = JwtUtils.GenerateTokenOptions(signingCredentials, await claims, Configuration);
            var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

            return Ok(new { accessToken = token, result });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEmail(string id, string newEmail)
        {
            var user = await UserManager.FindByIdAsync(id);
            var result = await UserManager.GenerateChangeEmailTokenAsync(user, newEmail);

            var confirmationLink = Url.Action(nameof(ConfirmChangeEmail), "Account", new { token = result, id = user.Id, newEmail }, Request.Scheme);

            var htmlContent = string.Format(
                    @"Vous avez demandé à modifié votre adresse email. S'il s'agissait bien de vous, vous pouvez cliquer sur le <a href='{0}'>lien</a> suivant pour confirmer la nouvelle adresse.<br />S'il ne s'agissait pas de vous, ne faites rien et nous vous invitons à modifier votre mot de passe au plus vite.<br /><br />Cordialement, l'équipe FishOn",
                    confirmationLink);


            // send email to the user with the confirmation link
            await MetierFactory.EmailMetier().SendEmailAsync(user.Email, "Email de confirmation", htmlContent);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ConfirmChangeEmail(string token, string id, string newEmail)
        {
            var user = await UserManager.FindByIdAsync(id);

            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError);

            var result = await UserManager.ChangeEmailAsync(user, newEmail, token);

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.ChangePasswordAsync(Utilisateur, model.Password, model.NewPassword);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("Password", "Mot de passe incorrect");
                    return BadRequest(new { message = "Mot de passe incorrect" });
                }

                return Ok();
            }

            return StatusCode(500);
        }
    }
}

