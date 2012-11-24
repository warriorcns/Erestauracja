using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Erestauracja.Models;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Erestauracja.ServiceReference;

namespace Erestauracja.Controllers
{
    [Authorize(Roles = "Klient")]
    public class BasketController : Controller
    {
        //
        // GET: /Basket/
        public ActionResult Index()
        {
            string lista = String.Empty;
            lista = Restore();

            BasketOut value = null;
            if(!String.IsNullOrWhiteSpace(lista))
            {
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.GetBasket(lista);
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    value = null;
                }
                if (value == null)
                {
                    ModelState.AddModelError("", "Pobieranie danych nie powiodło się.");
                }
            }
            return View(value);
        }

        public ActionResult Delete(int id)
        {
            string lista = String.Empty;
            lista = Restore();
            string usun = String.Empty;

            foreach(string product in lista.Split('|'))
            {
                string[] dane = product.Split('~');
                if (id == Int32.Parse(dane[0]))
                {
                    usun = product;
                    break;
                }
            }
            if (lista.Split('|').Length == 1)
            {
                DeleteAll();
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(usun))
                {
                    int index = lista.IndexOf(usun);
                    string newlista = (index == 0) ? lista.Remove(index, usun.Length + 1) : lista.Remove(index - 1, usun.Length + 1);

                    int x = 0;
                    foreach (string item in newlista.Split('|'))
                    {
                        string[] data = item.Split('~');
                        data[0] = x.ToString();
                        x++;
                    }
                    Store(newlista);
                }
            }
            return RedirectToAction("Index");
        }

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
                    newlista = (index == 0) ? newlista.Remove(index, item.Length + 1) : newlista.Remove(index - 1, item.Length + 1);
                }
            }

            int x = 0;
            foreach(string item in newlista.Split('|'))
            {
                string[] data = item.Split('~');
                data[0] = x.ToString();
                x++;
            }
            Store(newlista);
        }

        public void DeleteAll()
        {
            if (Request.Cookies["basket"] != null)
            {
                HttpCookie myCookie = new HttpCookie("basket");
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(myCookie);
            }
        }

        public ActionResult ClearBasket()
        {
            DeleteAll();

            return RedirectToAction("Index");
        }

        public ActionResult Realize(String data)
        {
            BasketRest rest = null;
            if (data != null)
            {
                byte[] buffer = Convert.FromBase64String(data);
                Stream myStream = new MemoryStream(buffer);
                try
                {
                    IFormatter formatter = new BinaryFormatter();
                    rest = (BasketRest)formatter.Deserialize(myStream);
                }
                finally
                {
                    myStream.Close();
                }
            }

            DeleteRest(rest.RestaurantId);
           
            return RedirectToAction("Index");
        }
        
        //wychwytuje dane z guzika - do koszyka
        public ActionResult ToBasket(string resId, string catId, string prodId, string opcjaCenowa, string dodatki, string opcje, string count, string comm)
        {
            string basket = Restore();

            if (String.IsNullOrWhiteSpace(basket))
            {
                string nowy = "0~" + resId.ToString() + "~" + prodId.ToString() + "~" + opcjaCenowa + "~" + dodatki + "~" + opcje + "~" + count + "~" + comm;
                Store(nowy);
            }
            else
            {
                string nowy = resId.ToString() + "~" + prodId.ToString() + "~" + opcjaCenowa + "~" + dodatki + "~" + opcje + "~" + count + "~" + comm;
                int x = 0;
                foreach (string item in basket.Split('|'))
                {
                    string[] data = item.Split('~');
                    data[0] = x.ToString();
                    x++;
                }
                basket = x.ToString() +"~" +nowy + "|" + basket;
                Store(basket);
            }

            return Json(new { redirectToUrl = Url.Action("Menu", "Restaurant", new { id = Int32.Parse(resId) }) });
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
    }
}
