using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using DtoSubsystem;
using ImdbRestService.Handlers;

namespace ImdbRestService.Mapper
{
    /// <summary>
    /// Class in charge of handling person requests
    /// </summary>
    public class PersonMapper : IMapper
    {
        private readonly IImdbEntities _imdbEntities;

        public PersonMapper(IImdbEntities imdbEntities = null)
        {
            _imdbEntities = imdbEntities;
        }


        /// <summary>
        /// Method handling the response data by checking the path, get the people,
        /// serialize them and returning them in the message in a new ResponseData objec
        /// </summary>
        /// <param name="personName">Name of the person to search for</param>
        /// <param name="responseData"> the response data to be returned if no operations are available to the path </param>
        /// <returns></returns>
        public async Task<ResponseData> Get(string personName, ResponseData responseData)
        {
            var people = GetPeopleByName(personName);

            if (people.Count == 0)
            {
                people.Add(new PersonDto() {ErrorMsg = "Person not found"});
            }

            var msg = new JavaScriptSerializer().Serialize(people);
            return new ResponseData(msg, HttpStatusCode.OK);
        }

        /// <summary>
        /// Empty POST call
        /// </summary>
        /// <param name="path"></param>
        /// <param name="responseData"></param>
        /// <returns></returns>
        public Task<ResponseData> Post(string path, ResponseData responseData)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Method recieving people by name from the local database
        /// </summary>
        /// <param name="name"> the name to search for </param>
        /// <returns> a list of PersonDto's containing information on the persons found </returns>
        private List<PersonDto> GetPeopleByName(string name)
        {
            using (var entities = _imdbEntities ?? new ImdbEntities())
            {
                return (from person in entities.People
                    join participant in entities.Participates on person.Id equals participant.ParticipateId
                    where person.Name.Contains(name)
                    select new PersonDto
                    {
                        Id = person.Id,
                        Name = person.Name,
                        CharacterName = participant.CharName
                    }).Take(100).ToList();
            }
        }
    }
}
