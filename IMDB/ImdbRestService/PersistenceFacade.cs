using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using DtoSubsystem;
using ImdbRestService.Handlers;
using ImdbRestService.Mapper;

namespace ImdbRestService
{
    public class PersistenceFacade : IPersistenceFacade
    {
        private readonly Dictionary<System.Type, IMapper> _mappers;

        public PersistenceFacade()
        {
            _mappers = new Dictionary<System.Type, IMapper>
            {
                {new MovieDetailsDto().GetType(), new MovieDetailsMapper()},
                {new MovieDto().GetType(), new MovieMapper()},
                {new PersonDetailsDto().GetType(), new PersonDetailsMapper()},
                {new PersonDto().GetType(), new PersonMapper()},
                {new RatingDto().GetType(), new RatingMapper()},
                {new LoginDto().GetType(), new LoginMapper()},
                {new RegistrationDto().GetType(), new RegistrationMapper()},
                {new TestDto().GetType(), new TestMapper()}
            };
        }


        /// <summary>
        /// Get content from the database
        /// </summary>
        /// <param name="data">Name of the data to get</param>
        /// <param name="requestedDto">Dto format requested</param>
        /// <returns>Result</returns>
        public async Task<ResponseData> Get(string data, Dto requestedDto)
        {
            var response = new ResponseData("Database is unavailable", HttpStatusCode.InternalServerError);
            IMapper matchingMapper;
            _mappers.TryGetValue(requestedDto.GetType(), out matchingMapper);

            if (matchingMapper != null)
            {
                var result = await matchingMapper.Get(data, response);

                // If our database has no data check in the external one and add the new data
                if ((result.HttpStatusCode == HttpStatusCode.NoContent) && (matchingMapper.GetType() == new MovieMapper().GetType()))
                {
                    result = await new MyMovieApiMapper().Get(data, response);

                    if (result.HttpStatusCode == HttpStatusCode.OK)
                    {

                        await matchingMapper.Post(result.Message, response);
                    }
                }

                return result;
            }
         

            var msg = new JavaScriptSerializer().Serialize(new ReplyDto
            {
                Executed = false,
                Message = "No valid mapper was found"
            });

            return new ResponseData(msg, HttpStatusCode.OK);
        }

        /// <summary>
        /// Post content to the database
        /// </summary>
        /// <param name="path">Path send in the post request including the posted data</param>
        /// <param name="requestedDto">Dto format to post for send in</param>
        /// <returns>Result</returns>
        public async Task<ResponseData> Post(string path, Dto requestedDto)
        {
            var response = new ResponseData("Database is unavailable", HttpStatusCode.InternalServerError);

            IMapper matchingMapper;
            _mappers.TryGetValue(requestedDto.GetType(), out matchingMapper);

            if (matchingMapper != null) return await matchingMapper.Post(path, response);

            var msg = new JavaScriptSerializer().Serialize(new ReplyDto
            {
                Executed = false,
                Message = "No valid handler was found"
            });

            return new ResponseData(msg, HttpStatusCode.OK);
        }
    }
}
