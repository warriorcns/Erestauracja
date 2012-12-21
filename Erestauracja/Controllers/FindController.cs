using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Erestauracja.ServiceReference;

namespace Erestauracja.Controllers
{
    
    public class FindController : Controller
    {

        public ActionResult getReq(string town, string res, bool first)
        {
            return Json(new { redirectToUrl = Url.Action("Index", "Find", new { town = String.Empty, res = String.Empty, first = true }) });
        
        }
        //
        // GET: /Find/
        public ActionResult Index(string town, string res, bool first)
        {
            List<RestaurantInCity> list = null;
            ViewData["town"] = town;
            ViewData["res"] = res;
            ViewData["first"] = first;

            if (String.IsNullOrWhiteSpace(town) && String.IsNullOrWhiteSpace(res))
            {
                if (first == false)
                {
                    ModelState.AddModelError("", "Pole nazwa miasta lub nazwa restauracji, musi być wypełnione");
                }
                list = new List<RestaurantInCity>();
            }
            else
            {
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        list = new List<RestaurantInCity>(client.GetSearchResult(town, res));
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    list = null;
                }
                if (list == null)
                {
                    ModelState.AddModelError("", "Szukanie restauracji nie powiodło się.");

                    return View(list);
                }
                else
                {
                    foreach (RestaurantInCity item in list)
                    {
                        string onClick = String.Format(" \"Redirect('{0}')\" ", item.ID);
                        item.InfoWindowContent = item.DisplayName + " " + "</br>" + item.Address + " " + item.Town + " " + item.PostalCode + "</br>" + "Telefon " + item.Telephone + "</br>" + "Średnia ocena: " + "<span class=\"stars\" data-rating=" + item.AverageRating.ToString("F",System.Globalization.CultureInfo.CreateSpecificCulture("en-CA")) + "></span>"  + "</br>" + "<a href=" + "#" + " onclick=" + onClick + " class=" + "button" + ">" + "Wybierz." + "</a>";
                    }
                }
            }
            return View(list);
        }

         //
        // GET: /Find/
        public ActionResult Search(string town, string res)
        {
            return Json(new { redirectToUrl = Url.Action("Index", "Find", new { town = town, res = res, first = false }) });
        }

    }
}
