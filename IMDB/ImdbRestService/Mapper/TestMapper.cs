using System.Net;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using DtoSubsystem;

namespace ImdbRestService.Mapper
{
    /// <summary>
    /// Mapper representing our in memory data
    /// </summary>
    public class TestMapper : IMapper
    {
        /// <summary>
        /// Get movies from our memory
        /// </summary>
        /// <param name="testData">testData to provide</param>
        /// <param name="responseData">Reponse to be returned</param>
        /// <returns>Return of our test stub</returns>
        public async Task<ResponseData> Get(string testData, ResponseData responseData)
        {
            var msg =
               new JavaScriptSerializer().Serialize(new ReplyDto { Executed = true, Message = "I am your data :)" });
            return new ResponseData(msg, HttpStatusCode.OK);
        }

        /// <summary>
        /// Post movies to our memory
        /// </summary>
        /// <param name="path">testData to provide</param>
        /// <param name="responseData">Reponse to be returned</param>
        /// <returns>Return of our test stub</returns>
        public async Task<ResponseData> Post(string path, ResponseData responseData)
        {
             var msg =
                new JavaScriptSerializer().Serialize(new TestDto() {Executed = true, Message = "I'm getting fat :o"});
            return new ResponseData(msg, HttpStatusCode.OK);
        }
    }
}
