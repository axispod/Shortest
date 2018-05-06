using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Shortest.Services
{
    public class ShortLinkBuilder : IShortLinkBuilder
    {
        private readonly IIdentifierConverter identifierConverter;
        private readonly IUrlHelperFactory urlHelperFactory;

        public ShortLinkBuilder(
            IIdentifierConverter identifierConverter, 
            IUrlHelperFactory urlHelperFactory)
        {
            this.identifierConverter = identifierConverter;
            this.urlHelperFactory = urlHelperFactory;
        }

        public string Build(ActionContext context, long id)
        {
            var urlHelper = urlHelperFactory.GetUrlHelper(context);
            return urlHelper.Action(new UrlActionContext
            {
                Controller = "Navigation",
                Action = "Index",
                Values = new
                {
                    id = identifierConverter.Encode(id)
                },
                Host = context.HttpContext.Request.Host.ToString(),
                Protocol = context.HttpContext.Request.Scheme
            });
        }
    }
}