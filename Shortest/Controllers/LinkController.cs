using System.Linq;
using DataAccess;
using DataAccess.Models;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Shortest.Models.ViewModels;

namespace Shortest.Controllers
{
    [Route("_/[controller]")]
    public class LinkController : Controller
    {
        private readonly IDataLinkService linkService;
        private readonly IShortLinkBuilder shortLinkBuilder;

        public LinkController(IDataLinkService linkService, IShortLinkBuilder shortLinkBuilder)
        {
            this.linkService = linkService;
            this.shortLinkBuilder = shortLinkBuilder;
        }

        [HttpGet("u{id}")]
        public JsonResult Get(int id)
        {
            return Json(
                linkService
                    .GetLinks(id)
                    .Select(link => new LinkViewModel
                    {
                        CreationDate = link.CreationDate,
                        OriginalUrl = link.Url,
                        ShortUrl = shortLinkBuilder.Build(ControllerContext, link.Id),
                        RedirectsCount = link.Redirects
                    }));
        }

        [HttpPost]
        public JsonResult Add([FromQuery]string url)
        {
            var link = new Link
            {
                Url = url
            };

            linkService.Add(link);

            return Json(new AddViewModel
            {
                ShortUrl = shortLinkBuilder.Build(ControllerContext, link.Id)
            });
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            linkService.Remove(new Link
            {
                Id = id
            });
        }
    }
}
