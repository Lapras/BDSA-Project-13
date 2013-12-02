using System.Collections.Generic;
using System.Net;

namespace ImdbRestService.Handlers
{
    public interface IHandler
    {
        bool CanHandle(string pathSegment);
        ResponseData Handle(List<string> path, ResponseData responseData);
    }
}