using ASP_Client.Models;
using DtoSubsystem;

namespace ASP_Client.ModelInitializers
{
    public interface IModelInitializer
    {
        MovieDetailsViewModel InitializeMovieDetailsViewModelSearchDetails(MovieDetailsDto movieDetailsDto);
        MovieDetailsViewModel InitializeMovieDetailsViewModelRating(MovieDetailsDto movieDetailsDto);
    }
}