﻿using System;
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
using Erestauracja.Helpers;

namespace Erestauracja.Controllers
{
    //[CustomAuthorizeAttribute(Roles = "Admin")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            ViewBag.Message = "Witaj na stronie głównej!";
    
            var Miasta = new SelectList(new []{
                                              new {ID="1",Name="Tczew"},
                                              new{ID="2",Name="Gdansk"},
                                              new{ID="3",Name="Gdynia"},
                                              new{ID="4",Name="Sopot"},
                                          },
                            "ID","Name",1);
            ViewData["Miasta"]=Miasta;
            //IEnumerable<SelectListItem> selectList = null;
            var selectList = new SelectList(new[]{
                                              new {ID=String.Empty,Name=String.Empty},
                                          },
                            "ID", "Name", 1);
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem
            {
                Text = String.Empty,
                Value = String.Empty
            });
            //ViewData["rest"] = selectList;
            ViewData["rest"] = items;
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
            IEnumerable<SelectListItem> selectList;
            if (listares != null)
            {
                selectList =
                from c in listares
                select new SelectListItem
                {
                    Selected = ( c.ResId == 0 ),
                    Text = c.Name,
                    Value = c.ResId.ToString() + "|" + c.TownId.ToString()

                };
                ViewData["rest"] = selectList;
                return Json(selectList);
            }
            else
            {
                var smth = string.Empty;
                return Json(smth);
            }

            //return RedirectToAction("Index");
            


        }

        public ActionResult Info()
        {
            return View();
        }

        //
        // GET: /Home/Errors
        public ActionResult Errors()
        {
            return View();
        }

        //
        // POST: /Home/Errors
        [HttpPost]
        public ActionResult Errors(ErrorModels model)
        {
            if (ModelState.IsValid)
            {
                bool value = false;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.SendError(model.Email, model.Text);
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    value = false;
                }

                if (value == false)
                {
                    ModelState.AddModelError("", "Wysyłanie zgłoszenia nie powiodło się.");
                }
                else
                {
                    return RedirectToAction("ErrorSuccess", "Home");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Home/ErrorSuccess
        public ActionResult ErrorSuccess()
        {
            return View();
        }
    }
}
