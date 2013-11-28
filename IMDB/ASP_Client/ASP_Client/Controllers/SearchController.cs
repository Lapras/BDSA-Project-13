using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using ASP_Client.Models;
using DtoSubsystem;
using Newtonsoft.Json;

namespace ASP_Client.Controllers
{
    public class SearchController : Controller
    {     
        public async Task<ActionResult> Index(string searchString)
        {
            var foundMovies = await GetMoviesAsync(searchString);

            var movieOverviewViewModel = new MovieOverviewViewModel();

            movieOverviewViewModel.FoundMovies = foundMovies;

            return View(movieOverviewViewModel);
        }

        private async Task<List<MovieDto>> GetMoviesAsync(string searchString)
        {
            using (var httpClient = new HttpClient())
            {
                return JsonConvert.DeserializeObject<List<MovieDto>>(
                    await httpClient.GetStringAsync("http://localhost:54321/movies/?title=" + searchString)
                );
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
