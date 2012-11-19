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

namespace Erestauracja.Controllers
{
    //[Authorize(Roles = "Klient")]
    public class BasketController : Controller
    {
        //
        // GET: /Basket/
        public ActionResult Index()
        {
            return View();
        }

        //wychwytuje dane z guzika - do koszyka
        public ActionResult ToBasket(string resId, string catId, string prodId, string opcjaCenowa, string dodatki, string opcje, string count, string comm)
        {
            //sprawdzić czy niepuste i czy da sie parse na count
            //
            //pobieram liste typu BasketModel z ciastek
            List<BasketModel> lista = null;
            lista = Restore();
        //    List<BasketModel> lista = new List<BasketModel>();
            if (lista.Count < 10)
            {
                BasketModel model = new BasketModel();
                model.RestaurantId = Int32.Parse(resId);
                model.CategoryID = Int32.Parse(catId);
                model.Comment = comm;
                model.Count = Int32.Parse(count);
                model.NonPriceOption = dodatki;
                model.NonPriceOption2 = opcje;
                model.PriceOption = opcjaCenowa;
                model.ProductId = Int32.Parse(prodId);

                lista.Add(model);

                Store(lista);
            }
            else
            {
                //napisać że już nie ma miejsca na ciastka
            }

            return Json(new { redirectToUrl = Url.Action("Menu", "Restaurant", new { id = Int32.Parse(resId) }) });
        }


        public void Store(List<BasketModel> myClass)
        {
            string[] ciastka = "basket1|basket2|basket3|basket4|basket5|basket6|basket7|basket8|basket9|basket10".Split('|');
            int i = 0;
            foreach (BasketModel item in myClass)
            {
                item.Id = i;
                HttpCookie cookie = new HttpCookie(ciastka[i])
                    {
                        // Set the expiry date of the cookie to 1 day
                        Expires = DateTime.Now.AddDays(1)
                    };
                Stream myStream = new MemoryStream();
                try
                {
                    // Create a binary formatter and serialize the
                    // myClass into the memorystream
                    IFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(myStream, item);
                    // Go to the beginning of the stream and
                    // fill a byte array with the contents of the
                    // memory stream
                    myStream.Seek(0, SeekOrigin.Begin);
                    byte[] buffer = new byte[myStream.Length];
                    myStream.Read(buffer, 0, (int)myStream.Length);
                    // Store the buffer as a base64 string in the cookie
                    cookie.Value = Convert.ToBase64String(buffer);
                    // Add the cookie to the current http context
                    Response.Cookies.Add(cookie);
                }
                finally
                {
                    //close the stream
                    myStream.Close();
                }
                i++;
            }
        }

        public List<BasketModel> Restore()
        {
            string ciastka = "basket1|basket2|basket3|basket4|basket5|basket6|basket7|basket8|basket9|basket10";
            List<BasketModel> lista = new List<BasketModel>();
            foreach (string cook in ciastka.Split('|'))
            {
                // Always remember to check that the cookie is not empty
                HttpCookie cookie = Request.Cookies[cook];
                if (cookie != null)
                {
                    // Convert the base64 string into a byte array
                    byte[] buffer = Convert.FromBase64String(cookie.Value);
                    // Create a memory stream from the byte array
                    Stream myStream = new MemoryStream(buffer);
                    try
                    {
                        // Create a binary formatter and deserialize the
                        // contents of the memory stream into MyClass
                        IFormatter formatter = new BinaryFormatter();
                        BasketModel streamedClass = (BasketModel)formatter.Deserialize(myStream);
                        //return streamedClass;
                        lista.Add(streamedClass);
                    }
                    finally
                    {
                        //close the stream
                        myStream.Close();
                    }
                }
            }
            return lista;
        }
    }
}
