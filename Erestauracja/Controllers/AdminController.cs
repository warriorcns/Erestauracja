using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Erestauracja.Authorization;
using Erestauracja.Providers;
using Erestauracja.Models;

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


        public ActionResult CreateUser()
        {
            return View();
        }

        public ActionResult ManageUsers()
        {
            return View();
        }

        // GET: /Admin/CreateRoles
        public ActionResult CreateRoles()
        {
            return View();
        }

        // POST: /Admin/CreateRoles
        [HttpPost]
        public ActionResult CreateRoles(UserRoleModel model)
        {
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

        public ActionResult ManageRoles()
        {
            return View();
        }

    }
}
