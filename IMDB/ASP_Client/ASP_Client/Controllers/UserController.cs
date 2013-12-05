using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using ASP_Client.ClientRequests;
using ASP_Client.Models;
using ASP_Client.Session;

namespace ASP_Client.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserRepository _userRepository;

        public UserController() : this(null, new UserRepository())
        {
        }

        public UserController(IUserSession userSession, IUserRepository userRepository) : base(userSession)
        {
            _userRepository = userRepository;
        }

        public ActionResult Login()
        {
            return View();
        }

        public async Task<ActionResult> Login(UserModel user)
        {
            if (ModelState.IsValid)
            {
                var response = await _userRepository.Login(user);

                    if (response.IsSuccessStatusCode)
                    {
                        UserSession.Login(user);

                        return RedirectToAction("Index", "Movie");
                    }
                
            }
            return View(user);
        }

        public ActionResult LogOut()
        {
            UserSession.Logout();
            return RedirectToAction("Index", "Movie");
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        public async Task<ActionResult> Registration(UserModel user)
        {
            if (ModelState.IsValid)
            {
                var response = await _userRepository.Registration(user);

                if (response.IsSuccessStatusCode)
                    {
                        RedirectToAction("Login", "User");
                    }
                
            }
            return View(user);
        }
    }
}
