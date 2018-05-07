using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Shortest.Auth
{
    public class AuthConfiguaration
    {
        public const string Issuer = "MyAuthServer";
        public const string Audience = "http://localhost:13249/";
        public const int Lifetime = 120;

        public const string UserIdClaimType = "USER IDENTIFIER";

        const string Key = "super_mega_turbo_secret_key";
        public static SymmetricSecurityKey GetSymmetricSecurityKey => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}