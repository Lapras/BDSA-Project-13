using System.Collections.Generic;
using System.Linq;
using ASP_Client.Models;
using DtoSubsystem;

namespace ASP_Client.ModelInitializers
{
    class ModelInitializer : IModelInitializer
    {
        public MovieDetailsViewModel InitializeMovieDetailsViewModelSearchDetails(MovieDetailsDto movieDetailsDto)
        {
            var movieDetailsViewModel = new MovieDetailsViewModel();

            movieDetailsViewModel.Id = movieDetailsDto.Id;
            movieDetailsViewModel.Title = movieDetailsDto.Title;
            movieDetailsViewModel.Year = movieDetailsDto.Year;

            var temp = movieDetailsDto.Participants.Select(participant => new PersonViewModel
            {
                Id = participant.Id,
                Name = participant.Name,
                CharacterName = participant.CharacterName
            }).ToList();

            movieDetailsViewModel.Participants = temp;

            return movieDetailsViewModel;
        }

        public MovieDetailsViewModel InitializeMovieDetailsViewModelRating(MovieDetailsDto ratedMovieDetailsDto)
        {
            var movieDetailsViewModel = new MovieDetailsViewModel();

            movieDetailsViewModel.AvgRating = ratedMovieDetailsDto.AvgRating;
            movieDetailsViewModel.Title = ratedMovieDetailsDto.Title;
            movieDetailsViewModel.Id = ratedMovieDetailsDto.Id;
            movieDetailsViewModel.Year = ratedMovieDetailsDto.Year;

            var temp = ratedMovieDetailsDto.Participants.Select(participant => new PersonViewModel
            {
                Id = participant.Id,
                Name = participant.Name,
                CharacterName = participant.CharacterName
            }).ToList();

            movieDetailsViewModel.Participants = temp;

            return movieDetailsViewModel;
        }
    }
}