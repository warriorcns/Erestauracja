using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Erestauracja.Models;
using Erestauracja.Providers;
using Erestauracja.ServiceReference;

namespace Erestauracja.Controllers
{
    
    public class HomeController : Controller
    {
        //glowny widok
        public ActionResult Index()
        {
            CustomRoleProvider rp = new CustomRoleProvider();
            if (User.Identity.IsAuthenticated)
            {
                if (!rp.IsUserInRole(User.Identity.Name, "Klient"))
                {
                    return RedirectToAction("Unauthorized");
                }
            }

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

        //metoda obslugujaca przekierowanie w zaleznosci od roli
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
            else if (role.IsUserInRole(User.Identity.Name, "Restauracja"))
            {
                return RedirectToAction("locked", "POS");
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

        //wyszukuje restauracje - wywolanie poprzez jquery
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

        //
        // GET: /Home/Info
        public ActionResult Info()
        {
            CustomRoleProvider rp = new CustomRoleProvider();
            if (User.Identity.IsAuthenticated)
            {
                if (!rp.IsUserInRole(User.Identity.Name, "Klient"))
                {
                    return RedirectToAction("Unauthorized");
                }
            }
            return View();
        }

        //
        // GET: /Home/Errors
        public ActionResult Errors()
        {
            CustomRoleProvider rp = new CustomRoleProvider();
            if (User.Identity.IsAuthenticated)
            {
                if (!rp.IsUserInRole(User.Identity.Name, "Klient"))
                {
                    return RedirectToAction("Unauthorized");
                }
            }
            return View();
        }

        //
        // POST: /Home/Errors
        [HttpPost]
        public ActionResult Errors(ErrorModels model)
        {
            CustomRoleProvider rp = new CustomRoleProvider();
            if (User.Identity.IsAuthenticated)
            {
                if (!rp.IsUserInRole(User.Identity.Name, "Klient"))
                {
                    return RedirectToAction("Unauthorized");
                }
            }

            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    model.Email = "User: " + User.Identity.Name;
                }
                if (!String.IsNullOrWhiteSpace(model.Email))
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
                else
                {
                    ModelState.AddModelError("", "Nie podano adresu email.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Home/ErrorSuccess
        public ActionResult ErrorSuccess()
        {
            CustomRoleProvider rp = new CustomRoleProvider();
            if (User.Identity.IsAuthenticated)
            {
                if (!rp.IsUserInRole(User.Identity.Name, "Klient"))
                {
                    return RedirectToAction("Unauthorized");
                }
            }

            return View();
        }

        //sprawdzanie czy restauracja jest online - wywolanie przez jquery
        public string IsOnline(int id)
        {
            bool value = false;
            try
            {
                Erestauracja.ServiceReference.EresServiceClient client = new Erestauracja.ServiceReference.EresServiceClient();
                using (client)
                {
                    //  Uchwyt IsOnline = new Uchwyt(client.IsRestaurantOnline);
                    //  value = IsOnline(id);
                    value = client.IsRestaurantOnline(id);
                }
                client.Close();
            }
            catch (Exception e)
            {
                value = false;
            }

            if (value == false)
            {
                return "Offline";
            }
            else
            {
                return "Online";
            }
        }

        //ustawienie aktywnosci uzytkownika
        public void setAct()
        {
            if (User.Identity.IsAuthenticated)
            {
                bool value = false;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.SetUserActivity(User.Identity.Name);
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    value = false;
                }
            }
        }
    }
}
