using System.Data;
using Microsoft.Data.Sqlite;

namespace DataAccess.Sqlite
{
    public class SqliteDataConnectionFactory : IDataConnectionFactory
    {
        private readonly SqliteConnectionStringBuilder connectionStringBuilder 
            = new SqliteConnectionStringBuilder
            {
                DataSource = "./SqliteDB.db"
            };

        public IDbConnection CreateConnection()
        {
            var connection = new SqliteConnection(connectionStringBuilder.ConnectionString);
            connection.Open();
            return connection;
        }
    }
}