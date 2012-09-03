using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Erestauracja.Authorization;

namespace Erestauracja.Controllers
{
    [CustomAuthorizeAttribute(Roles = "Menadżer")]
    public class ManagePanelController : Controller
    {
        //
        // GET: /ManagePanel/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /ManagePanel/
        public ActionResult Restaurant()
        {
            return View();
        }

        //
        // GET: /ManagePanel/
        public ActionResult Personnel()
        {
            return View();
        }  
  
        //
        // GET: /ManagePanel/
        public ActionResult Reports()
        {
            return View();

        }
        
    }
}
