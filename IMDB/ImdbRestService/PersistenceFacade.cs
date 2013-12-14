using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using DtoSubsystem;
using ImdbRestService.Handlers;
using ImdbRestService.Mapper;

namespace ImdbRestService
{
    public class PersistenceFacade
    {
        private readonly Dictionary<System.Type, IMapper> _handlers;

        public PersistenceFacade()
        {
            _handlers = new Dictionary<System.Type, IMapper>
            {
                {new MovieDetailsDto().GetType(), new MovieDetailsMapper()},
                {new MovieDto().GetType(), new MovieMapper()},
                {new PersonDetailsDto().GetType(), new PersonDetailsMapper()},
                {new PersonDto().GetType(), new PersonMapper()},
                {new ReviewDto().GetType(), new ReviewMapper()},
                {new LoginDto().GetType(), new LoginMapper()},
                {new RegistrationDto().GetType(), new RegistrationMapper()}
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
            _handlers.TryGetValue(requestedDto.GetType(), out matchingMapper);

            if (matchingMapper != null) return await matchingMapper.Get(data, response);

            var msg = new JavaScriptSerializer().Serialize(new ReplyDto
            {
                Executed = false,
                Message = "No valid handler was found"
            });

            return new ResponseData(msg, HttpStatusCode.OK);
        }

        /// <summary>
        /// Post content to the database
        /// </summary>
        /// <param name="path">Path send in the post request including the posted data</param>
        /// <param name="requestedDto">Dto format to post for send in</param>
        /// <returns>Result</returns>
        public async Task<ResponseData> Post(List<string> path, Dto requestedDto)
        {
            var response = new ResponseData("Database is unavailable", HttpStatusCode.InternalServerError);

            IMapper matchingMapper;
            _handlers.TryGetValue(requestedDto.GetType(), out matchingMapper);

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
