using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using DataAccess;
using DataAccess.Models;
using FakeItEasy;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Shortest.Controllers;
using Shortest.Models.ViewModels;

namespace Shortest.Tests.Controllers
{
    public class LinkControllerTests
    {
        [Test]
        public void GetShouldReturnViewModel()
        {
            // Arrange
            var date1 = new DateTime(2012, 10, 12, 1, 2, 3);
            var date2 = new DateTime(2014, 2, 2, 4, 2, 3);

            var links = new[]
            {
                new Link
                {
                    Id = 1,
                    CreationDate = date1,
                    Url = "some url",
                    Redirects = 18
                },
                new Link
                {
                    Id = 2,
                    CreationDate = date2,
                    Url = "http://doma.in/url",
                    Redirects = 42
                }
            };

            var linkService = A.Fake<IDataLinkService>();
            A.CallTo(() => linkService.GetLinks(A<long>.Ignored))
                .Returns(links);

            var linkBuilder = A.Fake<IShortLinkBuilder>();
            A.CallTo(() => linkBuilder.Build(A<ActionContext>.Ignored, A<long>.Ignored))
                .ReturnsLazily((ActionContext context, long id) => $"ZXA{id}");

            var controller = new LinkController(linkService, linkBuilder, CreateTokenServiceMock());

            // Act
            var result = (controller.Get().Value as IEnumerable<LinkViewModel>).ToArray();

            // Assert
            Assert.That(result[0].CreationDate, Is.EqualTo(date1));
            Assert.That(result[0].OriginalUrl, Is.EqualTo("some url"));
            Assert.That(result[0].RedirectsCount, Is.EqualTo(18));
            Assert.That(result[0].ShortUrl, Is.EqualTo("ZXA1"));

            Assert.That(result[1].CreationDate, Is.EqualTo(date2));
            Assert.That(result[1].OriginalUrl, Is.EqualTo("http://doma.in/url"));
            Assert.That(result[1].RedirectsCount, Is.EqualTo(42));
            Assert.That(result[1].ShortUrl, Is.EqualTo("ZXA2"));
        }

        [Test]
        public void AddShouldAddNewLinkToDbAndReturnShortUrl()
        {
            // Arrange
            var linkService = A.Fake<IDataLinkService>();
            A.CallTo(() => linkService.Add(A<Link>.Ignored))
                .ReturnsLazily((Link link) => link);

            var linkBuilder = A.Fake<IShortLinkBuilder>();
            A.CallTo(() => linkBuilder.Build(A<ActionContext>.Ignored, A<long>.Ignored))
                .ReturnsLazily((ActionContext context, long id) => $"coded short url");

            var controller = new LinkController(linkService, linkBuilder, CreateTokenServiceMock());

            // Act
            var result = controller.Add("some url").Value as AddViewModel;

            // Assert
            Assert.That(result.ShortUrl, Is.EqualTo("coded short url"));
        }

        private ITokenService CreateTokenServiceMock()
        {
            var tokenService = A.Fake<ITokenService>();

            A.CallTo(() => tokenService.GetUserId(A<IPrincipal>.Ignored)).Returns(42);

            return tokenService;
        }
    }
}