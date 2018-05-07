using System;
using System.Collections.Generic;
using DataAccess.Models;

namespace DataAccess.Sqlite
{
    public class SqliteDataLinkService : IDataLinkService
    {
        private readonly IDataConnectionFactory connectionFactory;
        private readonly ISqliteHelperService helperService;

        public SqliteDataLinkService(
            IDataConnectionFactory connectionFactory, 
            ISqliteHelperService helperService)
        {
            this.connectionFactory = connectionFactory;
            this.helperService = helperService;
        }

        public Link Add(Link link)
        {
            using (var connection = connectionFactory.CreateConnection())
            {
                helperService.ExequtePlain(connection,
                    "INSERT INTO links(UserId, Url, Redirects, CreationDate)" +
                    "VALUES($userId, $url, 0, CURRENT_TIMESTAMP)",
                    new Parameter("userId", link.UserId),
                    new Parameter("url", link.Url));

                link.Id = helperService.GetScalar<long>(connection, "SELECT last_insert_rowid()");
                return link;
            }
        }

        public void Remove(Link link)
        {
            if(link.Id == 0)
                throw new ArgumentOutOfRangeException("Link id can't be 0");

            using (var connection = connectionFactory.CreateConnection())
            {
                helperService.ExequtePlain(connection, "DELETE FROM links WHERE rowid=$rowid AND UserId=$userId",
                    new Parameter("rowid", link.Id),
                    new Parameter("userId", link.UserId));
            }
        }

        public Link GetById(long id)
        {
            using (var connection = connectionFactory.CreateConnection())
            {
                return helperService.GetScalarMapped<Link>(connection,
                    "SELECT rowid, * " +
                    "FROM links " +
                    "WHERE rowid=$id", new Parameter("id", id));
            }
        }

        public void IncrementRedirectsCount(Link link)
        {
            using (var connection = connectionFactory.CreateConnection())
            {
                helperService.ExequtePlain(connection, 
                    "UPDATE links SET Redirects=Redirects+1 " +
                    "WHERE RowId=$id",
                    new Parameter("id", link.Id));
            }
        }

        public IEnumerable<Link> GetLinks(long userId)
        {
            using (var connection = connectionFactory.CreateConnection())
            {
                return helperService.GetMapped<Link>(connection,
                    "SELECT rowid, * " +
                    "FROM links " +
                    "WHERE UserId=$userId", new Parameter("userId", userId));
            }
        }
    }
}
