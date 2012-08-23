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

namespace Erestauracja.Providers
{
    public sealed class CustomRoleProvider : RoleProvider
    {
        //
        // Global connection string, generic exception message, event log info.
        //

      //  private string eventSource = "CustomRoleProvider";
      //  private string eventLog = "Erestauracja";
     //   private string exceptionMessage = "An exception occurred. Please check the Event Log.";

        private ConnectionStringSettings pConnectionStringSettings;
        private string connectionString;

        //
        // If false, exceptions are thrown to the caller. If true,
        // exceptions are written to the event log.
        //

   //     private bool pWriteExceptionsToEventLog = false;

    //    public bool WriteExceptionsToEventLog
   //     {
    //        get { return pWriteExceptionsToEventLog; }
    //        set { pWriteExceptionsToEventLog = value; }
    //    }

        //
        // System.Configuration.Provider.ProviderBase.Initialize Method
        //

        public override void Initialize(string name, NameValueCollection config)
        {

            //
            // Initialize values from web.config.
            //

            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "CustomRoleProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Sample MySQL Role provider");
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


            //if (config["writeExceptionsToEventLog"] != null)
            //{
            //    if (config["writeExceptionsToEventLog"].ToUpper() == "TRUE")
            //    {
            //        pWriteExceptionsToEventLog = true;
            //    }
            //}


            //
            // InitializeMySQLConnection.
            //

            pConnectionStringSettings = ConfigurationManager.
              ConnectionStrings[config["connectionStringName"]];

            if (pConnectionStringSettings == null || pConnectionStringSettings.ConnectionString.Trim() == "")
            {
                throw new ProviderException("Connection string cannot be blank.");
            }

            connectionString = pConnectionStringSettings.ConnectionString;
        }

        #region System.Web.Security.RoleProvider properties.

        private string pApplicationName;

        public override string ApplicationName
        {
            get { return pApplicationName; }
            set { pApplicationName = value; }
        }

        #endregion

        #region System.Web.Security.RoleProvider methods.

        //
        #region RoleProvider.AddUsersToRoles
        //

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

                foreach (string rolename in rolenames)
                {
                    if (IsUserInRole(login, rolename))
                    {
                        throw new ProviderException("User is already in role.");
                    }
                }
            }

            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.AddUsersToRoles);
            //  MySqlParameter loginParm = command.Parameters.Add("@login", MySqlDbType.VarChar, 255);
            //  MySqlParameter roleParm = command.Parameters.Add("@rolename", MySqlDbType.VarChar, 255);
            //jak nie działa to parametry czyścić i dodawać w foreach
            //  command.Parameters.AddWithValue("@applicationName", pApplicationName);
            command.Connection = conn;

            MySqlTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                command.Transaction = tran;

                foreach (string login in logins)
                {
                    foreach (string rolename in rolenames)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@login", login);
                        command.Parameters.AddWithValue("@rolename", rolename);
                        command.Parameters.AddWithValue("@applicationName", pApplicationName);
                        //  loginParm.Value = login;
                        //  roleParm.Value = rolename;
                        command.ExecuteNonQuery();
                    }
                }

                tran.Commit();
            }
            catch (MySqlException e)
            {
                try
                {
                    tran.Rollback();
                }
                catch { }


                //if (WriteExceptionsToEventLog)
                //{
                //    WriteToEventLog(e, "AddUsersToRoles");
                //}
                //else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion
        //
        #region RoleProvider.CreateRole
        //

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

            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.CreateRole);
            command.Parameters.AddWithValue("@rolename", rolename);
            command.Parameters.AddWithValue("@applicationName", ApplicationName);

            command.Connection = conn;

            try
            {
                conn.Open();

                command.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                //if (WriteExceptionsToEventLog)
                //{
                //    WriteToEventLog(e, "CreateRole");
                //}
                //else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        //
        #region RoleProvider.DeleteRole
        //

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

            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.DeleteRole);
            command.Parameters.AddWithValue("@rolename", rolename);
            command.Parameters.AddWithValue("@applicationName", ApplicationName);
            command.Connection = conn;

            MySqlCommand command2 = new MySqlCommand(Queries.DeleteUsersInRole);
            command2.Parameters.AddWithValue("@rolename", rolename);
            command2.Parameters.AddWithValue("@applicationName", ApplicationName);
            command2.Connection = conn;

            MySqlTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                command.Transaction = tran;
                command2.Transaction = tran;

                command2.ExecuteNonQuery();
                command.ExecuteNonQuery();

                tran.Commit();
            }
            catch (MySqlException e)
            {
                try
                {
                    tran.Rollback();
                }
                catch { }


                //if (WriteExceptionsToEventLog)
                //{
                //    WriteToEventLog(e, "DeleteRole");

                //    return false;
                //}
                //else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }

            return true;
        }
        #endregion

        //
        #region RoleProvider.GetAllRoles
        //

        public override string[] GetAllRoles()
        {
            string tmpRoleNames = "";

            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.GetAllRoles);
            command.Parameters.AddWithValue("@applicationName", ApplicationName);

            command.Connection = conn;

            MySqlDataReader reader = null;

            try
            {
                conn.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tmpRoleNames += reader.GetString(0) + ",";
                }
            }
            catch (MySqlException e)
            {
                //if (WriteExceptionsToEventLog)
                //{
                //    WriteToEventLog(e, "GetAllRoles");
                //}
                //else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            if (tmpRoleNames.Length > 0)
            {
                // Remove trailing comma.
                tmpRoleNames = tmpRoleNames.Substring(0, tmpRoleNames.Length - 1);
                return tmpRoleNames.Split(',');
            }

            return new string[0];
        }
        #endregion

        //
        #region RoleProvider.GetRolesForUser
        //

        public override string[] GetRolesForUser(string login)
        {
            string tmpRoleNames = "";

            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.GetRolesForUser);
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@applicationName", ApplicationName);

            command.Connection = conn;

            MySqlDataReader reader = null;

            try
            {
                conn.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tmpRoleNames += reader.GetString(0) + ",";
                }
            }
            catch (MySqlException e)
            {
                //if (WriteExceptionsToEventLog)
                //{
                //    WriteToEventLog(e, "GetRolesForUser");
                //}
                //else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            if (tmpRoleNames.Length > 0)
            {
                // Remove trailing comma.
                tmpRoleNames = tmpRoleNames.Substring(0, tmpRoleNames.Length - 1);
                return tmpRoleNames.Split(',');
            }

            return new string[0];
        }
        #endregion

        //
        #region RoleProvider.GetUsersInRole
        //

        public override string[] GetUsersInRole(string rolename)
        {
            string tmpUserNames = "";

            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.GetUsersInRole);
            command.Parameters.AddWithValue("@rolename", rolename);
            command.Parameters.AddWithValue("@applicationName", ApplicationName);

            command.Connection = conn;

            MySqlDataReader reader = null;
            try
            {
                conn.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tmpUserNames += reader.GetString(0) + ",";
                }
            }
            catch (MySqlException e)
            {
                //if (WriteExceptionsToEventLog)
                //{
                //    WriteToEventLog(e, "GetUsersInRole");
                //}
                //else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
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

        //
        #region RoleProvider.IsUserInRole
        //

        public override bool IsUserInRole(string login, string rolename)
        {
            bool userIsInRole = false;

            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.IsUserInRole);
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@rolename", rolename);
            command.Parameters.AddWithValue("@applicationName", ApplicationName);

            command.Connection = conn;

            try
            {
                conn.Open();

                // int numRecs = (int)command.ExecuteScalar();
                //Decimal numRecs = (decimal)(command.ExecuteScalar());
                object obj = command.ExecuteScalar();
                int numRecs = (obj == null ? 0 : Convert.ToInt32(obj.ToString()));
                if (numRecs > 0)
                {
                    userIsInRole = true;
                }
            }
            catch (MySqlException e)
            {
                //if (WriteExceptionsToEventLog)
                //{
                //    WriteToEventLog(e, "IsUserInRole");
                //}
                //else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }

            return userIsInRole;
        }
        #endregion

        //
        #region RoleProvider.RemoveUsersFromRoles
        //

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

            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.RemoveUsersFromRoles);
            //   MySqlParameter loginParm = command.Parameters.Add("@login", MySqlDbType.VarChar, 255);
            //    MySqlParameter roleParm = command.Parameters.Add("@rolename", MySqlDbType.VarChar, 255);
            //jak nie działa to parametry czyścić i dodawać w foreach
            //   command.Parameters.AddWithValue("@applicationName", pApplicationName);
            command.Connection = conn;

            MySqlTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                command.Transaction = tran;

                foreach (string login in logins)
                {
                    foreach (string rolename in rolenames)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@login", login);
                        command.Parameters.AddWithValue("@rolename", rolename);
                        command.Parameters.AddWithValue("@applicationName", pApplicationName);
                        //  loginParm.Value = login;
                        //  roleParm.Value = rolename;
                        command.ExecuteNonQuery();
                    }
                }

                tran.Commit();
            }
            catch (MySqlException e)
            {
                try
                {
                    tran.Rollback();
                }
                catch { }


                //if (WriteExceptionsToEventLog)
                //{
                //    WriteToEventLog(e, "RemoveUsersFromRoles");
                //}
                //else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        //
        #region RoleProvider.RoleExists
        //

        public override bool RoleExists(string rolename)
        {
            bool exists = false;

            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.RoleExists);
            command.Parameters.AddWithValue("@rolename", rolename);
            command.Parameters.AddWithValue("@applicationName", ApplicationName);

            command.Connection = conn;

            try
            {
                conn.Open();

                //Decimal numRecs = (decimal)(command.ExecuteScalar());
                // int numRecs = Convert.ToInt32(command.ExecuteScalar());
                object obj = command.ExecuteScalar();
                int numRecs = (obj == null ? 0 : Convert.ToInt32(obj.ToString()));
                if (numRecs > 0)
                {
                    exists = true;
                }
            }
            catch (MySqlException e)
            {
                //if (WriteExceptionsToEventLog)
                //{
                //    WriteToEventLog(e, "RoleExists");
                //}
                //else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }

            return exists;
        }
        #endregion
        //
        #region RoleProvider.FindUsersInRole
        //

        public override string[] FindUsersInRole(string rolename, string loginToMatch)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.FindUsersInRole);
            command.Parameters.AddWithValue("@login", loginToMatch);
            command.Parameters.AddWithValue("@rolename", rolename);
            command.Parameters.AddWithValue("@applicationName", pApplicationName);

            command.Connection = conn;

            string tmpUserNames = "";
            MySqlDataReader reader = null;

            try
            {
                conn.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    tmpUserNames += reader.GetString(0) + ",";
                }
            }
            catch (MySqlException e)
            {
                //if (WriteExceptionsToEventLog)
                //{
                //    WriteToEventLog(e, "FindUsersInRole");
                //}
                //else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }

                conn.Close();
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
        #endregion
        ////
        //// WriteToEventLog
        ////   A helper function that writes exception detail to the event log. Exceptions
        //// are written to the event log as a security measure to avoid private database
        //// details from being returned to the browser. If a method does not return a status
        //// or boolean indicating the action succeeded or failed, a generic exception is also 
        //// thrown by the caller.
        ////

        //private void WriteToEventLog(MySqlException e, string action)
        //{
        //    EventLog log = new EventLog();
        //    log.Source = eventSource;
        //    log.Log = eventLog;

        //    string message = exceptionMessage + "\n\n";
        //    message += "Action: " + action + "\n\n";
        //    message += "Exception: " + e.ToString();

        //    log.WriteEntry(message);
        //}
    }
}