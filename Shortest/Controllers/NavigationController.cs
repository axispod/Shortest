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

        [HttpGet("{id}")]
        public IActionResult Index(string id)
        {
            var longId = identifierConverter.Decode(id);

            var link = linkService.GetById(longId);
            if (link == null)
                return StatusCode(404);

            linkService.IncrementRedirectsCount(link);

            return new RedirectResult(link.Url); 
        }
    }
}