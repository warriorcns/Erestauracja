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

        public ActionResult ListRestaurantsFromCity()
        {
            ViewData["height"] = 300;
            ViewData["width"] = 500;

            return View();
        }

        public ActionResult Przykladmapy()
        {
            string status = String.Empty;
            ServiceReference.EresServiceClient country = new ServiceReference.EresServiceClient();
            IEnumerable<Town> data = country.GetTowns(out status, "Tczew", "83-110");
            foreach (Town item in data)
            {
                item.InfoWindowContent = @"<h2>País Vasco</h2>";
            }
          //  IEnumerable<Erestauracja.Controllers.RegionInfo> ttt;
           // ViewData["markers"] =  ttt;
            
            return View(data);
        }

        /// <summary>
        /// Funkcja otrzymuje id restauracji wybranej w index.aspx w home
        /// </summary>
        /// <param name="value">id restauracji</param>
        /// <returns>widok</returns>
        public ActionResult Restaurant(int value)
        {
           
            
            return View();
        }
    }
}
