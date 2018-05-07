using DataAccess;
using DataAccess.Models;
using FakeItEasy;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Shortest.Controllers;

namespace Shortest.Tests.Controllers
{
    public class NavigationControllerTests
    {
        [Test]
        public void IndexShouldReturnNotFoundStatusWhenLinkIsNotFound()
        {
            // Arrange
            var linkService = A.Fake<IDataLinkService>();
            A.CallTo(() => linkService.GetById(A<long>.Ignored)).
                Returns(null);

            var identifierConverter = A.Fake<IIdentifierConverter>();
            A.CallTo(() => identifierConverter.Decode(A<string>.Ignored)).Returns(42);

            var controller = new NavigationController(linkService, identifierConverter);

            // Act
            var result = controller.Index("1") as StatusCodeResult;

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(404));
        }

        [Test]
        public void IndexShouldIncreaseRedirectsCountAndReturnRedirectResult()
        {
            // Arrange
            var linkService = A.Fake<IDataLinkService>();
            A.CallTo(() => linkService.GetById(A<long>.Ignored)).
                Returns(new Link
                {
                    Url = "some url"
                });

            var identifierConverter = A.Fake<IIdentifierConverter>();
            A.CallTo(() => identifierConverter.Decode(A<string>.Ignored)).Returns(42);

            var controller = new NavigationController(linkService, identifierConverter);

            // Act
            var result = controller.Index("1") as RedirectResult;

            // Assert
            Assert.That(result.Url, Is.EqualTo("some url"));
            Assert.That(result.Permanent, Is.False);

            A.CallTo(() => linkService.IncrementRedirectsCount(A<Link>.Ignored)).MustHaveHappenedOnceExactly();
        }
    }
}