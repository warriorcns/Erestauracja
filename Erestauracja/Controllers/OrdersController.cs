using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Erestauracja.ServiceReference;

namespace Erestauracja.Controllers
{
    [Authorize(Roles = "Klient")]
    public class OrdersController : Controller
    {
        //
        // GET: /Orders/
        public ActionResult Index()
        {
            List<UserOrder> value = null;
            try
            {
                Erestauracja.ServiceReference.EresServiceClient client = new Erestauracja.ServiceReference.EresServiceClient();
                using (client)
                {
                    value = new List<UserOrder>(client.GetUserActiveOrder(User.Identity.Name));
                }
                client.Close();
            }
            catch (Exception e)
            {
                value = null;
            }

            if (value == null)
            {
                ModelState.AddModelError("", "Pobieranie aktywnych zamówień nie powiodło się.");
            }

            return View(value);
        }

    }
}
