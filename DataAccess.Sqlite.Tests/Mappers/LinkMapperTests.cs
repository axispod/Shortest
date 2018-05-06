using System;
using System.Data;
using DataAccess.Models;
using DataAccess.Sqlite.Mappers;
using FakeItEasy;
using NUnit.Framework;

namespace DataAccess.Sqlite.Tests.Mappers
{
    public class LinkMapperTests
    {
        [Test]
        public void MapShouldConvertDataProperly()
        {
            // Arrange
            var readerService = A.Fake<ISqliteDataReaderService>();
            A.CallTo(() => readerService.GetInt64(A<IDataReader>.Ignored, "rowid", A<long>.Ignored))
                .Returns(42);
            A.CallTo(() => readerService.GetInt64(A<IDataReader>.Ignored, "UserId", A<long>.Ignored))
                .Returns(12);
            A.CallTo(() => readerService.GetInt64(A<IDataReader>.Ignored, "Redirects", A<long>.Ignored))
                .Returns(24);
            A.CallTo(() => readerService.GetString(A<IDataReader>.Ignored, "Url", A<string>.Ignored))
                .Returns("some url");
            A.CallTo(() => readerService.GetDateTime(A<IDataReader>.Ignored, "CreationDate", A<DateTime>.Ignored))
                .Returns(new DateTime(2012, 1, 1));

            var mapper = new LinkMapper(readerService);

            // Act
            var result = mapper.Map(null) as Link;

            // Assert
            Assert.That(result.Id, Is.EqualTo(42));
            Assert.That(result.CreationDate, Is.EqualTo(new DateTime(2012, 1, 1)));
            Assert.That(result.Redirects, Is.EqualTo(24));
            Assert.That(result.Url, Is.EqualTo("some url"));
            Assert.That(result.UserId, Is.EqualTo(12));
        }
    }
}
