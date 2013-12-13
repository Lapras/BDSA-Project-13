using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages;
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

            if (foundMovies != null && foundMovies.First().ErrorMsg.IsEmpty())
            {
                if (foundMovies.Count != 0)
                {
                    movieOverviewViewModel.FoundMovies = foundMovies;
                }
            }
            else
            {
                if (foundMovies != null) movieOverviewViewModel.ErrorMsg = foundMovies.First().ErrorMsg;
            }

            return View(movieOverviewViewModel);
        }

         // GET: /Search/SearchMovieDetails/5

        /// <summary>
        /// Method getting a movie's details based on an id given to the CommunicationFacade and puts
        /// them in a MovieDetailsViewModel which is given to the IndexView.
        /// </summary>
        /// <param name="id">Id of the movie to look for</param>
        /// <returns>Desired movie in the view</returns>
        [HttpGet]
        public async Task<ActionResult> SearchMovieDetails(int id)
        {
            var movieDetails = await Storage.GetMovieDetailsLocallyAsync(id);

            var movieDetailsViewModel = new MovieDetailsViewModel();

            if (movieDetails != null && movieDetails.ErrorMsg.IsEmpty())
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
            else
            {
                if (movieDetails != null) movieDetailsViewModel.ErrorMsg = movieDetails.ErrorMsg;
            }

            return View(movieDetailsViewModel);

        }
       
        /// <summary>
        /// Same as the one with just the id, but used after the user rated a movie
        /// </summary>
        /// <param name="model">With the rating updated model</param>
        /// <returns>Desired movie in the view</returns>
        [HttpPost]
        public async Task<ActionResult> SearchMovieDetails(MovieDetailsViewModel model)
        {
            var username = UserSession.GetLoggedInUser().Name;
            var reviewDto = new ReviewDto {MovieId = model.Id, Username = username, Rating = model.UserRating};

            var serverReponse = await Storage.RateMovie(reviewDto);

            if (serverReponse.Executed)
            {
                var ratedMovie = await Storage.GetMovieDetailsLocallyAsyncForce(model.Id);

                if (ratedMovie.ErrorMsg.IsEmpty())
                {
                    model.AvgRating = ratedMovie.AvgRating;
                    model.Title = ratedMovie.Title;
                    model.Id = ratedMovie.Id;
                    model.Year = ratedMovie.Year;

                    var temp = ratedMovie.Participants.Select(participant => new PersonViewModel
                    {
                        Id = participant.Id,
                        Name = participant.Name,
                        CharacterName = participant.CharacterName
                    }).ToList();

                    model.Participants = temp;

                    return View(model);
                }

                return View(new MovieDetailsViewModel
                {
                    ErrorMsg = ratedMovie.ErrorMsg
                });
            }


            model.ErrorMsg = serverReponse.Message;
           
            return View(model);
        }
    }
}
