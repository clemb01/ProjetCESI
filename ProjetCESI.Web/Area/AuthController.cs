using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjetCESI.Core;
using ProjetCESI.Web.Controllers;
using ProjetCESI.Web.Models;
using ProjetCESI.Web.Outils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Area
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AuthController : BaseController
    {
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await UserManager.FindByNameAsync(model.Username);

                if (user != null)
                {
                    var checkEmail = await UserManager.IsEmailConfirmedAsync(user);

                    if (!checkEmail)
                    {
                        return Unauthorized(new { message = "Mail non validé. Veuillez vérifier votre boite mail pour valider votre Email. Lien renvoi email: " + Url.Action(nameof(AccountController.RenvoyerEmailConfirm), "Account", new { username = model.Username }, Request.Scheme) });
                    }

                    var checkPassword = await UserManager.CheckPasswordAsync(user, model.Password);

                    if (!checkPassword)
                    {
                        return BadRequest(new { message = "Mot de passe incorrect" });
                    }

                    var signingCredentials = JwtUtils.GetSigningCredentials(Configuration);
                    var claims = JwtUtils.GetClaims(user, UserManager);
                    var tokenOptions = JwtUtils.GenerateTokenOptions(signingCredentials, await claims, Configuration);
                    var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                    return Ok(new { accessToken = token, user });
                }
            }            

            return BadRequest("Invalid Authentication");
        }
    }

    public class RefreshRequest
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
