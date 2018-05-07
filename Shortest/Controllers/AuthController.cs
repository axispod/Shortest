using DataAccess;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Shortest.Controllers
{
    [Route("_/[controller]")]
    public class AuthController : Controller
    {
        private readonly ITokenService tokenService;
        private readonly IDataUserService userService;

        public AuthController(ITokenService tokenService, IDataUserService userService)
        {
            this.tokenService = tokenService;
            this.userService = userService;
        }

        [HttpPost("register")]
        public IActionResult Register(string username, string password)
        {
            var user = userService.Register(username, password);

            if (user == null)
                return StatusCode(404);

            return StatusCode(200);
        }

        [HttpPost("token")]
        public IActionResult Token(string username, string password)
        {
            var token = tokenService.CreateToken(username, password);
            if (token == null)
            {
                return StatusCode(404);
            }

            return Json(token);
        }
    }
}