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
using Erestauracja.Helpers;
using Erestauracja.ServiceReference;
using Erestauracja.Providers;

namespace Erestauracja.Controllers
{
    //[CustomAuthorizeAttribute(Roles = "Admin")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<RestaurantTop> value = null;
            Statistics stats = null;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value = new List<RestaurantTop>(client.GetTopRestaurant());
                    stats = client.GetStatistics();
                }
                client.Close();
            }
            catch (Exception e)
            {
                value = null;
                stats = null;
            }

            if (value == null)
            {
                ModelState.AddModelError("", "Pobieranie restauracji nie powiodło się.");
                ViewData["top"] = new List<RestaurantTop>();
            }
            else
            {
                ViewData["top"] = value;
            }

            if (stats == null)
            {
                ModelState.AddModelError("", "Pobieranie statystyk nie powiodło się.");
                ViewData["stat"] = new Statistics();
            }
            else
            {
                ViewData["stat"] = stats;
            }

            #region śmieci
            //    ViewBag.Message = "Witaj na stronie głównej!";
            //var Miasta = new SelectList(new []{
            //                                  new {ID="1",Name="Tczew"},
            //                                  new{ID="2",Name="Gdansk"},
            //                                  new{ID="3",Name="Gdynia"},
            //                                  new{ID="4",Name="Sopot"},
            //                              },
            //                "ID","Name",1);
            //ViewData["Miasta"]=Miasta;
            //IEnumerable<SelectListItem> selectList = null;
            #endregion
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
            ViewData["rest"] = items;
            return View();
        }

        public ActionResult Unauthorized()
        {
            CustomRoleProvider role = (CustomRoleProvider)System.Web.Security.Roles.Providers["CustomRoleProvider"];
            if (User.Identity.Name.Contains("|"))
            {
                string[] logs = User.Identity.Name.Split('|');
                if (role.IsUserInRole(logs[0], "Restauracja") && role.IsUserInRole(logs[1], "Pracownik"))
                {
                    return RedirectToAction("Index", "POS");
                }
                else if (role.IsUserInRole(logs[0], "Restauracja") && !role.IsUserInRole(logs[1], "Pracownik"))
                {
                    return RedirectToAction("locked", "POS");
                }
                else if (!role.IsUserInRole(logs[0], "Restauracja"))
                {
                    FormsAuthentication.SignOut();
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(); 
                }

            }
            else if (role.IsUserInRole(User.Identity.Name, "Menadżer"))
            {
                return RedirectToAction("Index", "ManagePanel");
            }
            else if (role.IsUserInRole(User.Identity.Name, "Admin"))
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                return View();
            }
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
