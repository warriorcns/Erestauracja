using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Erestauracja.Models;
using Erestauracja.Providers;
using System.Web.Mvc.Html;
using Erestauracja.Authorization;
using Erestauracja.ServiceReference;
using System.Net;
using System.Globalization;
using Jmelosegui.Mvc.Controls;


namespace Erestauracja.Controllers
{
    
    public class AccountController : Controller
    {    
        //
        // GET: /Account/Account
        //role ktore maja dostep do danego zasobu - inaczej przekierowuje na strone logowania - do zmiany.
        //[Authorize]
        [CustomAuthorizeAttribute(Roles = "Klient, Menadżer, PracownikFull, PracownikLow")]
        public ActionResult Account()
        {
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
                model.PostalCode=user.PostalCode;
                model.Country = user.Country;
                model.Birthdate = user.Birthdate;
                model.Sex = user.Sex;
                model.Telephone = user.Telephone;

                ViewData["model"] = model;
                    
                return View();
            }
            else
            {
                return RedirectToAction("LogOn", "Account");
            }
        }

        //
        // GET: /Account/EditData
        //[Authorize]
        [CustomAuthorizeAttribute(Roles = "Klient, Menadżer, PracownikFull, PracownikLow")]
        public ActionResult EditData()
        {
            List<SelectListItem> sex = new List<SelectListItem>();
            ServiceReference.EresServiceClient country = new ServiceReference.EresServiceClient();
            try
            {
                string status = string.Empty;
                IEnumerable<Town> data = country.GetTowns(out status, "Tczew", "83-110");

                ViewData["Map"] = data;

           
                //ServiceReference.EresServiceClient country = new ServiceReference.EresServiceClient();

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

              //  ViewData["model"] = model;

                if (model.Sex.Equals("Mężczyzna"))
                {
                    sex.Add(new SelectListItem { Text = "Mężczyzna", Value = "Mężczyzna" });
                    sex.Add(new SelectListItem { Text = "Kobieta", Value = "Kobieta" });
                }
                else {
                    sex.Add(new SelectListItem { Text = "Kobieta", Value = "Kobieta" });
                    sex.Add(new SelectListItem { Text = "Mężczyzna", Value = "Mężczyzna" }); 
                }
                ViewData["sex"] = sex;
                ViewData["miasta"] = new List<Town>();

                return View(model);
            }
            else
            {
                return RedirectToAction("LogOn", "Account");
                
            }
        }
        
        //
        // POST: /Account/EditData
        //[Authorize]
        [HttpPost]
        [CustomAuthorizeAttribute(Roles = "Klient, Menadżer, PracownikFull, PracownikLow")]
        public ActionResult EditData(UserDataModel model)
        {
            List<SelectListItem> sex = new List<SelectListItem>();
            sex.Add(new SelectListItem { Text = "Mężczyzna", Value = "Mężczyzna" });
            sex.Add(new SelectListItem { Text = "Kobieta", Value = "Kobieta" });
            ViewData["sex"] = sex;
            string status = String.Empty;

            if (ModelState.IsValid)
            {
                List<Town> value = null;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = new List<Town>(client.GetTowns(out status, model.Town, model.PostalCode));
                        List<string> listapobrana = new List<string>(client.GetCountriesList());

                        //List<string> listapobrana = new List<string>(client.GetCountriesList());
                        List<SelectListItem> countryList = new List<SelectListItem>();


                        foreach (string item in listapobrana)
                        {
                            countryList.Add(new SelectListItem { Text = item, Value = item });
                        }
                        ViewData["countryList"] = countryList;
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
                if (value.Count == 1)
                {
                    CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];
                    CustomMembershipUser user = (CustomMembershipUser)customMemebership.GetUser(User.Identity.Name, true);
                    //CreateUser(model.Login, model.Password, model.Email, model.Name, model.Surname, model.Address, model.TownID, model.Country, model.Birthdate, model.Sex, model.Telephone, model.Question, model.Answer, true, out createStatus);
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
                        //CustomRoleProvider role = (CustomRoleProvider)System.Web.Security.Roles.Providers["CustomRoleProvider"];
                        //role.AddUsersToRoles(new string[] { user.Login }, new string[] { "Klient" });

                        return RedirectToAction("Account", "Account");
                    }
                }
                else if (value.Count > 1)
                {
                    
                    ModelState.AddModelError("", status);
                    ViewData["miasta"] = value;
                    //return RedirectToAction("", "Account",model);

                    //tu trzeba przekazac modelem (miasta razem z jego wartosciami) wspolrzedne i inne dane z miasta do wypelnienia markerow..
                    
                    ServiceReference.EresServiceClient country = new ServiceReference.EresServiceClient();
                    IEnumerable<Town> data = country.GetTowns(out status, model.Town, model.PostalCode);

                    foreach (Town item in data)
                    {
                        string onClick = String.Format(" \"ChoseAndSend('{0}', '{1}')\" ", item.TownName, item.PostalCode);

                        item.InfoWindowContent = item.TownName + " " + item.PostalCode + "</br>" +
                            "<a href=" + "#" + " onclick=" + onClick + " class=" + "button" + ">" + "Wybierz." + "</a>";
                    }
                    ViewData["Map"] = data;
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", status);
                }

            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/Settings
        [Authorize]
        [CustomAuthorizeAttribute(Roles = "Klient, Menadżer, PracownikFull, PracownikLow")]
        public ActionResult Settings()
        {
            if (Request.IsAuthenticated)
            {

                return View();
            }
            else
            {
                return RedirectToAction("LogOn", "Account");
            }
        }

        //
        // GET: /Account/OrderHistory
        [Authorize]
        [CustomAuthorizeAttribute(Roles = "Klient, Menadżer, PracownikFull, PracownikLow")]
        public ActionResult OrderHistory()
        {
            if (Request.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LogOn", "Account");
            }
        }

        //
        // GET: /Account/Comments
        [Authorize]
        [CustomAuthorizeAttribute(Roles = "Klient, Menadżer, PracownikFull, PracownikLow")]
        public ActionResult Comments()
        {
            if (Request.IsAuthenticated)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LogOn", "Account");
            }
        }

        //
        // GET: /Account/LogOn
        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn
        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.Login, model.Password))
                {
                    CustomRoleProvider role = (CustomRoleProvider)System.Web.Security.Roles.Providers["CustomRoleProvider"];
                    if (role.IsUserInRole(model.Login, "Menadżer"))
                    {
                        FormsAuthentication.SetAuthCookie(model.Login, model.RememberMe);
                        return RedirectToAction("Index", "ManagePanel");
                    }
                    else if (role.IsUserInRole(model.Login, "Admin"))
                    {
                        FormsAuthentication.SetAuthCookie(model.Login, model.RememberMe);
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(model.Login, model.RememberMe);
                        if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                            && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                        {
                            return Redirect(returnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
   
        //
        // GET: /Account/LogOff   
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register
        public ActionResult Register()
        {
            
            string status = String.Empty;
            List<SelectListItem> sex = new List<SelectListItem>();
            sex.Add(new SelectListItem {Text = "Mężczyzna", Value = "Mężczyzna"});
            sex.Add(new SelectListItem {Text = "Kobieta", Value = "Kobieta"});
            ViewData["sex"] = sex;

            ServiceReference.EresServiceClient country = new ServiceReference.EresServiceClient();
            try
            {
                List<string> listapobrana = new List<string>(country.GetCountriesList());
                List<SelectListItem> countryList = new List<SelectListItem>();

                foreach (string item in listapobrana)
                {
                    countryList.Add(new SelectListItem { Text = item, Value = item });
                }
                ViewData["countryList"] = countryList;

                IEnumerable<Town> data = country.GetTowns(out status, "Tczew", "83-110");
                country.Close();
                ViewData["Map"] = data;
                
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Pobranie listy panstw nie powiodło się.");
            }
            ViewData["miasta"] = new List<Town>();

            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {

            List<SelectListItem> sex = new List<SelectListItem>();
            sex.Add(new SelectListItem { Text = "Mężczyzna", Value = "Mężczyzna" });
            sex.Add(new SelectListItem { Text = "Kobieta", Value = "Kobieta" });
            ViewData["sex"] = sex;

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


            string status = String.Empty;
            if (ModelState.IsValid)
            {
                List<Town> value=null;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = new List<Town>(client.GetTowns(out status, model.Town,model.PostalCode));
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

                if (value.Count == 1)
                {
                    MembershipCreateStatus createStatus;
                    CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];
                    CustomMembershipUser user = customMemebership.CreateUser(model.Login, model.Password, model.Email, model.Name, model.Surname, model.Address, value[0].ID, model.Country, model.Birthdate, model.Sex, model.Telephone, model.Question, model.Answer, true, out createStatus);
                    if (user != null)
                    {
                        CustomRoleProvider role = (CustomRoleProvider)System.Web.Security.Roles.Providers["CustomRoleProvider"];
                        role.AddUsersToRoles(new string[] { user.Login }, new string[] { "Klient" });
                    }
                    if (createStatus == MembershipCreateStatus.Success)
                    {
                        FormsAuthentication.SetAuthCookie(model.Login, false /* createPersistentCookie */);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("", ErrorCodeToString(createStatus));
                    }
                }
                else if(value.Count>1)
                {
                    ModelState.AddModelError("", status);
                    ViewData["miasta"] = value;
                    //return RedirectToAction("", "Account",model);

                    //tu trzeba przekazac modelem (miasta razem z jego wartosciami) wspolrzedne i inne dane z miasta do wypelnienia markerow..
                    //string status = String.Empty;
                    ServiceReference.EresServiceClient country = new ServiceReference.EresServiceClient();
                    IEnumerable<Town> data = country.GetTowns(out status, model.Town, model.PostalCode);
                    
                    foreach (Town item in data)
                    {
                        string onClick = String.Format(" \"ChoseAndSend('{0}', '{1}')\" ", item.TownName, item.PostalCode);

                        item.InfoWindowContent = item.TownName + " " + item.PostalCode + "</br>"+
                            "<a href=" + "#" + " onclick=" + onClick + " class=" + "button" + ">" + "Wybierz." + "</a>";
                    }
                    ViewData["Map"] = data;
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", status);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword
        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
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

        //
        // GET: /Account/ChangePasswordSuccess
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        //
        // GET: /Account/PasswordReset
        public ActionResult PasswordReset()
        {
            CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];

            if (!customMemebership.EnablePasswordReset) throw new Exception("Resetowanie hasła nie jest dozwolone");
            return View();
        }

        //
        // POST: /Account/ChangePassword
        [HttpPost]
        public ActionResult PasswordReset(string login)
        {
            CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];
            if (!customMemebership.EnablePasswordReset) throw new Exception("Resetowanie hasła nie jest dozwolone");
            return RedirectToAction("QuestionAndAnswer", new { login = login });
        }

        //
        // GET: /Account/QuestionAndAnswer
        public ActionResult QuestionAndAnswer(string login)
        {
            CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];

            if (!customMemebership.EnablePasswordReset) throw new Exception("Resetowanie hasła nie jest dozwolone\r\n");
            ViewData["Login"] = login;
            string pyt = customMemebership.GetUserQuestion(login);
            ViewData["Pytanie"] = pyt;
            return View();
        }

        //
        // POST: /Account/QuestionAndAnswer
        [HttpPost]
        public ActionResult QuestionAndAnswer(string login, string odpowiedz)
        {
            CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];
            if (!customMemebership.EnablePasswordReset) throw new Exception("Resetowanie hasła nie jest dozwolone\r\n");
            customMemebership.ResetPassword(login, odpowiedz);
            return RedirectToAction("PasswordResetFinal");
        }

        //
        // GET: /Account/PasswordResetFinal
        public ActionResult PasswordResetFinal()
        {
            CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];
            if (!customMemebership.EnablePasswordReset) throw new Exception("Resetowanie hasła nie jest dozwolone\r\n");
            
            return View();
        }

        //
        // GET: /Account/RegisterManager
        public ActionResult RegisterManager()
        {
            return View();
        }

        //
        // POST: /Account/RegisterManager
        [HttpPost]
        public ActionResult RegisterManager(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus createStatus;
                CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];
                CustomMembershipUser user = customMemebership.CreateUser(model.Login, model.Password, model.Email, model.Name, model.Surname, model.Address, 123456, model.Country, model.Birthdate, model.Sex, model.Telephone, model.Question, model.Answer, true, out createStatus);
                if (user != null)
                {
                    CustomRoleProvider role = (CustomRoleProvider)System.Web.Security.Roles.Providers["CustomRoleProvider"];
                    role.AddUsersToRoles(new string[] { user.Login }, new string[] { "Menadżer" });
                }
                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsAuthentication.SetAuthCookie(model.Login, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "ManagePanel");
                }
                else
                {
                    ModelState.AddModelError("", ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ExistingManager
        public ActionResult ExistingManager()
        {
            return View();
        }

        //
        // POST: /Account/ExistingManager
        [HttpPost]
        public ActionResult ExistingManager(LogOnModel model)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.Login, model.Password))
                {
                    FormsAuthentication.SignOut();
                    CustomRoleProvider role = (CustomRoleProvider)System.Web.Security.Roles.Providers["CustomRoleProvider"];
                    string[] rola = role.GetRolesForUser(model.Login);
                    role.RemoveUsersFromRoles(new string[] { model.Login }, rola);
                    role.AddUsersToRoles(new string[] { model.Login }, new string[] { "Menadżer" });
                    FormsAuthentication.SetAuthCookie(model.Login, model.RememberMe);
                    return RedirectToAction("Index", "ManagePanel");
                }
                else
                {
                    ModelState.AddModelError("", "");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        public static Coordinate GetCoordinates(string region)
        {
            using (var client = new WebClient())
            {

                string uri = "http://maps.google.com/maps/geo?q='" + region +
                  "'&output=csv&key=ABQIAAAAzr2EBOXUKnm_jVnk0OJI7xSosDVG8KKPE1" +
                  "-m51RBrvYughuyMxQ-i1QfUnH94QxWIa6N4U6MouMmBA";

                string[] geocodeInfo = client.DownloadString(uri).Split(',');

                NumberStyles style;
                CultureInfo culture;

                style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
                culture = CultureInfo.CreateSpecificCulture("en-CA");

                double lat;
                double lng;

                double.TryParse(geocodeInfo[2].ToString(), style, culture, out lat);
                double.TryParse(geocodeInfo[3].ToString(), style, culture, out lng);
                return new Coordinate(lat, lng);
            }
        }

        //przetłumaczyć reszte
        //
        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Podany login już istnieje. Wprowadź inny login.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "Użytkownik o poadany adersie email już istnieje. Wprowadź inny adres email.";

                case MembershipCreateStatus.InvalidPassword:
                    return "Podane hasło jest nieprawidłowe. Wprowadź prawidłowe hasło.";

                case MembershipCreateStatus.InvalidEmail:
                    return "Podany adres email jest nieprawidłowy. Wprowadź prawidłowy adres email.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "Odpowiedz do pytania pozwalającego odzyskać hasło jest nieprawidłowa. Wprowadź prawidłową odpowiedź.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }

    //Geocoding
    public class Coordinate
    {
        public double Latitude;
        public double Longitude;

        public Coordinate(double Latitude, double Longitude)
        {
            this.Latitude = Latitude;
            this.Longitude = Longitude;
        }

    }
}
