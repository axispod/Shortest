using System;
using DataAccess.Sqlite.Mappers;

namespace DataAccess.Sqlite
{
    public interface ISqliteDataMapperFactory
    {
        IDataMapper Create(Type modelType);
    }
}