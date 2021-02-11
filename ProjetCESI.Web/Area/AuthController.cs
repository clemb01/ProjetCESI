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
        public async Task<ResponseAPI> Login(LoginViewModel model)
        {
            var response = new ResponseAPI();

            if (ModelState.IsValid)
            {
                User user = await UserManager.FindByNameAsync(model.Username);

                if (user != null)
                {
                    var checkEmail = await UserManager.IsEmailConfirmedAsync(user);

                    if (!checkEmail)
                    {
                        response.IsError = true;
                        response.StatusCode = "401";
                        response.Message = "Mail non validé. Veuillez vérifier votre boite mail pour valider votre Email. Lien renvoi email: " + Url.Action(nameof(AccountController.RenvoyerEmailConfirm), "Account", new { username = model.Username }, Request.Scheme);

                        return response;
                    }

                    var checkPassword = await UserManager.CheckPasswordAsync(user, model.Password);

                    if (!checkPassword)
                    {
                        response.IsError = true;
                        response.StatusCode = "400";
                        response.Message = "Mot de passe incorrect";

                        return response;
                    }

                    if (user.UtilisateurSupprime)
                    {
                        response.IsError = true;
                        response.StatusCode = "400";
                        response.Message = "Ce compte à été supprimé";

                        return response;
                    }

                    var signingCredentials = JwtUtils.GetSigningCredentials(Configuration);
                    var claims = JwtUtils.GetClaims(user, UserManager);
                    var tokenOptions = JwtUtils.GenerateTokenOptions(signingCredentials, await claims, Configuration);
                    var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                    response.StatusCode = "200";
                    response.Data = new { accessToken = token, user };

                    return response;
                }
            }

            response.IsError = true;
            response.StatusCode = "400";
            response.Message = "Invalid Authentication";

            return response;
        }
    }

    public class RefreshRequest
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
