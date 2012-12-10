using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            return View();
        }

    }
}
