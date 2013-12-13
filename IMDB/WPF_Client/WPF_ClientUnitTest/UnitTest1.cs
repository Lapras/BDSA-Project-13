using System;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DtoSubsystem;
using WPF_Client.Model;
using WPF_Client.View;
using WPF_Client.ViewModel;
using WPF_Client.Controller;
using WPF_Client;

namespace WPF_ClientUnitTest
{
    [TestClass]
    public class UnitTest1
    {


        [TestInitialize]
        public void init()
        {
            

        }


        /// <summary>
        ///  Tests whether the correct movies are found.
        /// </summary>
        [TestMethod]
        public void MovieSearch_CorrectMoviesDisplayed()
        {
            var twillight1 = new MovieDto
            {
                Title = "Twillight 1"
            };

            var twillight2 = new MovieDto
            {
                Title = "Twillight 2"
            };

            var twillight3 = new MovieDto
            {
                Title = "Twillight 3"
            };

            
            // We simluate that the model returns the correct result twillight 1, twillight 2 and twillight 3.
            var movieCollection = new ObservableCollection<MovieDto>() { twillight1, twillight2, twillight3 };
            var modelMock = new Mock<IModel>();
            modelMock.Setup(m => m.MovieDtos("Twillight")).Returns(movieCollection);

            SearchController._model = modelMock.Object; // inject the controller responsible for searching with the mock model.
            SearchController.Search("Twillight", 0); // The search controller searches.
            
            var movieSearchResultViewModel = new MovieSearchResultViewModel(); // we create the view model (its constructor takes the search results from the controller)

            // We test if the SearchResultView really shows the correct movies.
            Assert.AreEqual(movieCollection, movieSearchResultViewModel.MovieDtos);
            Assert.AreEqual(movieCollection.Count, movieSearchResultViewModel.MovieDtos.Count);


            
        }

        



        /// <summary>
        ///  Tests whether a user gets logged in.
        /// </summary>
        [TestMethod]
        public void Login_CorrectlyLoggedIn()
        {

            var modelMock = new Mock<IModel>();
            modelMock.Setup(m => m.Login("Simon", "password")).Returns(true);
            SessionController._model = modelMock.Object;

            // we simulate that a viewmodel wants to log in a user and therefore calls the sessioncontroller
            // with the login information
            SessionController.Login("Simon", "password");

            Assert.AreEqual("Simon",SessionController._currentUser);
            Assert.AreEqual(true, SessionController._isLoggedIn);

        }


        /// <summary>
        ///  Tests whether a user correctly doesn't login because of incorrect information.
        /// </summary>
        [TestMethod]
        public void Login_CorrectlyNotLoggedIn()
        {

            var modelMock = new Mock<IModel>();
            modelMock.Setup(m => m.Login("Simon1", "password")).Returns(false);
            SessionController._model = modelMock.Object;

            // we simulate that a viewmodel wants to log in a user and therefore calls the sessioncontroller
            // with the login information
            SessionController.Login("Simon1", "password");

            Assert.AreEqual(null, SessionController._currentUser);
            Assert.AreEqual(false, SessionController._isLoggedIn);

        }
        



        /// <summary>
        ///  Tests whether a user gets logged out.
        /// </summary>
        [TestMethod]
        public void Logout_CorrectlyLoggedOut()
        {
            SessionController._isLoggedIn = true;
            SessionController._currentUser = "Morten";

            // we simulate that a viewmodel wants to log out a user and therefore calls the sessioncontroller
            SessionController.Logout();

            Assert.AreEqual(null, SessionController._currentUser);
            Assert.AreEqual(false, SessionController._isLoggedIn);

        }



        /// <summary>
        ///  Tests whether a user is correctly created.
        /// </summary>
        [TestMethod]
        public void CreateProfile_CorrectlyCreated()
        {

            var modelMock = new Mock<IModel>();
            //we mock that the model successfully creates the user.
            modelMock.Setup(m => m.CreateProfile("Mogens", "password")).Returns(true);
            UserProfileController._model = modelMock.Object;

            //we simlulate that we create a profile
            var result = UserProfileController.CreateProfile("Mogens", "password");


            Assert.AreEqual(true, result);


        }


        /// <summary>
        ///  Tests whether the correct moviedetail is showed.
        /// </summary>
        [TestMethod]
        public void Movie_CorrectMovieDetailDisplayed()
        {
            MovieDetailsDto testMovieDetail = new MovieDetailsDto()
            {
                Id = 10,
                Kind = "Sci-fi",
                Title = "Predator",
                Year = 1987
            };


            var modelMock = new Mock<IModel>();
            modelMock.Setup(m => m.MovieDetailsDto(10)).Returns(testMovieDetail);
            HollywoodController._model = modelMock.Object;


            //simluate that the MovieSearchResultViewModel selects a movie with the id 10.
            HollywoodController.GetMovie(10);

            
            MovieProfileViewModel movieProfileViewModel = new MovieProfileViewModel();


            Assert.AreEqual(testMovieDetail, movieProfileViewModel.MovieDetailsDto);

            
        }


        /// <summary>
        ///  Tests whether the correct personDetail is display
        /// </summary>
        [TestMethod]
        public void Actor_CorrectPersonDetailDisplayed()
        {
            PersonDetailsDto testPersonDetail = new PersonDetailsDto()
            {
                Id = 10,
                Name = "Mark Wahlberg",
                
            };


            var modelMock = new Mock<IModel>();
            modelMock.Setup(m => m.PersonDetailsDto(10)).Returns(testPersonDetail);
            HollywoodController._model = modelMock.Object;


            //simluate that an actor is selected with the id 10.
            HollywoodController.GetActor(10);


            ActorProfileViewModel actorProfileViewModel = new ActorProfileViewModel();


            Assert.AreEqual(testPersonDetail, actorProfileViewModel.PersonDetailsDto);


        }




        /// <summary>
        ///  Tests whether there correctly aren't any movies found and display
        /// </summary>
        [TestMethod]
        public void Movie_NoMoviesFound()
        {
            // We simluate that the model returns the correct empty result
            var movieCollection = new ObservableCollection<MovieDto>() { };
            var modelMock = new Mock<IModel>();
            modelMock.Setup(m => m.MovieDtos("Twillight")).Returns(movieCollection);

            SearchController._model = modelMock.Object; // inject the controller responsible for searching with the mock model.

            SearchController.Search("Twillight", 0); // The search controller searches for twillight.

            var movieSearchResultViewModel = new MovieSearchResultViewModel(); // we create the view model (its constructor takes the search results from the controller)

            // We test if the SearchResultView really shows the correct movies.
            Assert.AreEqual(movieCollection, movieSearchResultViewModel.MovieDtos);
            Assert.AreEqual(movieCollection.Count, movieSearchResultViewModel.MovieDtos.Count);

        }




    }
}
