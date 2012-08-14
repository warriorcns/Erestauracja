using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Erestauracja.Models;
using Erestauracja.Providers;

namespace Erestauracja.Controllers
{
    public class AccountController : Controller
    {

        //
        // GET: /Account/LogOn
        public ActionResult LogOn()
        {
            return View();
        }

        public ActionResult Account()
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
        // POST: /Account/LogOn

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.Login, model.Password))
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
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                MembershipCreateStatus createStatus;
                CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];
                CustomMembershipUser user = customMemebership.CreateUser(model.Login, model.Password, model.Email, model.Name, model.Surname, model.Address, int.Parse(model.TownID), model.Country, model.Birthdate, model.Sex, model.Telephone, model.Question, model.Answer, true, out createStatus);
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
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
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
