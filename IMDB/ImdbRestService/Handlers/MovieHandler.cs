using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;
using DtoSubsystem;

namespace ImdbRestService.Handlers
{
    public class MovieHandler : IHandler
    {
        private const string PathSegment = "movies";

        public bool CanHandle(string pathSegment)
        {
            return pathSegment.Equals(PathSegment, StringComparison.CurrentCultureIgnoreCase);
        }
        
        public ResponseData Handle(List<string> path, ResponseData responseData)
        {
            if (path != null && path.Count == 1)
            {
                var firstSegment = path.First();
                if (firstSegment.StartsWith("?"))
                {
                    var key = firstSegment.Substring(1).Split(new[] {'='})[0];
                    var value = firstSegment.Split(new[] {'='})[1];

                    if (key == "title")
                    {
                        var movies = GetMoviesByTitle(value);

                        var msg = new JavaScriptSerializer().Serialize(movies);
                        return new ResponseData(msg, HttpStatusCode.OK);
                    }
                }
            }

            return responseData;
        }

        public List<MovieDto> GetMoviesByTitle(string title)
        {
            using (var entities = new ImdbEntities())
            {
                if (String.IsNullOrEmpty(title))
                {
                    return (from m in entities.Movies
                            select new MovieDto()
                            {
                                Id = m.Id,
                                Title = m.Title,
                                Year = m.Year
                            }).Take(20).ToList();
                }
                
                return (from m in entities.Movies
                    where m.Title.Contains(title)
                    select new MovieDto()
                    {
                        Id = m.Id,
                        Title = m.Title,
                        Year = m.Year
                    }).ToList();
            }
        } 
    }
}