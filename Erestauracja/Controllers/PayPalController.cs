using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Erestauracja.Models;
using System.Configuration;
using System.Net;
using System.Text;
using System.IO;
using Erestauracja.Authorization;
using Erestauracja.Providers;
using System.Globalization;
using Erestauracja.Controllers;

namespace Erestauracja.Controllers
{
    [CustomAuthorizeAttribute(Roles = "Klient")]
    public class PayPalController : Controller
    {
        //
        // GET: /PayPal/
        //[HttpPost]
        /// <summary>
        /// Obsluguje pobrane dane z zamowienia.
        /// </summary>
        /// <param name="comm"></param>
        /// <param name="id"></param>
        /// <param name="resid"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PostToPayPal(string com, int id, int resid)
        {
            PayPal pp = new PayPal();
            pp.cmd = "_xclick";

            System.Web.HttpCookie myCookie = new System.Web.HttpCookie("Comment");
            DateTime now = DateTime.Now;
            myCookie.Expires = now.AddDays(1);
            myCookie.Value = com;
            Response.Cookies.Add(myCookie);

            System.Web.HttpCookie res = new System.Web.HttpCookie("ResID");
            res.Expires = now.AddDays(1);
            res.Value = resid.ToString();
            Response.Cookies.Add(res);

            //klucz konta biznesowego       
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    pp.business = client.GetRestaurantEmail(resid);
                }
                client.Close();
            }
            catch (Exception e)
            {
                //blad
                ViewData["alert"] = "Dane nie zostały wysłane - błąd.";
                return RedirectToAction("CancelFromPaypal", id);               
            }

            //czy uzywamy sandboxa 
            bool useSandbox = Convert.ToBoolean(ConfigurationManager.AppSettings["UseSandbox"]);

            //linki do wyslania danych
            if (useSandbox)
                ViewBag.actionURL = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            else
                ViewBag.actionURL = "https://www.paypal.com/cgi-bin/webscr";

            pp.@return = ConfigurationManager.AppSettings["ReturnURL"];
            pp.cancel_return = ConfigurationManager.AppSettings["CancelURL"] + "/" + id;
            pp.notify_url = ConfigurationManager.AppSettings["NotifyURL"];
            pp.currency_code = ConfigurationManager.AppSettings["CurrencyCode"];

            String paypalData = string.Empty;
            string[] data = null;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    //cena zamowienia | cena dostawy
                    paypalData = client.GetPayPalData(resid, id);
                }
                client.Close();
            }
            catch (Exception e)
            {
                paypalData = null;
            }

            if (( paypalData != null || paypalData != string.Empty ) && paypalData.Contains("|"))
            {
                data = paypalData.Split('|');
                pp.amount = data[0].Replace(",",".");
                pp.shipping = data[1].Replace(",", ".");
                pp.item_number = id.ToString();
            }
            else
            { 
                //blad
                ViewData["alert"] = "Dane nie zostały wysłane - błąd.";
                return RedirectToAction("CancelFromPaypal", id);
            }

            return View(pp);
            }

        public ActionResult RedirectFromPaypal()
        {
            return View();
        }

        public ActionResult CancelFromPaypal(int id)
        {
            ViewData["id"] = id;
            return View();
        }

        /// <summary>
        /// Weryfikacja danych - do tej metody paypal przekierowuje, gdy klient jest przekierowany do naszej aplikacji
        /// </summary>
        /// <param name="pp"></param>
        /// <returns></returns>
        public ActionResult IPN(PayPal pp)
        {
            string response = string.Empty;
                        

           
            // Receive IPN request from PayPal and parse all the variables returned
            var formVals = new Dictionary<string, string>();
            formVals.Add("cmd", "_notify-validate");
          //  for (int i = 0; i < 300; i++) { ; }
            // if you want to use the PayPal sandbox change this from false to true
            response = GetPayPalResponse(formVals, true);
            

            if (response == "VERIFIED")
            {
                string transactionID = Request["txn_id"];
                string sAmountPaid = Request["mc_gross"];
                string deviceID = Request["custom"];

                //validate the order
                //Decimal amountPaid = 0;
                //Decimal.TryParse(sAmountPaid, out amountPaid);

                //Erestauracja.ServiceReference.EresServiceClient client = new Erestauracja.ServiceReference.EresServiceClient();

                //PayPal pp = new PayPal();
                //pp.txn_id = Request["txn_id"];
                //pp.mc_gross = Request["mc_gross"];
                //pp.txn_type = Request["txn_type"];
                int id = int.Parse(pp.item_number);

                #region ciasteczka - odczyt
                System.Web.HttpCookie myCookie = new System.Web.HttpCookie("Comment");
                myCookie = Request.Cookies["Comment"];
                string comm = string.Empty;
                if (myCookie != null)
                {
                    comm = myCookie.Value;
                }

                System.Web.HttpCookie res = new System.Web.HttpCookie("ResID");
                res = Request.Cookies["ResID"];
                int resid = -1;
                if (res != null)
                {
                    resid = int.Parse(res.Value);
                }
                #endregion

                //zrob z danymi co Ci sie podoba.. 
                bool value = false;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.Pay(User.Identity.Name, id, comm, "PayPal~" + pp.txn_id + "~" + pp.txn_type);
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    value = false;
                }
                if (value == false)
                {
                    ModelState.AddModelError("", "Wysyłanie zamówienia nie powiodło się.");

                    //wyświetl info że nie powiodło sie 
                    //i jakeś info co zrobić w takiej sytuacji
                    return RedirectToAction("PayError", "Basket", new { id = id });
                }
                else
                {
                    //usuwanie zamówionych dań z koszyka
                    DeleteRest(resid);

                    //wyświetl potwierdzenie
                    //z info że ok że może zobaczyć w aktualnych zamówieniach i że dostał email
                    //zapisz id zamówienia że zostało zapłacone
                    return RedirectToAction("PaySuccess", "Basket", new { id = id });
                    //return View(pp);
                }


            }
            else
            {
                //invalid
                ViewData["alert"] = "Transakcja nie została zweryfikowana.";
                return View();
            }
           
        }

        string GetPayPalResponse(Dictionary<string, string> formVals, bool useSandbox)
        {

            // Parse the variables
            // Choose whether to use sandbox or live environment
            string paypalUrl = useSandbox ? "https://www.sandbox.paypal.com/cgi-bin/webscr"
            : "https://www.paypal.com/cgi-bin/webscr";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(paypalUrl);

            // Set values for the request back
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";

            byte[] param = Request.BinaryRead(Request.ContentLength);
            string strRequest = Encoding.ASCII.GetString(param);

            StringBuilder sb = new StringBuilder();
            sb.Append(strRequest);

            foreach (string key in formVals.Keys)
            {
                sb.AppendFormat("&{0}={1}", key, formVals[key]);
            }
       //     strRequest += sb.ToString();
            strRequest = sb.ToString();
            req.ContentLength = strRequest.Length;

            //for proxy
            //WebProxy proxy = new WebProxy(new Uri("http://urlort#");
            //req.Proxy = proxy;
            //Send the request to PayPal and get the response
            string response = "";
            using (StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII))
            {

                streamOut.Write(strRequest);
                streamOut.Close();
                using (StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream()))
                {
                    response = streamIn.ReadToEnd();
                }
            }

            return response;
        }


        #region usuwanie ciastek z koszyka
        public void DeleteRest(int id)
        {
            string lista = String.Empty;
            lista = Restore();
            List<string> usun = new List<string>();

            foreach (string product in lista.Split('|'))
            {
                string[] dane = product.Split('~');
                if (id == Int32.Parse(dane[1]))
                {
                    usun.Add(product);
                }
            }

            string newlista = lista;
            foreach (String item in usun)
            {
                if (!String.IsNullOrWhiteSpace(item))
                {
                    int index = newlista.IndexOf(item);
                    if (newlista.Length == item.Length)
                    {
                        newlista = newlista.Remove(index, item.Length);
                    }
                    else
                    {
                        newlista = ( index == 0 ) ? newlista.Remove(index, item.Length + 1) : newlista.Remove(index - 1, item.Length + 1);
                    }
                }
            }

            int x = 0;
            foreach (string item in newlista.Split('|'))
            {
                string[] data = item.Split('~');
                data[0] = x.ToString();
                x++;
            }
            Store(newlista);
        }

        public void Store(string myClass)
        {
            HttpCookie cookie = new HttpCookie("basket")
            {
                // Set the expiry date of the cookie to 1 day
                Expires = DateTime.Now.AddDays(6)
            };
            cookie.Value = myClass;
            // Add the cookie to the current http context
            Response.Cookies.Add(cookie);
        }

        public string Restore()
        {
            // Always remember to check that the cookie is not empty
            HttpCookie cookie = Request.Cookies["basket"];
            if (cookie != null)
            {
                return cookie.Value;
            }
            return String.Empty;
        }
        #endregion
    }
}
