using System.Collections.Generic;
using System.Data;
using DataAccess.Models;

namespace DataAccess.Sqlite
{
    public interface ISqliteHelperService
    {
        bool IsTableExists(IDbConnection connection, string name);
        T GetScalar<T>(IDbConnection connection, string query, params Parameter[] parameters);

        T GetScalarMapped<T>(
            IDbConnection connection,
            string query,
            params Parameter[] parameters)
            where T : new();

        IEnumerable<T> GetMapped<T>(
            IDbConnection connection,
            string query,
            params Parameter[] parameters)
            where T : new();

        void ExequtePlain(IDbConnection connection, string query, params Parameter[] parameters);
    }
}