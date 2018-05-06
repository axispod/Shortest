using System.Data;

namespace DataAccess.Sqlite.Mappers
{
    public interface IDataMapper
    {
        object Map(IDataReader reader);
    }
}