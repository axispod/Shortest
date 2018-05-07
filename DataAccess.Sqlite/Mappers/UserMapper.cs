using System.Data;
using DataAccess.Models;

namespace DataAccess.Sqlite.Mappers
{
    public class UserMapper : IDataMapper
    {
        private readonly ISqliteDataReaderService readerService;

        public UserMapper(ISqliteDataReaderService readerService)
        {
            this.readerService = readerService;
        }

        public object Map(IDataReader reader)
        {
            return new User
            {
                Id = readerService.GetInt64(reader, "rowid"),
                Username = readerService.GetString(reader, "Username")
            };
        }
    }
}