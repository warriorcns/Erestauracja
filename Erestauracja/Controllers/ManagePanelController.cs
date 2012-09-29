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
using Erestauracja.ServiceReference;

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
        public ActionResult EditRestaurant(int id)
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

            RestaurantInfo rest = null;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    rest = client.GetRestaurant(User.Identity.Name, id);
                }
                client.Close();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Pobieranie restauracji nie powiodło się.");
            }
            if (rest == null)
            {
                ModelState.AddModelError("", "Pobieranie restauracji nie powiodło się.");
            }
            else
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
        // GET: /ManagePanel/MainPage
        public ActionResult MainPage(int id)
        {
            ViewData["id"] = id;
            if (id > 0)
            {
                MainPageContent value = null;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.GetMainPage(User.Identity.Name, id);
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    value = null;
                }

                if (value == null)
                {
                    ModelState.AddModelError("", "Pobieranie danych o restauracji nie powiodło się.");
                }
                else
                {
                    MainPageModel nowy = new MainPageModel();
                    nowy.Description = value.Description;
                    nowy.Foto = value.Foto;
                    nowy.SpecialOffers = value.SpecialOffers;
                    nowy.RestaurantID = id;
                    return View(nowy);
                }
            }
            return RedirectToAction("Restaurant");
        }

        //
        // GET: /ManagePanel/EditMainPage
        public ActionResult EditMainPage(int id)
        { 
            if (id > 0)
            {
                ViewData["id"] = id;
                MainPageContent value = null;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.GetMainPage(User.Identity.Name, id);
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    value = null;
                }

                if (value == null)
                {
                    ModelState.AddModelError("", "Pobieranie danych o restauracji nie powiodło się.");
                }
                else
                {
                    MainPageModel nowy = new MainPageModel();
                    nowy.Description = value.Description;
                    nowy.Foto = value.Foto;
                    nowy.SpecialOffers = value.SpecialOffers;
                    nowy.RestaurantID = id;
                    return View(nowy);
                }
            }
            return RedirectToAction("Restaurant");
        }

        //
        // POST: /ManagePanel/EditMainPage
        [HttpPost]
        public ActionResult EditMainPage(MainPageModel model)
        {
            ViewData["id"] = model.RestaurantID;
            if (ModelState.IsValid)
            {
                bool value = false;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.EditMainPage(model.Description, model.Foto, model.SpecialOffers, model.RestaurantID, User.Identity.Name);
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
                    return RedirectToAction("MainPage", "ManagePanel", new { id = model.RestaurantID });
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /ManagePanel/Menu
        public ActionResult Menu(int id)
        {
            ViewData["id"] = id;
            return View();
        }

        //
        // GET: /ManagePanel/Delivery
        public ActionResult Delivery(int id)
        {
            ViewData["id"] = id;
            if (id > 0)
            {
                DeliveryPageContent value = null;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.GetDeliveryPage(User.Identity.Name, id);
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    value = null;
                }

                if (value == null)
                {
                    ModelState.AddModelError("", "Pobieranie danych o restauracji nie powiodło się.");
                }
                else
                {
                    DeliveryPageModel nowy = new DeliveryPageModel();
                    nowy.Delivery = value.Delivery;
                    nowy.RestaurantID = id;

                    return View(nowy);
                }
            }
            return RedirectToAction("Restaurant");
        }

        //
        // GET: /ManagePanel/EditDeliveryPage
        public ActionResult EditDeliveryPage(int id)
        {
            if (id > 0)
            {
                ViewData["id"] = id;
                DeliveryPageContent value = null;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.GetDeliveryPage(User.Identity.Name, id);
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    value = null;
                }

                if (value == null)
                {
                    ModelState.AddModelError("", "Pobieranie danych o restauracji nie powiodło się.");
                }
                else
                {
                    DeliveryPageModel nowy = new DeliveryPageModel();
                    nowy.Delivery = value.Delivery;
                    nowy.RestaurantID = id;
                    return View(nowy);
                }
            }
            return RedirectToAction("Restaurant");
        }

        //
        // POST: /ManagePanel/EditDeliveryPage
        [HttpPost]
        public ActionResult EditDeliveryPage(DeliveryPageModel model)
        {
            ViewData["id"] = model.RestaurantID;
            if (ModelState.IsValid)
            {
                bool value = false;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.EditDeliveryPage(model.Delivery, model.RestaurantID, User.Identity.Name);
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
                    return RedirectToAction("Delivery", "ManagePanel", new { id = model.RestaurantID });
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /ManagePanel/Parties
        public ActionResult Parties(int id)
        {
            ViewData["id"] = id;
            if (id > 0)
            {
                MainPageContent value = null;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.GetMainPage(User.Identity.Name, id);
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    value = null;
                }

                if (value == null)
                {
                    ModelState.AddModelError("", "Pobieranie danych o restauracji nie powiodło się.");
                }
                else
                {
                    MainPageModel nowy = new MainPageModel();
                    nowy.Description = value.Description;
                    nowy.Foto = value.Foto;
                    nowy.SpecialOffers = value.SpecialOffers;
                    nowy.RestaurantID = id;
                    return View(nowy);
                }
            }
            return RedirectToAction("Restaurant");
        }

        //
        // GET: /ManagePanel/Gallery
        public ActionResult Gallery(int id)
        {
            ViewData["id"] = id;
            if (id > 0)
            {
                MainPageContent value = null;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.GetMainPage(User.Identity.Name, id);
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    value = null;
                }

                if (value == null)
                {
                    ModelState.AddModelError("", "Pobieranie danych o restauracji nie powiodło się.");
                }
                else
                {
                    MainPageModel nowy = new MainPageModel();
                    nowy.Description = value.Description;
                    nowy.Foto = value.Foto;
                    nowy.SpecialOffers = value.SpecialOffers;
                    nowy.RestaurantID = id;
                    return View(nowy);
                }
            }
            return RedirectToAction("Restaurant");
        }

        //
        // GET: /ManagePanel/Contact
        public ActionResult Contact(int id)
        {
            ViewData["id"] = id;
            if (id > 0)
            {
                MainPageContent value = null;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.GetMainPage(User.Identity.Name, id);
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    value = null;
                }

                if (value == null)
                {
                    ModelState.AddModelError("", "Pobieranie danych o restauracji nie powiodło się.");
                }
                else
                {
                    MainPageModel nowy = new MainPageModel();
                    nowy.Description = value.Description;
                    nowy.Foto = value.Foto;
                    nowy.SpecialOffers = value.SpecialOffers;
                    nowy.RestaurantID = id;
                    return View(nowy);
                }
            }
            return RedirectToAction("Restaurant");
        }
    }
}
