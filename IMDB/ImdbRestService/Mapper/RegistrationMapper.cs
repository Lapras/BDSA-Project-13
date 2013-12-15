using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using DtoSubsystem;
using ImdbRestService.Handlers;
using Newtonsoft.Json;

namespace ImdbRestService.Mapper
{
    public class RegistrationMapper : IMapper
    {
        private readonly IImdbEntities _imdbEntities;

        public RegistrationMapper(IImdbEntities imdbEntities = null)
        {
            _imdbEntities = imdbEntities;
        }

        /// <summary>
        /// Empty GET method
        /// </summary>
        /// <param name="data"></param>
        /// <param name="responseData"></param>
        /// <returns></returns>
        public Task<ResponseData> Get(string data, ResponseData responseData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method handling the response data for a POST reqest by checking the path, 
        /// processing the data and replying with an appropriate reply
        /// </summary>
        /// <param name="path"> the path used to see how to operate the data </param>
        /// <param name="responseData"> the response data to be returned if no operations are available to the path </param>
        /// <returns></returns>
        public async Task<ResponseData> Post(string path, ResponseData responseData)
        {
            var receivedData = JsonConvert.DeserializeObject<RegistrationDto>(path);

            if (!ProfileAlreadyExist(receivedData.Name))
            {
                // acutally push to database
                AddProfileToDb(receivedData.Name, receivedData.Password);

                var msg = new JavaScriptSerializer().Serialize(new ReplyDto { Executed = true, Message = "Profile was created" });
                return new ResponseData(msg, HttpStatusCode.OK);
            }
            else
            {
                var msg = new JavaScriptSerializer().Serialize(new ReplyDto { Executed = false, Message = "Profile already exists" });
                return new ResponseData(msg, HttpStatusCode.Conflict);
            }
        }

        /// <summary>
        /// Checks if a profile name already exists in our database
        /// </summary>
        /// <param name="profileName">Searched profileName</param>
        /// <returns>True if the name already exists</returns>
        public bool ProfileAlreadyExist(string profileName)
        {
            using (var entities = _imdbEntities ?? new ImdbEntities())
            {


                Console.WriteLine("Searching for existing accounts:");
                foreach (var test in entities.User)
                {
                    Console.WriteLine("FOUND: " + test.Id + " " + test.name);
                }


                var matchingProfiles = (from p in entities.User
                                        where p.name == profileName
                                        select p.name).ToList();
                if (matchingProfiles.Count > 0)
                {
                    return true;
                }
                return false;


            }

        }

        /// <summary>
        /// Add a profile to the database
        /// </summary>
        /// <param name="name">Name of the profile to add to the database</param>
        /// <param name="password">Password of the profile to add to the database</param>
        private void AddProfileToDb(string name, string password)
        {
            // Adding profiles to app server database 

            using (var entities = _imdbEntities ?? new ImdbEntities())
            {
                int id;

                if (!entities.User.Any())
                {
                    id = 1;
                }
                else
                {
                    id = entities.User.Max(u => u.Id) + 1;
                }

                entities.User.Add(new User
                {
                    Id = id,
                    name = name,
                    password = password
                });

                entities.SaveChanges();

            }
        }
    }
}
