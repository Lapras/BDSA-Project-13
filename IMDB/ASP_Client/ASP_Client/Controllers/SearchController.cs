using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ASP_Client.Models;

namespace ASP_Client.Controllers
{
    public class SearchController : Controller
    {
        
        public ActionResult Index(string searchString)
        {
            using (var entities = new ImdbEntities())
            {
                var movieOverviewViewModel = new MovieOverviewViewModel();

                var movies = from m in entities.Movies
                    select new MovieViewModel()
                {
                    Id = m.Id,
                    Title = m.Title,
                    Year = m.Year
                };

                if (!String.IsNullOrEmpty(searchString))
                {
                    movies = movies.Where(s => s.Title.Contains(searchString));
                }

                movieOverviewViewModel.FoundMovies.AddRange(movies);

                return View(movieOverviewViewModel);
            }
        }

        //
        // GET: /Search/MovieDetails/5

        public ActionResult MovieDetails(int id)
        {
            using (var entities = new ImdbEntities())
            {
                var movie = entities.Movies.Find(id);

                var participants = (from participant in entities.Participate
                    where participant.Movie_Id == id
                    join person in entities.People on participant.Person_Id equals person.Id
                    select new ActorViewModel()
                    {
                        Id = person.Id,
                        Name = person.Name,
                        CharacterName = participant.CharName
                    }).ToList();

                var movieDetailsView = new MovieDetailsViewModel()
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Year = movie.Year,
                    Participants = participants
                };

                return View(movieDetailsView);
            }
        }

        public ActionResult PersonDetails(int id)
        {
            using (var entities = new ImdbEntities())
            {
                var person = entities.People.Find(id);

                var personDetails = new PersonDetailsViewModel()
                {
                    Id = id,
                    Name = person.Name,
                    Gender = person.Gender
                };

                return View(personDetails);
            }
        }
    }
}
