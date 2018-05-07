using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Shortest.Auth
{
    public static class Auth
    {
        public static void AddAuth(this IServiceCollection services)
        {
            services.AddTransient<ITokenService, JwtTokenService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = AuthConfiguaration.Issuer,

                        ValidateAudience = true,
                        ValidAudience = AuthConfiguaration.Audience,
                        ValidateLifetime = true,

                        IssuerSigningKey = AuthConfiguaration.GetSymmetricSecurityKey,
                        ValidateIssuerSigningKey = true,
                    };
                });

            services.AddAuthorization(options =>
            {
                options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });
        }
    }
}