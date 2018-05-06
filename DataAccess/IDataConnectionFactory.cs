using System.Data;

namespace DataAccess
{
    public interface IDataConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}