using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using DtoSubsystem;
using ImdbRestService;
using ImdbRestService.Handlers;
using ImdbRestService.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ImdbRestServiceTests
{
    [TestClass]
    public class MovieHandlerTests
    {
        private MovieHandler _handler;

        [TestInitialize]
        public void Init()
        {
            var repositoryMock = new Mock<IExternalMovieDatabaseRepository>();
            repositoryMock.Setup(x => x.GetMoviesFromIMDbAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new List<MovieDto>()));
            var entitiesMock = new Mock<IImdbEntities>();
            
            entitiesMock.Setup(x => x.Movies).Returns(
                new FakeDbSet<Movie>
                {
                    new Movie { Id = 0, Title = "Title", Year = 2013 }
                });
            _handler = new MovieHandler(entitiesMock.Object, repositoryMock.Object);
        }

        [TestMethod]
        public void GettingExistingMovie()
        {
            var responseData = new ResponseData("", HttpStatusCode.BadRequest);
            responseData = _handler.Handle(new List<string> { "?title=Title" }, responseData).Result;

            Assert.AreEqual(responseData.HttpStatusCode, HttpStatusCode.OK);
            Assert.AreEqual(responseData.Message, "[{\"Id\":0,\"Title\":\"Title\",\"Year\":2013,\"EpisodeNumber\":0,\"Kind\":null,\"SeasonNumber\":null,\"SeriesYear\":null,\"EpisodeOf_Id\":null,\"ErrorMsg\":null}]");
        }

        [TestMethod]
        public void TryingToGetNonExistingMovie()
        {
            var responseData = new ResponseData("", HttpStatusCode.OK);
            responseData = _handler.Handle(new List<string> { "?title=NoMovie" }, responseData).Result;

            Assert.AreEqual(responseData.HttpStatusCode, HttpStatusCode.OK);
            Assert.AreEqual(responseData.Message, "[]");
        }

    }
}
