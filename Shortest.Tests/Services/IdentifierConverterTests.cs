using NUnit.Framework;
using Shortest.Services;

namespace Shortest.Tests.Services
{
    public class IdentifierConverterTests
    {
        [TestCase(0)]
        [TestCase(10)]
        [TestCase(42)]
        [TestCase(124215612621434)]
        [TestCase(521342342141112)]
        public void NumberAfterApplyingEncodeThenDecodeShouldNotChange(long value)
        {
            // Arrange
            var service = new IdentifierConverter();

            // Act
            var encoded = service.Encode(value);
            var result = service.Decode(encoded);

            // Assert
            Assert.That(encoded, Is.Not.EqualTo(value));
            Assert.That(result, Is.EqualTo(value));
        }
    }
}