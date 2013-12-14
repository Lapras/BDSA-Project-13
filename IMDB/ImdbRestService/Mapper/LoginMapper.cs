using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using DtoSubsystem;
using ImdbRestService.Mapper;
using Newtonsoft.Json;

namespace ImdbRestService.Handlers
{
    /// <summary>
    /// Class in charge of handling movie requests
    /// </summary>
    public class LoginMapper : IMapper
    {
        private readonly IImdbEntities _imdbEntities;

        public LoginMapper(IImdbEntities imdbEntities = null)
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

        public async Task<ResponseData> Post(List<string> path, ResponseData responseData)
        {
                    if (LoginDataIsValid(path[1], path[2]))
                    {
                        var msg = new JavaScriptSerializer().Serialize(new ReplyDto { Executed = true, Message = "User was logged in"});
                        return new ResponseData(msg, HttpStatusCode.OK);
                    }
                    else
                    {
                        var msg = new JavaScriptSerializer().Serialize(new ReplyDto { Executed = false, Message = "Username or password is invalid" });
                        return new ResponseData(msg, HttpStatusCode.Forbidden);
                    }
        }

        /// <summary>
        /// Checks if the provides name and password match in out database
        /// </summary>
        /// <param name="name">Name provided by the user</param>
        /// <param name="password">Password provided by the user</param>
        /// <returns>True if valid</returns>
        private bool LoginDataIsValid(string name, string password)
        {
            using (var entities = _imdbEntities ?? new ImdbEntities())
            {
                var matchingProfile = (from p in entities.User
                    where p.name == name && p.password == password
                    select p.name).ToList();
                if (matchingProfile.Count > 0)
                {
                    return true;
                }
                return false;
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
    }
}
