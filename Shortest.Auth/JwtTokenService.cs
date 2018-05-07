using System;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using DataAccess;
using Infrastructure;
using Infrastructure.Models;

namespace Shortest.Auth
{
    public class JwtTokenService : ITokenService
    {
        private readonly IDataUserService userService;

        public JwtTokenService(IDataUserService userService)
        {
            this.userService = userService;
        }

        public AuthModel CreateToken(string username, string password)
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return null;
            }

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                issuer: AuthConfiguaration.Issuer,
                audience: AuthConfiguaration.Audience,
                notBefore: now,
                claims: identity.Claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthConfiguaration.Lifetime)),
                signingCredentials: new SigningCredentials(AuthConfiguaration.GetSymmetricSecurityKey, SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new AuthModel
            {
                AccessToken = encodedJwt,
                Username = identity.Name
            };

            return response;
        }

        public long GetUserId(IPrincipal principal)
        {
            var claimsIdentity = principal?.Identity as ClaimsIdentity;

            var userIdString = claimsIdentity?.Claims
                .Where(c => c.Type == AuthConfiguaration.UserIdClaimType)
                .Select(c => c.Value)
                .FirstOrDefault();

            if (userIdString == null)
                return 0;

            return long.TryParse(userIdString, out var id) ? id : 0;
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            var user = userService.Get(username, password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Username),
                    new Claim(AuthConfiguaration.UserIdClaimType, user.Id.ToString())
                };
                ClaimsIdentity claimsIdentity =
                    new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                        ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
    }
}