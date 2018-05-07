using System.Collections.Generic;
using DataAccess.Models;

namespace DataAccess
{
    public interface IDataLinkService
    {
        Link Add(Link link);
        void Remove(Link link);
        Link GetById(long id);
        void IncrementRedirectsCount(Link link);
        IEnumerable<Link> GetLinks(long userId);
    }
}