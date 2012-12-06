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
using System.Threading;


namespace Erestauracja.Controllers
{
    
    public class AccountController : Controller
    {    
        //
        // GET: /Account/Account
        //role ktore maja dostep do danego zasobu - inaczej przekierowuje na strone logowania - do zmiany.
        //[Authorize]
        [CustomAuthorizeAttribute(Roles = "Klient")]
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
                model.PostalCode=user.PostalCode;
                model.Country = user.Country;

                //string tmp = string.Empty;
                //tmp = user.Birthdate.ToString("dd/MM/yyyy");
                //model.Birthdate = DateTime.Parse(tmp, cultureinfo);
                

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
        [CustomAuthorizeAttribute(Roles = "Klient")]
        public ActionResult EditData()
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
        // POST: /Account/EditData
        //[Authorize]
        [HttpPost]
        [CustomAuthorizeAttribute(Roles = "Klient")]
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
                        CustomMembershipUser user = (CustomMembershipUser)customMemebership.GetUser(User.Identity.Name, true);
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

                            return RedirectToAction("Account", "Account");
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
        // GET: /Account/OrderHistory
        [Authorize]
        [CustomAuthorizeAttribute(Roles = "Klient")]
        public ActionResult OrderHistory()
        {
            if (Request.IsAuthenticated)
            {
                DateTime from = DateTime.Now.Subtract(new TimeSpan(7, 0, 0, 0));
                DateTime to = DateTime.Now;

                ViewData["from"] = from;
                ViewData["to"] = to;

                List<UserOrder> value = null;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = new List<UserOrder>(client.GetOrderHistory(User.Identity.Name, from, to));
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    value = null;
                }
                if (value == null)
                {
                    ModelState.AddModelError("", "Pobieranie zamówienia nie powiodło się.");

                    return View(value);
                }

                return View(value);
            }
            else
            {
                return RedirectToAction("LogOn", "Account");
            }
        }

        [Authorize]
        [CustomAuthorizeAttribute(Roles = "Klient")]
        public ActionResult OrderHistory2(string from, string to)
        {
            if (Request.IsAuthenticated)
            {
                DateTime fromm = DateTime.Parse(from);
                DateTime too = DateTime.Parse(to);

                ViewData["from"] = from;
                ViewData["to"] = to;

                List<UserOrder> value = null;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = new List<UserOrder>(client.GetOrderHistory(User.Identity.Name, fromm, too));
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    value = null;
                }
                if (value == null)
                {
                    ModelState.AddModelError("", "Pobieranie zamówienia nie powiodło się.");

                    return View(value);
                }

                return View(value);
            }
            else
            {
                return RedirectToAction("LogOn", "Account");
            }
        }

        //
        // GET: /Account/Comments
        [Authorize]
        [CustomAuthorizeAttribute(Roles = "Klient")]
        public ActionResult Comments()
        {
            if (Request.IsAuthenticated)
            {
                List<Comment> value = null;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = new List<Comment>(client.GetUserComments(User.Identity.Name));
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    value = null;
                }

                if (value == null)
                {
                    ModelState.AddModelError("", "Pobieranie komentarzy nie powiodło się.");
                }

                return View(value);
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
                    else if (role.IsUserInRole(model.Login, "Restauracja"))
                    {
                        FormsAuthentication.SetAuthCookie(model.Login, model.RememberMe);
                        return RedirectToAction("Locked", "POS");
                    }
                    else if (role.IsUserInRole(model.Login, "Pracownik"))
                    {
                        ModelState.AddModelError("", "Zaloguj się najpierw na konto restauracji.");
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
            //ustawienie danych o płci
            List<SelectListItem> sex = new List<SelectListItem>();
            sex.Add(new SelectListItem {Text = "Mężczyzna", Value = "Mężczyzna"});
            sex.Add(new SelectListItem {Text = "Kobieta", Value = "Kobieta"});
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

            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        public ActionResult Register(RegisterModel model)
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
        // GET: /Account/ChangePassword
        [Authorize]
        [CustomAuthorizeAttribute(Roles = "Klient")]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword
        
        [HttpPost]
        [CustomAuthorizeAttribute(Roles = "Klient")]
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
        [CustomAuthorizeAttribute(Roles = "Klient")]
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

            return View();
        }

        //
        // POST: /Account/RegisterManager
        [HttpPost]
        public ActionResult RegisterManager(RegisterModel model)
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
                    if (value.Count == 1)//Dodaje menadżera
                    {
                        MembershipCreateStatus createStatus;
                        CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];
                        CustomMembershipUser user = customMemebership.CreateUser(model.Login, model.Password, model.Email, model.Name, model.Surname, model.Address, value[0].ID, model.Country, model.Birthdate, model.Sex, model.Telephone, model.Question, model.Answer, true, out createStatus);
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
}
