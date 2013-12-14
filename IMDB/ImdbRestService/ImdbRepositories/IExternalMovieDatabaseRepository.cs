using System.Collections.Generic;
using System.Threading.Tasks;
using DtoSubsystem;

namespace ImdbRestService.ImdbRepositories
{
    /// <summary>
    /// Interface for the class communicating with the external Imdb database
    /// </summary>
    public interface IExternalMovieDatabaseRepository
    {
        /// <summary>
        /// Get a movie from the MyMovieApi interface
        /// </summary>
        /// <param name="searchString">Name of the movie to search for</param>
        /// <returns>List of matching movies</returns>
        Task<List<MovieDto>> GetMoviesFromImdbAsync(string searchString);
    }
}
