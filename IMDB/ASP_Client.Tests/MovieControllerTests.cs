using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ASP_Client.ClientRequests;
using ASP_Client.Controllers;
using DtoSubsystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ASP_Client.Tests
{
    [TestClass]
    public class MovieControllerTests
    {
        private MovieController _controller;
        [TestInitialize]
        public void Init()
        {
            var repositoryMock = new Mock<IMovieRepository>();
            repositoryMock.Setup(x => x.GetMoviesAsync(It.IsAny<string>())).Returns(Task.FromResult(new List<MovieDto>()
            {
                new MovieDto() {Id = 1, Title = "TitleOne", Year = 2001},
                new MovieDto() {Id = 2, Title = "TitleTwo", Year = 2002},
                new MovieDto() {Id = 3, Title = "TitleThree", Year = 2003}
            }));
            repositoryMock.Setup(x => x.GetMovieDetailsAsync(It.IsAny<int>())).Returns(Task.FromResult(new MovieDetailsDto()
            {
                Id = 1,
                Title = "Title",
                Year = 2001
            }));
            _controller = new MovieController(repositoryMock.Object);
        }

        [TestMethod]
        public void Test()
        {
            var result = _controller.SearchMovieDetails(1).Result as ViewResult;
            Assert.AreEqual("SearchMovieDetails", result.ViewName);
        }
    }
}
