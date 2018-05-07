using System;
using DataAccess.Models;
using DataAccess.Sqlite.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Sqlite
{
    public class SqliteDataMapperFactory : ISqliteDataMapperFactory
    {
        private readonly IServiceProvider serviceProvider;

        public SqliteDataMapperFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IDataMapper Create(Type modelType)
        {
            if(modelType == typeof(Link))
                return ActivatorUtilities.CreateInstance<LinkMapper>(serviceProvider);

            if (modelType == typeof(User))
                return ActivatorUtilities.CreateInstance<UserMapper>(serviceProvider);

            return null;
        }
    }
}