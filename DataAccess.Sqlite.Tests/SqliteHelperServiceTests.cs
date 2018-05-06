using System.Collections.Generic;
using System.Data;
using DataAccess.Models;
using FakeItEasy;
using NUnit.Framework;

namespace DataAccess.Sqlite.Tests
{
    public class SqliteHelperServiceTests
    {
        [Test]
        public void GetScalarShouldPassQueryToCommand()
        {
            // Arrange
            var command = A.Fake<IDbCommand>();
            A.CallTo(() => command.ExecuteScalar())
                .Returns("some result");

            var connection = A.Fake<IDbConnection>();
            A.CallTo(() => connection.CreateCommand())
                .Returns(command);

            var service = new SqliteHelperService(null);

            // Act
            service.GetScalar<string>(connection, "some query");

            // Assert
            
            A.CallTo(command)
                .Where(m => m.Method.Name == "set_CommandText" && m.Arguments[0] as string == "some query")
                .MustHaveHappened();
        }

        [Test]
        public void GetScalarShouldReturnResultOfExecuteScalar()
        {
            // Arrange
            var command = A.Fake<IDbCommand>();
            A.CallTo(() => command.ExecuteScalar())
                .Returns("some result");

            var connection = A.Fake<IDbConnection>();
            A.CallTo(() => connection.CreateCommand())
                .Returns(command);

            var service = new SqliteHelperService(null);

            // Act
            var result = service.GetScalar<string>(connection, "some query");

            // Assert
            Assert.That(result, Is.EqualTo("some result"));
        }

        [Test]
        public void GetScalarMappedShouldApplyParametersToCommand()
        {
            // Arrange
            List<IDbDataParameter> parametersList = new List<IDbDataParameter>();

            var parameters = A.Fake<IDataParameterCollection>();
            A.CallTo(() => parameters.Add(A<object>.Ignored))
                .Invokes((object p) => parametersList.Add(p as IDbDataParameter));

            var command = A.Fake<IDbCommand>();
            A.CallTo(() => command.ExecuteScalar())
                .Returns("some result");
            A.CallTo(() => command.Parameters).Returns(parameters);
            A.CallTo(() => command.CreateParameter()).ReturnsLazily(CreateMockParameter);

            var connection = A.Fake<IDbConnection>();
            A.CallTo(() => connection.CreateCommand())
                .Returns(command);

            var service = new SqliteHelperService(null);

            // Act
            service.GetScalar<string>(
                connection, 
                "some query", 
                new Parameter("p1", "v1"),
                new Parameter("ppp2", "vvv2"));

            // Assert
            Assert.That(parametersList[0].ParameterName, Is.EqualTo("p1"));
            Assert.That(parametersList[0].Value, Is.EqualTo("v1"));
            Assert.That(parametersList[1].ParameterName, Is.EqualTo("ppp2"));
            Assert.That(parametersList[1].Value, Is.EqualTo("vvv2"));
        }

        // Parameter clojure
        private IDbDataParameter CreateMockParameter()
        {
            string name = null;
            object value = null;

            var parameter = A.Fake<IDbDataParameter>();
            A.CallTo(parameter)
                .Where(m => m.Method.Name == "set_ParameterName")
                .Invokes((string n) => name = n);
            A.CallTo(parameter)
                .Where(m => m.Method.Name == "get_ParameterName")
                .WithReturnType<string>()
                .ReturnsLazily(() => name);

            A.CallTo(parameter)
                .Where(m => m.Method.Name == "set_Value")
                .Invokes((object v) => value = v);
            A.CallTo(parameter)
                .Where(m => m.Method.Name == "get_Value")
                .WithReturnType<object>()
                .ReturnsLazily(() => value);

            return parameter;
        }
    }
}