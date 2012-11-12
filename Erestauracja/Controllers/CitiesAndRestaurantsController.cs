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
    //[Authorize(Roles = "Klient, Menadżer, PracownikFull, PracownikLow")]
    public class CitiesAndRestaurantsController : Controller
    {
        //
        // GET: /City/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListRestaurantsFromCity(string id)
        {
            //pobrać z serwisu RestaurantsFromCity RestaurantsFromCity(string cityName);
     //       ViewData["id"] = id;
     //       ViewData["height"] = 300;
    //        ViewData["width"] = 500;

            RestaurantsFromCity value = null;
            ViewData["Map"] = (IEnumerable<RestaurantsFromCity>)( new List<RestaurantsFromCity>() ); 
            if(!(String.IsNullOrWhiteSpace(id)))
            {
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.RestaurantsFromCity(id);
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Pobieranie restauracji nie powiodło się.");
                }
                if (value == null)
                {
                    ModelState.AddModelError("", "Pobieranie restauracji nie powiodło się.");
                    value = new RestaurantsFromCity();
                    value.Restaurants[0].DisplayName = "Brak";
                    //ustawienie pustych danych do mapki
              //      ViewData["Map"] = (IEnumerable<RestaurantsFromCity>)(new RestaurantsFromCity());
                }
                else
                {
                    foreach(RestaurantInCity item in value.Restaurants)
                    {
                    //    //tu powinno być przekierowanie do strony restauracji
                    //    //!!
                    //   //     !!
                        string onClick = String.Format(" \"Redirect('{0}')\" ", item.ID);
                        item.InfoWindowContent = item.DisplayName + " " + "</br>" + "<a href=" + "#" + " onclick=" + onClick + " class=" + "button" + ">" + "Wybierz." + "</a>";
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Nieprawidłowa nazwa miasta.");
                value = new RestaurantsFromCity();
                value.Restaurants[0].DisplayName = "Brak";
                value.CityName = "Brak";
            }

            return View(value);
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

            return RedirectToAction("Action", new { id = 99 });
            //return View();
        }
    }
}
