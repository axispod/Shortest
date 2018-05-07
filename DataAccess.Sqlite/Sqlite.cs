using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Sqlite
{
    public static class Sqlite
    {
        public static void AddSqlite(this IServiceCollection services)
        {
            services.AddTransient<IDataConnectionFactory, SqliteDataConnectionFactory>();
            services.AddTransient<IDataLinkService, SqliteDataLinkService>();
            services.AddTransient<IDataUserService, SqliteDataUserService>();
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