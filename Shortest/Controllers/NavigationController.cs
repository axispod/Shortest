using DataAccess;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace Shortest.Controllers
{
    public class NavigationController : Controller
    {
        private readonly IDataLinkService linkService;
        private readonly IIdentifierConverter identifierConverter;

        public NavigationController(IDataLinkService linkService, IIdentifierConverter identifierConverter)
        {
            this.linkService = linkService;
            this.identifierConverter = identifierConverter;
        }

        // GET
        // http://localhost:13249/3QWkCkp8p6b
        // http://localhost:13249/5RmpN2e3Gio - Тестовое задание
        // http://localhost:13249/6245NQ3xiyU - Dapper

        [HttpGet("{id}")]
        public IActionResult Index(string id)
        {
            var longId = identifierConverter.Decode(id);

            var link = linkService.GetById(longId);
            if (link == null)
                return StatusCode(404);

            linkService.IncrementRedirectsCount(link);

            // I use the permanent redirection because user can't edit links
            return new RedirectResult(link.Url); 
        }
    }
}