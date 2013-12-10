using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
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
        private const string PathSegment = "User";



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
            if (path != null && path.Count == 2)
            {
                var key = path.First();

                Console.WriteLine("KEY: " + key);

                if (key == "Registration")
                {
                    path[1] = path[1].Replace("k__BackingField", "");
                    path[1] = path[1].Replace("<", "");
                    path[1] = path[1].Replace(">", "");

                    // Parse Json object back to data
                    var data = JsonConvert.DeserializeObject<UserModelDto>(path[1]);
                    
                    Console.WriteLine(data.Name + " " + data.Password);

                    if (!ProfileAlreadyExist(data.Name))
                    {
                        // acutally push to database
                        AddProfileToDb(data);

                        var msg = new JavaScriptSerializer().Serialize(new ReplyDto {Executed = true, Message = "Profile was created"});
                        return new ResponseData(msg, HttpStatusCode.OK);
                    }
                    else
                    {
                        var msg = new JavaScriptSerializer().Serialize(new ReplyDto {Executed = false, Message = "Profile already exists"});
                        return new ResponseData(msg, HttpStatusCode.Conflict);
                    }
                }
              
                if (key == "Login")
                {
                    path[1] = path[1].Replace("k__BackingField", "");
                    path[1] = path[1].Replace("<", "");
                    path[1] = path[1].Replace(">", "");

                    // Parse Json object back to data
                    var data = JsonConvert.DeserializeObject<UserModelDto>(path[1]);

                    if (LoginDataIsValid(data.Name, data.Password))
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
            }
            return responseData;
        }

        private bool LoginDataIsValid(string email, string password)
        {
            using (var entities = new ImdbEntities())
            {
                var matchingProfile = (from p in entities.User
                    where p.name == email && p.password == password
                    select p.name).ToList();
                if (matchingProfile.Count > 0)
                {
                    return true;
                }
                return false;
            }
        }


        public bool ProfileAlreadyExist(string profileName)
        {
            using (var entities = new ImdbEntities())
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


        private void AddProfileToDb(UserModelDto profileData)
        {
            // Adding profiles to app server database 

            using (var context = new ImdbEntities())
            {
                int id;

                if (context.User.Count() == 0)
                {
                    id = 1;
                }
                else
                {
                    id = context.User.Max(u => u.Id) + 1;
                }

                context.User.Add(new User
                {
                    Id = id,
                    name = profileData.Name,
                    salt = "muh",
                    email = profileData.Name,
                    password = profileData.Password
                });

                context.SaveChanges();

            }
        }

        public ResponseData FailureReply(Exception e)
        {
            var msg = new JavaScriptSerializer().Serialize(new ReplyDto { Executed = false, Message = e.Message });
            return new ResponseData(msg, HttpStatusCode.OK);
        }
    }
}
