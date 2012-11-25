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
using System.Threading;
using System.Globalization;
using AppLimit.CloudComputing.SharpBox;
using System.IO;
using AppLimit.CloudComputing.SharpBox.StorageProvider.DropBox;

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
        // GET: /ManagePanel/EditData
        //[Authorize]
        [CustomAuthorizeAttribute(Roles = "Menadżer")]
        public ActionResult EditData(string login)
        {
            System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("fr-FR");
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("fr-FR");
            if (Request.IsAuthenticated)
            {
                CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];
                CustomMembershipUser user = (CustomMembershipUser)customMemebership.GetUser(login, true);
                UserDataModel model = new UserDataModel();
                model.Login = user.Login;
                model.Email = user.Email;
                model.Name = user.Name;
                model.Surname = user.Surname;
                model.Address = user.Address;
                model.Town = user.Town;
                model.PostalCode = user.PostalCode;
                model.Country = user.Country;
                model.Birthdate = user.Birthdate;
                model.Sex = user.Sex;
                model.Telephone = user.Telephone;

                //ustawienie danych o płci
                List<SelectListItem> sex = new List<SelectListItem>();
                if (model.Sex.Equals("Mężczyzna"))
                {
                    sex.Add(new SelectListItem { Text = "Mężczyzna", Value = "Mężczyzna" });
                    sex.Add(new SelectListItem { Text = "Kobieta", Value = "Kobieta" });
                }
                else
                {
                    sex.Add(new SelectListItem { Text = "Kobieta", Value = "Kobieta" });
                    sex.Add(new SelectListItem { Text = "Mężczyzna", Value = "Mężczyzna" });
                }
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
                ViewData["Map"] = (IEnumerable<Town>)(new List<Town>());

                return View(model);
            }
            else
            {
                return RedirectToAction("LogOn", "Account");
            }
        }

        //
        // POST: /ManagePanel/EditData
        //[Authorize]
        [HttpPost]
        [CustomAuthorizeAttribute(Roles = "Menadżer")]
        public ActionResult EditData(UserDataModel model)
        {
            System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("fr-FR");
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("fr-FR");
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
                    if (value.Count == 1)//edytuj usera
                    {
                        CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];
                        CustomMembershipUser user = (CustomMembershipUser)customMemebership.GetUser(model.Login, true);
                        if (user != null)
                        {
                            user.Name = model.Name;
                            user.Surname = model.Surname;
                            user.Address = model.Address;
                            user.Town = model.Town;
                            user.PostalCode = model.PostalCode;
                            user.Country = model.Country;
                            user.Birthdate = model.Birthdate;
                            user.Sex = model.Sex;
                            user.Telephone = model.Telephone;

                            customMemebership.UpdateUser(user);

                            return RedirectToAction("Personnel", "ManagePanel");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Edycja danych nie powiodła się");
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
        // GET: /ManagePanel/EditPassword
        //[Authorize]
        [CustomAuthorizeAttribute(Roles = "Menadżer")]
        public ActionResult EditPassword(string login)
        {
            if (Request.IsAuthenticated)
            {
                EmployeePasswordModel model = new EmployeePasswordModel();
                model.EmployeeLogin = login;
                return View(model);
            }
            else
            {
                return RedirectToAction("LogOn", "Account");
            }
        }

        //
        // POST: /ManagePanel/EditPassword
        //[Authorize]
        [HttpPost]
        [CustomAuthorizeAttribute(Roles = "Menadżer")]
        public ActionResult EditPassword(EmployeePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                bool changePasswordSucceeded;
                try
                {
                    CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];
                    CustomMembershipUser currentUser = (CustomMembershipUser)customMemebership.GetUser(model.EmployeeLogin, false);
                    changePasswordSucceeded = customMemebership.ChangeEmployeePassword(currentUser.Login, model.Password);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }

                if (changePasswordSucceeded)
                {
                    //return RedirectToAction("ChangePasswordSuccess");
                    return RedirectToAction("Personnel", "ManagePanel");
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
                            //musi być to parsowanie bo mvc to gówno
                            decimal price;
                            NumberStyles style = NumberStyles.AllowDecimalPoint;
                            price = Decimal.Parse(model.DeliveryPrice, style);

                            MembershipCreateStatus createStatus;
                            CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];
                            bool user = customMemebership.CreateRestaurant(model.Login, model.Email, model.Password, model.Question, model.Answer, model.Name, model.DisplayName, model.Address, value[0].ID, model.Country, model.Telephone, model.Nip, model.Regon, model.DeliveryTime, User.Identity.Name, out createStatus, price);

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
                model.DeliveryPrice = rest.DeliveryPrice;
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
                                value2 = client.EditRestaurant(model.Name, model.DisplayName, model.Address, value[0].ID, model.Country, model.Telephone, model.Nip, model.Regon, model.DeliveryTime, model.IsEnabled, User.Identity.Name, model.Id, model.DeliveryPrice);
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
                MainPageModel nowy = new MainPageModel();
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.GetMainPage(User.Identity.Name, id);
                    }
                    client.Close();
                    #region DropBox Connection
                    try
                    {
                        // Creating the cloudstorage object 
                        CloudStorage dropBoxStorage = new CloudStorage();

                        // get the configuration for dropbox 
                        var dropBoxConfig = CloudStorage.GetCloudConfigurationEasy(nSupportedCloudConfigurations.DropBox);

                        // declare an access token
                        ICloudStorageAccessToken accessToken = null;

                        // load a valid security token from file
                        string path = Server.MapPath(Url.Content("~/Content/token.txt"));

                        //using (FileStream fs = System.IO.File.Open("C:\\dropboxtoken.txt", FileMode.Open, FileAccess.Read, FileShare.None))
                        using (FileStream fs = System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None))
                        {
                            accessToken = dropBoxStorage.DeserializeSecurityToken(fs);
                        }

                        // open the connection 
                        var storageToken = dropBoxStorage.Open(dropBoxConfig, accessToken);


                        var mainFolder = dropBoxStorage.GetFolder("/Public/images/");

                        //check if folder exists in child collection
                        #region check childs


                        //lista folderow
                        List<String> folders = new List<String>();

                        // enumerate all child (folder and files) 
                        foreach (var fof in mainFolder)
                        {
                            // check if we have a directory 
                            Boolean bIsDirectory = fof is ICloudDirectoryEntry;

                            if (bIsDirectory)
                            {
                                //get folder names
                                folders.Add(fof.Name);
                            }
                        }
                        bool isFolder = false;
                        foreach (var fof in folders)
                        {
                            if (fof.Equals(id.ToString()))
                            {
                                isFolder = true;
                            }
                        }
                        #endregion


                        if (!isFolder)
                        {
                            // get a specific directory in the cloud storage eg. "/images/1"
                            var newdirFolder = dropBoxStorage.CreateFolder("/Public/images/" + id.ToString());
                            var newdirFolder2 = dropBoxStorage.CreateFolder("/Public/images/" + id.ToString() + "/logo");
                        }
                        else
                        {
                            var newdirFolder2 = dropBoxStorage.CreateFolder("/Public/images/" + id.ToString() + "/logo");
                        }

                        var resFolder = dropBoxStorage.GetFolder("/Public/images/" + id.ToString() + "/logo");


                        ICloudFileSystemEntry fse;

                        Uri logo;
                        if (resFolder.Count != 0)
                        {
                            // enumerate all child (folder and files) 
                            foreach (var fof in resFolder)
                            {
                                // check if we have a directory 
                                Boolean bIsDirectory = fof is ICloudDirectoryEntry;

                                fse = dropBoxStorage.GetFileSystemObject(fof.Name, resFolder);
                                if (!bIsDirectory)
                                {
                                    //pobiera liste linkow do plikow w katalogu rodzica
                                    logo = DropBoxStorageProviderTools.GetPublicObjectUrl(accessToken, fse);
                                    nowy.Foto = "<img src=" + logo.AbsoluteUri + " style=\"height:200px;display: block;margin-left: auto; margin-right: auto;\" \\>";
                                }
                            }
                        }
                        else
                        {
                            //default logo
                            var defaultLogoFolder = dropBoxStorage.GetFolder("/Public/images/defaultlogo");

                            ICloudFileSystemEntry fse1;

                            // lista linkow uri
                            Uri defaultLogo;
                            String htmlImgLogo = string.Empty;

                            // enumerate all child (folder and files) 
                            foreach (var fof in defaultLogoFolder)
                            {
                                // check if we have a directory 
                                Boolean bIsDirectory = fof is ICloudDirectoryEntry;

                                fse1 = dropBoxStorage.GetFileSystemObject(fof.Name, defaultLogoFolder);
                                if (!bIsDirectory)
                                {
                                    //pobiera liste linkow do plikow w katalogu rodzica
                                    defaultLogo = DropBoxStorageProviderTools.GetPublicObjectUrl(accessToken, fse1);
                                    htmlImgLogo = "<img style=\"backgroud: url(" + defaultLogo.AbsoluteUri + "); background-size: auto 100%;  \" \\>";
                                    nowy.Foto = "<img src=" + defaultLogo.AbsoluteUri + " style=\"height:200px;display: block;margin-left: auto; margin-right: auto;\" \\>";
                                    
                                }
                            }
                            ViewData["logo"] = htmlImgLogo;
                        }

                        

                        dropBoxStorage.Close();



                    }
                    catch (AppLimit.CloudComputing.SharpBox.Exceptions.SharpBoxException ex)
                    {
                        ;
                    }
                    #endregion
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
                    
                    nowy.Description = value.Description;
                    //nowy.Foto = value.Foto;
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
                MainPageModel nowy = new MainPageModel();
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.GetMainPage(User.Identity.Name, id);
                    }
                    client.Close();
                    #region DropBox Connection
                    try
                    {
                        // Creating the cloudstorage object 
                        CloudStorage dropBoxStorage = new CloudStorage();

                        // get the configuration for dropbox 
                        var dropBoxConfig = CloudStorage.GetCloudConfigurationEasy(nSupportedCloudConfigurations.DropBox);

                        // declare an access token
                        ICloudStorageAccessToken accessToken = null;

                        // load a valid security token from file
                        string path = Server.MapPath(Url.Content("~/Content/token.txt"));

                        //using (FileStream fs = System.IO.File.Open("C:\\dropboxtoken.txt", FileMode.Open, FileAccess.Read, FileShare.None))
                        using (FileStream fs = System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None))
                        {
                            accessToken = dropBoxStorage.DeserializeSecurityToken(fs);
                        }

                        // open the connection 
                        var storageToken = dropBoxStorage.Open(dropBoxConfig, accessToken);


                        var mainFolder = dropBoxStorage.GetFolder("/Public/images/");

                        //check if folder exists in child collection
                        #region check childs


                        //lista folderow
                        List<String> folders = new List<String>();

                        // enumerate all child (folder and files) 
                        foreach (var fof in mainFolder)
                        {
                            // check if we have a directory 
                            Boolean bIsDirectory = fof is ICloudDirectoryEntry;

                            if (bIsDirectory)
                            {
                                //get folder names
                                folders.Add(fof.Name);
                            }
                        }
                        bool isFolder = false;
                        foreach (var fof in folders)
                        {
                            if (fof.Equals(id.ToString()))
                            {
                                isFolder = true;
                            }
                        }
                        #endregion


                        if (!isFolder)
                        {
                            // get a specific directory in the cloud storage eg. "/images/1"
                            var newdirFolder = dropBoxStorage.CreateFolder("/Public/images/" + id.ToString());
                            var newdirFolder2 = dropBoxStorage.CreateFolder("/Public/images/" + id.ToString() + "/logo");
                        }
                        else
                        {
                            var newdirFolder2 = dropBoxStorage.CreateFolder("/Public/images/" + id.ToString() + "/logo");
                        }

                        var resFolder = dropBoxStorage.GetFolder("/Public/images/" + id.ToString() + "/logo");


                        ICloudFileSystemEntry fse;

                        // lista linkow uri
                        Uri logo;
                        if (resFolder.Count != 0)
                        {
                            // enumerate all child (folder and files) 
                            foreach (var fof in resFolder)
                            {
                                // check if we have a directory 
                                Boolean bIsDirectory = fof is ICloudDirectoryEntry;

                                fse = dropBoxStorage.GetFileSystemObject(fof.Name, resFolder);
                                if (!bIsDirectory)
                                {
                                    //pobiera liste linkow do plikow w katalogu rodzica
                                    //logo = DropBoxStorageProviderTools.GetPublicObjectUrl(accessToken, fse);
                                    nowy.File = new images();
                                    nowy.File.link = DropBoxStorageProviderTools.GetPublicObjectUrl(accessToken, fse);
                                    nowy.File.name = fof.Name;
                                    nowy.Foto = "<img src=" + nowy.File.link.AbsoluteUri + " style=\"height:200px;display: block;margin-left: auto; margin-right: auto;\" \\>";
                                }
                            }
                        }
                        else
                        {
                            //default logo
                            var defaultLogoFolder = dropBoxStorage.GetFolder("/Public/images/defaultlogo");

                            ICloudFileSystemEntry fse1;

                            // lista linkow uri
                            Uri defaultLogo;
                            String htmlImgLogo = string.Empty;

                            // enumerate all child (folder and files) 
                            foreach (var fof in defaultLogoFolder)
                            {
                                // check if we have a directory 
                                Boolean bIsDirectory = fof is ICloudDirectoryEntry;

                                fse1 = dropBoxStorage.GetFileSystemObject(fof.Name, defaultLogoFolder);
                                if (!bIsDirectory)
                                {
                                    //pobiera liste linkow do plikow w katalogu rodzica
                                    defaultLogo = DropBoxStorageProviderTools.GetPublicObjectUrl(accessToken, fse1);
                                    htmlImgLogo = "<img style=\"backgroud: url(" + defaultLogo.AbsoluteUri + "); background-size: auto 100%;  \" \\>";
                                    //nowy.Foto = "<img src=" + defaultLogo.AbsoluteUri + " style=\"backgroud: url(" + defaultLogo.AbsoluteUri + "); background-size: auto 100%; ><\\span>"; 
                                    nowy.File = new images();
                                    nowy.File.link = DropBoxStorageProviderTools.GetPublicObjectUrl(accessToken, fse1);
                                    nowy.File.name = fof.Name;
                                    nowy.Foto = "<img src=" + nowy.File.link.AbsoluteUri + " style=\"height:200px;display: block;margin-left: auto; margin-right: auto;\" \\>";
                                
                                    //nowy.Foto = "<img src=" + defaultLogo.AbsoluteUri + " style=\"height:200px;display: block;margin-left: auto; margin-right: auto;\" \\>";

                                }
                            }
                            //ViewData["logo"] = htmlImgLogo;
                        }



                        dropBoxStorage.Close();



                    }
                    catch (AppLimit.CloudComputing.SharpBox.Exceptions.SharpBoxException ex)
                    {
                        Response.Write(ex.ToString());
                    }
                    #endregion
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
                    
                    nowy.Description = value.Description;
                    //nowy.Foto = value.Foto;
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
            //MainPageModel nowy = new MainPageModel();
            if (ModelState.IsValid)
            {
                bool value = false;
                model.Foto = string.Empty;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.EditMainPage(model.Description, model.Foto, model.SpecialOffers, model.RestaurantID, User.Identity.Name);
                    }
                    client.Close();
                    #region DropBox Connection
                    try
                    {
                        // Creating the cloudstorage object 
                        CloudStorage dropBoxStorage = new CloudStorage();

                        // get the configuration for dropbox 
                        var dropBoxConfig = CloudStorage.GetCloudConfigurationEasy(nSupportedCloudConfigurations.DropBox);

                        // declare an access token
                        ICloudStorageAccessToken accessToken = null;

                        // load a valid security token from file
                        string path = Server.MapPath(Url.Content("~/Content/token.txt"));

                        //using (FileStream fs = System.IO.File.Open("C:\\dropboxtoken.txt", FileMode.Open, FileAccess.Read, FileShare.None))
                        using (FileStream fs = System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None))
                        {
                            accessToken = dropBoxStorage.DeserializeSecurityToken(fs);
                        }

                        // open the connection 
                        var storageToken = dropBoxStorage.Open(dropBoxConfig, accessToken);


                        var mainFolder = dropBoxStorage.GetFolder("/Public/images/");

                        //check if folder exists in child collection
                        #region check childs


                        //lista folderow
                        List<String> folders = new List<String>();

                        // enumerate all child (folder and files) 
                        foreach (var fof in mainFolder)
                        {
                            // check if we have a directory 
                            Boolean bIsDirectory = fof is ICloudDirectoryEntry;

                            if (bIsDirectory)
                            {
                                //get folder names
                                folders.Add(fof.Name);
                            }
                        }
                        bool isFolder = false;
                        foreach (var fof in folders)
                        {
                            if (fof.Equals(model.RestaurantID.ToString()))
                            {
                                isFolder = true;
                            }
                        }
                        #endregion


                        if (!isFolder)
                        {
                            // get a specific directory in the cloud storage eg. "/images/1"
                            var newdirFolder = dropBoxStorage.CreateFolder("/Public/images/" + model.RestaurantID.ToString());
                            var newdirFolder2 = dropBoxStorage.CreateFolder("/Public/images/" + model.RestaurantID.ToString() + "/logo");
                        }
                        else
                        {
                            var newdirFolder2 = dropBoxStorage.CreateFolder("/Public/images/" + model.RestaurantID.ToString() + "/logo");
                        }

                        var resFolder = dropBoxStorage.GetFolder("/Public/images/" + model.RestaurantID.ToString() + "/logo");


                        ICloudFileSystemEntry fse;

                        // lista linkow uri
                        Uri logo;
                        if (resFolder.Count != 0)
                        {
                            // enumerate all child (folder and files) 
                            foreach (var fof in resFolder)
                            {
                                // check if we have a directory 
                                Boolean bIsDirectory = fof is ICloudDirectoryEntry;

                                fse = dropBoxStorage.GetFileSystemObject(fof.Name, resFolder);
                                if (!bIsDirectory)
                                {
                                    //pobiera liste linkow do plikow w katalogu rodzica
                                    //logo = DropBoxStorageProviderTools.GetPublicObjectUrl(accessToken, fse);
                                    model.File = new images();
                                    model.File.link = DropBoxStorageProviderTools.GetPublicObjectUrl(accessToken, fse);
                                    model.File.name = fof.Name;
                                    model.Foto = "<img src=" + model.File.link.AbsoluteUri + " style=\"height:200px;display: block;margin-left: auto; margin-right: auto;\" \\>";
                                }
                            }
                        }
                        else
                        {
                            //default logo
                            var defaultLogoFolder = dropBoxStorage.GetFolder("/Public/images/defaultlogo");

                            ICloudFileSystemEntry fse1;

                            // lista linkow uri
                            Uri defaultLogo;
                            String htmlImgLogo = string.Empty;

                            // enumerate all child (folder and files) 
                            foreach (var fof in defaultLogoFolder)
                            {
                                // check if we have a directory 
                                Boolean bIsDirectory = fof is ICloudDirectoryEntry;

                                fse1 = dropBoxStorage.GetFileSystemObject(fof.Name, defaultLogoFolder);
                                if (!bIsDirectory)
                                {
                                    //pobiera liste linkow do plikow w katalogu rodzica
                                    defaultLogo = DropBoxStorageProviderTools.GetPublicObjectUrl(accessToken, fse1);
                                    htmlImgLogo = "<img style=\"backgroud: url(" + defaultLogo.AbsoluteUri + "); background-size: auto 100%;  \" \\>";
                                    //nowy.Foto = "<img src=" + defaultLogo.AbsoluteUri + " style=\"backgroud: url(" + defaultLogo.AbsoluteUri + "); background-size: auto 100%; ><\\span>"; 
                                    model.File = new images();
                                    model.File.link = DropBoxStorageProviderTools.GetPublicObjectUrl(accessToken, fse1);
                                    model.File.name = fof.Name;
                                    model.Foto = "<img src=" + model.File.link.AbsoluteUri + " style=\"height:200px;display: block;margin-left: auto; margin-right: auto;\" \\>";

                                    //nowy.Foto = "<img src=" + defaultLogo.AbsoluteUri + " style=\"height:200px;display: block;margin-left: auto; margin-right: auto;\" \\>";

                                }
                            }
                            //ViewData["logo"] = htmlImgLogo;
                        }



                        dropBoxStorage.Close();



                    }
                    catch (AppLimit.CloudComputing.SharpBox.Exceptions.SharpBoxException ex)
                    {
                        Response.Write(ex.ToString());
                    }
                    #endregion
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
            MainPageModel model = new MainPageModel();
            ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
            if (client.IsUserInRole(User.Identity.Name, "Menadżer") && (client.IsRestaurantOwner(User.Identity.Name, id)))
            {
                #region DropBox Connection
                try
                {
                    // Creating the cloudstorage object 
                    CloudStorage dropBoxStorage = new CloudStorage();

                    // get the configuration for dropbox 
                    var dropBoxConfig = CloudStorage.GetCloudConfigurationEasy(nSupportedCloudConfigurations.DropBox);

                    // declare an access token
                    ICloudStorageAccessToken accessToken = null;

                    // load a valid security token from file
                    string path = Server.MapPath(Url.Content("~/Content/token.txt"));

                    //using (FileStream fs = System.IO.File.Open("C:\\dropboxtoken.txt", FileMode.Open, FileAccess.Read, FileShare.None))
                    using (FileStream fs = System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        accessToken = dropBoxStorage.DeserializeSecurityToken(fs);
                    }

                    // open the connection 
                    var storageToken = dropBoxStorage.Open(dropBoxConfig, accessToken);


                    var mainFolder = dropBoxStorage.GetFolder("/Public/images/");

                    //check if folder exists in child collection
                    #region check childs


                    //lista folderow
                    List<String> folders = new List<String>();

                    // enumerate all child (folder and files) 
                    foreach (var fof in mainFolder)
                    {
                        // check if we have a directory 
                        Boolean bIsDirectory = fof is ICloudDirectoryEntry;

                        if (bIsDirectory)
                        {
                            //get folder names
                            folders.Add(fof.Name);
                        }
                    }
                    bool isFolder = false;
                    foreach (var fof in folders)
                    {
                        if (fof.Equals(id.ToString()))
                        {
                            isFolder = true;
                        }
                    }
                    #endregion


                    if (!isFolder)
                    {
                        // get a specific directory in the cloud storage eg. "/images/1"
                        var newdirFolder = dropBoxStorage.CreateFolder("/Public/images/" + id.ToString());
                    }

                    var resFolder = dropBoxStorage.GetFolder("/Public/images/" + id.ToString());


                    ICloudFileSystemEntry fse;

                    // lista linkow uri
                    List<Uri> uris = new List<Uri>();


                    //model.FileName = new List<string>();
                    //model.links = new List<Uri>();
                    model.Files = new List<images>();
                    images img = null;
                    bool ifFolderHasFiles = false;
                    // enumerate all child (folder and files) 
                    foreach (var fof in resFolder)
                    {
                        // check if we have a directory 
                        Boolean bIsDirectory = fof is ICloudDirectoryEntry;

                        fse = dropBoxStorage.GetFileSystemObject(fof.Name, resFolder);
                        if (!bIsDirectory)
                        {
                            ifFolderHasFiles = true;
                            img = new images();
                            img.name = fof.Name;
                            img.link = DropBoxStorageProviderTools.GetPublicObjectUrl(accessToken, fse);
                            model.Files.Add(img);
                        }
                    }
                    if (!ifFolderHasFiles)
                    {
                        ViewData["alert"] = "Galeria nie zawiera żadnych zdjęć.";
                    }

                    dropBoxStorage.Close();

                    //ViewData["imagesuris"] = uris;

                }
                catch (AppLimit.CloudComputing.SharpBox.Exceptions.SharpBoxException ex)
                {
                    ;
                }
                #endregion
                ViewData["id"] = id;
                //MainPageModel model = new MainPageModel();
                model.RestaurantID = id;
                return View(model);
            }
            else 
            {
                return RedirectToAction("Restaurant");
            }
            
        }


        /// <summary>
        /// File Upload
        /// </summary>
        /// <param name="fileUpload"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Gallery(HttpPostedFileBase fileUpload, int id)
        {
            ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
            if (client.IsUserInRole(User.Identity.Name, "Menadżer") && ( client.IsRestaurantOwner(User.Identity.Name, id) ))
            {
                if (fileUpload != null)
                {
                    if (( fileUpload.ContentType.Equals("image/jpeg") || fileUpload.ContentType.Equals("image/png") ))
                    {
                        MainPageModel model = new MainPageModel();

                        #region DropBox Connection

                        try
                        {
                            // Creating the cloudstorage object 
                            CloudStorage dropBoxStorage = new CloudStorage();

                            // get the configuration for dropbox 
                            var dropBoxConfig = CloudStorage.GetCloudConfigurationEasy(nSupportedCloudConfigurations.DropBox);

                            // declare an access token
                            ICloudStorageAccessToken accessToken = null;

                            // load a valid security token from file
                            string path = Server.MapPath(Url.Content("~/Content/token.txt"));

                            //using (FileStream fs = System.IO.File.Open("C:\\dropboxtoken.txt", FileMode.Open, FileAccess.Read, FileShare.None))
                            using (FileStream fs = System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None))
                            {
                                accessToken = dropBoxStorage.DeserializeSecurityToken(fs);
                            }

                            // open the connection 
                            var storageToken = dropBoxStorage.Open(dropBoxConfig, accessToken);

                            // get a specific directory in the cloud storage eg. "/images/1"
                            var mainFolder = dropBoxStorage.GetFolder("/Public/images/");

                            //check if folder exists in child collection
                            #region check childs

                            ICloudFileSystemEntry fse;

                            // lista linkow uri
                            //List<Uri> uris = new List<Uri>();

                            //lista folderow
                            List<String> folders = new List<String>();

                            // enumerate all child (folder and files) 
                            foreach (var fof in mainFolder)
                            {
                                // check if we have a directory 
                                Boolean bIsDirectory = fof is ICloudDirectoryEntry;


                                //fse = dropBoxStorage.GetFileSystemObject(fof.Name, publicFolder);
                                //if (!bIsDirectory)
                                //{
                                //    //pobiera liste linkow do plikow w katalogu rodzica
                                //    uris.Add(DropBoxStorageProviderTools.GetPublicObjectUrl(accessToken, fse));
                                //}

                                if (bIsDirectory)
                                {
                                    //get folder names
                                    folders.Add(fof.Name);
                                }
                            }
                            bool isFolder = false;
                            foreach (var fof in folders)
                            {
                                if (fof.Equals(id.ToString()))
                                {
                                    isFolder = true;
                                }
                            }
                            #endregion


                            if (isFolder)
                            {
                                //var newdirFolder2 = dropBoxStorage.CreateFolder("/Public/images/" + id.ToString());
                                var resFolder = dropBoxStorage.GetFolder("/Public/images/" + id.ToString());

                                if (fileUpload != null)
                                {
                                    var fileName = Path.GetFileName(fileUpload.FileName);
                                    var filepath = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                                    fileUpload.SaveAs(filepath);
                                    String srcFile = Environment.ExpandEnvironmentVariables(filepath);
                                    dropBoxStorage.UploadFile(srcFile, resFolder);
                                    //trzeba ten plik wyjebac odrazu po zapisaniu go na dropa
                                    System.IO.File.Delete(filepath);
                                }
                            }
                            else
                            {
                                // Create a directory 
                                var newdirFolder = dropBoxStorage.CreateFolder("/Public/images/" + id.ToString());

                                var resFolder = dropBoxStorage.GetFolder("/Public/images/" + id.ToString());
                                if (fileUpload != null)
                                {
                                    var fileName = Path.GetFileName(fileUpload.FileName);
                                    var filepath = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                                    fileUpload.SaveAs(filepath);
                                    String srcFile = Environment.ExpandEnvironmentVariables(filepath);
                                    dropBoxStorage.UploadFile(srcFile, resFolder);
                                    //trzeba ten plik wyjebac odrazu po zapisaniu go na dropa
                                    System.IO.File.Delete(filepath);
                                }
                            }






                            #region get pictures

                            //ICloudFileSystemEntry fse;
                            var resFolderPub = dropBoxStorage.GetFolder("/Public/images/" + id.ToString());
                            // lista linkow uri
                            //List<Uri> uris = new List<Uri>();


                            model.Files = new List<images>();
                            images img = null;
                            // enumerate all child (folder and files) 
                            foreach (var fof in resFolderPub)
                            {
                                // check if we have a directory 
                                Boolean bIsDirectory = fof is ICloudDirectoryEntry;

                                fse = dropBoxStorage.GetFileSystemObject(fof.Name, resFolderPub);
                                if (!bIsDirectory)
                                {
                                    img = new images();
                                    img.name = fof.Name;
                                    img.link = DropBoxStorageProviderTools.GetPublicObjectUrl(accessToken, fse);
                                    model.Files.Add(img);
                                }
                            }


                            #endregion


                            dropBoxStorage.Close();
                            //ViewData["imagesuris"] = uris;

                        }
                        catch (AppLimit.CloudComputing.SharpBox.Exceptions.SharpBoxException ex)
                        {
                            Response.Write(ex.ToString());
                        }


                        #endregion
                        //return RedirectToAction("EditMainPage", new { id = id });
                        
                        model.RestaurantID = id;
                        ViewData["id"] = id;
                        return View(model);
                    }

                    else
                    {
                        //zly format zdjecia
                        ViewData["alert"] = "Akceptowalne formaty zdjęcia to jpeg i png.";
                        ViewData["id"] = id;
                        return View();
                    }
                }
                else
                {
                    MainPageModel model = new MainPageModel();
                    ViewData["alert"] = "Wybierz plik i wciśnij \"Prześlij\"";
                    #region DropBox Connection
                    try
                    {
                        // Creating the cloudstorage object 
                        CloudStorage dropBoxStorage = new CloudStorage();

                        // get the configuration for dropbox 
                        var dropBoxConfig = CloudStorage.GetCloudConfigurationEasy(nSupportedCloudConfigurations.DropBox);

                        // declare an access token
                        ICloudStorageAccessToken accessToken = null;

                        // load a valid security token from file
                        string path = Server.MapPath(Url.Content("~/Content/token.txt"));

                        //using (FileStream fs = System.IO.File.Open("C:\\dropboxtoken.txt", FileMode.Open, FileAccess.Read, FileShare.None))
                        using (FileStream fs = System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None))
                        {
                            accessToken = dropBoxStorage.DeserializeSecurityToken(fs);
                        }

                        // open the connection 
                        var storageToken = dropBoxStorage.Open(dropBoxConfig, accessToken);


                        var mainFolder = dropBoxStorage.GetFolder("/Public/images/");

                        //check if folder exists in child collection
                        #region check childs


                        //lista folderow
                        List<String> folders = new List<String>();

                        // enumerate all child (folder and files) 
                        foreach (var fof in mainFolder)
                        {
                            // check if we have a directory 
                            Boolean bIsDirectory = fof is ICloudDirectoryEntry;

                            if (bIsDirectory)
                            {
                                //get folder names
                                folders.Add(fof.Name);
                            }
                        }
                        bool isFolder = false;
                        foreach (var fof in folders)
                        {
                            if (fof.Equals(id.ToString()))
                            {
                                isFolder = true;
                            }
                        }
                        #endregion


                        if (!isFolder)
                        {
                            // get a specific directory in the cloud storage eg. "/images/1"
                            var newdirFolder = dropBoxStorage.CreateFolder("/Public/images/" + id.ToString());
                        }

                        var resFolder = dropBoxStorage.GetFolder("/Public/images/" + id.ToString());


                        ICloudFileSystemEntry fse;

                        // lista linkow uri
                        List<Uri> uris = new List<Uri>();


                        //model.FileName = new List<string>();
                        //model.links = new List<Uri>();
                        model.Files = new List<images>();
                        images img = null;
                        // enumerate all child (folder and files) 
                        foreach (var fof in resFolder)
                        {
                            // check if we have a directory 
                            Boolean bIsDirectory = fof is ICloudDirectoryEntry;

                            fse = dropBoxStorage.GetFileSystemObject(fof.Name, resFolder);
                            if (!bIsDirectory)
                            {
                                img = new images();
                                img.name = fof.Name;
                                img.link = DropBoxStorageProviderTools.GetPublicObjectUrl(accessToken, fse);
                                model.Files.Add(img);
                            }
                        }


                        dropBoxStorage.Close();

                        //ViewData["imagesuris"] = uris;

                    }
                    catch (AppLimit.CloudComputing.SharpBox.Exceptions.SharpBoxException ex)
                    {
                        ;
                    }
                    #endregion
                    ViewData["id"] = id;
                    
                    model.RestaurantID = id;
                    return View(model);
                }
            }
            else
            {
                return RedirectToAction("Restaurant");
            }
            //ViewData["id"] = id;

            
        }

        /// <summary>
        /// File delete - get request from javascript function
        /// </summary>
        /// <param name="plik"></param>
        /// <param name="resid"></param>
        /// <returns>Redirect to Gallery</returns>
        public void FileDelete(string plik, int resid)
        {
            ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
            if (client.IsUserInRole(User.Identity.Name, "Menadżer") && ( client.IsRestaurantOwner(User.Identity.Name, resid) ))
            {
                #region DropBox Connection
                try
                {
                    // Creating the cloudstorage object 
                    CloudStorage dropBoxStorage = new CloudStorage();

                    // get the configuration for dropbox 
                    var dropBoxConfig = CloudStorage.GetCloudConfigurationEasy(nSupportedCloudConfigurations.DropBox);

                    // declare an access token
                    ICloudStorageAccessToken accessToken = null;

                    // load a valid security token from file
                    string path = Server.MapPath(Url.Content("~/Content/token.txt"));

                    //using (FileStream fs = System.IO.File.Open("C:\\dropboxtoken.txt", FileMode.Open, FileAccess.Read, FileShare.None))
                    using (FileStream fs = System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        accessToken = dropBoxStorage.DeserializeSecurityToken(fs);
                    }

                    // open the connection 
                    var storageToken = dropBoxStorage.Open(dropBoxConfig, accessToken);

                    // Delete a file 
                    dropBoxStorage.DeleteFileSystemEntry("/Public/images/" + resid.ToString() + "/" + plik);
                    dropBoxStorage.Close();
                }
                catch (AppLimit.CloudComputing.SharpBox.Exceptions.SharpBoxException ex)
                {
                    ;
                }
                #endregion
                //return Json(new { redirectToUrl = Url.Action("Gallery", "ManagePanel", new { id = (int)resid }) });
                //return RedirectToAction("RedirectToGallery", new { id = (int)resid });
            }
            else
            {
                RedirectToAction("Restaurant");
            }
        }


        public ActionResult RedirectToGallery(int id)
        {
            return RedirectToAction("Gallery", new { id = (int)id });
        }


        /// <summary>
        /// GET - upload logo View
        /// </summary>
        /// <param name="id"></param>
        /// <returns>View</returns>
        public ActionResult UploadLogo(int id)
        {
            ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
            if (client.IsUserInRole(User.Identity.Name, "Menadżer") && ( client.IsRestaurantOwner(User.Identity.Name, id) ))
            {
                ViewData["id"] = id;
                return View();
            }
            else
            {
                return RedirectToAction("Restaurant");
            }
        }

        /// <summary>
        /// POST - Obsluguje upload logo dla restauracji
        /// </summary>
        /// <param name="logofile"></param>
        /// <param name="id"></param>
        /// <returns>View</returns>
        [HttpPost]
        public ActionResult UploadLogo(HttpPostedFileBase logofile, int id)
        {
            ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
            if (client.IsUserInRole(User.Identity.Name, "Menadżer") && ( client.IsRestaurantOwner(User.Identity.Name, id) ))
            {
                if (logofile != null)
                {
                    if (logofile.ContentType.Equals("image/jpeg") || logofile.ContentType.Equals("image/png"))
                    {
                        #region DropBox Connection

                        try
                        {
                            // Creating the cloudstorage object 
                            CloudStorage dropBoxStorage = new CloudStorage();

                            // get the configuration for dropbox 
                            var dropBoxConfig = CloudStorage.GetCloudConfigurationEasy(nSupportedCloudConfigurations.DropBox);

                            // declare an access token
                            ICloudStorageAccessToken accessToken = null;

                            // load a valid security token from file
                            string path = Server.MapPath(Url.Content("~/Content/token.txt"));

                            //using (FileStream fs = System.IO.File.Open("C:\\dropboxtoken.txt", FileMode.Open, FileAccess.Read, FileShare.None))
                            using (FileStream fs = System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None))
                            {
                                accessToken = dropBoxStorage.DeserializeSecurityToken(fs);
                            }

                            // open the connection 
                            var storageToken = dropBoxStorage.Open(dropBoxConfig, accessToken);

                            // get a specific directory in the cloud storage eg. "/images/1"
                            var mainFolder = dropBoxStorage.GetFolder("/Public/images/");

                            //check if folder exists in child collection
                            #region check childs

                            //ICloudFileSystemEntry fse;

                            // lista linkow uri
                            List<Uri> uris = new List<Uri>();

                            //lista folderow
                            List<String> folders = new List<String>();

                            // enumerate all child (folder and files) 
                            foreach (var fof in mainFolder)
                            {
                                // check if we have a directory 
                                Boolean bIsDirectory = fof is ICloudDirectoryEntry;


                                //fse = dropBoxStorage.GetFileSystemObject(fof.Name, publicFolder);
                                //if (!bIsDirectory)
                                //{
                                //    //pobiera liste linkow do plikow w katalogu rodzica
                                //    uris.Add(DropBoxStorageProviderTools.GetPublicObjectUrl(accessToken, fse));
                                //}
                                if (bIsDirectory)
                                {
                                    //get folder names
                                    folders.Add(fof.Name);
                                }
                            }
                            bool isFolder = false;
                            foreach (var fof in folders)
                            {
                                if (fof.Equals(id.ToString()))
                                {
                                    isFolder = true;
                                }
                            }

                            if (isFolder)
                            {
                                var newdirFolder2 = dropBoxStorage.CreateFolder("/Public/images/" + id.ToString() + "/logo");
                                var resFolder = dropBoxStorage.GetFolder("/Public/images/" + id.ToString() + "/logo");

                                if (logofile != null)
                                {
                                    var fileName = Path.GetFileName(logofile.FileName);
                                    var filepath = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                                    logofile.SaveAs(filepath);
                                    String srcFile = Environment.ExpandEnvironmentVariables(filepath);
                                    dropBoxStorage.UploadFile(srcFile, resFolder);
                                    //trzeba ten plik wyjebac odrazu po zapisaniu go na dropa
                                    System.IO.File.Delete(filepath);
                                }
                            }
                            else
                            {
                                // Create a directory 
                                var newdirFolder = dropBoxStorage.CreateFolder("/Public/images/" + id.ToString());
                                var newdirFolder2 = dropBoxStorage.CreateFolder("/Public/images/" + id.ToString() + "/logo");
                                var resFolder = dropBoxStorage.GetFolder("/Public/images/" + id.ToString() + "/logo");
                                if (logofile != null)
                                {
                                    var fileName = Path.GetFileName(logofile.FileName);
                                    var filepath = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                                    logofile.SaveAs(filepath);
                                    String srcFile = Environment.ExpandEnvironmentVariables(filepath);
                                    dropBoxStorage.UploadFile(srcFile, resFolder);
                                    //trzeba ten plik wyjebac odrazu po zapisaniu go na dropa
                                    System.IO.File.Delete(filepath);
                                }
                            }


                            #endregion

                            //ICloudFileSystemEntry fse;




                            //#region get pictures

                            //ICloudFileSystemEntry fse;

                            //// lista linkow uri
                            //List<Uri> uris = new List<Uri>();

                            //// enumerate all child (folder and files) 
                            //foreach (var fof in publicFolder)
                            //{
                            //    // check if we have a directory 
                            //    Boolean bIsDirectory = fof is ICloudDirectoryEntry;

                            //    fse = dropBoxStorage.GetFileSystemObject(fof.Name, publicFolder);
                            //    if (!bIsDirectory)
                            //    {
                            //        //pobiera liste linkow do plikow w katalogu rodzica
                            //        uris.Add(DropBoxStorageProviderTools.GetPublicObjectUrl(accessToken, fse));
                            //    }
                            //}

                            //#endregion


                            dropBoxStorage.Close();
                            //ViewData["imagesuris"] = uris;

                        }
                        catch (AppLimit.CloudComputing.SharpBox.Exceptions.SharpBoxException ex)
                        {
                            Response.Write(ex.ToString());
                        }


                        #endregion
                        return RedirectToAction("EditMainPage", new { id = id });
                    }
                    else
                    {
                        //zly format zdjecia
                        ViewData["alert"] = "Akceptowalne formaty zdjęcia to jpeg i png.";
                        ViewData["id"] = id;
                        return View();
                    }
                }
                else 
                { 
                    //pusty plik
                    ViewData["alert"] = "Wybierz plik i wciśnij \"Prześlij\"";
                    ViewData["id"] = id;
                    return View();
                }
            }
            else
            {
                return RedirectToAction("Restaurant");
            }
            
        }

        /// <summary>
        /// Logo delete - get request from javascript function
        /// </summary>
        /// <param name="plik"></param>
        /// <param name="resid"></param>
        /// <returns></returns>
        public ActionResult LogoDelete(string plik, int resid)
        {
            ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
            if (client.IsUserInRole(User.Identity.Name, "Menadżer") && ( client.IsRestaurantOwner(User.Identity.Name, resid) ))
            {
                #region DropBox Connection
                try
                {
                    // Creating the cloudstorage object 
                    CloudStorage dropBoxStorage = new CloudStorage();

                    // get the configuration for dropbox 
                    var dropBoxConfig = CloudStorage.GetCloudConfigurationEasy(nSupportedCloudConfigurations.DropBox);

                    // declare an access token
                    ICloudStorageAccessToken accessToken = null;

                    // load a valid security token from file
                    string path = Server.MapPath(Url.Content("~/Content/token.txt"));

                    //using (FileStream fs = System.IO.File.Open("C:\\dropboxtoken.txt", FileMode.Open, FileAccess.Read, FileShare.None))
                    using (FileStream fs = System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        accessToken = dropBoxStorage.DeserializeSecurityToken(fs);
                    }

                    // open the connection 
                    var storageToken = dropBoxStorage.Open(dropBoxConfig, accessToken);

                    var mainFolder = dropBoxStorage.GetFolder("/Public/images/" + resid.ToString() + "/logo/");

                    //check if folder exists in child collection
                    #region check childs


                    //lista folderow
                    List<String> folders = new List<String>();

                    bool ifExists = false;

                    // enumerate all child (folder and files) 
                    foreach (var fof in mainFolder)
                    {
                        // check if we have a directory 
                        Boolean bIsDirectory = fof is ICloudDirectoryEntry;

                        if (!bIsDirectory)
                        {
                            //get files names
                            folders.Add(fof.Name);
                            ifExists = true;
                        }
                    }

                    #endregion

                    if (ifExists)
                    {
                        //jezeli jakies pliki sa w tym katalogu - usun zadany plik
                        dropBoxStorage.DeleteFileSystemEntry("/Public/images/" + resid.ToString() + "/logo/" + plik);
                    }
                                        
                    dropBoxStorage.Close();
                }
                catch (AppLimit.CloudComputing.SharpBox.Exceptions.SharpBoxException ex)
                {
                    ;
                }
                #endregion
                //return Json(new { redirectToUrl = Url.Action("EditMainPage", "ManagePanel", new { id = resid }) });
                ViewData["id"] = resid;
                return RedirectToAction("EditMainPage", new { id = resid });
            }

            else
            {
                return RedirectToAction("Restaurant");
            }
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
                    if (value.PriceOption != null)
                    {
                        string[] tab = value.PriceOption.Split(',');
                        int j = 10;
                        for (int i = 1; i <= tab.Length; i++)
                        {
                            if (i == tab.Length)
                                prices += j.ToString() + ".00";
                            else
                                prices += j.ToString() + ".00|";
                            j += 5;
                        }
                    }
                    else
                    {
                        prices += "10.00";
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

        //
        // GET: /ManagePanel/Account
        public ActionResult Account()
        {
            System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("fr-FR");
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("fr-FR");
            if (Request.IsAuthenticated)
            {
                //  MembershipCreateStatus createStatus;
                CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];
                // CustomMembershipUser user = customMemebership.GetUser(User.Identity.Name, true);
                //  MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                CustomMembershipUser user = (CustomMembershipUser)customMemebership.GetUser(User.Identity.Name, true);
                UserDataModel model = new UserDataModel();
                model.Login = user.Login;
                model.Email = user.Email;
                model.Name = user.Name;
                model.Surname = user.Surname;
                model.Address = user.Address;
                model.Town = user.Town;
                model.PostalCode = user.PostalCode;
                model.Country = user.Country;
                model.Birthdate = user.Birthdate;
                model.Sex = user.Sex;
                model.Telephone = user.Telephone;

             //   ViewData["model"] = model;

                return View(model);
            }
            else
            {
                return RedirectToAction("LogOn", "Account");
            }
        }

        //
        // GET: /ManagePanel/EditMenager
        //[Authorize]
        public ActionResult EditMenager()
        {
            System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("fr-FR");
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("fr-FR");
            if (Request.IsAuthenticated)
            {
                CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];
                CustomMembershipUser user = (CustomMembershipUser)customMemebership.GetUser(User.Identity.Name, true);
                UserDataModel model = new UserDataModel();
                model.Login = user.Login;
                model.Email = user.Email;
                model.Name = user.Name;
                model.Surname = user.Surname;
                model.Address = user.Address;
                model.Town = user.Town;
                model.PostalCode = user.PostalCode;
                model.Country = user.Country;
                model.Birthdate = user.Birthdate;
                model.Sex = user.Sex;
                model.Telephone = user.Telephone;

                //ustawienie danych o płci
                List<SelectListItem> sex = new List<SelectListItem>();
                if (model.Sex.Equals("Mężczyzna"))
                {
                    sex.Add(new SelectListItem { Text = "Mężczyzna", Value = "Mężczyzna" });
                    sex.Add(new SelectListItem { Text = "Kobieta", Value = "Kobieta" });
                }
                else
                {
                    sex.Add(new SelectListItem { Text = "Kobieta", Value = "Kobieta" });
                    sex.Add(new SelectListItem { Text = "Mężczyzna", Value = "Mężczyzna" });
                }
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
                ViewData["Map"] = (IEnumerable<Town>)(new List<Town>());

                return View(model);
            }
            else
            {
                return RedirectToAction("LogOn", "Account");
            }
        }

        //
        // POST: /ManagePanel/EditMenager
        //[Authorize]
        [HttpPost]
        public ActionResult EditMenager(UserDataModel model)
        {
            System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("fr-FR");
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("fr-FR");
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
                    if (value.Count == 1)//edytuj usera
                    {
                        CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];
                        CustomMembershipUser user = (CustomMembershipUser)customMemebership.GetUser(User.Identity.Name, true);
                        if (user != null)
                        {
                            user.Email = model.Email;
                            user.Name = model.Name;
                            user.Surname = model.Surname;
                            user.Address = model.Address;
                            user.Town = model.Town;
                            user.PostalCode = model.PostalCode;
                            user.Country = model.Country;
                            user.Birthdate = model.Birthdate;
                            user.Sex = model.Sex;
                            user.Telephone = model.Telephone;

                            customMemebership.UpdateUser(user);

                            return RedirectToAction("Account", "ManagePanel");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Edycja danych nie powiodła się");
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
        // GET: /ManagePanel/ChangePasswordMenager
        [Authorize]
        public ActionResult ChangePasswordMenager()
        {
            return View();
        }

        //
        // POST: /ManagePanel/ChangePasswordMenager
        [Authorize]
        [HttpPost]
        public ActionResult ChangePasswordMenager(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {

                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];
                    CustomMembershipUser currentUser = (CustomMembershipUser)customMemebership.GetUser(User.Identity.Name, true);
                    // MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    // changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
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
    }
}
