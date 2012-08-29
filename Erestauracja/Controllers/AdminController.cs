using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Erestauracja.Authorization;

namespace Erestauracja.Controllers
{
    [CustomAuthorizeAttribute(Roles = "admin")]
    //[Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        
        public ActionResult Index()
        {
            if (!User.IsInRole("admin"))
            {
                //Response.Redirect("/Home/Index");
                //Response.Write("Pomyslnie zweryfikowano role uzytkownika");
                //Response.Redirect("Unauthorized.aspx");
            }

            return View();

            
        }

        public ActionResult phpmyadmin()
        {

            return View();
        }
        

    }
}
