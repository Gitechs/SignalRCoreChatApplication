using ChatApplication.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatApplication.Utilities
{
    public class JwtGenerator
    {
        private readonly JwtSetting jwtSetting;

        public JwtGenerator(JwtSetting jwtSetting)
        {
            this.jwtSetting = jwtSetting;
            this.jwtSetting = jwtSetting;
        }
        public string GenerateAsync(User user)
        {

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtSetting.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(GetClaimsAsync(user)),
                NotBefore = DateTime.UtcNow.AddMinutes(0),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = jwtSetting.Audience,
                Issuer = jwtSetting.Issuer,
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(120)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private IEnumerable<Claim> GetClaimsAsync(User user)
        {
            //var result = await signInManager.ClaimsFactory.CreateAsync(user);
            //add custom claims
            // var claims = await signInManager.UserManager.GetClaimsAsync(user);
            var claims = new List<Claim> {
                new Claim (ClaimTypes.NameIdentifier, user.Id.ToString ()),
                new Claim (ClaimTypes.NameIdentifier, user.UserName),
                new Claim (ClaimTypes.Email, user.Email),
            };

            return claims;
        }
    }
}
