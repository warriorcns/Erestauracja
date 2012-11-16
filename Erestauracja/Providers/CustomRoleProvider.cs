using System.Web.Security;
using System.Configuration.Provider;
using System.Collections.Specialized;
using System;
using System.Data;
//using System.Data.Odbc;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Diagnostics;
using System.Web;
using System.Globalization;

//dać create

namespace Erestauracja.Providers
{
    public sealed class CustomRoleProvider : RoleProvider
    {
        /// <summary>
        /// Generic exception event.
        /// </summary>
        private string eventSource = "CustomRoleProvider";

        /// <summary>
        /// Generic exception log info.
        /// </summary>
        private string eventLog = "Erestauracja";

        /// <summary>
        /// Generic exception message.
        /// </summary>
        private string exceptionMessage = "An exception occurred. Please check the Event Log.";

        private bool pWriteExceptionsToEventLog = false;
        /// <summary>
        /// If false, exceptions are thrown to the caller. If true,
        /// exceptions are written to the event log.
        /// </summary>
        public bool WriteExceptionsToEventLog
        {
            get { return pWriteExceptionsToEventLog; }
            set { pWriteExceptionsToEventLog = value; }
        }

        #region  System.Configuration.Provider.ProviderBase.Initialize Method
        
        /// <summary>
        /// Initialize values from web.config.
        /// </summary>
        /// <param name="name">Nazwa providera</param>
        /// <param name="config">NameValueCollection</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "CustomRoleProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Custom MySQL Role provider");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);

            if (config["applicationName"] == null || config["applicationName"].Trim() == "")
            {
                pApplicationName = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
            }
            else
            {
                pApplicationName = config["applicationName"];
            }


            if (config["writeExceptionsToEventLog"] != null)
            {
                if (config["writeExceptionsToEventLog"].ToUpper() == "TRUE")
                {
                    pWriteExceptionsToEventLog = true;
                }
            }
        }

        #endregion
        
        #region System.Web.Security.RoleProvider properties.

        private string pApplicationName;
        public override string ApplicationName
        {
            get { return pApplicationName; }
            set { pApplicationName = value; }
        }

        #endregion

        #region System.Web.Security.RoleProvider methods.

        #region Role methods:

        /// <summary>
        /// Dodaje użytkowników do ról
        /// </summary>
        /// <param name="logins">Tablica loginów typu <c>string[]</c></param>
        /// <param name="rolenames">Tablica ról typu <c>string[]</c></param>
        public override void AddUsersToRoles(string[] logins, string[] rolenames)
        {
            foreach (string rolename in rolenames)
            {
                if (!RoleExists(rolename))
                {
                    throw new ProviderException("Role name not found.");
                }
            }
            foreach (string login in logins)
            {
                if (login.Contains(","))
                {
                    throw new ArgumentException("User names cannot contain commas.");
                }
                if (login.Contains("|"))
                {
                    throw new ArgumentException("User names cannot contain this |.");
                }
                foreach (string rolename in rolenames)
                {
                    if (IsUserInRole(login, rolename))
                    {
                        throw new ProviderException("User is already in role.");
                    }
                }
            }

            bool value = false;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value = client.AddUsersToRoles(logins, rolenames);
                }
                client.Close();
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "AddUsersToRoles");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            if (value == false)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(new Exception("Dodawanie użytkowników do ról nie powiodło się."), "AddUsersToRoles");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw new Exception("Dodawanie użytkowników do ról nie powiodło się.");
                }
            }
        }

        /// <summary>
        /// Tworzy nowe role
        /// </summary>
        /// <param name="rolename">Nazwa roli</param>
        public override void CreateRole(string rolename)
        {
            if (rolename.Contains(","))
            {
                throw new ArgumentException("Role names cannot contain commas.");
            }

            if (RoleExists(rolename))
            {
                throw new ProviderException("Role name already exists.");
            }

            bool value = false;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value = client.CreateRole(rolename);
                }
                client.Close();
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "CreateRole");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            if (value == false)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(new Exception("Dodawanie nowej roli nie powiodło się."), "CreateRole");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw new Exception("Dodawanie nowej roli nie powiodło się.");
                }
            }
        }

        /// <summary>
        /// Usuwa rolę
        /// </summary>
        /// <remarks>
        /// Jeśli throwOnPopulatedRole == true, zawsze zwróci ProviderException
        /// </remarks>
        /// <param name="rolename">Nazwa roli</param>
        /// <param name="throwOnPopulatedRole">If true, throw an exception if roleName has one or more members and do not delete roleName.</param>
        /// <returns>True jeśli metoda wykonała się poprawnie</returns>
        public override bool DeleteRole(string rolename, bool throwOnPopulatedRole)
        {
            if (!RoleExists(rolename))
            {
                throw new ProviderException("Role does not exist.");
            }

            if (throwOnPopulatedRole && GetUsersInRole(rolename).Length > 0)
            {
                throw new ProviderException("Cannot delete a populated role.");
            }

            bool value = false;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value = client.DeleteRole(rolename);
                }
                client.Close();
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "DeleteRole");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            if (value == false)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(new Exception("Usuwanie nowej roli nie powiodło się."), "DeleteRole");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw new Exception("Usuwanie nowej roli nie powiodło się.");
                }
            }
            return true;
        }

        /// <summary>
        /// Pobiera nazwy wszystkich ról.
        /// </summary>
        /// <returns>Tablice typu <c>string</c> z nazwami ról</returns>
        public override string[] GetAllRoles()
        {
            string tmpRoleNames = "";

            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    tmpRoleNames = client.GetAllRoles();
                }
                client.Close();
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetAllRoles");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            if (tmpRoleNames == null)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(new Exception("Pobieranie ról nie powiodło się."), "GetAllRoles");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw new Exception("Pobieranie ról nie powiodło się.");
                }
            }

            if (tmpRoleNames.Length > 0)
            {
                // Remove trailing comma.
                tmpRoleNames = tmpRoleNames.Substring(0, tmpRoleNames.Length - 1);
                return tmpRoleNames.Split(',');
            }

            return new string[0];
        }

        /// <summary>
        /// Zwraca role przypisane do użytkownika w postaci <c>string[]</c>
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>Tablice typu <c>string</c> z nazwami ról przypisanych do użytkownika</returns>
        public override string[] GetRolesForUser(string login)
        {
            string tmpRoleNames = "";

            if (login.Contains("|"))
            {
                string[] logs = login.Split('|');
                login = logs[1];
            }
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    tmpRoleNames = client.GetRolesForUser(login);
                }
                client.Close();
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetRolesForUser");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            if (tmpRoleNames == null)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(new Exception("Pobieranie ról nie powiodło się."), "GetRolesForUser");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw new Exception("Pobieranie ról nie powiodło się.");
                }
            }

            if (tmpRoleNames.Length > 0)
            {
                // Remove trailing comma.
                tmpRoleNames = tmpRoleNames.Substring(0, tmpRoleNames.Length - 1);
                return tmpRoleNames.Split(',');
            }

            return new string[0];
        }

        /// <summary>
        /// Zwraca tablice typu <c>string[]</c> użytkowników przypisanych do danej roli
        /// </summary>
        /// <param name="rolename">Nazwa roli</param>
        /// <returns>Tablice typu <c>string</c> z nazwami użytkowników przypisanych do roli</returns>
        public override string[] GetUsersInRole(string rolename)
        {
            string tmpUserNames = "";
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    tmpUserNames = client.GetUsersInRole(rolename);
                }
                client.Close();
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUsersInRole");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            if (tmpUserNames == null)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(new Exception("Pobieranie użytkowników nie powiodło się."), "GetUsersInRole");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw new Exception("Pobieranie użytkowników nie powiodło się.");
                }
            }
            if (tmpUserNames.Length > 0)
            {
                // Remove trailing comma.
                tmpUserNames = tmpUserNames.Substring(0, tmpUserNames.Length - 1);
                return tmpUserNames.Split(',');
            }

            return new string[0];
        }

        /// <summary>
        /// Sprawdza czy użytkownik posiada określoną rolę
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="rolename">Nazwa roli</param>
        /// <returns>True jeśli użytkownik posiada role</returns>
        public override bool IsUserInRole(string login, string rolename)
        {
            bool userIsInRole = false;

            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    userIsInRole = client.IsUserInRole(login, rolename);
                }
                client.Close();
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "IsUserInRole");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }

            return userIsInRole;
        }

        /// <summary>
        /// Usuwa użytkowników z ról
        /// </summary>
        /// <param name="logins">Tablica loginów typu <c>string[]</c></param>
        /// <param name="rolenames">Tablica ról typu <c>string[]</c></param>
        public override void RemoveUsersFromRoles(string[] logins, string[] rolenames)
        {
            foreach (string rolename in rolenames)
            {
                if (!RoleExists(rolename))
                {
                    throw new ProviderException("Role name not found.");
                }
            }

            foreach (string login in logins)
            {
                foreach (string rolename in rolenames)
                {
                    if (!IsUserInRole(login, rolename))
                    {
                        throw new ProviderException("User is not in role.");
                    }
                }
            }

            bool value = false;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value = client.RemoveUsersFromRoles(logins, rolenames);
                }
                client.Close();
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "RemoveUsersFromRoles");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            if (value == false)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(new Exception("Usuwanie użytkowników z ról nie powiodło się."), "RemoveUsersFromRoles");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw new Exception("Usuwanie użytkowników z ról nie powiodło się.");
                }
            }
        }

        /// <summary>
        /// Sprawdza czy dana rola istnieje w bazie
        /// </summary>
        /// <param name="rolename">Nazwa roli</param>
        /// <returns>True jeśli rola istnieje</returns>
        public override bool RoleExists(string rolename)
        {
            bool exists = false;

            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    exists = client.RoleExists(rolename);
                }
                client.Close();
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "RoleExists");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }

            return exists;
        }

        /// <summary>
        /// Wyszukuje użytkowników z loginem rozpoczynającym się od loginToMatch, przypisanych do danej roli
        /// </summary>
        /// <param name="rolename">Nazwa roli</param>
        /// <param name="loginToMatch">Login do wyszukiwania - gdy jest null, "" lub " " wyszuka wszystkie loginy</param>
        /// <returns>Tablice typu <c>string</c> z nazwami użytkowników przypisanych do roli</returns>
        public override string[] FindUsersInRole(string rolename, string loginToMatch)
        {
            string tmpUserNames = null;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    tmpUserNames = client.FindUsersInRole(rolename, loginToMatch);
                }
                client.Close();
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "FindUsersInRole");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            if (tmpUserNames == null)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(new Exception("Pobieranie użytkowników nie powiodło się."), "FindUsersInRole");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw new Exception("Pobieranie użytkowników nie powiodło się.");
                }
            }

            if (tmpUserNames.Length > 0)
            {
                // Remove trailing comma.
                tmpUserNames = tmpUserNames.Substring(0, tmpUserNames.Length - 1);
                return tmpUserNames.Split(',');
            }

            return new string[0];
        }

        #endregion

        /// <summary>
        /// A helper function that writes exception detail to the event log. Exceptions
        /// are written to the event log as a security measure to avoid private database
        /// details from being returned to the browser. If a method does not return a status
        /// or boolean indicating the action succeeded or failed, a generic exception is also 
        /// thrown by the caller.
        /// </summary>
        /// <param name="e">Exception</param>
        /// <param name="action">Nazwa wykonywaniej akcji</param>
        private void WriteToEventLog(Exception e, string action)
        {
            /*
            * 
            * If the sample provider encounters an exception when working with the data source, it writes the details of the exception to the Application Event Log instead of returning the exception to the ASP.NET application. This is done as a security measure to avoid exposing private information about the data source in the ASP.NET application.
            * The sample provider specifies an event Source of "OdbcRoleProvider". Before your ASP.NET application will be able to write to the Application Event Log successfully, you will need to create the following registry key.
            * HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Services\Eventlog\Application\OdbcRoleProvider
            * 
            */
            EventLog log = new EventLog();
            log.Source = eventSource;
            log.Log = eventLog;

            string message = "An exception occurred communicating with the data source.\n\n";
            message += "Action: " + action + "\n\n";
            message += "Exception: " + e.ToString();

            log.WriteEntry(message, EventLogEntryType.Error);
        }
        
        #endregion
    }
}