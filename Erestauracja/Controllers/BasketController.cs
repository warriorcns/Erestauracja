using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Mvc;
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
                    if(newlista.Length == item.Length)
                    {
                        newlista = newlista.Remove(index, item.Length);
                    }
                    else
                    {
                        newlista = (index == 0) ? newlista.Remove(index, item.Length + 1) : newlista.Remove(index - 1, item.Length + 1);
                    }
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

        public ActionResult Realize(String data, string error = null)
        {
            if (error != null)
            {
                ViewData["error"] = error;
                return View();
            }

            //destrializacja obiektu zawierającego dane z koszyka
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
            //sprawdzić czy liczba nie przekracza 20
            int count = -1;
            foreach(BasketProduct item in rest.Products)
            {
                count += item.Count;
                if (count > 20)
                {
                    ViewData["error"] = "Maksymalna ilość zamówionych produktów w jednym zamówieniu to 20";
                    return View();
                }
            }

            //wysyłanie zamówienia
            int value = -1;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value = client.SaveOrder(User.Identity.Name, rest);
                }
                client.Close();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Zapisywanie zamówienia nie powiodło się.");
            }
            if (value == -1)
            {
                ModelState.AddModelError("", "Zapisywanie zamówienia nie powiodło się.");

                return RedirectToAction("Index");
            }
            else
            {
                ViewData["id"] = value;
            }

            return View(rest);
        }

        public ActionResult Cash(string com, int id, int res)
        {
            bool test = false;
            try
            {
                Erestauracja.ServiceReference.EresServiceClient client = new Erestauracja.ServiceReference.EresServiceClient();
                using (client)
                {
                    test = client.IsRestaurantOnline(res);
                }
                client.Close();
            }
            catch (Exception e)
            {
                return RedirectToAction("Realize", new { data = "", error = "Błąd podczas sprawdzania czy restauracja jest dostępna" });
            }

            if (test == false)
            {
                return RedirectToAction("Realize", new { data = "", error = "Restauracja jest offline" });
            }
/////////////////////////////////////////////////////////////
            bool value = false;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value = client.Pay(User.Identity.Name, id, com, "cash");

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
                return RedirectToAction("PayError", new { id = id });
            }
            else
            {
                //usuwanie zamówionych dań z koszyka
                DeleteRest(res);

                //wyświetl potwierdzenie
                //z info że ok że może zobaczyć w aktualnych zamówieniach i że dostał email
                //zapisz id zamówienia że zostało zapłacone

                return RedirectToAction("PaySuccess", new { id = id });
            }

            return RedirectToAction("PayError", new { id = id });
        }

        public ActionResult PayError(int id = -1)
        {
            if (id > 0)
            {
                ViewData["id"] = id;
            }
            else
            {
                ViewData["id"] = null;
            }
            return View();

        }

        public ActionResult PaySuccess(int? id)
        {
            if (id > 0)
            {
                ViewData["id"] = id;
            }
            else
            {
                ViewData["id"] = null;
            }
            return View();

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

      //  private delegate bool Uchwyt(int arg);

        public string IsOnline(int id)
        {
            bool value = false;
            try
            {
                Erestauracja.ServiceReference.EresServiceClient client = new Erestauracja.ServiceReference.EresServiceClient();
                using (client)
                {
                  //  Uchwyt IsOnline = new Uchwyt(client.IsRestaurantOnline);
                  //  value = IsOnline(id);
                    value = client.IsRestaurantOnline(id);
                }
                client.Close();
            }
            catch (Exception e)
            {
                value = false;
            }

            if (value == false)
            {
                return "Offline";
            }
            else
            {
                return "Online";
            }
        }
    }
}
