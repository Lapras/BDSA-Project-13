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
        private const string PathSegment = "Registration";

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

                if (key == "Registration")
                {
                    path[1] = path[1].Replace("k__BackingField", "");
                    path[1] = path[1].Replace("<", "");
                    path[1] = path[1].Replace(">", "");

                    // Parse Json object back to data
                    var data = JsonConvert.DeserializeObject<UserModelDto>(path[1]);

                    if (ProfileAlreadyExist(data.Email))
                    {
                        // acutally push to database
                        AddProfileToDb(data);

                        var msg = new JavaScriptSerializer().Serialize(new ReplyDto() {Executed = true, Message = "Profile was created"});
                        return new ResponseData(msg, HttpStatusCode.OK);
                    }
                    else
                    {
                        var msg = new JavaScriptSerializer().Serialize(new ReplyDto() {Executed = false, Message = "Profile already exists"});
                        return new ResponseData(msg, HttpStatusCode.OK);
                    }
                }
              
                if (key == "Login")
                {
                    path[1] = path[1].Replace("k__BackingField", "");
                    path[1] = path[1].Replace("<", "");
                    path[1] = path[1].Replace(">", "");

                    // Parse Json object back to data
                    var data = JsonConvert.DeserializeObject<UserModelDto>(path[1]);

                    if (LoginDataIsValid(data.Email, data.Password))
                    {
                        // Missing : I guess create a session

                        var msg = new JavaScriptSerializer().Serialize(new ReplyDto() { Executed = true, Message = "User was logged in"});
                        return new ResponseData(msg, HttpStatusCode.OK);
                    }
                    else
                    {
                        var msg = new JavaScriptSerializer().Serialize(new ReplyDto() { Executed = false, Message = "Username or password is invalid" });
                        return new ResponseData(msg, HttpStatusCode.OK);
                    }
                }
            }
            return responseData;
        }

        private bool LoginDataIsValid(string email, string password)
        {
            using (var entities = new ImdbEntities())
            {

                /*
                var matchingProfile = (from p in entities.Profile
                             where p.Name == profileName, p.Password == password
                             select p.Name).ToList();
              if(matchingProfile.Count > 0) {
                  return true;
              }
                    return false;
               */
            }

            return true;
        }


        public bool ProfileAlreadyExist(string profileName)
        {
            using (var entities = new ImdbEntities())
            {

                /*
                var matchingProfiles = (from p in entities.Profile
                             where p.Name == profileName
                             select p.Name).ToList();
              if(!matchingProfiles.contain(profileName)) {
                  return true;
              }
                    return false;
               */
            }

            return false;
        }


        private void AddProfileToDb(UserModelDto profileData)
        {
            // Adding profiles to app server database 

            using (var context = new ImdbEntities())
            {
                /*
                context.Profiles.Add(new Profile
                {
                    Id = movie.Id,
                    email = profileData.Email,
                    password = profileData.Password
                */

                 context.SaveChanges();
            }
        }

        public ResponseData FailureReply(Exception e)
        {
            var msg = new JavaScriptSerializer().Serialize(new ReplyDto() { Executed = false, Message = e.Message });
            return new ResponseData(msg, HttpStatusCode.OK);
        }
    }
}
