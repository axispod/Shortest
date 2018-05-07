using System.Linq;
using DataAccess;
using DataAccess.Models;
using Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shortest.Models.ViewModels;

namespace Shortest.Controllers
{
    [Route("_/[controller]")]
    public class LinkController : Controller
    {
        private readonly IDataLinkService linkService;
        private readonly IShortLinkBuilder shortLinkBuilder;
        private readonly ITokenService tokenService;

        public LinkController(IDataLinkService linkService, IShortLinkBuilder shortLinkBuilder, ITokenService tokenService)
        {
            this.linkService = linkService;
            this.shortLinkBuilder = shortLinkBuilder;
            this.tokenService = tokenService;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public JsonResult Get()
        {
            return Json(
                linkService
                    .GetLinks(GetUserId())
                    .Select(link => new LinkViewModel
                    {
                        Id = link.Id,
                        CreationDate = link.CreationDate,
                        OriginalUrl = link.Url,
                        ShortUrl = shortLinkBuilder.Build(ControllerContext, link.Id),
                        RedirectsCount = link.Redirects
                    }));
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        public JsonResult Add([FromQuery]string url)
        {
            var link = new Link
            {
                Url = url,
                UserId = GetUserId()
            };

            linkService.Add(link);

            return Json(new AddViewModel
            {
                ShortUrl = shortLinkBuilder.Build(ControllerContext, link.Id)
            });
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            linkService.Remove(new Link
            {
                Id = id,
                UserId = GetUserId()
            });
        }

        private long GetUserId()
        {
            return tokenService.GetUserId(User);
        }
    }
}
