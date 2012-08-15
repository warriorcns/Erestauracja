using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Web.UI.WebControls;
using Erestauracja;

namespace Erestauracja.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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

        
        public ActionResult About()
        {
            return View();
        }


        public ActionResult TopRestaurants()
        {
            return View();
        }

        public ActionResult Info()
        {
            return View();
        }

    }
}
