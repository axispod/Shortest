using System;
using System.Data;
using FakeItEasy;
using NUnit.Framework;

namespace DataAccess.Sqlite.Tests
{
    public class SqliteDataReaderServiceTests
    {
        // Tested only the GetInt64 method
        // Tests for other methods are similar
        [Test]
        public void GetInt64ShouldReturnDefaultValueWhenGetOrdinalThrowException()
        {
            // Arrange
            var reader = A.Fake<IDataReader>();
            A.CallTo(() => reader.GetOrdinal(A<string>.Ignored)).Throws<ArgumentOutOfRangeException>();
            A.CallTo(() => reader.GetInt64(A<int>.Ignored)).Returns(12);

            var readerService = new SqliteDataReaderService();

            // Act
            var result = readerService.GetInt64(reader, null, 12);

            // Assert
            Assert.That(result, Is.EqualTo(12));
        }

        [Test]
        public void GetInt64ShouldReturnDefaultValueWhenGetInt64ThrowException()
        {
            // Arrange
            var reader = A.Fake<IDataReader>();
            A.CallTo(() => reader.GetOrdinal(A<string>.Ignored)).Returns(12);
            A.CallTo(() => reader.GetInt64(A<int>.Ignored)).Throws<ArgumentOutOfRangeException>();

            var readerService = new SqliteDataReaderService();

            // Act
            var result = readerService.GetInt64(reader, null, 12);

            // Assert
            Assert.That(result, Is.EqualTo(12));
        }

        [Test]
        public void GetInt64ShouldPassValueFromGetOrdinalToGetInt64AndReturnResult()
        {
            // Arrange
            var reader = A.Fake<IDataReader>();
            A.CallTo(() => reader.GetOrdinal(A<string>.Ignored)).Returns(12);
            A.CallTo(() => reader.GetInt64(A<int>.Ignored)).Throws<ArgumentOutOfRangeException>();
            A.CallTo(() => reader.GetInt64(12)).Returns(42);

            var readerService = new SqliteDataReaderService();

            // Act
            var result = readerService.GetInt64(reader, null);

            // Assert
            Assert.That(result, Is.EqualTo(42));
        }
    }
}