using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Erestauracja.Authorization;
using Erestauracja.Models;
using Erestauracja.Providers;
using Erestauracja.ServiceReference;

namespace Erestauracja.Controllers
{
    public class POSController : Controller
    {
        //
        // GET: /POS/
        /// <summary>
        /// Glowny page z buttonami
        /// </summary>
        /// <returns></returns>
        [CustomAuthorizeAttribute(Roles = "Pracownik")]
        public ActionResult Index()
        {
            //pobrać ststus z RestaurantOnlineStatus(string login)
            return View();
        }


        //
        // GET: /POS/ActiveOrders
        /// <summary>
        /// Aktywne zamowienia
        /// </summary>
        /// <returns></returns>
        //[CustomAuthorizeAttribute(Roles = "Pracownik")]
        public ActionResult ActiveOrders()
        {
            ServiceReference.AllOrders value = null;
            try
            {
                Erestauracja.ServiceReference.EresServiceClient client = new Erestauracja.ServiceReference.EresServiceClient();
                using (client)
                {
                    //  Uchwyt IsOnline = new Uchwyt(client.IsRestaurantOnline); 
                    //  value = IsOnline(id); 
                    value = client.GetOrders(User.Identity.Name);
                }
                client.Close();
            }
            catch (Exception e)
            {
                value = null;
            }

            if (value == null)
            {
                return View();
            }
            else
            {
                return View(value);
            }
        }

        
        //metoda filtrujaca liste zamowien - wywolanie przez jquery
        public ActionResult FilterOrders(string from, string to)
        {
            if (Request.IsAuthenticated)
            {
                //return RedirectToAction("OrderHistory", new { from = from, to = to});
                return Json(new { redirectToUrl = Url.Action("AllOrders", "POS", new { from = from, to = to }) });
            }
            else
            {
                return RedirectToAction("LogOn", "Account");
            }
        }

        //widok wszystkie zamowienia
        public ActionResult AllOrders(string from, string to)
        {
            if (Request.IsAuthenticated)
            {
                to += " 23:59:59";
                DateTime fromm = DateTime.Parse(from);
                DateTime too = DateTime.Parse(to);

                ViewData["from"] = fromm;
                ViewData["to"] = too;

                Order[] value = null;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.GetAllOrders(User.Identity.Name, fromm, too);
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    value = null;
                }
                if (value == null)
                {
                    ModelState.AddModelError("", "Pobieranie danych nie powiodło się.");

                    return View(value);
                }

                return View(value);
            }
            else
            {
                return RedirectToAction("LogOn", "Account");
            }
        }

        //zmiana statusu zamowienia
        public ActionResult setStatus(int id, string stat)
        {

            bool status = false;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    status = client.SetOrderStatus(id, User.Identity.Name, stat);
                }
                client.Close();
            }
            catch (Exception e)
            {
                status = false;
            }
            if (status)
            {
                //drukowanie bonu - ? 
                return Json(new { redirectToUrl = Url.Action("ActiveOrders", "POS") });
            }
            else
            {
                ModelState.AddModelError("", "Zmiana statusu zamówienia nie powiodła się.");
                return Json(new { redirectToUrl = Url.Action("ActiveOrders", "POS") });
            }
        }

        //
        // GET: /POS/SalesDocuments
        /// <summary>
        /// Dokumenty sprzedazy
        /// </summary>
        /// <returns></returns>
        //[CustomAuthorizeAttribute(Roles = "Pracownik")]
        public ActionResult SalesDocuments()
        {
            return View(); 
        }

        
        /// <summary>
        /// Pokazuje zamawiane produkty w ramach jednego zamowienia
        /// </summary>
        /// <returns></returns>
        //[CustomAuthorizeAttribute(Roles = "Pracownik")]
        public ActionResult OrderedProducts()
        {
            return View();
        }

        /// <summary>
        /// Blokuje terminal.
        /// </summary>
        /// <returns>Redirecttoaction Locked</returns>
        public ActionResult End() 
        {
            string[] logs = null;
            if (User.Identity.Name.Contains("|"))
            {
                logs = User.Identity.Name.Split('|');
                FormsAuthentication.SetAuthCookie(logs[0], false);
            }

            return RedirectToAction("Locked");
        }

        //
        // GET: /POS/Locked
        /// <summary>
        /// Panel logowania dla pracownikow.
        /// </summary>
        /// <returns>Nowy widok</returns>
        [CustomAuthorizeAttribute(Roles = "Restauracja")]
        public ActionResult Locked()
        {
            
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(360));
            Response.Cache.SetCacheability(HttpCacheability.Private);
            Response.Cache.SetSlidingExpiration(true);

            if (User.Identity.Name.Contains("|"))
            {
                string[] logs = null;
                logs = User.Identity.Name.Split('|');
                FormsAuthentication.SetAuthCookie(logs[0], false);
                return RedirectToAction("Locked");
            }
            //pobieram pracowników
            try
            {
                List<SelectListItem> pracownicy = new List<SelectListItem>();
                List<string> listaLoginow = null;
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                        listaLoginow = new List<string>(client.GetEmployeesInRestaurant(User.Identity.Name));
                }
                client.Close();

                if (listaLoginow == null)
                {
                    ViewData["logins"] = new List<SelectListItem>();
                    ModelState.AddModelError("", "Pobranie listy loginów nie powiodło się.");
                }
                else
                {
                    foreach (string item in listaLoginow)
                    {
                        pracownicy.Add(new SelectListItem { Text = item, Value = item });
                    }
                    ViewData["logins"] = pracownicy;
                }
            }
            catch (Exception e)
            {
                ViewData["logins"] = new List<SelectListItem>();
                ModelState.AddModelError("", "Pobranie listy loginów nie powiodło się.");
            }
            return View();
        }

        //
        // POST: /POS/Locked
        [HttpPost]
        [CustomAuthorizeAttribute(Roles = "Restauracja")]
        public ActionResult Locked(LogOnModel model)
        {
            //jak bedzie przeładowanie to ot nie bedzie potzrebne
            string[] logs = null;
            if (User.Identity.Name.Contains("|"))
            {
                logs = User.Identity.Name.Split('|');
                FormsAuthentication.SetAuthCookie(logs[0], false);
            }
            if (ModelState.IsValid)
            {
                CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];
                if (customMemebership.ValidateEmployee(model.Login, model.Password, User.Identity.Name))
                {
                    if(logs==null)
                        FormsAuthentication.SetAuthCookie(User.Identity.Name + "|" + model.Login, model.RememberMe);
                    else
                        FormsAuthentication.SetAuthCookie(logs[0] + "|" + model.Login, model.RememberMe);   
                    return RedirectToAction("Index", "POS");
                }
                else
                {
                    ModelState.AddModelError("", "");
                }
            }

            // If we got this far, something failed, redisplay form
            //pobieramy pracowników
            try
            {
                List<SelectListItem> pracownicy = new List<SelectListItem>();
                List<string> listaLoginow = null;
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    listaLoginow = new List<string>(client.GetEmployeesInRestaurant(User.Identity.Name));
                }
                client.Close();

                if (listaLoginow == null)
                {
                    ViewData["logins"] = new List<SelectListItem>();
                    ModelState.AddModelError("", "Pobranie listy loginów nie powiodło się.");
                }
                else
                {
                    foreach (string item in listaLoginow)
                    {
                        pracownicy.Add(new SelectListItem { Text = item, Value = item });
                    }
                    ViewData["logins"] = pracownicy;
                }
            }
            catch (Exception e)
            {
                ViewData["logins"] = new List<SelectListItem>();
                ModelState.AddModelError("", "Pobranie listy loginów nie powiodło się.");
            }

            return View(model);
        }

        //ustawienie nowego statusu dla restauracji
        public void status(string st)
        {
            bool value = false;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value = client.SetRestaurantOnline(User.Identity.Name, st);
                }
                client.Close();
            }
            catch (Exception e)
            {
                value = false;
            }
        }

        //pobranie aktualnego statusu restauracji
        public string getStatus()
        {
            bool value = false;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value = client.RestaurantOnlineStatus(User.Identity.Name);
                }
                client.Close();
            }
            catch (Exception e)
            {
                value = false;
            }
            return (value) ? "Online" : "Offline"; 
        }

        //ustawienie daty ostatniej aktywnosci restauracji
        public void setAct()
        { 
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    client.SetRestaurantActivity(User.Identity.Name);
                }
                client.Close();
            }
            catch (Exception e)
            {
                ;
            }
        }
    }
}
