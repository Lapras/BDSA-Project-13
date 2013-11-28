using System;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WPF_Client.Dtos;
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
        public void MovieSearch_CorrectMoviesFound()
        {
            var twillight1 = new MovieSearchDto
            {
                Title = "Twillight 1"
            };

            var twillight2 = new MovieSearchDto
            {
                Title = "Twillight 2"
            };

            var twillight3 = new MovieSearchDto
            {
                Title = "Twillight 3"
            };

            //Mediator.SearchString = "Twillight"; // We simulate that the user searches for "Twillight".
            //Mediator.SearchType = 0; // We simulate that the user selected the search type: movies.

            
            

            // We simluate that the model returns the correct result twillight 1, twillight 2 and twillight 3.
            var movieCollection = new ObservableCollection<MovieSearchDto>() { twillight1, twillight2, twillight3 };
            var modelMock = new Mock<IModel>();
            modelMock.Setup(m => m.MovieSearchDtos("Twillight")).Returns(movieCollection);

            SearchController._model = modelMock.Object;
            //var searchResultViewModel = new SearchResultViewModel(model); // We inject the searchResultViewModel with our model mock.
            SearchController.Search("Twillight", 0);

            var searchResultViewModel = new SearchResultViewModel();

            // We test if the SearchResultView really shows the correct movies.
            Assert.AreEqual(movieCollection, searchResultViewModel.MovieSearchDtos);


        }
    }
}
