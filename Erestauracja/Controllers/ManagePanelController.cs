using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Erestauracja.Authorization;
using Erestauracja.Models;
using Erestauracja.Providers;
using MySql.Data.MySqlClient;
using System.Data;

namespace Erestauracja.Controllers
{
    [CustomAuthorizeAttribute(Roles = "Menadżer")]
    public class ManagePanelController : Controller
    {
        //
        // GET: /ManagePanel/Index
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /ManagePanel/Restaurant
        public ActionResult Restaurant()
        {
            List<ServiceReference.Restaurant> value = null;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value = new List<ServiceReference.Restaurant>(client.GetRestaurantsByManagerLogin(User.Identity.Name));
                }
                client.Close();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Pobieranie restauracji nie powiodło się.");
            }
            if (value == null)
            {
                ModelState.AddModelError("", "Pobieranie restauracji nie powiodło się.");
            }

            ViewData["restauracje"] = value;
           /*
            //string constr = "Server=TestServer;Database=TestDB;uid=test;pwd=test;";
            //string query = "SELECT ProductID, ProductName, UnitPrice FROM Products";
            ServiceReference.EresServiceClient res = new ServiceReference.EresServiceClient();    
            //Tutaj wywolac metode ktora pobiera liste tablic stringow (restauracje)
            //List<string>[] listaRes = res.
            //SqlDataAdapter da = new SqlDataAdapter(query, constr);
            //DataTable table = new DataTable();
            //da.Fill(table);
            //ListView1.DataSource = table;
            //ListView1.DataBind();
            * */
            return View();
        }

        //
        // GET: /ManagePanel/Personnel
        public ActionResult Personnel()
        {
            return View();
        }  
  
        //
        // GET: /ManagePanel/Reports
        public ActionResult Reports()
        {
            return View();

        }

        //
        // GET: /ManagePanel/AddRestaurant
        public ActionResult AddRestaurant()
        {
            return View();
        }

        //
        // POST: /ManagePanel/AddRestaurant
        [HttpPost]
        public ActionResult AddRestaurant(RegisterRestaurantModel model)
        {
            if (ModelState.IsValid)
            {
                //CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];
                //CustomMembershipUser user = (CustomMembershipUser)customMemebership.GetUser(User.Identity.Name, true);
                CustomRoleProvider role = (CustomRoleProvider)System.Web.Security.Roles.Providers["CustomRoleProvider"];
                if (role.IsUserInRole(User.Identity.Name, "Menadżer"))
                {
                    bool value = false;
                    try
                    {
                        ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                        using (client)
                        {
                            value = client.AddRestaurant(model.Name, model.DisplayName, model.Address, model.TownId, model.Country, model.Telephone, model.Email, model.Nip, model.Regon, model.Password, User.Identity.Name, model.DeliveryTime);
                        }
                        client.Close();
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", "Dodawanie restauracji nie powiodło się.");
                    }

                    if (value == false)
                    {
                        ModelState.AddModelError("", "Dodawanie restauracji nie powiodło się.");
                    }
                    else
                    {
                        return RedirectToAction("Restaurant", "ManagePanel");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Nie jesteś zalogowany jako menadżer.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /ManagePanel/EditRestaurant
        public ActionResult EditRestaurant(Erestauracja.ServiceReference.Restaurant rest)
        {
            EditRestaurantModel model = new EditRestaurantModel();
            model.Name = rest.Name;
            model.DisplayName = rest.DisplayName;
            model.Address = rest.Address;
            model.TownId = rest.TownID;
            model.Country = rest.Country;
            model.Telephone = rest.Telephone;
            model.Email = rest.Email;
            model.Nip = rest.Nip;
            model.Regon = rest.Regon;
            model.DeliveryTime = rest.DeliveryTime;
            if (model!=null)
            {
                    return View(model);
            }

            ModelState.AddModelError("", "Wczytywanie danych o restauracji nie powiodło się.");
            return RedirectToAction("Index");
        }

        //
        // POST: /ManagePanel/EditRestaurant
        [HttpPost]
        public ActionResult EditRestaurant(EditRestaurantModel model)
        {
            if (ModelState.IsValid)
            {
                   return RedirectToAction("Restaurant", "ManagePanel");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /ManagePanel/ManageRestaurant
        public ActionResult ManageRestaurant(Erestauracja.ServiceReference.Restaurant model)
        {
            if (ModelState.IsValid)
            {
                return View(model);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
        
    }
}
