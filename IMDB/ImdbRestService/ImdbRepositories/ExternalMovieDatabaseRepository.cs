using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using DtoSubsystem;
using Newtonsoft.Json;

namespace ImdbRestService.ImdbRepositories
{
    public class ExternalMovieDatabaseRepository : IExternalMovieDatabaseRepository
    {
        /// <summary>
        /// Get a movie from the MyMovieApi interface
        /// </summary>
        /// <param name="searchString">Name of the movie to search for</param>
        /// <returns>List of matching movies</returns>
        public async Task<List<MovieDto>> GetMoviesFromIMDbAsync(string searchString)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync("http://mymovieapi.com/?title=" + searchString);

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();

                        if (result.Equals("{\"code\":404, \"error\":\"Film not found\"}"))
                        {
                            return new List<MovieDto>();
                        }

                        return JsonConvert.DeserializeObject<List<MovieDto>>(result);
                    }

                    return new List<MovieDto>();

                }
            }
            catch (Exception)
            {
                Console.WriteLine("External database not available");
                return new List<MovieDto>();
            }
        }
    }
}