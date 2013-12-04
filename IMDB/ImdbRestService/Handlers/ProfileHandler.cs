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
    public class ProfileHandler : IHandler
    {
        private const string PathSegment = "createProfile";

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

                var key = firstSegment.Substring(1).Split(new[] {'='})[0];
                var value = firstSegment.Split(new[] {'='})[1];

                if (key == "createProfile")
                {
                    string name = "test";

                    if (ProfileAlreadyExist(name))
                    {
                    }
                    


                }
            }
            return responseData;
        }


        public bool ProfileAlreadyExist(string profileName)
        {
            using (var entities = new ImdbEntities())
            {
             /*   var matchingProfiles = (from p in entities.Profile
                             where p.Name == profileName
                             select p.Name).ToList();
*/

            }

            return false;
        } 

/*
        /// <summary>
        /// Method recieving a movie by id from the local database
        /// </summary>
        /// <param name="id"> id of the movie we search for </param>
        /// <returns> movie we requested </returns>
        public List<MovieDetailsDto> GetMovieById(int id)
        {
            using (var entities = new ImdbEntities())
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
                                        select new PersonDto()
                                        {
                                            Id = grouping.Key.Id,
                                            Name = grouping.Key.Name,
                                            CharacterName = grouping.Key.CharName
                                        }).ToList();

                var movie = (from m in entities.Movies
                             where m.Id == id
                             select new MovieDetailsDto()
                             {
                                 Id = m.Id,
                                 Title = m.Title,
                                 Year = m.Year,
                                 Kind = m.Kind,
                                 EpisodeNumber = m.EpisodeNumber,
                                 EpisodeOf_Id = m.EpisodeOf_Id,
                                 SeasonNumber = m.SeasonNumber,
                                 SeriesYear = m.SeriesYear
                             }).ToList();

                movie[0].Participants = new List<PersonDto>();

                foreach (var participant in participants)
                {
                    movie[0].Participants.Add(participant);
                }


                return movie;
            }
        }
*/

    }
}