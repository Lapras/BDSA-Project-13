using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using DtoSubsystem;

namespace ImdbRestService.Handlers
{
    /// <summary>
    /// Class in charge of handling person requests
    /// </summary>
    public class PersonHandler : IHandler
    {
        private const string PathSegment = "person";
        private readonly IImdbEntities _imdbEntities;

        public PersonHandler(IImdbEntities imdbEntities = null)
        {
            _imdbEntities = imdbEntities;
        }

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
        /// Method handling the response data by checking the path, get the people,
        /// serialize them and returning them in the message in a new ResponseData objec
        /// </summary>
        /// <param name="path"> the path used to see how to operate the data </param>
        /// <param name="responseData"> the response data to be returned if no operations are available to the path </param>
        /// <returns></returns>
        public async Task<ResponseData> Handle(List<string> path, ResponseData responseData)
        {
            if (path != null && path.Count == 1)
            {

                string msg;

                var firstSegment = path.First();
                if (firstSegment.StartsWith("?"))
                {
                    var key = firstSegment.Substring(1).Split(new[] {'='})[0];
                    var value = firstSegment.Split(new[] {'='})[1];

                    switch (key)
                    {
                        case "person":

                            var people = GetPeopleByName(value);

                            // Maybe search in external database when not found like in movies?

                            msg = new JavaScriptSerializer().Serialize(people);
                            return new ResponseData(msg, HttpStatusCode.OK);
                    }
                }
                else
                {
                    switch (firstSegment)
                    {
                        default:

                            var person = GetPersonById(Convert.ToInt32(firstSegment));

                            msg = new JavaScriptSerializer().Serialize(person);
                            return new ResponseData(msg, HttpStatusCode.OK);
                    }
                }
            }
            return responseData;
        }

        /// <summary>
        /// Method recieving people by name from the local database
        /// </summary>
        /// <param name="name"> the name to search for </param>
        /// <returns> a list of MovieDto's containing information on the movies found </returns>
        public List<PersonDto> GetPeopleByName(string name)
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

        /// <summary>
        /// Method recieving a person by id from the local database
        /// </summary>
        /// <param name="id"> id of the movie we search for </param>
        /// <returns> movie we requested </returns>
        public PersonDetailsDto GetPersonById(int id)
        {
            try
            {

                using (var entities = _imdbEntities ?? new ImdbEntities())
                {
                    var person = (from people in entities.People
                        join participant in entities.Participates on people.Id equals participant.ParticipateId
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

                    //   person.Info = new string[additionalDetailsOnPerson.Length, 2];

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
                return new PersonDetailsDto { ErrorMsg = "Local Database not available" };
            }



        }
    }
}
