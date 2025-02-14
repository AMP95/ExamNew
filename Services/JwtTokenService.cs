﻿using DTOs.Dtos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Utilities;
using Utilities.Interfaces;

namespace Exam.Authentication
{
    public class TokenSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public int Lifetime { get; set; }
    }

    public class JwtTokenService : ITokenService<UserDto>
    {
        public static string ISSUER { get; set; }
        public static string AUDIENCE { get; set; }
        public static string KEY { get; set; }
        public static int LIFETIME { get; set; }

        public static void LoadSettings(TokenSettings settings)
        {
            ISSUER = settings.Issuer;
            AUDIENCE = settings.Audience;
            KEY = settings.Key;
            LIFETIME = settings.Lifetime;
        }

        public string GetToken(UserDto logist)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, logist.Name),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, logist.Role.GetDescription())
                };

            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return GetToken(claimsIdentity);
        }

        private string GetToken(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: ISSUER,
                    audience: AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromDays(LIFETIME)),
                    signingCredentials: new SigningCredentials(GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));


            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }
}
