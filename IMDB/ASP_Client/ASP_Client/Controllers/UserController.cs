using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using ASP_Client.ClientRequests;
using ASP_Client.Models;
using ASP_Client.Session;

namespace ASP_Client.Controllers
{
    /// <summary>
    /// UserController class is the controller for every view concerning a representation of a user action.
    /// From here the requests to register, login and logout is sent.
    /// Inherits from BaseController.
    /// </summary>
    public class UserController : BaseController
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Basic constructor which calls the other constructor with a new UserRepository
        /// </summary>
        public UserController()
            : this(null, new UserRepository())
        {
        }

        /// <summary>
        /// Constructor taking in an IUserSession and an IUserRepository, setting the _userRepository field and making
        /// the userSession the one inheritted from the base class
        /// </summary>
        /// <param name="userSession"> Session inherited from the base class BaseController </param>
        /// <param name="userRepository"> The repository in charge of actually sending the requests the client is requesting </param>
        public UserController(IUserSession userSession, IUserRepository userRepository)
            : base(userSession)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Method opening the login view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Method starting a task set to login the user and starting a session if the response is positive
        /// </summary>
        /// <param name="user"> The user model containing the data from the user </param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Login(UserModel user)
        {
            if (ModelState.IsValid)
            {
                var response = await _userRepository.Login(user);

                if (response.Executed)
                {
                    UserSession.Login(user);

                    //var requestAuthentication = Request.IsAuthenticated;

                    return RedirectToAction("SearchMovie", "Movie");
                }

            }
            return View(user);
        }

        /// <summary>
        /// Logout method ending the session
        /// </summary>
        /// <returns></returns>
        public ActionResult LogOut()
        {
            UserSession.Logout();
            return RedirectToAction("SearchMovie", "Movie");
        }

        /// <summary>
        /// Method opening the registration view
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        /// <summary>
        /// Method starting a task set to register a new user
        /// </summary>
        /// <param name="user"> Model containing data from the user </param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> Registration(UserModel user)
        {
            if (ModelState.IsValid)
            {
                var response = await _userRepository.Registration(user);

                if (response.Executed)
                {
                    await Login(user);
                    return RedirectToAction("SearchMovie", "Movie");
                }
            }
            return View(user);
        }
    }
}
