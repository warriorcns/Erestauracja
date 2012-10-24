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


        // GET: /POS/logon
        /// <summary>
        /// Drugie logowanie dla pracownika danej restauracji
        /// </summary>
        /// <returns></returns>
        //[CustomAuthorizeAttribute(Roles = "Restauracja")]
        public ActionResult Logon()
        {
            //zaloguj i przekieruj
            return RedirectToAction("Index");
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

        //
        // GET: /POS/End
        /// <summary>
        /// Blokowanie posu - wylogowanie pracownika
        /// </summary>
        /// <returns></returns>
        //[CustomAuthorizeAttribute(Roles = "Pracownik")]
        public ActionResult End()
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

    }
}
