using System.Collections.Generic;

namespace ImdbRestService.Handlers
{
    /// <summary>
    /// Interface making sure that classes implementing it can check if they are able
    /// to handle the path segment, and handle them if they can.
    /// </summary>
    public interface IHandler
    {
        bool CanHandle(string pathSegment);
        ResponseData Handle(List<string> path, ResponseData responseData);
    }
}