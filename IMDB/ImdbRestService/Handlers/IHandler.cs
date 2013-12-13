using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ImdbRestService.Handlers
{
    /// <summary>
    /// Interface making sure that classes implementing it can check if they are able
    /// to handle the path segment, and handle them if they can.
    /// </summary>
    public interface IHandler
    {
        /// <summary>
        /// Method checking if the given path segment matches the one that
        /// this handler can handle
        /// </summary>
        /// <param name="pathSegment"> the input path segment string </param>
        /// <returns> wether or not the class is able to handle the request </returns>
        bool CanHandle(string pathSegment);

        /// <summary>
        /// Method handling the response data by checking the path, get the data,
        /// serialize them and returning them in the message in a new ResponseData objec
        /// </summary>
        /// <param name="path"> the path used to see how to operate the data </param>
        /// <param name="responseData"> the response data to be returned if no operations are available to the path </param>
        /// <returns></returns>
        Task<ResponseData> Handle(List<string> path, ResponseData responseData);
    }
}