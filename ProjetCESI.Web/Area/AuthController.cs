using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ProjetCESI.Core;
using ProjetCESI.Web.Controllers;
using ProjetCESI.Web.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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

                    var signingCredentials = GetSigningCredentials();
                    var claims = GetClaims(user);
                    var tokenOptions = GenerateTokenOptions(signingCredentials, await claims);
                    var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                    return Ok(new { accessToken = token, accessTokenExpiration = "", refreshToken = "" });
                }
            }            

            return BadRequest("Invalid Authentication");
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(Configuration["JWTSettings:securityKey"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var tokenOptions = new JwtSecurityToken(
            issuer: Configuration["JWTSettings:validIssuer"],
            audience: Configuration["JWTSettings:validAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(Configuration["JWTSettings:expiryInMinutes"])),
            signingCredentials: signingCredentials);
            return tokenOptions;
        }

        private async Task<List<Claim>> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var roles = await UserManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
    }
}
