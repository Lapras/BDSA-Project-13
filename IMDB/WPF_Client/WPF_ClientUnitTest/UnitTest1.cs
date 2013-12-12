using System;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using DtoSubsystem;
using WPF_Client.Model;
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
            SessionController.LoginIn("Simon", "password");

            Assert.AreEqual("Simon",SessionController._currentUser);
            Assert.AreEqual(true, SessionController._isLoggedIn);

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

    }
}
