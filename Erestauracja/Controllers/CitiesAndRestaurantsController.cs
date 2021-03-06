﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Erestauracja.ServiceReference;

namespace Erestauracja.Controllers
{
    
    public class CitiesAndRestaurantsController : Controller
    {
        //
        // GET: /City/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /CitiesAndRestaurants/ListRestaurantsFromCity
        public ActionResult ListRestaurantsFromCity(string id)
        {
            RestaurantsFromCity value = null;
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
                    value.CityName = id;
                    value.Restaurants = new RestaurantInCity[1];
                    value.Restaurants[0] = new RestaurantInCity();
                    value.Restaurants[0].DisplayName = "Brak";
                }
                else
                {
                    foreach(RestaurantInCity item in value.Restaurants)
                    {
                        string onClick = String.Format(" \"Redirect('{0}')\" ", item.ID);
                        item.InfoWindowContent = item.DisplayName + " " + "</br>" + item.Address + " " + item.Town + " " + item.PostalCode + "</br>" + "Telefon " + item.Telephone + "</br>" + "Średnia ocena: " + "<span class=\"stars\" data-rating=" + item.AverageRating.ToString("F", System.Globalization.CultureInfo.CreateSpecificCulture("en-CA")) + "></span>"  +"</br>" + "<a href=" + "#" + " onclick=" + onClick + " class=" + "button" + ">" + "Wybierz." + "</a>";
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Nieprawidłowa nazwa miasta.");
                return RedirectToAction("Index", "Home");
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
