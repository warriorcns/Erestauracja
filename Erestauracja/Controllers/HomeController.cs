using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Web.UI.WebControls;
using Erestauracja;
using System.Web.Security;
using Erestauracja.Authorization;

namespace Erestauracja.Controllers
{
    
    //[CustomAuthorizeAttribute(Roles = "Admin")]
    //tu admin musi wejsc by go przekierowalo
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
           if (Roles.IsUserInRole(User.Identity.Name, "Admin"))
            {
                //Redirect to admin page
                Response.Redirect("~/Admin/Index");
            }
           if (Roles.IsUserInRole(User.Identity.Name, "Menadżer"))
           {
               //Redirect to menadzer page
               Response.Redirect("~/ManagePanel");
           }

            ViewBag.Message = "Witaj na stronie głównej!";
    
            var Miasta = new SelectList(new []
                                          {
                                              new {ID="1",Name="Tczew"},
                                              new{ID="2",Name="Gdansk"},
                                              new{ID="3",Name="Gdynia"},
                                              new{ID="4",Name="Sopot"},
                                          },
                            "ID","Name",1);
            ViewData["Miasta"]=Miasta;

            var Restauracje = new SelectList(new[]
                                          {
                                              new {ID="1",Name="De grasso"},
                                              new{ID="2",Name="La Scalla"},
                                              new{ID="3",Name="Mc Donald"},
                                              new{ID="4",Name="Subway"},
                                          },
                            "ID", "Name", 1);
            ViewData["Restauracje"] = Restauracje;
                        
            return View();
        }

        
        

        [CustomAuthorizeAttribute(Roles = "Klient, Menadżer, PracownikFull, PracownikLow")]
        public ActionResult TopRestaurants()
        {
            return View();
        }
        
        
        public ActionResult Unauthorized()
        {
            return View();
        }

    }
}
