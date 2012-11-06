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
using System.Web.Security;

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

                ViewData["restauracje"] = new List<ServiceReference.Restaurant>();
            }
            else
            {
                ViewData["restauracje"] = value;
            }

            return View();
        }

        //
        // GET: /ManagePanel/Personnel
        public ActionResult Personnel()
        {
            List<ServiceReference.Presonnel> value = null;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value = new List<ServiceReference.Presonnel>(client.GetPersonnel(User.Identity.Name));
                }
                client.Close();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Pobieranie pracowników nie powiodło się.");
            }

            if (value == null)
            {
                ViewData["pracownicy"] = new List<ServiceReference.Presonnel>();
                ModelState.AddModelError("", "Pobieranie pracowników nie powiodło się.");
            }
            else
            {
                ViewData["pracownicy"] = value;
            }

            return View();
        }  
  
        //
        // GET: /ManagePanel/Reports
        public ActionResult Reports()
        {
            return View();

        }

        //
        // GET: /ManagePanel/AddEmployee
        public ActionResult AddEmployee(int id)
        {
            //ustawienie danych o płci
            List<SelectListItem> sex = new List<SelectListItem>();
            sex.Add(new SelectListItem { Text = "Mężczyzna", Value = "Mężczyzna" });
            sex.Add(new SelectListItem { Text = "Kobieta", Value = "Kobieta" });
            ViewData["sex"] = (IEnumerable<SelectListItem>)sex;

            //pobranie listy państw
            try
            {
                List<SelectListItem> countryList = new List<SelectListItem>();
                List<string> listaPanstw = null;
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    listaPanstw = new List<string>(client.GetCountriesList());
                }
                client.Close();

                foreach (string item in listaPanstw)
                {
                    countryList.Add(new SelectListItem { Text = item, Value = item });
                }
                ViewData["countryList"] = countryList;
            }
            catch (Exception e)
            {
                ViewData["countryList"] = new List<SelectListItem>();
                ModelState.AddModelError("", "Pobranie listy panstw nie powiodło się.");
            }

            //ustawienie pustych danych do mapki
            ViewData["Map"] = (IEnumerable<Town>)(new List<Town>());

            RegisterEmployeeModel model = new RegisterEmployeeModel();
            model.RestaurantId = id;
            return View(model);
        }

        //
        // POST: /ManagePanel/AddEmployee
        [HttpPost]
        public ActionResult AddEmployee(RegisterEmployeeModel model)
        {
            List<Town> value = null;
            if (ModelState.IsValid)
            {
                string status = String.Empty;

                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = new List<Town>(client.GetTowns(out status, model.Town, model.PostalCode));
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Pobieranie miast nie powiodło się.");
                }
                if (value == null)
                {
                    ModelState.AddModelError("", "Pobieranie miast nie powiodło się.");
                }
                else
                {
                    if (value.Count == 1)//dodaj usera
                    {
                        MembershipCreateStatus createStatus;
                        CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];
                        CustomMembershipUser user = customMemebership.CreateUser(model.Login, model.Password, model.Email, model.Name, model.Surname, model.Address, value[0].ID, model.Country, model.Birthdate, model.Sex, model.Telephone, model.Question, model.Answer, true, out createStatus);
                        if (user != null)
                        {
                            CustomRoleProvider role = (CustomRoleProvider)System.Web.Security.Roles.Providers["CustomRoleProvider"];
                            role.AddUsersToRoles(new string[] { user.Login }, new string[] { "Pracownik" });
                        }
                        if (createStatus == MembershipCreateStatus.Success)
                        {
                            bool ok = false;
                            try
                            {
                                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                                using (client)
                                {
                                       ok = client.AddUserToRestaurant(user.Id,model.RestaurantId);
                                }
                                client.Close();
                            }
                            catch (Exception e)
                            {
                                //powinno przekierować
                                //!!!!!!!!!!!!!!!!!!!
                                //!!!!!!!!!!!!!!!!!!!
                                //powinno gdzieś pokazać że user jest zrobiony ale nie przypisany
                                //teraz wraca do strony gdzie sie poprawia błędy 
                                //ale tego usera już nie bedzie można stworzyć
                                ModelState.AddModelError("", "Przypisnie użytkownika do restauracji nie powiodło się.");
                            }
                            if (ok == false)
                            {
                                //powinno przekierować
                                //!!!!!!!!!!!!!!!!!!!
                                //!!!!!!!!!!!!!!!!!!!
                                //powinno gdzieś pokazać że user jest zrobiony ale nie przypisany
                                //teraz wraca do strony gdzie sie poprawia błędy 
                                //ale tego usera już nie bedzie można stworzyć
                                ModelState.AddModelError("", "Przypisnie użytkownika do restauracji nie powiodło się.");
                            }
                            else
                            {
                                return RedirectToAction("Personnel", "ManagePanel");
                            }

                            
                        }
                        else
                        {
                            ModelState.AddModelError("", createStatus.ToString());
                        }
                    }
                    else if (value.Count > 1)//wczytaj miasta do mapki
                    {
                        foreach (Town item in value)
                        {
                            string onClick = String.Format(" \"ChoseAndSend('{0}', '{1}')\" ", item.TownName, item.PostalCode);
                            item.InfoWindowContent = item.TownName + " " + item.PostalCode + "</br>" + "<a href=" + "#" + " onclick=" + onClick + " class=" + "button" + ">" + "Wybierz." + "</a>";
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", status);
                    }
                }
            }

            // If we got this far, something failed, redisplay form

            //ustawienie danych o płci
            List<SelectListItem> sex = new List<SelectListItem>();
            sex.Add(new SelectListItem { Text = "Mężczyzna", Value = "Mężczyzna" });
            sex.Add(new SelectListItem { Text = "Kobieta", Value = "Kobieta" });
            ViewData["sex"] = sex;

            //pobranie listy państw
            try
            {
                List<SelectListItem> countryList = new List<SelectListItem>();
                List<string> listaPanstw = null;
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    listaPanstw = new List<string>(client.GetCountriesList());
                }
                client.Close();

                foreach (string item in listaPanstw)
                {
                    countryList.Add(new SelectListItem { Text = item, Value = item });
                }
                ViewData["countryList"] = countryList;
            }
            catch (Exception e)
            {
                ViewData["countryList"] = new List<SelectListItem>();
                ModelState.AddModelError("", "Pobranie listy panstw nie powiodło się.");
            }

            //ustawienie pustych danych do mapki
            if (value == null)
            {
                ViewData["Map"] = (IEnumerable<Town>)(new List<Town>());
            }
            else
            {
                ViewData["Map"] = value;
            }

            return View(model);
        }

        //
        // GET: /ManagePanel/AddRestaurant
        public ActionResult AddRestaurant()
        {
            //pobranie listy państw
            try
            {
                List<SelectListItem> countryList = new List<SelectListItem>();
                List<string> listaPanstw = null;
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    listaPanstw = new List<string>(client.GetCountriesList());
                }
                client.Close();

                foreach (string item in listaPanstw)
                {
                    countryList.Add(new SelectListItem { Text = item, Value = item });
                }
                ViewData["countryList"] = countryList;
            }
            catch (Exception e)
            {
                ViewData["countryList"] = new List<SelectListItem>();
                ModelState.AddModelError("", "Pobranie listy panstw nie powiodło się.");
            }

            //ustawienie pustych danych do mapki
            ViewData["Map"] = (IEnumerable<Town>)(new List<Town>());

            return View();
        }

        //
        // POST: /ManagePanel/AddRestaurant
        [HttpPost]
        public ActionResult AddRestaurant(RegisterRestaurantModel model)
        {
            List<Town> value = null;
            if (ModelState.IsValid)
            {
                string status = String.Empty;

                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = new List<Town>(client.GetTowns(out status, model.Town, model.PostalCode));
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Pobieranie miast nie powiodło się.");
                }
                if (value == null)
                {
                    ModelState.AddModelError("", "Pobieranie miast nie powiodło się.");
                }
                else
                {
                    if (value.Count == 1)//dodaj usera
                    {
                        CustomRoleProvider role = (CustomRoleProvider)System.Web.Security.Roles.Providers["CustomRoleProvider"];
                        if (role.IsUserInRole(User.Identity.Name, "Menadżer"))
                        {
                            MembershipCreateStatus createStatus;
                            CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];
                            bool user = customMemebership.CreateRestaurant(model.Login, model.Email, model.Password, model.Question, model.Answer, model.Name, model.DisplayName, model.Address, value[0].ID, model.Country, model.Telephone, model.Nip, model.Regon, model.DeliveryTime, User.Identity.Name, out createStatus);

                            if (createStatus == MembershipCreateStatus.Success && user == true)
                            {
                                return RedirectToAction("Restaurant", "ManagePanel");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Dodawanie restauracji nie powiodło się.");
                            }
                        }
                    }
                    else if (value.Count > 1)//wczytaj miasta do mapki
                    {
                        foreach (Town item in value)
                        {
                            string onClick = String.Format(" \"ChoseAndSend('{0}', '{1}')\" ", item.TownName, item.PostalCode);
                            item.InfoWindowContent = item.TownName + " " + item.PostalCode + "</br>" + "<a href=" + "#" + " onclick=" + onClick + " class=" + "button" + ">" + "Wybierz." + "</a>";
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", status);
                    }
                }
            }

            // If we got this far, something failed, redisplay form

            //pobranie listy państw
            try
            {
                List<SelectListItem> countryList = new List<SelectListItem>();
                List<string> listaPanstw = null;
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    listaPanstw = new List<string>(client.GetCountriesList());
                }
                client.Close();

                foreach (string item in listaPanstw)
                {
                    countryList.Add(new SelectListItem { Text = item, Value = item });
                }
                ViewData["countryList"] = countryList;
            }
            catch (Exception e)
            {
                ViewData["countryList"] = new List<SelectListItem>();
                ModelState.AddModelError("", "Pobranie listy panstw nie powiodło się.");
            }

            //ustawienie pustych danych do mapki
            if (value == null)
            {
                ViewData["Map"] = (IEnumerable<Town>)(new List<Town>());
            }
            else
            {
                ViewData["Map"] = value;
            }

            return View(model);
        }

        //
        // GET: /ManagePanel/EditRestaurant
        public ActionResult EditRestaurant(int id)
        {
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
                model.Town = rest.Town;
                model.PostalCode = rest.PostalCode;
                model.Country = rest.Country;
                model.Telephone = rest.Telephone;
                model.Nip = rest.Nip;
                model.Regon = rest.Regon;
                model.DeliveryTime = rest.DeliveryTime;
                model.IsEnabled = rest.IsEnabled;
                ModelState.Clear();

                //pobranie listy państw
                try
                {
                    List<SelectListItem> countryList = new List<SelectListItem>();
                    List<string> listaPanstw = null;
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        listaPanstw = new List<string>(client.GetCountriesList());
                    }
                    client.Close();

                    foreach (string item in listaPanstw)
                    {
                        countryList.Add(new SelectListItem { Text = item, Value = item });
                    }
                    ViewData["countryList"] = countryList;
                }
                catch (Exception e)
                {
                    ViewData["countryList"] = new List<SelectListItem>();
                    ModelState.AddModelError("", "Pobranie listy panstw nie powiodło się.");
                }

                //ustawienie pustych danych do mapki
                ViewData["Map"] = (IEnumerable<Town>)(new List<Town>());

                return View(model);
            }

            //WYŚWIETLKIć GDZIEŚ INFO ŻE NIE WCZYTAŁO DANYCH najlepiej w index
       //     ModelState.AddModelError("", "Wczytywanie danych o restauracji nie powiodło się.");
            return RedirectToAction("Index");
        }

        //
        // POST: /ManagePanel/EditRestaurant
        [HttpPost]
        public ActionResult EditRestaurant(EditRestaurantModel model)
        {
            List<Town> value = null;
            if (ModelState.IsValid)
            {
                string status = String.Empty;

                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = new List<Town>(client.GetTowns(out status, model.Town, model.PostalCode));
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Pobieranie miast nie powiodło się.");
                }
                if (value == null)
                {
                    ModelState.AddModelError("", "Pobieranie miast nie powiodło się.");
                }
                else
                {
                    if (value.Count == 1)//edytuj dane restauracji
                    {
                        bool value2 = false;
                        try
                        {
                            ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                            using (client)
                            {
                                value2 = client.EditRestaurant(model.Name, model.DisplayName, model.Address, value[0].ID, model.Country, model.Telephone, model.Nip, model.Regon, model.DeliveryTime, model.IsEnabled, User.Identity.Name, model.Id);
                            }
                            client.Close();
                        }
                        catch (Exception e)
                        {
                            value2 = false;
                        }
                        if (value2 == false)
                        {
                            ModelState.AddModelError("", "Edytowanie restauracji nie powiodło się.");
                        }
                        else
                        {
                            return RedirectToAction("Restaurant", "ManagePanel");
                        }
                    }
                    else if (value.Count > 1)//wczytaj miasta do mapki
                    {
                        foreach (Town item in value)
                        {
                            string onClick = String.Format(" \"ChoseAndSend('{0}', '{1}')\" ", item.TownName, item.PostalCode);
                            item.InfoWindowContent = item.TownName + " " + item.PostalCode + "</br>" + "<a href=" + "#" + " onclick=" + onClick + " class=" + "button" + ">" + "Wybierz." + "</a>";
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", status);
                    }
                }
            }

            // If we got this far, something failed, redisplay form

            //pobranie listy państw
            try
            {
                List<SelectListItem> countryList = new List<SelectListItem>();
                List<string> listaPanstw = null;
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    listaPanstw = new List<string>(client.GetCountriesList());
                }
                client.Close();

                foreach (string item in listaPanstw)
                {
                    countryList.Add(new SelectListItem { Text = item, Value = item });
                }
                ViewData["countryList"] = countryList;
            }
            catch (Exception e)
            {
                ViewData["countryList"] = new List<SelectListItem>();
                ModelState.AddModelError("", "Pobranie listy panstw nie powiodło się.");
            }

            //ustawienie pustych danych do mapki
            if (value == null)
            {
                ViewData["Map"] = (IEnumerable<Town>)(new List<Town>());
            }
            else
            {
                ViewData["Map"] = value;
            }

            return View(model);
        }

        //
        // GET: /ManagePanel/ChangePassword
        [Authorize]
        public ActionResult ChangePassword(string login)
        {
            ChangeResPasswordModel model = new ChangeResPasswordModel();
            model.Login = login;
            return View(model);
        }

        //
        // POST: /ManagePanel/ChangePassword
        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangeResPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];
                    CustomMembershipUser currentUser = (CustomMembershipUser)customMemebership.GetUser(model.Login, false);
                    changePasswordSucceeded = customMemebership.ChangePassword(currentUser.Login, model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /ManagePanel/ChangePasswordSuccess
        public ActionResult ChangePasswordSuccess()
        {
            return View();
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
            if (id > 0)
            {
                ViewData["id"] = id;

                List<Menu> value2 = null;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value2 = new List<Menu>(client.GetMenu(id));
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    value2 = null;
                }

                if (value2 == null)
                {
                    ModelState.AddModelError("", "Pobieranie danych o restauracji nie powiodło się.");
                }
                else
                {
                    ClientMenuModel model = new ClientMenuModel();
                    model.RestaurantID = id;
                    model.Menu = value2;

                    return View(model);
                }
            }
            return RedirectToAction("Restaurant");
            //return RedirectToAction("Restaurant", "ManagePanel");
        }

        //
        // GET: /ManagePanel/EditMainPage
        public ActionResult EditMenuPage(int id)
        { 
            if (id > 0)
            {
                ViewData["id"] = id;
                
                List<Category> value = null;
                List<Menu> value2 = null;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = new List<Category>(client.GetCategories(User.Identity.Name, id));
                        value2 = new List<Menu>(client.GetMenuManager(User.Identity.Name, id));
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    value = null;
                    value2 = null;
                }

                if (value == null || value2==null)
                {
                    ModelState.AddModelError("", "Pobieranie danych o restauracji nie powiodło się.");
                }
                else
                {
                    MenuModel model = new MenuModel();
                    model.RestaurantID = id;
                    model.Kategorie = value;
                    model.Menu = value2;

                    return View(model);
                }
            }
            return RedirectToAction("Restaurant", "ManagePanel");
        }
        
        //
        // GET: /ManagePanel/AddCategory
        public ActionResult AddCategory(int id)
        {
            ViewData["id"] = id;
            CategoryModel model = new CategoryModel();
            model.RestaurantID = id;
            return View(model);
        }

        //
        // POST: /ManagePanel/AddCategory
        [HttpPost]
        public ActionResult AddCategory(CategoryModel model)
        {
            ViewData["id"] = model.RestaurantID;
            if (ModelState.IsValid)
            {
                CustomRoleProvider role = (CustomRoleProvider)System.Web.Security.Roles.Providers["CustomRoleProvider"];
                if (role.IsUserInRole(User.Identity.Name, "Menadżer"))
                {
                    bool value = false;
                    try
                    {
                        ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                        using (client)
                        {
                            value = client.AddCategory(model.RestaurantID, model.CategoryName, model.CategoryDescription, model.PriceOption, model.NonPriceOption, model.NonPriceOption2, User.Identity.Name);
                        }
                        client.Close();
                    }
                    catch (Exception e)
                    {
                        value = false;
                    }

                    if (value == false)
                    {
                        ModelState.AddModelError("", "Dodawanie kategori nie powiodło się.");
                    }
                    else
                    {
                        return RedirectToAction("EditMenuPage", "ManagePanel", new { id = model.RestaurantID});
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
        public ActionResult Events(int id)
        {
            ViewData["id"] = id;
            if (id > 0)
            {
                EventsPageContent value = null;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.GetEventsPage(User.Identity.Name, id);
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
                    EventsPageModel nowy = new EventsPageModel();
                    nowy.Events = value.Events;
                    nowy.RestaurantID = id;

                    return View(nowy);
                }
            }
            return RedirectToAction("Restaurant");
        }

        //
        // GET: /ManagePanel/EditEventsPage
        public ActionResult EditEventsPage(int id)
        {
            if (id > 0)
            {
                ViewData["id"] = id;
                EventsPageContent value = null;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.GetEventsPage(User.Identity.Name, id);
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
                    EventsPageModel nowy = new EventsPageModel();
                    nowy.Events = value.Events;
                    nowy.RestaurantID = id;
                    return View(nowy);
                }
            }
            return RedirectToAction("Restaurant");
        }

        //
        // POST: /ManagePanel/EditEventsPage
        [HttpPost]
        public ActionResult EditEventsPage(EventsPageModel model)
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
                        value = client.EditEventsPage(model.Events, model.RestaurantID, User.Identity.Name);
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
                    return RedirectToAction("Events", "ManagePanel", new { id = model.RestaurantID });
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /ManagePanel/Gallery
        public ActionResult Gallery(int id)
        {
            ViewData["id"] = id;
            return View();
            //if (id > 0)
            //{
            //    MainPageContent value = null;
            //    try
            //    {
            //        ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
            //        using (client)
            //        {
            //            value = client.GetMainPage(User.Identity.Name, id);
            //        }
            //        client.Close();
            //    }
            //    catch (Exception e)
            //    {
            //        value = null;
            //    }

            //    if (value == null)
            //    {
            //        ModelState.AddModelError("", "Pobieranie danych o restauracji nie powiodło się.");
            //    }
            //    else
            //    {
            //        MainPageModel nowy = new MainPageModel();
            //        nowy.Description = value.Description;
            //        nowy.Foto = value.Foto;
            //        nowy.SpecialOffers = value.SpecialOffers;
            //        nowy.RestaurantID = id;
            //        return View(nowy);
            //    }
            //}
            //return RedirectToAction("Restaurant");
        }

        //
        // GET: /ManagePanel/Contact
        public ActionResult Contact(int id)
        {
            ViewData["id"] = id;
            if (id > 0)
            {
                ContactPageContent value = null;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.GetContactPage(User.Identity.Name, id);
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
                    ContactPageModel nowy = new ContactPageModel();
                    nowy.Contact = value.Contact;
                    nowy.RestaurantID = id;
                    return View(nowy);
                }
            }
            return RedirectToAction("Restaurant");
        }

        //
        // GET: /ManagePanel/EditContactPage
        public ActionResult EditContactPage(int id)
        {
            if (id > 0)
            {
                ViewData["id"] = id;
                ContactPageContent value = null;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.GetContactPage(User.Identity.Name, id);
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
                    ContactPageModel nowy = new ContactPageModel();
                    nowy.Contact = value.Contact;
                    nowy.RestaurantID = id;
                    return View(nowy);
                }
            }
            return RedirectToAction("Restaurant");
        }

        //
        // POST: /ManagePanel/EditContactPage
        [HttpPost]
        public ActionResult EditContactPage(ContactPageModel model)
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
                        value = client.EditContactPage(model.Contact, model.RestaurantID, User.Identity.Name);
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
                    return RedirectToAction("Contact", "ManagePanel", new { id = model.RestaurantID });
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

         //
        // GET: /ManagePanel/EditCategoryPage
        public ActionResult EditCategoryPage(int id, int cat)
        {
            if (id > 0 && cat >0)
            {
                ViewData["id"] = id;
                Category value = null;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.GetCategory(User.Identity.Name, id, cat);
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
                    return View(value);
                }
            }
            return RedirectToAction("Restaurant");
        }

        //
        // POST: /ManagePanel/EditCategoryPage
        [HttpPost]
        public ActionResult EditCategoryPage(Category model)
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
                        value = client.EditCategory(User.Identity.Name, model.RestaurantID, model.CategoryID, model.CategoryName, model.CategoryDescription, model.PriceOption, model.NonPriceOption, model.NonPriceOption2);
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    value = false;
                }

                if (value == false)
                {
                    ModelState.AddModelError("", "Edytowanie kategorii nie powiodło się.");
                }
                else
                {
                    return RedirectToAction("EditMenuPage", "ManagePanel", new { id = model.RestaurantID });
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /ManagePanel/DeleteCategory
        public ActionResult DeleteCategory(int id, int cat)
        {
            if (id > 0 && cat >0)
            {
                ViewData["id"] = id;
                bool value = false;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                      //  value = client.DeleteCategory(User.Identity.Name, id, cat);
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    value = false;
                }

                if (value == false)
                {
                    ModelState.AddModelError("", "Usuwanie kategorii nie powiodło się.");
                }
                else
                {
                    return RedirectToAction("EditMenuPage", new { id = id});
                }
            }
            return RedirectToAction("Restaurant");
        }

        //
        // GET: /ManagePanel/AddProduct
        public ActionResult AddProduct(int id)
        {
            if (id > 0)
            {
                ViewData["id"] = id;

                List<Category> value = null;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = new List<Category>(client.GetCategories(User.Identity.Name, id));
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
                    List<SelectListItem> categories = new List<SelectListItem>();
                    categories.Add(new SelectListItem { Text = "", Value = "" });
                    foreach (Category item in value)
                    {
                        categories.Add(new SelectListItem { Text = item.CategoryName, Value = item.CategoryID.ToString() });
                    }

                    ViewData["categories"] = categories;

                    AddProductModel model = new AddProductModel();
                    model.RestaurantID = id;

                    return View(model);
                }
            }
            return RedirectToAction("Restaurant");
        }

        //
        // POST: /ManagePanel/AddProduct
        [HttpPost]
        public ActionResult AddProduct(AddProductModel model)
        {
            ViewData["id"] = model.RestaurantID;
            if (ModelState.IsValid)
            {
                CustomRoleProvider role = (CustomRoleProvider)System.Web.Security.Roles.Providers["CustomRoleProvider"];
                if (role.IsUserInRole(User.Identity.Name, "Menadżer"))
                {
                    bool value = false;
                    try
                    {
                        ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                        using (client)
                        {
                            value = client.AddProduct(model.RestaurantID, model.Category, model.ProductName, model.ProductDescription, model.Price, User.Identity.Name);
                        }
                        client.Close();
                    }
                    catch (Exception e)
                    {
                        value = false;
                    }

                    if (value == false)
                    {
                        ModelState.AddModelError("", "Dodawanie produktu nie powiodło się.");
                    }
                    else
                    {
                        return RedirectToAction("EditMenuPage", "ManagePanel", new { id = model.RestaurantID });
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Nie jesteś zalogowany jako menadżer.");
                }
            }

            List<Category> value2 = null;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value2 = new List<Category>(client.GetCategories(User.Identity.Name, model.RestaurantID));
                }
                client.Close();
            }
            catch (Exception e)
            {
                value2 = null;
            }

            if (value2 == null)
            {
                ModelState.AddModelError("", "Pobieranie danych o restauracji nie powiodło się.");
            }
            else
            {
                List<SelectListItem> categories = new List<SelectListItem>();
                categories.Add(new SelectListItem { Text = "", Value = "" });
                foreach (Category item in value2)
                {
                    categories.Add(new SelectListItem { Text = item.CategoryName, Value = item.CategoryID.ToString() });
                }

                ViewData["categories"] = categories;

              //  AddProductModel model = new AddProductModel();
             //   model.RestaurantID = id;

             //   return View(model);
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public ActionResult GetPrices(int id, string txt, int resid)
        {
            string prices = String.Empty;

            if (resid > 0)
            {
                ViewData["id"] = resid;

                Category value = null;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.GetCategory(User.Identity.Name, resid, id);
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
                    prices = "Wprowadz cenę kolejno dla: "+value.PriceOption+ " (np. ";
                    string[] tab = value.PriceOption.Split(',');
                    int j = 10;
                    for (int i = 1; i <= tab.Length; i++)
                    {
                        if(i==tab.Length)
                            prices += j.ToString() + ".00";
                        else
                            prices += j.ToString() + ".00|";
                        j += 5;
                    }
                    prices += ")";
                }
            }
          //  prices = "Wprowadz cenę kolejno dla ";

            //Cena (Wprowadz cene kolejno dla mała|srednia|duża np. (10.00|20.00|30.00) )

            //to zostawiam Ci dla przykladu, moze sie przyda
            //
            //IEnumerable<SelectListItem> selectList =
            //from c in listares
            //select new SelectListItem
            //{
            //    Selected = ( c.ResId == 0 ),
            //    Text = c.Name,
            //    Value = c.ResId.ToString()
            //};

            return Json(prices);
        }

        //id produktu
        //cat żeby wyśiwetlić kategorie w ddl
        //res żeby sprawdzić czy jest właścicielem
        //
        // GET: /ManagePanel/EditProduct
        public ActionResult EditProduct(int id, int cat, int res)
        {
            
            if (id > 0 && cat >-1 && res > 0)
            {
                ViewData["id"] = res;

                List<Category> value = null;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = new List<Category>(client.GetCategories(User.Identity.Name, res));
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
                    List<SelectListItem> categories = new List<SelectListItem>();
                 //   categories.Add(new SelectListItem { Text = "", Value = "" });
                    foreach (Category item in value)
                    {
                        categories.Add(new SelectListItem { Text = item.CategoryName, Value = item.CategoryID.ToString() });
                    }

                    ViewData["categories"] = categories;
                    /////////////////////////////////
                    Product value2 = null;
                    try
                    {
                        ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                        using (client)
                        {
                            value2 = client.GetProduct(User.Identity.Name, res, id);
                        }
                        client.Close();
                    }
                    catch (Exception e)
                    {
                        value2 = null;
                    }

                    if (value2 == null)
                    {
                        ModelState.AddModelError("", "Pobieranie danych o restauracji nie powiodło się.");
                    }
                    else
                    {
                        ProductModel model = new ProductModel();
                        model.isAvailable = value2.IsAvailable;
                        model.Category = value2.CategoryId;
                        model.Price = value2.Price;
                        model.ProductDescription = value2.ProductDescription;
                        model.ProductName = value2.ProductName;
                        model.RestaurantID = value2.RestaurantId;
                        model.ProductId = value2.ProductId;
                        return View(model);
                    }
                }
            }
            return RedirectToAction("Restaurant");
        }

        //
        // POST: /ManagePanel/EditProduct
        [HttpPost]
        public ActionResult EditProduct(ProductModel model)
        {
            ViewData["id"] = model.RestaurantID;
            if (ModelState.IsValid)
            {
                CustomRoleProvider role = (CustomRoleProvider)System.Web.Security.Roles.Providers["CustomRoleProvider"];
                if (role.IsUserInRole(User.Identity.Name, "Menadżer"))
                {
                    bool value = false;
                    try
                    {
                        ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                        using (client)
                        {
                            value = client.EditProduct(User.Identity.Name, model.RestaurantID, model.ProductId, model.Category, model.ProductName, model.ProductDescription, model.Price, model.isAvailable);
                        }
                        client.Close();
                    }
                    catch (Exception e)
                    {
                        value = false;
                    }

                    if (value == false)
                    {
                        ModelState.AddModelError("", "Edycja produktu nie powiodło się.");
                    }
                    else
                    {
                        return RedirectToAction("EditMenuPage", "ManagePanel", new { id = model.RestaurantID });
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Nie jesteś zalogowany jako menadżer.");
                }
            }

            List<Category> value2 = null;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value2 = new List<Category>(client.GetCategories(User.Identity.Name, model.RestaurantID));
                }
                client.Close();
            }
            catch (Exception e)
            {
                value2 = null;
            }

            if (value2 == null)
            {
                ModelState.AddModelError("", "Pobieranie danych o restauracji nie powiodło się.");
            }
            else
            {
                List<SelectListItem> categories = new List<SelectListItem>();
                categories.Add(new SelectListItem { Text = "", Value = "" });
                foreach (Category item in value2)
                {
                    categories.Add(new SelectListItem { Text = item.CategoryName, Value = item.CategoryID.ToString() });
                }

                ViewData["categories"] = categories;
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}
