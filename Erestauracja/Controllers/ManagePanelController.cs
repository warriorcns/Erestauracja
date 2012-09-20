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
                        value = false;
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
            try
            {
                ServiceReference.EresServiceClient country = new ServiceReference.EresServiceClient();

                List<string> listapobrana = new List<string>(country.GetCountriesList());
                List<SelectListItem> countryList = new List<SelectListItem>();


                foreach (string item in listapobrana)
                {
                    countryList.Add(new SelectListItem { Text = item, Value = item });
                }
                ViewData["countryList"] = countryList;

                country.Close();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Pobranie listy panstw nie powiodło się.");
            }

            if (rest != null)
            {
                EditRestaurantModel model = new EditRestaurantModel();
                model.Id = rest.ID;
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

                ModelState.Clear();
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
            try
            {
                ServiceReference.EresServiceClient country = new ServiceReference.EresServiceClient();

                List<string> listapobrana = new List<string>(country.GetCountriesList());
                List<SelectListItem> countryList = new List<SelectListItem>();


                foreach (string item in listapobrana)
                {
                    countryList.Add(new SelectListItem { Text = item, Value = item });
                }
                ViewData["countryList"] = countryList;

                country.Close();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Pobranie listy panstw nie powiodło się.");
            }

            if (ModelState.IsValid)
            {
                bool value = false;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.EditRestaurant(model.Name, model.DisplayName, model.Address, model.TownId, model.Country, model.Telephone, model.Email, model.Nip, model.Regon, model.DeliveryTime, User.Identity.Name, model.Id);
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    value = false;
                }

                if (value == false)
                {
                    ModelState.AddModelError("", "Edytowanie restauracji nie powiodło się.");
                }
                else
                {
                    return RedirectToAction("Restaurant", "ManagePanel");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /ManagePanel/ManageRestaurant
        public ActionResult MainPage(Erestauracja.ServiceReference.Restaurant model)
        {
            //if (ModelState.IsValid)
            //{
            //    ViewData["html"] = "<h3><strong><big>q</big><strike>wert</strike><em><big>y</big></em></strong></h3>";
            //    return View(model);
            //}
            TestModel nowy = new TestModel();
            nowy.Html="<h3 align=\"center\"><b><u>Nasza</u></b> restauracja zajmuje się przygotowaniem posiłków:<br></h3><h3><ol><li><strong>kuchni polskiej;&nbsp;</strong></li></ol><p><strong>Dania można zamawiać telefonicznie, jak i zjeść <strike>na miejscu</strike> w bardzo klimatycznym <a href=\"http://lokal.pl\">lokalu</a>.</strong></p></h3>";
            return View(nowy);
        }

        public ActionResult jHtmltest()
        {

            return View();
        }
        
        
    }
}
