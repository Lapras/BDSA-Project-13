using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ASP_Client.Models;


namespace ASP_Client.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet]
        public ActionResult Home()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Home(HomeViewModel model)
        {
            if (model.SearchString != null)
            {
                if (model.Choice.Equals("Movie"))
                {
                    return RedirectToAction("SearchMovie", "Movie", new { searchString = model.SearchString});
                }

                return RedirectToAction("SearchPerson", "Person", new { searchString = model.SearchString });
            }

            return View();
        }

    }
}
