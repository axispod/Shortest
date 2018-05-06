using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

[assembly:InternalsVisibleTo("DataAccess.Sqlite.Tests")]

namespace DataAccess.Sqlite
{
    public static class Sqlite
    {
        public static void AddSqlite(this IServiceCollection services)
        {
            services.AddTransient<IDataConnectionFactory, SqliteDataConnectionFactory>();
            services.AddTransient<IDataLinkService, SqliteDataLinkService>();
            services.AddTransient<IDataSeedService, SqliteDataSeedService>();
            services.AddTransient<ISqliteDataReaderService, SqliteDataReaderService>();
            services.AddTransient<ISqliteDataMapperFactory, SqliteDataMapperFactory>();
            services.AddTransient<ISqliteHelperService, SqliteHelperService>();
        }

        public static void SeedSqlite(this IApplicationBuilder app)
        {
            var dataSeedService = app.ApplicationServices.GetService<IDataSeedService>();
            dataSeedService.Seed();
        }
    }
}