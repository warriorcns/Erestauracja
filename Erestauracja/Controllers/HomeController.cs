using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Erestauracja.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Witaj na stronie głównej!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }


        public ActionResult TopRestaurants()
        {
            return View();
        }

        public ActionResult Info()
        {
            return View();
        }
        
    }
}
