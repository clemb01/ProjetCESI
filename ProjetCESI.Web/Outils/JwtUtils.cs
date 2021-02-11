using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjetCESI.Core;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ProjetCESI.Web.Outils
{
    public static class JwtUtils
    {
        public static SigningCredentials GetSigningCredentials(IConfiguration config)
        {
            var key = Encoding.UTF8.GetBytes(config["JWTSettings:securityKey"]);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        public static JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims, IConfiguration config)
        {
            var tokenOptions = new JwtSecurityToken(
            issuer: config["JWTSettings:validIssuer"],
            audience: config["JWTSettings:validAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(config["JWTSettings:expiryInMinutes"])),
            signingCredentials: signingCredentials);
            return tokenOptions;
        }

        public static async Task<List<Claim>> GetClaims(User user, UserManager<User> userManager)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            return claims;
        }
    }
}
