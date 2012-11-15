using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Erestauracja;
using System.Web.Security;
using Erestauracja.Authorization;
using Erestauracja.Models;

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
        //[CustomAuthorizeAttribute(Roles = "Pracownik")]
        public ActionResult Index()
        {
            return View();
        }


        //// GET: /POS/logon
        ///// <summary>
        ///// Drugie logowanie dla pracownika danej restauracji
        ///// </summary>
        ///// <returns></returns>
        ////[CustomAuthorizeAttribute(Roles = "Restauracja")]
        //public ActionResult Logon()
        //{
        //    //zaloguj i przekieruj
        //    return RedirectToAction("Index");
        //}


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
        public void End() 
        {

        }

        //
        // GET: /POS/Locked
        /// <summary>
        /// Panel logowania dla pracownikow.
        /// </summary>
        /// <returns>Nowy widok</returns>
        public ActionResult Locked()
        {
            //pobieram pracowników
            //pobranie listy państw
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

        public ActionResult scrollexample()
        {
            return View();
        }

    }
}
