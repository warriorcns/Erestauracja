using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Erestauracja.Views;

namespace Erestauracja.Controllers
{
    [Authorize(Roles = "Klient, Menadżer, PracownikFull, PracownikLow")]
    public class CitiesAndRestaurantsController : Controller
    {
        //
        // GET: /City/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListRestaurantsFromCity(int? height, int? width)
        {
            ViewData["height"] = height ?? 100;
            ViewData["width"] = width ?? 100;
            return View();
        }

        public ActionResult Przykladmapy()
        {

            return View();
        }
    }
}
