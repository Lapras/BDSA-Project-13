using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using DtoSubsystem;
using Newtonsoft.Json;

namespace ImdbRestService.Handlers
{
    /// <summary>
    /// Class in charge of handling movie requests
    /// </summary>
    public class MovieHandler : IHandler
    {
        private const string PathSegment = "movies";

        /// <summary>
        /// Method checking if the given path segment matches the one that
        /// this handler can handle
        /// </summary>
        /// <param name="pathSegment"> the input path segment string </param>
        /// <returns> wether or not the class is able to handle the request </returns>
        public bool CanHandle(string pathSegment)
        {
            return pathSegment.Equals(PathSegment, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Method handling the response data by checking the path, get the movies,
        /// serialize them and returning them in the message in a new ResponseData objec
        /// </summary>
        /// <param name="path"> the path used to see how to operate the data </param>
        /// <param name="responseData"> the response data to be returned if no operations are available to the path </param>
        /// <returns></returns>
        public async Task<ResponseData> Handle(List<string> path, ResponseData responseData)
        {
            if (path != null && path.Count == 1)
            {
                var firstSegment = path.First();
                if (firstSegment.StartsWith("?"))
                {
                    var key = firstSegment.Substring(1).Split(new[] { '=' })[0];
                    var value = firstSegment.Split(new[] { '=' })[1];

                    if (key == "title")
                    {
                        var movies = GetMoviesByTitle(value);

                        if (movies.Count == 0)
                        {
                            movies = await GetMoviesFromIMDbAsync(value);
                            // Adding found movies to app server database
                            //if (movies.Count != 0)
                            //{
                            //    using (var entities = new ImdbEntities())
                            //    {
                            //        foreach (var movieDto in movies)
                            //        {
                            //            entities.Movies.Add(new Movie()
                            //            {
                            //                Id = movieDto.Id
                            //            })
                            //        }
                            //    }
                            //}
                        }

                        var msg = new JavaScriptSerializer().Serialize(movies);
                        return new ResponseData(msg, HttpStatusCode.OK);
                    }
                }
            }

            return responseData;
        }

        /// <summary>
        /// Method recieving movies by title from the local database
        /// </summary>
        /// <param name="title"> the title to search for </param>
        /// <returns> a list of MovieDto's containing information on the movies found </returns>
        public List<MovieDto> GetMoviesByTitle(string title)
        {
            using (var entities = new ImdbEntities())
            {
                if (String.IsNullOrEmpty(title))
                {
                    return (from m in entities.Movies
                            select new MovieDto()
                            {
                                Id = m.Id,
                                Title = m.Title,
                                Year = m.Year
                            }).Take(20).ToList();
                }

                return (from m in entities.Movies
                        where m.Title.Contains(title)
                        select new MovieDto()
                        {
                            Id = m.Id,
                            Title = m.Title,
                            Year = m.Year
                        }).ToList();
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

    }
}