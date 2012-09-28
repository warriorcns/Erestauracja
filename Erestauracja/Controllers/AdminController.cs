using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Erestauracja.Authorization;
using Erestauracja.Providers;
using Erestauracja.Models;
using System.Web.Security;
using Erestauracja.ServiceReference;

namespace Erestauracja.Controllers
{
    [CustomAuthorizeAttribute(Roles = "Admin")]
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        
        public ActionResult Index()
        {
            if (!User.IsInRole("Admin"))
            {
                //Response.Redirect("/Home/Index");
                //Response.Write("Pomyslnie zweryfikowano role uzytkownika");
                //Response.Redirect("Unauthorized.aspx");
            }

            return View();

            
        }

        public ActionResult phpmyadmin()
        {

            return View();
        }

        // GET: /Admin/CreateUser
        public ActionResult CreateUser()
        {
            string status = String.Empty;
            List<SelectListItem> sex = new List<SelectListItem>();
            sex.Add(new SelectListItem { Text = "Mężczyzna", Value = "Mężczyzna" });
            sex.Add(new SelectListItem { Text = "Kobieta", Value = "Kobieta" });
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

        // POST: /Admin/CreateUser
        [HttpPost]
        public ActionResult CreateUser(RegisterModel model)
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
                List<Town> value = null;
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
                else if (value.Count > 1)
                {
                    ModelState.AddModelError("", status);
                    ViewData["miasta"] = value;
                    //ServiceReference.EresServiceClient country = new ServiceReference.EresServiceClient();
                    //IEnumerable<Town> data = country.GetTowns(out status, model.Town, model.PostalCode);
                    //string but = "<a href=" + "#" + " onclick=" + "ChoseAndSend()" + " class=" + "button" + " >" + "Wybierz." + "</a>";
                    //foreach (Town item in data)
                    //{
                    //    item.InfoWindowContent = item.TownName + " " + item.PostalCode + "</br>" + but;
                    //}
                    //ViewData["Map"] = data;
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

        // GET: /Admin/ManageUsers
        public ActionResult ManageUsers()
        {
            int count = 0;
            CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];
            MembershipUserCollection users = customMemebership.GetAllUsers(0, 20, out count);
            ViewData["users"] = users;
            return View();
        }

        public ActionResult deleteUser(string user)
        {
            CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];
            customMemebership.DeleteUser(user, true);
            return RedirectToAction("ManageUsers");
        }

        // GET: /Admin/CreateRoles
        public ActionResult CreateRoles()
        {
            CustomRoleProvider r = (CustomRoleProvider)System.Web.Security.Roles.Providers["CustomRoleProvider"];


            string[] roles = r.GetAllRoles();

            ViewData["Roles"] = roles;
            return View();
        }

        // POST: /Admin/CreateRoles
        [HttpPost]
        public ActionResult CreateRoles(UserRoleModel model)
        {

            CustomRoleProvider r = (CustomRoleProvider)System.Web.Security.Roles.Providers["CustomRoleProvider"];


            string[] roles = r.GetAllRoles();
            
            ViewData["Roles"] = roles;

            if (ModelState.IsValid)
            {
                //CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];
                try
                {
                    ServiceReference.EresServiceClient u = new ServiceReference.EresServiceClient();
                    u.CreateRole(model.RoleName);
                    u.Close();
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "Tworzenie roli nie powiodło się.");
                }
            }
            return View();
        }

        public ActionResult deleteRole(string role) 
        {
            //usuwanie wybranej roli

            //string test = role;
            CustomRoleProvider r = (CustomRoleProvider)System.Web.Security.Roles.Providers["CustomRoleProvider"];
            try
            {
                r.DeleteRole(role, true);
            }
            catch(Exception e)
            {
            
            }
            return RedirectToAction("CreateRoles");
        }


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
                return "Pytanie odzyskiwania hasła jest nieprawidłowe. Wprowadź poprawne pytanie.";

                case MembershipCreateStatus.InvalidUserName:
                return "Login jest nieprawidłowy. Wprowadź prawidłowy login.";

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
