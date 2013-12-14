using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using DtoSubsystem;
using ImdbRestService.Handlers;
using ImdbRestService.ImdbRepositories;

namespace ImdbRestService.Mapper
{
    public class MovieDetailsMapper : IMapper
    {
        private readonly IImdbEntities _imdbEntities;
        private readonly IExternalMovieDatabaseRepository _externalMovieDatabaseRepository;

        public MovieDetailsMapper(IImdbEntities imdbEntities = null, IExternalMovieDatabaseRepository externalMovieDatabaseRepository = null)
        {
            _imdbEntities = imdbEntities;
            _externalMovieDatabaseRepository = externalMovieDatabaseRepository ?? new ExternalMovieDatabaseRepository();
        }

        /// <summary>
        /// Method handling the response data for a GET reqest by checking the path, 
        /// processing the data and replying with an appropriate reply
        /// </summary>
        /// <param name="identifier"> the id used to get the data</param>
        /// <param name="responseData"> the response data to be returned if no operations are available to the path </param>
        /// <returns></returns>
        public async Task<ResponseData> Get(string identifier, ResponseData responseData)
        {
            var movie = GetMovieById(Convert.ToInt32(identifier));

            var msg = new JavaScriptSerializer().Serialize(movie);
            return new ResponseData(msg, HttpStatusCode.OK);
        }

        /// <summary>
        /// Method handling the response data for a GET reqest by checking the path, 
        /// processing the data and replying with an appropriate reply
        /// </summary>
        /// <param name="path"> the path used to see how to operate the data </param>
        /// <param name="responseData"> the response data to be returned if no operations are available to the path </param>
        /// <returns></returns>
        public Task<ResponseData> Post(List<string> path, ResponseData responseData)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Method recieving a movie by id from the local database
        /// </summary>
        /// <param name="id"> id of the movie we search for </param>
        /// <returns> movie we requested </returns>
        public MovieDetailsDto GetMovieById(int id)
        {
            try
            {
                using (var entities = _imdbEntities ?? new ImdbEntities())
                {
                    var participants = (from peo in entities.People
                                        join par in entities.Participates on peo.Id equals par.Person_Id
                                        join m in entities.Movies on par.Movie_Id equals m.Id
                                        where m.Id == id
                                        group peo by new
                                        {
                                            peo.Id,
                                            peo.Name,
                                            par.CharName
                                        }
                                            into grouping
                                            select new PersonDto
                                            {
                                                Id = grouping.Key.Id,
                                                Name = grouping.Key.Name,
                                                CharacterName = grouping.Key.CharName
                                            }).ToList();

                    var movie = (from m in entities.Movies
                                 where m.Id == id
                                 select new MovieDetailsDto
                                 {
                                     Id = m.Id,
                                     Title = m.Title,
                                     Year = m.Year,
                                     Kind = m.Kind,
                                     EpisodeNumber = m.EpisodeNumber,
                                     EpisodeOf_Id = m.EpisodeOf_Id,
                                     SeasonNumber = m.SeasonNumber,
                                     SeriesYear = m.SeriesYear,
                                     AvgRating = m.Avg_rating
                                 }).ToList();

                    movie[0].Participants = new List<PersonDto>();

                    foreach (var participant in participants)
                    {
                        movie[0].Participants.Add(participant);
                    }


                    return movie[0];
                }
            }
            catch (Exception)
            {
                Console.Write("Local Database is not available");
                return new MovieDetailsDto { ErrorMsg = "Local Database not available", Participants = new List<PersonDto>() };
            }

        }
    }
}
