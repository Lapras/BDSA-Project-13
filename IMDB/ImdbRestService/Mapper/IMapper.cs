using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImdbRestService.Mapper
{
    /// <summary>
    /// Interface for database mapper classes
    /// </summary>
    public interface IMapper
    {
        /// <summary>
        /// Method handling the response data for a GET reqest by checking the path, 
        /// processing the data and replying with an appropriate reply
        /// </summary>
        /// <param name="data"> the data used to operate</param>
        /// <param name="responseData"> the response data to be returned if no operations are available to the path </param>
        /// <returns></returns>
        Task<ResponseData> Get(string data, ResponseData responseData);

        /// <summary>
        /// Method handling the response data for a GET reqest by checking the path, 
        /// processing the data and replying with an appropriate reply
        /// </summary>
        /// <param name="path"> the path used to see how to operate the data </param>
        /// <param name="responseData"> the response data to be returned if no operations are available to the path </param>
        /// <returns></returns>
        Task<ResponseData> Post(List<string> path, ResponseData responseData);
    }
}