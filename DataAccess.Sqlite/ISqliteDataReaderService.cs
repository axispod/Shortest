using System;
using System.Data;

namespace DataAccess.Sqlite
{
    public interface ISqliteDataReaderService
    {
        long GetInt64(IDataReader reader, string name, long defaultValue = 0);
        string GetString(IDataReader reader, string name, string defaultValue = null);
        DateTime GetDateTime(IDataReader reader, string name, DateTime defaultValue = new DateTime());
    }
}