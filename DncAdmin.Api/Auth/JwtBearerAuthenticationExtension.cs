using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DncAdmin.Api.Auth
{
    public static class JwtBearerAuthenticationExtension
    {

        public static string GetJwtAccessToken(JwtAuthenticationSettings authenticationSettings, List<Claim> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                issuer: authenticationSettings.Issuer,
                claims: claims,
                signingCredentials: creds
                );


            return tokenHandler.WriteToken(jwtToken);
        }
    }
}
