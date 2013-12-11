using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls.WebParts;
using DtoSubsystem;

namespace ImdbRestService.Handlers
{
    public class PersonHandler : IHandler
    {
        private const string PathSegment = "person";

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
                var firstSegment = path.First();
                if (firstSegment.StartsWith("?"))
                {
                    var key = firstSegment.Substring(1).Split(new[] {'='})[0];
                    var value = firstSegment.Split(new[] {'='})[1];


                    String msg;

                    switch (key)
                    {
                        case "person":

                            var people = GetPeopleByName(value);

                            // Maybe search in external database when not found like in movies?

                            msg = new JavaScriptSerializer().Serialize(people);
                            return new ResponseData(msg, HttpStatusCode.OK);
                            break;

                        case "personId":

                           // var person = GetPersonById(Convert.ToInt32(value));

                            msg = new JavaScriptSerializer().Serialize(new PersonDetailsDto() {Name = "Rasmus"});
                            return new ResponseData(msg, HttpStatusCode.OK);
                            break;
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
            using (var entities = new ImdbEntities())
            {
                return (from person in entities.People
                    join participant in entities.Participates on person.Id equals participant.ParticipateId
                    where person.Name.Contains(name)
                    select new PersonDto()
                    {
                        Id = person.Id,
                        Name = person.Name,
                        CharacterName = participant.CharName
                    }).Take(20).ToList();
            }
        }

        /// <summary>
        /// Method recieving a person by id from the local database
        /// </summary>
        /// <param name="id"> id of the movie we search for </param>
        /// <returns> movie we requested </returns>
        public PersonDetailsDto GetPersonById(int id)
        {
            using (var entities = new ImdbEntities())
            {
                var person = (from people in entities.People join participant in entities.Participates on people.Id equals participant.ParticipateId 
                              where people.Id == id
                              select new PersonDetailsDto()
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
                    select new {infoType.Name, personInfo.Info}).ToArray();

                person.Info = new string[][] {};

                for (int i = 0; i < additionalDetailsOnPerson.Length; i++)
                {
                    person.Info[i][0] = additionalDetailsOnPerson[i].Name;
                    person.Info[i][1] = additionalDetailsOnPerson[i].Info;
                }

                return person;
            }
        }

    }
}
