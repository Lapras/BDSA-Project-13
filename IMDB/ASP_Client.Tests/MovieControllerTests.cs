using System.Collections.Generic;
using System.Threading.Tasks;
using ASP_Client.Controllers;
using ASP_Client.ModelInitializers;
using ASP_Client.Models;
using ASP_Client.Repositories;
using ASP_Client.Session;
using DtoSubsystem;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ASP_Client.Tests
{
    [TestClass]
    public class MovieControllerTests
    {
        private MovieController _positiveController;
        private Mock<IMovieRepository> _positiveMovieRepositoryMock;
        private Mock<IModelInitializer> _positiveModelInitializerMock;
        private Mock<IUserSession> _positiveUserSessionMock;
        private Mock<IModelInitializer> _negativeModelInitializerMock;
        private Mock<IMovieRepository> _negativeMovieRepositoryMock;
        private Mock<IUserSession> _negativeUserSessionMock;
        private MovieController _negativeController;
        private Mock<IMovieRepository> _customRepositoryMock;

        [TestInitialize]
        public void Init()
        {
            InitializePositiveController();

            InitializeNegativeController();

            InitializeCustomMovieRepositoryMock();
        }

        private void InitializeCustomMovieRepositoryMock()
        {
            _customRepositoryMock = new Mock<IMovieRepository>();
            _customRepositoryMock.Setup(x => x.RateMovie(It.IsAny<RatingDto>())).Returns(Task.FromResult(new ReplyDto()
            {
                Executed = true
            }));
            _customRepositoryMock.Setup(x => x.GetMovieDetailsAsyncForce(It.IsAny<int>()))
                .Returns(Task.FromResult(new MovieDetailsDto()
                {
                    ErrorMsg = "RatingError"
                }));
        }

        private void InitializeNegativeController()
        {
            InitializeNegativeModelInitializerMock();

            InitializeNegativeMovieRepositoryMock();

            InitializeNegativeUserSessionMock();

            _negativeController = new MovieController(_negativeMovieRepositoryMock.Object, _negativeModelInitializerMock.Object, _negativeUserSessionMock.Object);
        }

        private void InitializeNegativeUserSessionMock()
        {
            _negativeUserSessionMock = new Mock<IUserSession>();
            _negativeUserSessionMock.Setup(x => x.IsLoggedIn()).Returns(false);
        }

        private void InitializeNegativeMovieRepositoryMock()
        {
            _negativeMovieRepositoryMock = new Mock<IMovieRepository>();
            _negativeMovieRepositoryMock.Setup(x => x.GetMoviesAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(new List<MovieDto>()
                {
                    new MovieDto() {ErrorMsg = "ErrorMessage"}
                }));
            _negativeMovieRepositoryMock.Setup(x => x.GetMovieDetailsAsync(It.IsAny<int>())).Returns(Task.FromResult(new MovieDetailsDto()
            {
                ErrorMsg = "ErrorMessage"
            }));
            _negativeMovieRepositoryMock.Setup(x => x.RateMovie(It.IsAny<RatingDto>()))
                .Returns(Task.FromResult(new ReplyDto()
                {
                    Executed = false,
                    Message = "ExecutionError"
                }));
        }

        private void InitializeNegativeModelInitializerMock()
        {
            _negativeModelInitializerMock = new Mock<IModelInitializer>();
        }

        private void InitializePositiveController()
        {
            InitializePositiveModelInitializerMock();

            InitializePositiveMovieRepositoryMock();

            InitializePositiveUserSessionMock();

            _positiveController = new MovieController(_positiveMovieRepositoryMock.Object, _positiveModelInitializerMock.Object, _positiveUserSessionMock.Object);
        }

        private void InitializePositiveUserSessionMock()
        {
            _positiveUserSessionMock = new Mock<IUserSession>();
            _positiveUserSessionMock.Setup(x => x.GetLoggedInUser()).Returns(() => new UserModel()
            {
                Name = "Name",
                Password = "Password"
            });
            _positiveUserSessionMock.Setup(x => x.IsLoggedIn()).Returns(true);
        }

        private void InitializePositiveMovieRepositoryMock()
        {
            _positiveMovieRepositoryMock = new Mock<IMovieRepository>();
            _positiveMovieRepositoryMock.Setup(x => x.GetMoviesAsync(It.IsAny<string>())).Returns(Task.FromResult(new List<MovieDto>()
            {
                new MovieDto() {Id = 1, Title = "TitleOne", Year = 2001},
                new MovieDto() {Id = 2, Title = "TitleTwo", Year = 2002},
                new MovieDto() {Id = 3, Title = "TitleThree", Year = 2003}
            }));
            _positiveMovieRepositoryMock.Setup(x => x.GetMovieDetailsAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(new MovieDetailsDto()
                {
                    Id = 1,
                    Title = "Title",
                    Year = 2001
                }));
            _positiveMovieRepositoryMock.Setup(x => x.GetMovieDetailsAsyncForce(It.IsAny<int>()))
                .Returns(Task.FromResult(new MovieDetailsDto()));
            _positiveMovieRepositoryMock.Setup(x => x.RateMovie(It.IsAny<RatingDto>())).Returns(Task.FromResult(new ReplyDto()
            {
                Executed = true
            }));
        }

        private void InitializePositiveModelInitializerMock()
        {
            _positiveModelInitializerMock = new Mock<IModelInitializer>();
            _positiveModelInitializerMock.Setup(x => x.InitializeMovieDetailsViewModelRating(It.IsAny<MovieDetailsDto>()))
                .Returns(new MovieDetailsViewModel()
                {
                    Title = "RatedTitle",
                    Year = 2001,
                    AvgRating = 1
                });
            _positiveModelInitializerMock.Setup(x => x.InitializeMovieDetailsViewModelSearchDetails(It.IsAny<MovieDetailsDto>()))
                .Returns(new MovieDetailsViewModel()
                {
                    Title = "MovieTitle",
                    Year = 2001
                });
        }

        [TestMethod]
        public void GettingMovieDetailsReturnsCorrectMovie()
        {
            Assert.IsNull(_positiveController.MovieDetailsViewModel);

            const int id = 1;
            _positiveController.SearchMovieDetails(id);
            
            _positiveMovieRepositoryMock.Verify(x => x.GetMovieDetailsAsync(id), Times.Exactly(1));
            _positiveModelInitializerMock.Verify(x => x.InitializeMovieDetailsViewModelSearchDetails(It.IsAny<MovieDetailsDto>()), Times.Exactly(1));

            Assert.AreEqual("MovieTitle", _positiveController.MovieDetailsViewModel.Title);
        }

        [TestMethod]
        public void RatingMovieReturnsRatedMovie()
        {
            Assert.IsNull(_positiveController.MovieDetailsViewModel);
            
            _positiveController.SearchMovieDetails(new MovieDetailsViewModel());

            _positiveMovieRepositoryMock.Verify(x => x.RateMovie(It.IsAny<RatingDto>()), Times.Exactly(1));
            _positiveMovieRepositoryMock.Verify(x => x.GetMovieDetailsAsyncForce(It.IsAny<int>()), Times.Exactly(1));
            _positiveModelInitializerMock.Verify(x => x.InitializeMovieDetailsViewModelRating(It.IsAny<MovieDetailsDto>()), Times.Exactly(1));

            Assert.AreEqual("RatedTitle", _positiveController.MovieDetailsViewModel.Title);
        }

        [TestMethod]
        public void FindingSearchedMovies()
        {
            Assert.IsNull(_positiveController.MovieOverviewViewModel);

            const string searchString = "Title";
            _positiveController.SearchMovie(searchString);

            var count = _positiveController.MovieOverviewViewModel.FoundMovies.Count;

            _positiveMovieRepositoryMock.Verify(x => x.GetMoviesAsync(searchString), Times.Exactly(1));
            Assert.IsNotNull(_positiveController.MovieOverviewViewModel.FoundMovies);
            Assert.AreEqual(3, count);
        }

        [TestMethod]
        public void NegativeSearchMovieError()
        {
            Assert.IsNull(_negativeController.MovieDetailsViewModel);
            const string searchString = "Title";
            _negativeController.SearchMovie(searchString);

            _negativeMovieRepositoryMock.Verify(x => x.GetMoviesAsync(searchString), Times.Exactly(1));

            Assert.AreEqual("ErrorMessage", _negativeController.MovieOverviewViewModel.ErrorMsg);
        }

        [TestMethod]
        public void NegativeMovieDetails()
        {
            Assert.IsNull(_negativeController.MovieDetailsViewModel);
            const int id = 1;
            _negativeController.SearchMovieDetails(id);
            
            _negativeMovieRepositoryMock.Verify(x => x.GetMovieDetailsAsync(id), Times.Exactly(1));
            _negativeModelInitializerMock.Verify(x => x.InitializeMovieDetailsViewModelSearchDetails(It.IsAny<MovieDetailsDto>()), Times.Never);

            Assert.AreEqual("ErrorMessage", _negativeController.MovieDetailsViewModel.ErrorMsg);
        }

        [TestMethod]
        public void RatingMovieSessionExpired()
        {
            Assert.IsNull(_positiveController.MovieDetailsViewModel);

            _negativeController.SearchMovieDetails(new MovieDetailsViewModel());

            _negativeMovieRepositoryMock.Verify(x => x.RateMovie(It.IsAny<RatingDto>()), Times.Never);
            _negativeMovieRepositoryMock.Verify(x => x.GetMovieDetailsAsyncForce(It.IsAny<int>()), Times.Never);
            _negativeModelInitializerMock.Verify(x => x.InitializeMovieDetailsViewModelRating(It.IsAny<MovieDetailsDto>()), Times.Never);

            Assert.AreEqual("Your session expired, please log back in", _negativeController.MovieDetailsViewModel.ErrorMsg);
        }

        [TestMethod]
        public void RatingMovieExecutionFail()
        {
            _negativeController = new MovieController(_negativeMovieRepositoryMock.Object, _negativeModelInitializerMock.Object, _positiveUserSessionMock.Object);

            Assert.IsNull(_positiveController.MovieDetailsViewModel);

            _negativeController.SearchMovieDetails(new MovieDetailsViewModel());

            _negativeMovieRepositoryMock.Verify(x => x.RateMovie(It.IsAny<RatingDto>()), Times.Exactly(1));
            _negativeMovieRepositoryMock.Verify(x => x.GetMovieDetailsAsyncForce(It.IsAny<int>()), Times.Never);
            _negativeModelInitializerMock.Verify(x => x.InitializeMovieDetailsViewModelRating(It.IsAny<MovieDetailsDto>()), Times.Never);

            Assert.AreEqual("ExecutionError", _negativeController.MovieDetailsViewModel.ErrorMsg);
        }

        [TestMethod]
        public void RatingMovieError()
        {
            _negativeController = new MovieController(_customRepositoryMock.Object, _negativeModelInitializerMock.Object, _positiveUserSessionMock.Object);

            Assert.IsNull(_positiveController.MovieDetailsViewModel);

            _negativeController.SearchMovieDetails(new MovieDetailsViewModel());

            _customRepositoryMock.Verify(x => x.RateMovie(It.IsAny<RatingDto>()), Times.Exactly(1));
            _customRepositoryMock.Verify(x => x.GetMovieDetailsAsyncForce(It.IsAny<int>()), Times.Exactly(1));
            _negativeModelInitializerMock.Verify(x => x.InitializeMovieDetailsViewModelRating(It.IsAny<MovieDetailsDto>()), Times.Never);

            Assert.AreEqual("RatingError", _negativeController.MovieDetailsViewModel.ErrorMsg);
        }
    }
}
