using System.Data;
using DataAccess.Models;

namespace DataAccess.Sqlite.Mappers
{
    public class LinkMapper : IDataMapper
    {
        private readonly ISqliteDataReaderService readerService;

        public LinkMapper(ISqliteDataReaderService readerService)
        {
            this.readerService = readerService;
        }

        public object Map(IDataReader reader)
        {
            return new Link
            {
                Id = readerService.GetInt64(reader, "rowid"),
                UserId = readerService.GetInt64(reader, "UserId"),
                Url = readerService.GetString(reader, "Url"),
                Redirects = readerService.GetInt64(reader, "Redirects"),
                CreationDate = readerService.GetDateTime(reader, "CreationDate")
            };
        }
    }
}