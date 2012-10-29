using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Erestauracja.Controllers
{
    //[Authorize(Roles = "Klient, Menadżer, PracownikFull, PracownikLow")]
    public class FindController : Controller
    {
        //
        // GET: /Find/

        public ActionResult Index()
        {
            return View();
        }

    }
}
