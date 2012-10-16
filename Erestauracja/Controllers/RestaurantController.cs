using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
//using RestSharp;
using Erestauracja.Models;
using AppLimit.CloudComputing.SharpBox;
using AppLimit.CloudComputing.SharpBox.StorageProvider.DropBox;


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

            System.Web.HttpCookie myCookie = new System.Web.HttpCookie("MyTestCookie");
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
            System.Web.HttpCookie myCookie = new System.Web.HttpCookie("MyTestCookie");
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
            System.Web.HttpCookie myCookie = new System.Web.HttpCookie("MyTestCookie");
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
            System.Web.HttpCookie myCookie = new System.Web.HttpCookie("MyTestCookie");
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
            System.Web.HttpCookie myCookie = new System.Web.HttpCookie("MyTestCookie");
            myCookie = Request.Cookies["MyTestCookie"];
            int id;
            if (myCookie != null)
            {
                id = int.Parse(myCookie.Value);
            }
            #endregion
            return View();
        }

        public ActionResult Gallery(int id)
        {
            #region ciasteczka - odczyt
            System.Web.HttpCookie myCookie = new System.Web.HttpCookie("MyTestCookie");
            myCookie = Request.Cookies["MyTestCookie"];
            //int id;
            if (myCookie != null)
            {
                id = int.Parse(myCookie.Value);
            }
            #endregion

            #region DropBox Connection
            try
            {
                // Creating the cloudstorage object 
                CloudStorage dropBoxStorage = new CloudStorage();

                // get the configuration for dropbox 
                var dropBoxConfig = CloudStorage.GetCloudConfigurationEasy(nSupportedCloudConfigurations.DropBox);

                // declare an access token
                ICloudStorageAccessToken accessToken = null;

                // load a valid security token from file
                string path = Server.MapPath(Url.Content("~/Content/token.txt"));

                //using (FileStream fs = System.IO.File.Open("C:\\dropboxtoken.txt", FileMode.Open, FileAccess.Read, FileShare.None))
                using (FileStream fs = System.IO.File.Open(path, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    accessToken = dropBoxStorage.DeserializeSecurityToken(fs);
                }

                // open the connection 
                var storageToken = dropBoxStorage.Open(dropBoxConfig, accessToken);

                // get a specific directory in the cloud storage eg. "/images/1"
                var publicFolder = dropBoxStorage.GetFolder("/Public/images/" + id.ToString());

                ICloudFileSystemEntry fse;

                // lista linkow uri
                List<Uri> uris = new List<Uri>();

                // enumerate all child (folder and files) 
                foreach (var fof in publicFolder)
                {
                    // check if we have a directory 
                    Boolean bIsDirectory = fof is ICloudDirectoryEntry;

                    fse = dropBoxStorage.GetFileSystemObject(fof.Name, publicFolder);
                    if (!bIsDirectory)
                    {
                        //pobiera liste linkow do plikow w katalogu rodzica
                        uris.Add(DropBoxStorageProviderTools.GetPublicObjectUrl(accessToken, fse));
                    }
                }

                dropBoxStorage.Close();

                ViewData["imagesuris"] = uris;
                string test, test1;
                foreach (Uri link in uris)
                {

                    test = link.ToString();
                    test1 = link.AbsoluteUri;
                }
            }
            catch (AppLimit.CloudComputing.SharpBox.Exceptions.SharpBoxException ex)
            {
                ;
            }
            #endregion

            return View();
        }

        public ActionResult Contact()
        {
            #region ciasteczka - odczyt
            System.Web.HttpCookie myCookie = new System.Web.HttpCookie("MyTestCookie");
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

        //stara metoda.
        private void connection()
        {
            // Creating the cloudstorage object 
            CloudStorage dropBoxStorage = new CloudStorage();

            // get the configuration for dropbox 
            var dropBoxConfig = CloudStorage.GetCloudConfigurationEasy(nSupportedCloudConfigurations.DropBox);

            // declare an access token
            ICloudStorageAccessToken accessToken = null;

            // load a valid security token from file
            using (FileStream fs = System.IO.File.Open("C:\\dropboxtoken.txt", FileMode.Open, FileAccess.Read, FileShare.None))
            {
                accessToken = dropBoxStorage.DeserializeSecurityToken(fs);
            }


            // open the connection 
            var storageToken = dropBoxStorage.Open(dropBoxConfig, accessToken);




            // close the connection 
            dropBoxStorage.Close();
            
        }
       
    }
}
