using System.Collections.Generic;
using System.Threading.Tasks;
using DtoSubsystem;

namespace ImdbRestService.ImdbRepositories
{
    public interface IExternalMovieDatabaseRepository
    {
        Task<List<MovieDto>> GetMoviesFromIMDbAsync(string searchString);
    }
}
