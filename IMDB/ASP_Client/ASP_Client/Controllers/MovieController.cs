﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.Mvc;
using ASP_Client.Models;
using DtoSubsystem;
using Newtonsoft.Json;

namespace ASP_Client.Controllers
{
    /// <summary>
    /// MovieController class is the controller for every view concerning a representation of a movie.
    /// From here the views get the required models to show.
    /// </summary>
    public class MovieController : Controller
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
        public async Task<ActionResult> Index(string searchString)
        {
            //if (Session["User"] == null)
            //{
            //    return RedirectToAction("Login", "User");
            //}

            var foundMovies = await CommunicationFacade.GetMoviesAsync(searchString);  
            
            var movieOverviewViewModel = new MovieOverviewViewModel();

            if (foundMovies.Count != 0)
            {
                movieOverviewViewModel.FoundMovies = foundMovies;
            }


            return View(movieOverviewViewModel);
        }

         // GET: /Search/MovieDetails/5

        public async Task<ActionResult> MovieDetails(int id)
        {

            var movieDetails = await CommunicationFacade.GetMovieDetailsLocallyAsync(id);

            var movieDetailsViewModel = new MovieDetailsViewModel();

            if (movieDetails != null)
            {
                movieDetailsViewModel.Id = movieDetails.Id;
                movieDetailsViewModel.Title = movieDetails.Title;
                movieDetailsViewModel.Year = movieDetails.Year;

               var temp = movieDetails.Participants.Select(participant => new ActorViewModel()
               {
                   Id = participant.Id, 
                   Name = participant.Name, 
                   CharacterName = participant.CharacterName
               }).ToList();

                movieDetailsViewModel.Participants = temp;
            }

            return View(movieDetailsViewModel);

        }
    }
}
