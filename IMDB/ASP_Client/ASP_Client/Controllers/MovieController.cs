using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ASP_Client.Models;
using DtoSubsystem;

namespace ASP_Client.Controllers
{
    /// <summary>
    /// MovieController class is the controller for every view concerning a representation of a movie.
    /// From here the views get the required models to show.
    /// </summary>
    public class MovieController : BaseController
    {
        //private readonly IMoviesRepository _repository;

        //public MovieController() : this(new MoviesRepository())
        //{            
        //}

        //public MovieController(IMoviesRepository repository)
        //{
        //    _repository = repository;
        //}

        /// <summary>
        /// Method creating a list of movies based on a search string and puts them in a MovieOverviewViewModel which
        /// is given to the IndexView.
        /// The method first searches in the local database. If nothing is found, it searches in IMDb's database. If
        /// movies are found there they are added to the local database.
        /// </summary>
        /// <param name="searchString"> search criteria for movies </param>
        /// <returns> A Task containing an ActionResult to be handled </returns>        
        public async Task<ActionResult> SearchMovie(string searchString)
        {
            //if (Session["User"] == null)
            //{
            //    return RedirectToAction("Login", "User");
            //}

            var foundMovies = await Storage.GetMoviesAsync(searchString);

            var movieOverviewViewModel = new MovieOverviewViewModel();

            if (foundMovies.Count != 0)
            {
                movieOverviewViewModel.FoundMovies = foundMovies;
            }


            return View(movieOverviewViewModel);
        }

         // GET: /Search/SearchMovieDetails/5

        /// <summary>
        /// Method getting a movie's details based on an id given to the CommunicationFacade and puts
        /// them in a MovieDetailsViewModel which is given to the IndexView.
        /// </summary>
        /// <param name="id">Id of the movie to look for</param>
        /// <returns>Desired movies </returns>
        [HttpGet]
        public async Task<ActionResult> SearchMovieDetails(int id)
        {
            var movieDetails = await Storage.GetMovieDetailsLocallyAsync(id);

            var movieDetailsViewModel = new MovieDetailsViewModel();

            if (movieDetails != null)
            {
                movieDetailsViewModel.Id = movieDetails.Id;
                movieDetailsViewModel.Title = movieDetails.Title;
                movieDetailsViewModel.Year = movieDetails.Year;

                var temp = movieDetails.Participants.Select(participant => new PersonViewModel
               {
                   Id = participant.Id, 
                   Name = participant.Name, 
                   CharacterName = participant.CharacterName
               }).ToList();

                movieDetailsViewModel.Participants = temp;
            }

            return View(movieDetailsViewModel);

        }
        [HttpPost]
        public async Task<ActionResult> SearchMovieDetails(int movieId, int rating)
        {
            var username = UserSession.GetLoggedInUser().Name;
            var reviewDto = new ReviewDto() {MovieId = movieId, Username = username, Rating = rating};

            var serverReponse = await Storage.RateMovie(reviewDto);
            var ratedMovie = await Storage.GetMovieDetailsLocallyAsyncForce(movieId);

            var movieDetailsViewModel = new MovieDetailsViewModel();

            movieDetailsViewModel.Id = ratedMovie.Id;
            movieDetailsViewModel.Title = ratedMovie.Title;
            movieDetailsViewModel.Year = ratedMovie.Year;
            movieDetailsViewModel.Rating = ratedMovie.Rating; 

            var temp = ratedMovie.Participants.Select(participant => new PersonViewModel
            {
                Id = participant.Id,
                Name = participant.Name,
                CharacterName = participant.CharacterName
            }).ToList();

            movieDetailsViewModel.Participants = temp;

            // There is now RateMovie view... but how else to get the rating?
           return View(movieDetailsViewModel);
        }
    }
}
