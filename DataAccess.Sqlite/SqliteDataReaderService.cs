using System;
using System.Data;

namespace DataAccess.Sqlite
{
    public class SqliteDataReaderService : ISqliteDataReaderService
    {
        public long GetInt64(IDataReader reader, string name, long defaultValue = 0)
        {
            try
            {
                return reader.GetInt64(reader.GetOrdinal(name));
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public string GetString(IDataReader reader, string name, string defaultValue = null)
        {
            try
            {
                return reader.GetString(reader.GetOrdinal(name));
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public DateTime GetDateTime(IDataReader reader, string name, DateTime defaultValue = new DateTime())
        {
            try
            {
                return reader.GetDateTime(reader.GetOrdinal(name));
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}