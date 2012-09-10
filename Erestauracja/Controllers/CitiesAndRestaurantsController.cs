using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Subgurim.Controles;
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

        public ActionResult ListRestaurantsFromCity()
        {
            //ciekawe czy to zadziala, pewnie nie, bo MVC ;D
            GMap Gmap1 = new GMap();
            Gmap1.enableHookMouseWheelToZoom = true;
            return View();
        }
    }
}
