using Microsoft.AspNetCore.Mvc;

namespace Infrastructure
{
    public interface IShortLinkBuilder
    {
        string Build(ActionContext context, long id);
    }
}