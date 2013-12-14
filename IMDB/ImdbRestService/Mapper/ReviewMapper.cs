using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using DtoSubsystem;
using ImdbRestService.Handlers;
using ImdbRestService.ImdbRepositories;
using Newtonsoft.Json;

namespace ImdbRestService.Mapper
{
    public class ReviewMapper : IMapper
    {
        private readonly IImdbEntities _imdbEntities;
        private readonly IExternalMovieDatabaseRepository _externalMovieDatabaseRepository;

        public ReviewMapper(IImdbEntities imdbEntities = null, IExternalMovieDatabaseRepository externalMovieDatabaseRepository = null)
        {
            _imdbEntities = imdbEntities;
            _externalMovieDatabaseRepository = externalMovieDatabaseRepository ?? new ExternalMovieDatabaseRepository();
        }

        /// <summary>
        /// Method handling the response data for a GET reqest by checking the path, 
        /// processing the data and replying with an appropriate reply
        /// </summary>
        /// <param name="data"> the data used to operate</param>
        /// <param name="responseData"> the response data to be returned if no operations are available to the path </param>
        /// <returns></returns>
        public Task<ResponseData> Get(string data, ResponseData responseData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Method handling the response data for a GET reqest by checking the path, 
        /// processing the data and replying with an appropriate reply
        /// </summary>
        /// <param name="path"> the path used to see how to operate the data </param>
        /// <param name="responseData"> the response data to be returned if no operations are available to the path </param>
        /// <returns></returns>
        public async Task<ResponseData> Post(List<string> path, ResponseData responseData)
        {

            path[1] = path[1].Replace("k__BackingField", "");
            path[1] = path[1].Replace("<", "");
            path[1] = path[1].Replace(">", "");

            // Parse Json object back to data
            var data = JsonConvert.DeserializeObject<ReviewDto>(path[1]);


            if (MovieAndProfileExist(data.MovieId, data.Username) &&
                !AlreadyRated(data.MovieId, data.Username))
            {
                // acutally push to database

                AddRatingToDatabase(data);

                var ratingAddedMsg =
                    new JavaScriptSerializer().Serialize(new ReplyDto
                    {
                        Executed = true,
                        Message = "Rating was added"
                    });
                return new ResponseData(ratingAddedMsg, HttpStatusCode.OK);
            }

            if (MovieAndProfileExist(data.MovieId, data.Username) &&
                AlreadyRated(data.MovieId, data.Username))
            {
                // acutally push to database

                UpdateRatingToDatabase(data);

                var ratingAddedMsg =
                    new JavaScriptSerializer().Serialize(new ReplyDto
                    {
                        Executed = true,
                        Message = "Rating was added updated"
                    });
                return new ResponseData(ratingAddedMsg, HttpStatusCode.OK);
            }


            var ratingNotAddedMsg =
                new JavaScriptSerializer().Serialize(new ReplyDto
                {
                    Executed = false,
                    Message = "Rating could not be added"
                });
            return new ResponseData(ratingNotAddedMsg, HttpStatusCode.OK);

        }

        /// <summary>
        /// Try to either add or update a rating in the database
        /// </summary>
        /// <param name="data">Review data needed to modify the database</param>
        private void UpdateRatingToDatabase(ReviewDto data)
        {
            try
            {
                using (var entities = _imdbEntities ?? new ImdbEntities())
                {

                    int userId = FindUserIdFromUsername(data.Username);

                    var test =
                        entities.Rating.SingleOrDefault(r => r.movie_id == data.MovieId && r.user_Id == userId);

                    if (test != null) test.rating1 = data.Rating;


                    entities.SaveChanges();

                    foreach (var rating in entities.Rating)
                    {
                        Console.WriteLine("-----------");
                        Console.WriteLine(rating.id);
                        Console.WriteLine(rating.movie_id);
                        Console.WriteLine(rating.rating1 + " EDITED RATING");
                        Console.WriteLine(rating.user_Id);
                        Console.WriteLine("-----------");

                    }
                }
            }
            catch (Exception)
            {
                Console.Write("Database is not available");
            }
        }

        /// <summary>
        /// Check if a movie is already rated by a specific user
        /// </summary>
        /// <param name="movieId">Movie to check with</param>
        /// <param name="username">Name to check with</param>
        /// <returns>True if already rated</returns>
        private bool AlreadyRated(int movieId, string username)
        {
            try
            {
                using (var entities = _imdbEntities ?? new ImdbEntities())
                {
                    int userId = FindUserIdFromUsername(username);

                    var alreadyRated = (from r in entities.Rating
                                        where r.movie_id == movieId && r.user_Id == userId
                                        select r).ToList();

                    if (alreadyRated.Count >= 1)
                    {
                        return true;
                    }

                    return false;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Database is not available");
                return true;
            }
        }

        /// <summary>
        /// Add Rating to local database
        /// </summary>
        /// <param name="data">MovieId, Username, Rating</param>
        private void AddRatingToDatabase(ReviewDto data)
        {
            // Add rating to review table & Update rating attribute in movies

            using (var entities = _imdbEntities ?? new ImdbEntities())
            {
                //we find the next rating id
                int id;
                if (!entities.Rating.Any())
                {
                    id = 1;
                }
                else
                {

                    id = entities.Rating.Max(u => u.id) + 1;

                    //Console.WriteLine("RATING ID NO: " + id);
                }





                entities.Rating.Add(new Rating
                {
                    id = id,
                    rating1 = data.Rating,
                    user_Id = FindUserIdFromUsername(data.Username),
                    movie_id = data.MovieId
                });



                entities.SaveChanges();

                foreach (var rating in entities.Rating)
                {
                    Console.WriteLine("-----------");
                    Console.WriteLine(rating.id);
                    Console.WriteLine(rating.movie_id);
                    Console.WriteLine(rating.rating1);
                    Console.WriteLine(rating.user_Id);
                    Console.WriteLine("-----------");

                }



            }

        }

        /// <summary>
        /// Check if movie and user are in our database
        /// </summary>
        /// <param name="movieId">Id of the movie</param>
        /// <param name="username">Id of the user</param>
        /// <returns>True if both exist, else false</returns>
        private bool MovieAndProfileExist(int movieId, string username)
        {
            try
            {
                using (var entities = _imdbEntities ?? new ImdbEntities())
                {
                    var matchingMovies = (from movie in entities.Movies
                                          where movie.Id == movieId
                                          select movie.Title).ToList();

                    var matchingProfiles = (from user in entities.User
                                            where user.name == username
                                            select user.name).ToList();

                    return matchingProfiles.Count > 0 && matchingMovies.Count > 0;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Local database is not available");
                return true;
            }

        }

        /// <summary>
        /// Get the id of a user matching the username
        /// </summary>
        /// <param name="username">Name of the user to look for</param>
        /// <returns>The users id</returns>
        private int FindUserIdFromUsername(string username)
        {
            try
            {
                using (var entities = _imdbEntities ?? new ImdbEntities())
                {
                    var findProfile = (from u in entities.User
                                       where u.name == username
                                       select u).First();

                    return findProfile.Id;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Database is not available");
                return -1;
            }
        }
    }
}
