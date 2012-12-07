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
        public ActionResult PostToPayPal(string comm, int id, int resid)
        {
            PayPal pp = new PayPal();
            pp.cmd = "_xclick";

            System.Web.HttpCookie myCookie = new System.Web.HttpCookie("Comment");
            DateTime now = DateTime.Now;
            myCookie.Expires = now.AddDays(1);
            myCookie.Value = comm;
            Response.Cookies.Add(myCookie);

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
                ;               
            }

            //czy uzywamy sandboxa 
            bool useSandbox = Convert.ToBoolean(ConfigurationManager.AppSettings["UseSandbox"]);

            //linki do wyslania danych
            if (useSandbox)
                ViewBag.actionURL = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            else
                ViewBag.actionURL = "https://www.paypal.com/cgi-bin/webscr";

            pp.@return = ConfigurationManager.AppSettings["ReturnURL"];
            pp.cancel_return = ConfigurationManager.AppSettings["CancelURL"];
            pp.notify_url = ConfigurationManager.AppSettings["NotifyURL"];
            pp.currency_code = ConfigurationManager.AppSettings["CurrencyCode"];

            String paypalData = string.Empty;
            string[] data = null;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    //value = client.Pay(User.Identity.Name, id, com, "cash");
                    //cena zamowienia | cena dostawy
                    paypalData = client.GetPayPalData(resid, id);
                }
                client.Close();
            }
            catch (Exception e)
            {
                ;
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
                return RedirectToAction("CancelFromPaypal");
            }

            return View(pp);
            }

        

        public ActionResult RedirectFromPaypal()
        {
            return View();
        }

        public ActionResult CancelFromPaypal()
        {
            
            return View();
        }

        public ActionResult IPN(PayPal pp)
        {
            // Receive IPN request from PayPal and parse all the variables returned
            var formVals = new Dictionary<string, string>();
            formVals.Add("cmd", "_notify-validate");

            // if you want to use the PayPal sandbox change this from false to true
            string response = GetPayPalResponse(formVals, true);

            if (response == "VERIFIED")
            {
                string transactionID = Request["txn_id"];
                string sAmountPaid = Request["mc_gross"];
                string deviceID = Request["custom"];

                //validate the order
                Decimal amountPaid = 0;
                Decimal.TryParse(sAmountPaid, out amountPaid);

                //Erestauracja.ServiceReference.EresServiceClient client = new Erestauracja.ServiceReference.EresServiceClient();

                //PayPal pp = new PayPal();
                pp.txn_id = Request["txn_id"];
                pp.mc_gross = Request["mc_gross"];
                pp.txn_type = Request["txn_type"];
                int id = int.Parse(pp.item_number);

                #region ciasteczka - odczyt
                System.Web.HttpCookie myCookie = new System.Web.HttpCookie("Comment");
                myCookie = Request.Cookies["Comment"];
                string comm = string.Empty;
                if (myCookie != null)
                {
                    comm = myCookie.Value;
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
                    //return RedirectToAction("PayError");
                }
                else
                {
                    //usuwanie zamówionych dań z koszyka
                    //DeleteRest(res);

                    //wyświetl potwierdzenie
                    //z info że ok że może zobaczyć w aktualnych zamówieniach i że dostał email
                    //zapisz id zamówienia że zostało zapłacone
                    //return RedirectToAction("PaySuccess");
                }
                return View(pp);

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
            strRequest += sb.ToString();
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

        
    }
}
