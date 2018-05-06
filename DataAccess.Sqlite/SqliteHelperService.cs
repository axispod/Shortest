using System.Collections.Generic;
using System.Data;
using DataAccess.Models;

namespace DataAccess.Sqlite
{
    public class SqliteHelperService : ISqliteHelperService
    {
        private readonly ISqliteDataMapperFactory mapperFactory;

        public SqliteHelperService(ISqliteDataMapperFactory mapperFactory)
        {
            this.mapperFactory = mapperFactory;
        }

        public bool IsTableExists(IDbConnection connection, string name)
        {
            return name == GetScalar<string>(connection,
               "SELECT name FROM sqlite_master WHERE type='table' and name=$name",
               new Parameter("name", name));
        }

        public T GetScalar<T>(IDbConnection connection, string query, params Parameter[] parameters)
        {
            var checkCommand = connection.CreateCommand();
            checkCommand.CommandText = query;

            ApplyParameters(checkCommand, parameters);

            return (T)checkCommand.ExecuteScalar();
        }

        public T GetScalarMapped<T>(
            IDbConnection connection,
            string query,
            params Parameter[] parameters)
            where T : new()
        {
            var command = connection.CreateCommand();
            command.CommandText = query;

            ApplyParameters(command, parameters);

            var mapper = mapperFactory.Create(typeof(T));

            using (var reader = command.ExecuteReader())
            {
                if(reader.Read())
                    return (T)mapper.Map(reader);

                return default(T);
            }
        }

        public IEnumerable<T> GetMapped<T>(
            IDbConnection connection,
            string query,
            params Parameter[] parameters)
            where T : new()
        {
            var command = connection.CreateCommand();
            command.CommandText = query;

            ApplyParameters(command, parameters);

            var mapper = mapperFactory.Create(typeof(T));

            List<T> result = new List<T>();
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result.Add((T)mapper.Map(reader));
                }
            }

            return result;
        }

        public void ExequtePlain(IDbConnection connection, string query, params Parameter[] parameters)
        {
            var command = connection.CreateCommand();
            command.CommandText = query;

            ApplyParameters(command, parameters);

            command.ExecuteNonQuery();
        }

        private static void ApplyParameters(IDbCommand command, Parameter[] parameters)
        {
            foreach (var p in parameters)
            {
                var parameter = command.CreateParameter();
                parameter.Value = p.Value;
                parameter.ParameterName = p.Name;
                command.Parameters.Add(parameter);
            }
        }
    }
}