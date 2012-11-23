using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Erestauracja;
using System.Web.Security;
using Erestauracja.Authorization;
using Erestauracja.Models;
using Erestauracja.Providers;

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
            return View();
        }

        //
        // GET: /POS/AllOrders
        /// <summary>
        /// Wszystkie zamowienia
        /// </summary>
        /// <returns></returns>
        //[CustomAuthorizeAttribute(Roles = "Pracownik")]
        public ActionResult AllOrders()
        {
            return View();
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
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
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

        public ActionResult scrollexample()
        {
            return View();
        }
    

    }
}
