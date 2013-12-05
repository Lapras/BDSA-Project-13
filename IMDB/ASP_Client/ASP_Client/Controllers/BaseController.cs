using System.Web.Mvc;
using ASP_Client.Session;

namespace ASP_Client.Controllers
{
public class BaseController : Controller
{
        private IUserSession _userSession;

        protected IUserSession UserSession 
        {
            get { return _userSession ?? (_userSession = new UserSession(Session)); }
        }

        public BaseController()
        {
        }

        public BaseController(IUserSession userSession)
        {
            _userSession = userSession;
        }
}
}
