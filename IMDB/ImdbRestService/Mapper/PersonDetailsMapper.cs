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
    public class PersonDetailsMapper : IMapper
    {
          private readonly IImdbEntities _imdbEntities;
        private readonly IExternalMovieDatabaseRepository _externalMovieDatabaseRepository;

        public PersonDetailsMapper(IImdbEntities imdbEntities = null, IExternalMovieDatabaseRepository externalMovieDatabaseRepository = null)
        {
            _imdbEntities = imdbEntities;
            _externalMovieDatabaseRepository = externalMovieDatabaseRepository ?? new ExternalMovieDatabaseRepository();
        }


        /// <summary>
        /// Method handling the response data by checking the path, get the persons,
        /// serialize them and returning them in the message in a new ResponseData object
        /// </summary>
        /// <param name="personId">Id of the person to search for</param>
        /// <param name="responseData"> the response data to be returned if no operations are available to the path </param>
        /// <returns></returns>
        public async Task<ResponseData> Get(string personId, ResponseData responseData)
        {
            var person = GetPersonById(Convert.ToInt32(personId));

            var msg = new JavaScriptSerializer().Serialize(person);
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
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method recieving a person by id from the local database
        /// </summary>
        /// <param name="id"> id of the person we search for </param>
        /// <returns> person we requested </returns>
        public PersonDetailsDto GetPersonById(int id)
        {
            try
            {
                using (var entities = _imdbEntities ?? new ImdbEntities())
                {
                    var person = (from people in entities.People
                        join participant in entities.Participates on people.Id equals participant.Person_Id
                        where people.Id == id
                        select new PersonDetailsDto
                        {
                            Id = people.Id,
                            Name = people.Name,
                            Gender = people.Gender,
                            Role = participant.Role,
                            CharName = participant.CharName
                        }).ToList()[0];

                    var additionalDetailsOnPerson = (from people in entities.People
                        join personInfo in entities.PersonInfoes on people.Id equals personInfo.Person_Id
                        join infoType in entities.InfoTypes on personInfo.Type_Id equals infoType.Id
                        where person.Id == people.Id
                        select new InfoDto {Name = infoType.Name, Info = personInfo.Info}).ToList();



                    person.Info = new List<InfoDto>();

                    foreach (var detail in additionalDetailsOnPerson)
                    {
                        person.Info.Add(detail);
                    }

                    return person;
                }

            }
            catch (ArgumentOutOfRangeException)
            {
                Console.Write("Local database is not available");
                return new PersonDetailsDto {ErrorMsg = "Local Database not available"};
            }
        }
    }
}
