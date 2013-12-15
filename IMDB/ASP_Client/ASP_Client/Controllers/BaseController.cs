using System.Web.Mvc;
using ASP_Client.Session;

namespace ASP_Client.Controllers
{
    /// <summary>
    /// Class working as a base class for other controllers so that the can start and end a session.
    /// Implements the abstract Controller class
    /// </summary>
    public class BaseController : Controller
    {
        private IUserSession _userSession;

        // Get method for the _userSession. This way we can start a session as soon as we need it.
        public IUserSession UserSession
        {
            get
            {
                return _userSession = _userSession ?? (_userSession = new UserSession(Session));

            }
        }

        /// <summary>
        /// When an action is invoked, this method checks if the user is logged in and updates the ViewBag
        /// according to the check result
        /// </summary>
        /// <param name="filterContext"> Context from executed action </param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (UserSession.IsLoggedIn())
            {
                ViewBag.User = UserSession.GetLoggedInUser();
            }

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Basic constructor
        /// </summary>
        public BaseController()
        {
        }
        /// <summary>
        /// Constructor able to take in a specific IUserSession and sets the field _userSession to that
        /// </summary>
        /// <param name="userSession"> The session to be worked with </param>
        public BaseController(IUserSession userSession)
        {
            _userSession = userSession;
        }
    }
}