using DataAccess.Sqlite;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Shortest.Auth;
using Shortest.Services;

namespace Shortest
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IIdentifierConverter, IdentifierConverter>();
            services.AddTransient<IShortLinkBuilder, ShortLinkBuilder>();
            services.AddSqlite();

            services.AddAuth();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.SeedSqlite();
        }
    }
}
