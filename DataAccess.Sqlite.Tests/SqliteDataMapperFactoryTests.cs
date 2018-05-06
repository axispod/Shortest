using System;
using DataAccess.Models;
using DataAccess.Sqlite.Mappers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace DataAccess.Sqlite.Tests
{
    public class SqliteDataMapperFactoryTests
    {
        [TestCase(typeof(Link), typeof(LinkMapper))]
        public void CreateShouldCreateMappersProperly(Type modelType, Type mapperType)
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddSqlite();
            var serviceProvider = services.BuildServiceProvider();

            var mapperFactory = new SqliteDataMapperFactory(serviceProvider);

            // Act
            var result = mapperFactory.Create(modelType);

            // Assert
            Assert.That(result, Is.TypeOf(mapperType));
        }
    }
}