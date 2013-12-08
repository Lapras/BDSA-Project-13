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
        bool CanHandle(string pathSegment);
        Task<ResponseData> Handle(List<string> path, ResponseData responseData);
        ResponseData FailureReply(Exception e);
    }
}