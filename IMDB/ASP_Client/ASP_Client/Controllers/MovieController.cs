﻿using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
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
            var foundMovies = await GetMoviesLocallyAsync(searchString);

            var movieOverviewViewModel = new MovieOverviewViewModel();

            if (foundMovies.Count != 0)
            {
                movieOverviewViewModel.FoundMovies = foundMovies;
            }
            //else
            //{
            //    foundMovies = await GetMoviesFromIMDbAsync(searchString);

            //    movieOverviewViewModel.FoundMovies = foundMovies;
            //    //if (movieOverviewViewModel.FoundMovies.Count != 0)
            //    //{
            //    //    PostMoviesToLocalDbAsync(movieOverviewViewModel.FoundMovies);
            //    //}
            //}

            return View(movieOverviewViewModel);
        }

        private async Task<List<MovieDto>> GetMoviesLocallyAsync(string searchString)
        {
            using (var httpClient = new HttpClient())
            {
                return JsonConvert.DeserializeObject<List<MovieDto>>(
                    await httpClient.GetStringAsync("http://localhost:54321/movies/?title=" + searchString)
                );
            }
        }

        private async Task<List<MovieDto>> GetMoviesFromIMDbAsync(string searchString)
        {
            using (var httpClient = new HttpClient())
            {
                return JsonConvert.DeserializeObject<List<MovieDto>>(
                    await httpClient.GetStringAsync("http://mymovieapi.com/?title=" + searchString + "&limit=20")
                );
            }
        }

        private async void PostMoviesToLocalDbAsync(List<MovieDto> movies)
        {
            using (var httpClient = new HttpClient())
            {
                await httpClient.PostAsJsonAsync("http://localhost:54321/movies/", movies);
            }
        }

        //
        // GET: /Search/MovieDetails/5

        //public ActionResult MovieDetails(int id)
        //{
        //    using (var entities = new ImdbEntities())
        //    {
        //        var movie = entities.Movies.Find(id);

        //        var participants = (from participant in entities.Participate
        //            where participant.Movie_Id == id
        //            join person in entities.People on participant.Person_Id equals person.Id
        //            select new ActorViewModel()
        //            {
        //                Id = person.Id,
        //                Name = person.Name,
        //                CharacterName = participant.CharName
        //            }).ToList();

        //        var movieDetailsView = new MovieDetailsViewModel()
        //        {
        //            Id = movie.Id,
        //            Title = movie.Title,
        //            Year = movie.Year,
        //            Participants = participants
        //        };

        //        return View(movieDetailsView);
        //    }
        //}

        //public ActionResult PersonDetails(int id)
        //{
        //    using (var entities = new ImdbEntities())
        //    {
        //        var person = entities.People.Find(id);

        //        var personDetails = new PersonDetailsViewModel()
        //        {
        //            Id = id,
        //            Name = person.Name,
        //            Gender = person.Gender
        //        };

        //        return View(personDetails);
        //    }
        //}
    }
}
