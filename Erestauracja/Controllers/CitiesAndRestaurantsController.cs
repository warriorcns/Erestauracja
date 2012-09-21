using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Erestauracja.Models;
using Erestauracja.Providers;
using System.Web.Mvc.Html;
using Erestauracja.Authorization;
using Erestauracja.ServiceReference;
using System.Net;
using System.Globalization;
using Jmelosegui.Mvc.Controls;

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
            ViewData["height"] = height ?? 300;
            ViewData["width"] = width ?? 500;
            return View();
        }

        public ActionResult Przykladmapy()
        {
            string status = string.Empty;
            ServiceReference.EresServiceClient country = new ServiceReference.EresServiceClient();
            IEnumerable<Town> data = country.GetTowns(out status, "Tczew", "83-110");
            return View(data);
        }
    }
}
