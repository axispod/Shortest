namespace DataAccess.Sqlite
{
    public class SqliteDataSeedService : IDataSeedService
    {
        private readonly IDataConnectionFactory dataConnectionFactory;
        private readonly ISqliteHelperService helperService;

        public SqliteDataSeedService(
            IDataConnectionFactory dataConnectionFactory, 
            ISqliteHelperService helperService)
        {
            this.dataConnectionFactory = dataConnectionFactory;
            this.helperService = helperService;
        }

        public void Seed()
        {
            using (var connection = dataConnectionFactory.CreateConnection())
            {
                connection.Open();

                if (!helperService.IsTableExists(connection, "links"))
                {
                    helperService.ExequtePlain(connection, 
                        "CREATE TABLE links (" +
                        "UserId INT," +
                        "Url TEXT," +
                        "Redirects INT," +
                        "CreationDate INT" +
                        ")");
                }
            }
        }
    }
}