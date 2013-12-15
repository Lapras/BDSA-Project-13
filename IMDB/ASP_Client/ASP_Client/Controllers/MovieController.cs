using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.WebPages;
using ASP_Client.ClientRequests;
using ASP_Client.ModelInitializers;
using ASP_Client.Models;
using ASP_Client.Repositories;
using ASP_Client.Session;
using DtoSubsystem;

namespace ASP_Client.Controllers
{
    /// <summary>
    /// MovieController class is the controller for every view concerning a representation of a movie.
    /// From here the views get the required models to show.
    /// </summary>
    public class MovieController : BaseController
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IModelInitializer _modelInitializer;
        public MovieDetailsViewModel MovieDetailsViewModel { get; set; }
        public MovieOverviewViewModel MovieOverviewViewModel { get; set; }

        public MovieController() : this(new MovieRepository(), new ModelInitializer(), null)
        {            
        }

        public MovieController(IMovieRepository movieRepository, IModelInitializer modelInitializer, IUserSession userSession)
            : base(userSession)
        {
            _movieRepository = movieRepository;
            _modelInitializer = modelInitializer;
        }

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

            var foundMovies = await _movieRepository.GetMoviesAsync(searchString);

            MovieOverviewViewModel = new MovieOverviewViewModel();

            if (foundMovies != null && foundMovies.First().ErrorMsg.IsEmpty())
            {
                if (foundMovies.Count != 0)
                {
                    MovieOverviewViewModel.FoundMovies = foundMovies;
                }
            }
            else
            {
                if (foundMovies != null) MovieOverviewViewModel.ErrorMsg = foundMovies.First().ErrorMsg;
            }

            return View(MovieOverviewViewModel);
        }

        /// <summary>
        /// Method getting a movie's details based on an id given to the CommunicationFacade and puts
        /// them in a MovieDetailsViewModel which is given to the IndexView.
        /// </summary>
        /// <param name="id">Id of the movie to look for</param>
        /// <returns>Desired movie in the view</returns>
        [HttpGet]
        public async Task<ActionResult> SearchMovieDetails(int id)
        {
            var movieDetails = await _movieRepository.GetMovieDetailsAsync(id);

            MovieDetailsViewModel = new MovieDetailsViewModel();

            if (movieDetails != null && movieDetails.ErrorMsg.IsEmpty())
            {
                MovieDetailsViewModel = _modelInitializer.InitializeMovieDetailsViewModelSearchDetails(movieDetails);
            }
            else
            {
                if (movieDetails != null) MovieDetailsViewModel.ErrorMsg = movieDetails.ErrorMsg;
            }

            return View(MovieDetailsViewModel);

        }
       
        /// <summary>
        /// Same as the one with just the id, but used after the user rated a movie
        /// </summary>
        /// <param name="movieDetailsViewModel">With the rating updated model</param>
        /// <returns>Desired movie in the view</returns>
        [HttpPost]
        public async Task<ActionResult> SearchMovieDetails(MovieDetailsViewModel movieDetailsViewModel)
        {
            MovieDetailsViewModel = movieDetailsViewModel;

            if (UserSession.IsLoggedIn())
            {
                var user = UserSession.GetLoggedInUser();
                var rating = new RatingDto {MovieId = MovieDetailsViewModel.Id, Username = user.Name, Rating = MovieDetailsViewModel.UserRating};

                var serverReponse = await _movieRepository.RateMovie(rating);

                if (serverReponse.Executed)
                {
                    var ratedMovie = await _movieRepository.GetMovieDetailsAsyncForce(MovieDetailsViewModel.Id);

                    if (ratedMovie.ErrorMsg.IsEmpty())
                    {
                        MovieDetailsViewModel = _modelInitializer.InitializeMovieDetailsViewModelRating(ratedMovie);

                        return View(MovieDetailsViewModel);
                    }

                    MovieDetailsViewModel.ErrorMsg = ratedMovie.ErrorMsg;
                    return View(MovieDetailsViewModel);
                }

                MovieDetailsViewModel.ErrorMsg = serverReponse.Message;
            }
            else
            {
                MovieDetailsViewModel.ErrorMsg = "Your session expired, please log back in";
            }
            return View(MovieDetailsViewModel);
        }
    }
}
