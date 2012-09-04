using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Erestauracja.Authorization;
using Erestauracja.Models;
using Erestauracja.Providers;

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
                CustomMembershipProvider customMemebership = (CustomMembershipProvider)System.Web.Security.Membership.Providers["CustomMembershipProvider"];
                CustomMembershipUser user = (CustomMembershipUser)customMemebership.GetUser(User.Identity.Name, true);
                if (user != null)
                {
                    bool value = false;
                    try
                    {
                        ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                        using (client)
                        {
                            value = client.AddRestaurant(model.Name, model.DisplayName, model.Address, model.TownId, model.CountryId, model.Telephone, model.Email, model.Nip, model.Regon, model.Password ,user.Id, model.DeliveryTime);
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
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        
        
    }
}
