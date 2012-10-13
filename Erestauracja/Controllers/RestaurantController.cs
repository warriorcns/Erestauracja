using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;


namespace Erestauracja.Controllers
{
    //[Authorize(Roles = "Klient, Menadżer, PracownikFull, PracownikLow")]
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
            
            //dziala pobieranie do c:/Data
            //DownloadFiles("*.jpg");

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

        public static void DownloadFileFTP(string ftpfilepath)
        {
            // The serverUri parameter should start with the ftp:// scheme. 
            // ftp://5.153.38.77


            string inputfilepath = @"C:\Temp\FileName.exe";
            string ftphost = "5.153.38.77";
            // string ftpfilepath = "/Updater/Dir1/FileName.exe";


            string ftpfullpath = "ftp://" + ftphost + ftpfilepath;

            WebClient request = new WebClient();
            request.Credentials = new NetworkCredential("erestauracja", "Erestauracja123");
            byte[] fileData = request.DownloadData(ftpfullpath);

            FileStream file = System.IO.File.Create(inputfilepath);
            file.Write(fileData, 0, fileData.Length);
            file.Close();
            //MessageBox.Show("Download Complete");
        }


        public void DownloadFiles(string WildCard)
        {
            //WildCard = "*Parts.csv"
            string[] Files = GetFiles(WildCard);
            foreach (string file in Files)
            {
                DownloadFile(file);
            }
        }

        private string[] GetFiles(string WildCard)
        {
            string ReturnStr = "";
            //Connect to the FTP
            //FtpWebRequest request = WebRequest.Create("ftp://5.153.38.77/" + WildCard) as FtpWebRequest;
            FtpWebRequest request = WebRequest.Create("ftp://cba.pl/" + WildCard) as FtpWebRequest;
            //Specify we're Listing a directory
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential("erestauracja@eres.cba.pl", "Erestauracja123");

            StringWriter sw = new StringWriter();
            //Get a reponse
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();

            //Convert the response to a string
            int ch;
            while (( ch = responseStream.ReadByte() ) != -1)
                ReturnStr = ReturnStr + Convert.ToChar(ch);

            //clean up
            responseStream.Close();
            response.Close();

            //split the string by new line
            string[] sep = { "\r\n" };
            string[] Files = ReturnStr.Split(sep, StringSplitOptions.RemoveEmptyEntries);
            return Files;
        }

        private void DownloadFile(string FileName)
        {
            //Connect to the FTP
            //FtpWebRequest request = WebRequest.Create("ftp://5.153.38.77/" + FileName) as FtpWebRequest;
            FtpWebRequest request = WebRequest.Create("ftp://cba.pl/" + FileName) as FtpWebRequest;
            //Specify we're downloading a file
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential("erestauracja@eres.cba.pl", "Erestauracja123");

            //initialize the Filestream we're using to create the downloaded file locally
            FileStream localfileStream = new FileStream(@"c:\Data\" + FileName, FileMode.Create, FileAccess.Write);

            //Get a reponse
            WebResponse response = request.GetResponse();
            Stream responseStream = response.GetResponseStream();

            //create the file
            byte[] buffer = new byte[1024];
            int bytesRead = responseStream.Read(buffer, 0, 1024);
            while (bytesRead != 0)
            {
                localfileStream.Write(buffer, 0, bytesRead);
                bytesRead = responseStream.Read(buffer, 0, 1024);
            }

            //clean up
            localfileStream.Close();
            response.Close();
            responseStream.Close();

        }
    }
}
