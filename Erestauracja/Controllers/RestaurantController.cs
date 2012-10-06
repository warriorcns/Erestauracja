using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Erestauracja.Controllers
{
    [Authorize(Roles = "Klient, Menadżer, PracownikFull, PracownikLow")]
    public class RestaurantController : Controller
    {
        //public int Restaurantid ; 
        //
        // GET: /Restaurant/
        public ActionResult GetRequest(int id) 
        {
            //this.Restaurantid = id;
            //return RedirectToAction ("Index", new { id = id });

            #region ciasteczka - zapis

            HttpCookie myCookie = new HttpCookie("MyTestCookie");
            DateTime now = DateTime.Now;
            myCookie.Expires = now.AddMinutes(1);
            myCookie.Value = id.ToString();
            Response.Cookies.Add(myCookie);

            #endregion

            return Json(new { redirectToUrl = Url.Action("Index/" + id) });
        }


        public ActionResult Index()
        {
            //return RedirectToAction("Index");
            //this.Restaurantid = id;
            //int test = this.Restaurantid;

            #region ciasteczka - odczyt
            HttpCookie myCookie = new HttpCookie("MyTestCookie");
            myCookie = Request.Cookies["MyTestCookie"];
            int id;
            if (myCookie != null)
            {
                id = int.Parse(myCookie.Value);
            }
            #endregion

            return View();
        }

        public ActionResult Menu()
        {
            #region ciasteczka - odczyt
            HttpCookie myCookie = new HttpCookie("MyTestCookie");
            myCookie = Request.Cookies["MyTestCookie"];
            int id;
            if (myCookie != null)
            {
                id = int.Parse(myCookie.Value);
            }
            #endregion

            return View();
        }

        public ActionResult Delivery()
        {
            #region ciasteczka - odczyt
            HttpCookie myCookie = new HttpCookie("MyTestCookie");
            myCookie = Request.Cookies["MyTestCookie"];
            int id;
            if (myCookie != null)
            {
                id = int.Parse(myCookie.Value);
            }
            #endregion
            return View();
        }

        public ActionResult Parties()
        {
            #region ciasteczka - odczyt
            HttpCookie myCookie = new HttpCookie("MyTestCookie");
            myCookie = Request.Cookies["MyTestCookie"];
            int id;
            if (myCookie != null)
            {
                id = int.Parse(myCookie.Value);
            }
            #endregion
            return View();
        }

        public ActionResult Gallery()
        {
            #region ciasteczka - odczyt
            HttpCookie myCookie = new HttpCookie("MyTestCookie");
            myCookie = Request.Cookies["MyTestCookie"];
            int id;
            if (myCookie != null)
            {
                id = int.Parse(myCookie.Value);
            }
            #endregion

            return View();
        }

        public ActionResult Contact()
        {
            #region ciasteczka - odczyt
            HttpCookie myCookie = new HttpCookie("MyTestCookie");
            myCookie = Request.Cookies["MyTestCookie"];
            int id;
            if (myCookie != null)
            {
                id = int.Parse(myCookie.Value);
            }
            #endregion
            return View();
        }

        public ActionResult Test()
        {
            return View();

        }
    }
}
