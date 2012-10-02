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
using Erestauracja.Models;

namespace Erestauracja.Controllers
{
    
    //[CustomAuthorizeAttribute(Roles = "Admin")]
    //tu admin musi wejsc by go przekierowalo
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            if (User.IsInRole("Admin"))
            {
                Response.Redirect("/Admin/Index");
                //Response.Write("Pomyslnie zweryfikowano role uzytkownika");
                //Response.Redirect("Unauthorized.aspx");
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
            //IEnumerable<SelectListItem> selectList = null;
            var selectList = new SelectList(new[]
                                          {
                                              new {ID="0",Name="Wszystkie"},
                                              //new{ID="2",Name="La Scalla"},
                                              //new{ID="3",Name="Mc Donald"},
                                              //new{ID="4",Name="Subway"},
                                          },
                            "ID", "Name", 1);
            
            ViewData["rest"] = selectList;
          
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

        public ActionResult SearchRestaurants(String value) 
        { 
            //(m.TownName)
            ServiceReference.EresServiceClient res = new ServiceReference.EresServiceClient();
            Erestauracja.ServiceReference.RestaurantInTown[] listares = res.GetRestaurantByTown(value);
           
            //
            //wazne
            // przepisac tylko nazwy restauracji w petli i to przekazac do widoku za pomoca viewdata.
            List<SelectList> r = new List<SelectList>();
            IEnumerable<SelectListItem> selectList =
            from c in listares
            select new SelectListItem
            {
                Selected = ( c.ResId == 0 ),
                Text = c.Name,
                Value = c.ResId.ToString()
            };
            //ViewData["rest"] = selectList;
            //return RedirectToAction("Index");
            return Json(selectList);
        }

    }
}
