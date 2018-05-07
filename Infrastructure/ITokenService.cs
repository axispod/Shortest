using System.Security.Principal;
using Infrastructure.Models;

namespace Infrastructure
{
    public interface ITokenService
    {
        AuthModel CreateToken(string username, string password);
        long GetUserId(IPrincipal principal);
    }
}