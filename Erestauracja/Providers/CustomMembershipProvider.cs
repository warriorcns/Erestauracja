using System.Web.Security;
using System.Configuration.Provider;
using System.Collections.Specialized;
using System;
using System.Data;
//using System.Data.Odbc;
using MySql.Data;
using System.Configuration;
using System.Diagnostics;
using System.Web;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using MySql.Data.MySqlClient;
using System.Net.Mail;
using System.Net;

/*
This provider works with the following schema for the table of user data.

CREATE TABLE Users
(
  //PKID Guid NOT NULL PRIMARY KEY,
  Username Text (255) NOT NULL, - login
  //ApplicationName Text (255) NOT NULL,
  Email Text (128) NOT NULL,
  //Comment Text (255),
  Password Text (128) NOT NULL,
  PasswordQuestion Text (255),
  PasswordAnswer Text (255),
  IsApproved YesNo, 
  //LastActivityDate DateTime,
  //LastLoginDate DateTime,
  //LastPasswordChangedDate DateTime,
  //CreationDate DateTime, 
  //IsOnLine YesNo,
  //IsLockedOut YesNo,
  //LastLockedOutDate DateTime,
  //FailedPasswordAttemptCount Integer,
  //FailedPasswordAttemptWindowStart DateTime,
  //FailedPasswordAnswerAttemptCount Integer,
  //FailedPasswordAnswerAttemptWindowStart DateTime
)
*/

namespace Erestauracja.Providers
{
    public sealed class CustomMembershipProvider : MembershipProvider
    {
        //
        // Global connection string, generated password length, generic exception message, event log info.
        //

        //#region database
        //private DataSet ExecuteQuery(MySqlCommand command)
        //{
        //    MySqlConnection conn = new MySqlConnection(connectionString);
        //    conn.Open();
        //    command.Connection = conn;

        //    DataSet myDS = new DataSet();

        //    try
        //    {
        //        MySqlDataAdapter da = new MySqlDataAdapter(command);
        //        da.Fill(myDS);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }

        //    finally
        //    {
        //        //conn.Clone();
        //        conn.Close();
        //    }

        //    return myDS;
        //}
        //private int ExecuteNonQuery(MySqlCommand command, string action)
        //{
        //    MySqlConnection conn = new MySqlConnection(connectionString);
        //    //conn.Open();
        //    command.Connection = conn;

        //    int rowsaffected = 0;

        //    try
        //    {
        //        conn.Open();
        //        rowsaffected = command.ExecuteNonQuery();
        //    }
        //    catch (MySqlException e)
        //    {
        //        if (WriteExceptionsToEventLog)
        //        {
        //            WriteToEventLog(e, action);
        //            throw new ProviderException(exceptionMessage);
        //        }
        //        else
        //        {
        //            throw e;
        //        }
        //    }

        //    finally
        //    {
        //        conn.Close();
        //    }

        //    return rowsaffected;
        //}
        //#endregion

        private int newPasswordLength = 8;
        private string eventSource = "CustomMembershipProvider";
        private string eventLog = "Erestauracja";
        private string exceptionMessage = "An exception occurred. Please check the Event Log.";
        private string connectionString;

        //
        // Used when determining encryption key values.
        //

        private MachineKeySection machineKey;

        //
        // If false, exceptions are thrown to the caller. If true,
        // exceptions are written to the event log.
        //

        private bool pWriteExceptionsToEventLog;

        public bool WriteExceptionsToEventLog
        {
            get { return pWriteExceptionsToEventLog; }
            set { pWriteExceptionsToEventLog = value; }
        }

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
                name = "CustomMembershipProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Custom MySQL Membership provider");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);

            pApplicationName = GetConfigValue(config["applicationName"],
                                            System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            pMaxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            pPasswordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            pMinRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredNonAlphanumericCharacters"], "1"));
            pMinRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "7"));
            pPasswordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], ""));
            pEnablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            pEnablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"));
            pRequiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"));
            pRequiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));
            pWriteExceptionsToEventLog = Convert.ToBoolean(GetConfigValue(config["writeExceptionsToEventLog"], "true"));

            string temp_format = config["passwordFormat"];
            if (temp_format == null)
            {
                temp_format = "Hashed";
            }

            switch (temp_format)
            {
                case "Hashed":
                    pPasswordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "Encrypted":
                    pPasswordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    pPasswordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw new ProviderException("Password format not supported.");
            }

            //
            // Initialize MySQLConnection.
            //

            ConnectionStringSettings ConnectionStringSettings =
              ConfigurationManager.ConnectionStrings[config["connectionStringName"]];

            if (ConnectionStringSettings == null || ConnectionStringSettings.ConnectionString.Trim() == "")
            {
                throw new ProviderException("Connection string cannot be blank.");
            }

            connectionString = ConnectionStringSettings.ConnectionString;


            // Get encryption and decryption key information from the configuration.
            Configuration cfg =
              WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            machineKey = (MachineKeySection)cfg.GetSection("system.web/machineKey");

            if (machineKey.ValidationKey.Contains("AutoGenerate"))
                if (PasswordFormat != MembershipPasswordFormat.Clear)
                    throw new ProviderException("Hashed or Encrypted passwords " +
                                                "are not supported with auto-generated keys.");
        }

        //
        // A helper function to retrieve config values from the configuration file.
        //

        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (String.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }

        //
        // System.Web.Security.MembershipProvider properties.
        //

        private string pApplicationName;
        private bool pEnablePasswordReset;
        private bool pEnablePasswordRetrieval;
        private bool pRequiresQuestionAndAnswer;
        private bool pRequiresUniqueEmail;
        private int pMaxInvalidPasswordAttempts;
        private int pPasswordAttemptWindow;
        private MembershipPasswordFormat pPasswordFormat;

        public override string ApplicationName
        {
            get { return pApplicationName; }
            set { pApplicationName = value; }
        }

        public override bool EnablePasswordReset
        {
            get { return pEnablePasswordReset; }
        }

        public override bool EnablePasswordRetrieval
        {
            get { return pEnablePasswordRetrieval; }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { return pRequiresQuestionAndAnswer; }
        }

        public override bool RequiresUniqueEmail
        {
            get { return pRequiresUniqueEmail; }
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { return pMaxInvalidPasswordAttempts; }
        }

        public override int PasswordAttemptWindow
        {
            get { return pPasswordAttemptWindow; }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { return pPasswordFormat; }
        }

        private int pMinRequiredNonAlphanumericCharacters;

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return pMinRequiredNonAlphanumericCharacters; }
        }

        private int pMinRequiredPasswordLength;

        public override int MinRequiredPasswordLength
        {
            get { return pMinRequiredPasswordLength; }
        }

        private string pPasswordStrengthRegularExpression;

        public override string PasswordStrengthRegularExpression
        {
            get { return pPasswordStrengthRegularExpression; }
        }

        //
        // System.Web.Security.MembershipProvider methods.
        //

        //
        // MembershipProvider.ChangePassword
        //

        public override bool ChangePassword(string login, string oldPwd, string newPwd)
        {
            if (!ValidateUser(login, oldPwd))
                return false;


            ValidatePasswordEventArgs args =
              new ValidatePasswordEventArgs(login, newPwd, true);

            OnValidatingPassword(args);

            if (args.Cancel)
                if (args.FailureInformation != null)
                    throw args.FailureInformation;
                else
                    throw new MembershipPasswordException("Change password canceled due to new password validation failure.");

            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.ChangePassword);
            command.Parameters.AddWithValue("@password", EncodePassword(newPwd));
            command.Parameters.AddWithValue("@lastPasswordChangedDate", DateTime.Now);
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@applicationName", pApplicationName);
            command.Connection = conn;

            int rowsaffected = 0;

            try
            {
                conn.Open();
                rowsaffected = command.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ChangePassword");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }

            finally
            {
                conn.Close();
            }

            if (rowsaffected > 0)
            {
                return true;
            }
            return false;
        }

        //
        // MembershipProvider.ChangePasswordQuestionAndAnswer
        //

        public override bool ChangePasswordQuestionAndAnswer(string login, string password, string newPwdQuestion, string newPwdAnswer)
        {
            if (!ValidateUser(login, password))
                return false;

            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.ChangePasswordQuestionAndAnswer);
            command.Parameters.AddWithValue("@question", newPwdQuestion);
            command.Parameters.AddWithValue("@answer", EncodePassword(newPwdAnswer));
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@applicationName", pApplicationName);
            command.Connection = conn;

            int rowsaffected = 0;

            try
            {
                conn.Open();
                rowsaffected = command.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ChangePasswordQuestionAndAnswer");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }

            finally
            {
                conn.Close();
            }

            if (rowsaffected > 0)
            {
                return true;
            }
            return false;
        }

        ////
        //// MembershipProvider.CreateUser
        ////

        //public override MembershipUser CreateUser(string login,
        //    string password,
        //    string email,
        //    string name,
        //    string surname,
        //    string address,
        //    int townID,//tu zmienić na nazwe miasta lub kod i select do zapytania
        //    string country,
        //    DateTime birthdate,
        //    string sex,
        //    string telephone,
        //    string passwordQuestion,
        //    string passwordAnswer,
        //    bool isApproved,
        //    object providerUserKey,
        //    out MembershipCreateStatus status)
        //{
        //    ValidatePasswordEventArgs args =
        //      new ValidatePasswordEventArgs(login, password, true);

        //    OnValidatingPassword(args);

        //    if (args.Cancel)
        //    {
        //        status = MembershipCreateStatus.InvalidPassword;
        //        return null;
        //    }

        //    if (RequiresUniqueEmail && GetUserNameByEmail(email) != "")
        //    {
        //        status = MembershipCreateStatus.DuplicateEmail;
        //        return null;
        //    }

        //    MembershipUser u = GetUser(login, false);

        //    if (u == null)
        //    {
        //        DateTime createDate = DateTime.Now;

        //        //  if (providerUserKey == null)
        //        //  {
        //        //       providerUserKey = Guid.NewGuid();
        //        //   }
        //        //else
        //        //{
        //        //    if (!(providerUserKey is Guid))
        //        //    {
        //        //        status = MembershipCreateStatus.InvalidProviderUserKey;
        //        //        return null;
        //        //    }
        //        //}

        //        MySqlConnection conn = new MySqlConnection(connectionString);
        //        MySqlCommand command = new MySqlCommand(Queries.CreateUser);

        //        command.Parameters.AddWithValue("@login ", login);
        //        command.Parameters.AddWithValue("@password ", EncodePassword(password));
        //        command.Parameters.AddWithValue("@email ", email);
        //        command.Parameters.AddWithValue("@name ", name);
        //        command.Parameters.AddWithValue("@surname ", surname);
        //        command.Parameters.AddWithValue("@address ", address);
        //        command.Parameters.AddWithValue("@townID ", townID);
        //        command.Parameters.AddWithValue("@country ", country);
        //        command.Parameters.AddWithValue("@birthdate ", birthdate);
        //        command.Parameters.AddWithValue("@sex ", sex);
        //        command.Parameters.AddWithValue("@telephone ", telephone);
        //        command.Parameters.AddWithValue("@applicationName ", pApplicationName);
        //        command.Parameters.AddWithValue("@comment ", "");
        //        command.Parameters.AddWithValue("@passwordQuestion ", passwordQuestion);
        //        command.Parameters.AddWithValue("@passwordAnswer ", EncodePassword(passwordAnswer));
        //        command.Parameters.AddWithValue("@isApproved ", isApproved);
        //        command.Parameters.AddWithValue("@lastActivityDate ", createDate);
        //        command.Parameters.AddWithValue("@lastLoginDate ", createDate);
        //        command.Parameters.AddWithValue("@lastPasswordChangedDate ", createDate);
        //        command.Parameters.AddWithValue("@creationDate ", createDate);
        //        command.Parameters.AddWithValue("@isOnLine ", false);
        //        command.Parameters.AddWithValue("@isLockedOut ", false);
        //        command.Parameters.AddWithValue("@lastLockedOutDate ", createDate);
        //        command.Parameters.AddWithValue("@failedPasswordAttemptCount ", 0);
        //        command.Parameters.AddWithValue("@failedPasswordAttemptWindowStart ", createDate);
        //        command.Parameters.AddWithValue("@failedPasswordAnswerAttemptCount ", 0);
        //        command.Parameters.AddWithValue("@failedPasswordAnswerAttemptWindowStart ", createDate);
        //        command.Connection = conn;

        //        try
        //        {
        //            conn.Open();

        //            int recAdded = command.ExecuteNonQuery();

        //            if (recAdded > 0)
        //            {
        //                status = MembershipCreateStatus.Success;
        //            }
        //            else
        //            {
        //                status = MembershipCreateStatus.UserRejected;
        //            }
        //        }
        //        catch (MySqlException e)
        //        {
        //            if (WriteExceptionsToEventLog)
        //            {
        //                WriteToEventLog(e, "CreateUser");
        //            }

        //            status = MembershipCreateStatus.ProviderError;
        //        }
        //        finally
        //        {
        //            conn.Close();
        //        }


        //        return GetUser(login, false);
        //    }
        //    else
        //    {
        //        status = MembershipCreateStatus.DuplicateUserName;
        //    }


        //    return null;
        //}

        //
        // MembershipProvider.CreateUser
        //

        public override MembershipUser CreateUser(string username,
           string password,
           string email,
           string passwordQuestion,
           string passwordAnswer,
           bool isApproved,
           object providerUserKey,
           out MembershipCreateStatus status)
        {
            return this.CreateUser("", password, email, "", "", "", "0", "", DateTime.Now, "", "",
                                  passwordQuestion, passwordAnswer,
                                  isApproved,
                                  out status);
        }

        public CustomMembershipUser CreateUser(
            string login,
            string password,
            string email,
            string name,
            string surname,
            string address,
            string townID,//tu zmienić na nazwe miasta lub kod i select do zapytania
            string country,
            DateTime birthdate,
            string sex,
            string telephone,
            string passwordQuestion,
            string passwordAnswer,
            bool isApproved,
            //    object providerUserKey,
            out MembershipCreateStatus status)
        {
            ValidatePasswordEventArgs args =
              new ValidatePasswordEventArgs(login, password, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (RequiresUniqueEmail && GetUserNameByEmail(email) != "")
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            MembershipUser u = GetUser(login, false);

            if (u == null)
            {
                DateTime createDate = DateTime.Now;

                //  if (providerUserKey == null)
                //  {
                //       providerUserKey = Guid.NewGuid();
                //   }
                //else
                //{
                //    if (!(providerUserKey is Guid))
                //    {
                //        status = MembershipCreateStatus.InvalidProviderUserKey;
                //        return null;
                //    }
                //}
                MySqlConnection conn = new MySqlConnection(connectionString);
                MySqlCommand command = new MySqlCommand(Queries.CreateUser);
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@password", EncodePassword(password));
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@name", name);
                command.Parameters.AddWithValue("@surname", surname);
                command.Parameters.AddWithValue("@address", address);
                command.Parameters.AddWithValue("@townID", townID);
                command.Parameters.AddWithValue("@country", country);
                command.Parameters.AddWithValue("@birthdate", birthdate);
                command.Parameters.AddWithValue("@sex", sex);
                command.Parameters.AddWithValue("@telephone", telephone);
                command.Parameters.AddWithValue("@applicationName", pApplicationName);
                command.Parameters.AddWithValue("@comment", "");
                command.Parameters.AddWithValue("@passwordQuestion", passwordQuestion);
                command.Parameters.AddWithValue("@passwordAnswer", EncodePassword(passwordAnswer));
                command.Parameters.AddWithValue("@isApproved", isApproved);
                command.Parameters.AddWithValue("@lastActivityDate", createDate);
                command.Parameters.AddWithValue("@lastLoginDate", createDate);
                command.Parameters.AddWithValue("@lastPasswordChangedDate", createDate);
                command.Parameters.AddWithValue("@creationDate", createDate);
                command.Parameters.AddWithValue("@isOnLine", false);
                command.Parameters.AddWithValue("@isLockedOut", false);
                command.Parameters.AddWithValue("@lastLockedOutDate", createDate);
                command.Parameters.AddWithValue("@failedPasswordAttemptCount", 0);
                command.Parameters.AddWithValue("@failedPasswordAttemptWindowStart", createDate);
                command.Parameters.AddWithValue("@failedPasswordAnswerAttemptCount", 0);
                command.Parameters.AddWithValue("@failedPasswordAnswerAttemptWindowStart", createDate);
                command.Connection = conn;

                try
                {
                    conn.Open();

                    int recAdded = command.ExecuteNonQuery();

                    if (recAdded > 0)
                    {
                        status = MembershipCreateStatus.Success;
                    }
                    else
                    {
                        status = MembershipCreateStatus.UserRejected;
                    }
                }
                catch (MySqlException e)
                {
                    if (WriteExceptionsToEventLog)
                    {
                        WriteToEventLog(e, "CreateUser");
                    }

                    status = MembershipCreateStatus.ProviderError;
                }
                finally
                {
                    conn.Close();
                }

                return (CustomMembershipUser)GetUser(login, false);
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }


            return null;
        }



        //
        // MembershipProvider.DeleteUser
        //

        public override bool DeleteUser(string login, bool deleteAllRelatedData)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.DeleteUser);
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@applicationName", pApplicationName);
            command.Connection = conn;

            int rowsAffected = 0;

            try
            {
                conn.Open();
                rowsAffected = command.ExecuteNonQuery();
                if (deleteAllRelatedData)
                {
                    // Process commands to delete all data for the user in the database.
                    //dopisać
                    //co zmienić jak sie usera usówa
                    // + tranzakcje
                }
            }
            catch (MySqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "DeleteUser");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }

            if (rowsAffected > 0)
                return true;

            return false;
        }

        //
        // MembershipProvider.GetAllUsers
        //

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.AllUsersCount);
            command.Parameters.AddWithValue("@applicationName", ApplicationName);
            command.Connection = conn;

            MembershipUserCollection users = new MembershipUserCollection();

            MySqlDataReader reader = null;
            totalRecords = 0;

            try
            {
                conn.Open();
                totalRecords = (int)command.ExecuteScalar();

                if (totalRecords <= 0) { return users; }

                command.CommandText = Queries.GetAllUsers;

                reader = command.ExecuteReader();

                int counter = 0;
                int startIndex = pageSize * pageIndex;
                int endIndex = startIndex + pageSize - 1;

                while (reader.Read())
                {
                    if (counter >= startIndex)
                    {
                        CustomMembershipUser u = GetUserFromReader(reader);
                        users.Add(u);
                    }

                    if (counter >= endIndex) { command.Cancel(); }

                    counter++;
                }
            }
            catch (MySqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetAllUsers ");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            return users;
        }

        //
        // MembershipProvider.GetNumberOfUsersOnline
        //

        public override int GetNumberOfUsersOnline()
        {

            TimeSpan onlineSpan = new TimeSpan(0, System.Web.Security.Membership.UserIsOnlineTimeWindow, 0);
            DateTime compareTime = DateTime.Now.Subtract(onlineSpan);

            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.GetNumberOfUsersOnline);
            command.Parameters.AddWithValue("@lastActivityDate", compareTime);
            command.Parameters.AddWithValue("@applicationName", pApplicationName);
            command.Connection = conn;

            int numOnline = 0;

            try
            {
                conn.Open();

                numOnline = (int)command.ExecuteScalar();
            }
            catch (MySqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetNumberOfUsersOnline");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }

            return numOnline;
        }

        //
        // MembershipProvider.GetPassword
        //

        public override string GetPassword(string login, string answer)
        {
            if (!EnablePasswordRetrieval)
            {
                throw new ProviderException("Password Retrieval Not Enabled.");
            }

            if (PasswordFormat == MembershipPasswordFormat.Hashed)
            {
                throw new ProviderException("Cannot retrieve Hashed passwords.");
            }

            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.GetPassword);
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@applicationName", pApplicationName);
            command.Connection = conn;

            string password = "";
            string passwordAnswer = "";
            MySqlDataReader reader = null;

            try
            {
                conn.Open();

                reader = command.ExecuteReader(CommandBehavior.SingleRow);

                if (reader.HasRows)
                {
                    reader.Read();

                    if (reader.GetBoolean(2))
                        throw new MembershipPasswordException("The supplied user is locked out.");

                    password = reader.GetString(0);
                    passwordAnswer = reader.GetString(1);
                }
                else
                {
                    throw new MembershipPasswordException("The supplied user name is not found.");
                }
            }
            catch (MySqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetPassword");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }


            if (RequiresQuestionAndAnswer && !CheckPassword(answer, passwordAnswer))
            {
                UpdateFailureCount(login, "passwordAnswer");

                throw new MembershipPasswordException("Incorrect password answer.");
            }


            if (PasswordFormat == MembershipPasswordFormat.Encrypted)
            {
                password = UnEncodePassword(password);
            }

            return password;
        }

        //
        // MembershipProvider.GetUser(string, bool)
        //

        public override MembershipUser GetUser(string login, bool userIsOnline)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.GetUserByLogin);
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@applicationName", pApplicationName);
            command.Connection = conn;

            CustomMembershipUser u = null;
            MySqlDataReader reader = null;

            try
            {
                conn.Open();

                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    u = GetUserFromReader(reader);
                    reader.Close();
                    if (userIsOnline)
                    {
                        MySqlCommand updateCmd = new MySqlCommand(Queries.UpdateUserActivityByLogin);

                        updateCmd.Parameters.Add("@lastActivityDate", DateTime.Now);
                        updateCmd.Parameters.Add("@login", login);
                        updateCmd.Parameters.Add("@applicationName", pApplicationName);
                        updateCmd.Connection = conn;

                        updateCmd.ExecuteNonQuery();
                    }
                }

            }
            catch (MySqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUser(String, Boolean)");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }

                conn.Close();
            }

            return u;
        }

        //
        // MembershipProvider.GetUser(object, bool)
        //

        public override MembershipUser GetUser(object id, bool userIsOnline)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.GetUserByID);
            command.Parameters.AddWithValue("@id", (int)id);
            command.Connection = conn;

            CustomMembershipUser u = null;
            MySqlDataReader reader = null;

            try
            {
                conn.Open();

                reader = command.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    u = GetUserFromReader(reader);

                    if (userIsOnline)
                    {
                        MySqlCommand updateCmd = new MySqlCommand(Queries.UpdateUserActivityByID);
                        updateCmd.Parameters.Add("@lastActivityDate", DateTime.Now);
                        updateCmd.Parameters.Add("@id", id);
                        updateCmd.Connection = conn;

                        updateCmd.ExecuteNonQuery();
                    }
                }

            }
            catch (MySqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUser(Object, Boolean)");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }

                conn.Close();
            }

            return u;
        }

        //
        // GetUserFromReader
        //    A helper function that takes the current row from the OdbcDataReader
        // and hydrates a MembershiUser from the values. Called by the 
        // MembershipUser.GetUser implementation.
        //

        //do zmiany

        private CustomMembershipUser GetUserFromReader(MySqlDataReader reader)
        {
            //  object providerUserKey = reader.GetValue(0);
            int id = reader.GetInt32(0);
            string login = reader.GetString(1);
            string email = reader.GetString(2);
            string name = reader.GetString(3);
            string surname = reader.GetString(4);
            string address = reader.GetString(5);
            string townID = reader.GetString(6);
            string country = reader.GetString(7);
            DateTime birthdate = Convert.ToDateTime(reader.GetString(8)); //Convert.ToDateTime(reader["date"].ToString());//reader.GetDateTime(8);
            string sex = reader.GetString(9);
            string telephone = reader.GetString(10);

            string comment = "";
            if (reader.GetValue(11) != DBNull.Value)
                comment = reader.GetString(11);

            string passwordQuestion = "";
            if (reader.GetValue(12) != DBNull.Value)
                passwordQuestion = reader.GetString(12);

            bool isApproved = reader.GetBoolean(13);
            DateTime lastActivityDate = Convert.ToDateTime(reader.GetString(14)); //reader.GetDateTime(14);

            DateTime lastLoginDate = new DateTime();
            if (reader.GetValue(15) != DBNull.Value)
                lastLoginDate = Convert.ToDateTime(reader.GetString(15)); //reader.GetDateTime(15);

            DateTime lastPasswordChangedDate = Convert.ToDateTime(reader.GetString(16)); //reader.GetDateTime(16);
            DateTime creationDate = Convert.ToDateTime(reader.GetString(17)); //reader.GetDateTime(17);
            bool isLockedOut = reader.GetBoolean(18);

            DateTime lastLockedOutDate = new DateTime();
            if (reader.GetValue(19) != DBNull.Value)
                lastLockedOutDate = Convert.ToDateTime(reader.GetString(19)); //reader.GetDateTime(19);

            CustomMembershipUser u = new CustomMembershipUser(this.Name,
                //             login,
                //  providerUserKey,
                                                  email,
                                                  passwordQuestion,
                                                  comment,
                                                  isApproved,
                                                  isLockedOut,
                                                  creationDate,
                                                  lastLoginDate,
                                                  lastActivityDate,
                                                  lastPasswordChangedDate,
                                                  lastLockedOutDate,
                                                  id,
                                                  login,
                                                  name,
                                                  surname,
                                                  address,
                                                  townID,
                                                  country,
                                                  birthdate,
                                                  sex,
                                                  telephone);

            return u;
        }

        //
        // MembershipProvider.UnlockUser
        //

        public override bool UnlockUser(string login)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.UnlockUser);
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@applicationName", pApplicationName);
            command.Parameters.AddWithValue("@isLockedOut", false);
            command.Parameters.AddWithValue("@lastLockedOutDate", DateTime.Now);
            command.Connection = conn;

            int rowsAffected = 0;

            try
            {
                conn.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "UnlockUser");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }

            if (rowsAffected > 0)
                return true;

            return false;
        }

        //
        // MembershipProvider.GetUserNameByEmail
        //

        public override string GetUserNameByEmail(string email)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.GetUserNameByEmail);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@applicationName", pApplicationName);
            command.Connection = conn;

            string username = "";

            try
            {
                conn.Open();

                username = (string)command.ExecuteScalar();
            }
            catch (MySqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUserNameByEmail");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }

            if (username == null)
                username = "";

            return username;
        }

        //
        // MembershipProvider.ResetPassword
        //

        public override string ResetPassword(string login, string answer)
        {
            if (!EnablePasswordReset)
            {
                throw new NotSupportedException("Password reset is not enabled.");
            }

            if (answer == null && RequiresQuestionAndAnswer)
            {
                UpdateFailureCount(login, "passwordAnswer");

                throw new ProviderException("Password answer required for password reset.");
            }

            string newPassword =
              System.Web.Security.Membership.GeneratePassword(newPasswordLength, MinRequiredNonAlphanumericCharacters);


            ValidatePasswordEventArgs args =
              new ValidatePasswordEventArgs(login, newPassword, true);

            OnValidatingPassword(args);

            if (args.Cancel)
                if (args.FailureInformation != null)
                    throw args.FailureInformation;
                else
                    throw new MembershipPasswordException("Reset password canceled due to password validation failure.");

            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.GetPasswordAnswer);
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@applicationName", pApplicationName);
            command.Connection = conn;

            int rowsAffected = 0;
            string passwordAnswer = "";
            MySqlDataReader reader = null;

            try
            {
                conn.Open();

                reader = command.ExecuteReader(CommandBehavior.SingleRow);

                if (reader.HasRows)
                {
                    reader.Read();

                    if (reader.GetBoolean(1))
                        throw new MembershipPasswordException("The supplied user is locked out.");

                    passwordAnswer = reader.GetString(0);
                    reader.Close();
                }
                else
                {
                    throw new MembershipPasswordException("The supplied user name is not found.");
                }

                if (RequiresQuestionAndAnswer && !CheckPassword(answer, passwordAnswer))
                {
                    UpdateFailureCount(login, "passwordAnswer");

                    throw new MembershipPasswordException("Incorrect password answer.");
                }

                MySqlCommand updateCmd = new MySqlCommand(Queries.ResetPassword);
                updateCmd.Parameters.AddWithValue("@password", EncodePassword(newPassword));
                updateCmd.Parameters.AddWithValue("@lastPasswordChangedDate", DateTime.Now);
                updateCmd.Parameters.AddWithValue("@login", login);
                updateCmd.Parameters.AddWithValue("@applicationName", pApplicationName);
                updateCmd.Parameters.AddWithValue("@isLockedOut", false);
                updateCmd.Connection = conn;

                rowsAffected = updateCmd.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ResetPassword");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            if (rowsAffected > 0)
            {
                MySqlCommand getcommand = new MySqlCommand(Queries.GetEmailByLogin);
                getcommand.Parameters.AddWithValue("@login", login);
                getcommand.Parameters.AddWithValue("@applicationName", pApplicationName);
                getcommand.Connection = conn;

                string email = "";
                MySqlDataReader emailreader = null;

                try
                {
                    conn.Open();

                    emailreader = getcommand.ExecuteReader(CommandBehavior.SingleRow);

                    if (emailreader.HasRows)
                    {
                        emailreader.Read();
                        
                        // może być dla bezpieczeństwa ale już było sprawdzane
                    //    if (emailreader.GetBoolean(2))
                    //        throw new MembershipPasswordException("The supplied user is locked out.");

                        email = emailreader.GetString(0);
                    }
                    else
                    {
                        throw new MembershipPasswordException("The supplied login is not found.");
                    }
                }
                catch (MySqlException e)
                {
                    if (WriteExceptionsToEventLog)
                    {
                        WriteToEventLog(e, "GetEmail");

                        throw new ProviderException(exceptionMessage);
                    }
                    else
                    {
                        throw e;
                    }
                }
                finally
                {
                    if (emailreader != null) { emailreader.Close(); }
                    conn.Close();
                }

                if(email!="")
                {
                SmtpClient klient = new SmtpClient("smtp.gmail.com");
                MailMessage wiadomosc = new MailMessage();
                try
                {
                    wiadomosc.From = new MailAddress("erestauracja@gmail.com");
                    wiadomosc.To.Add(email);
                    wiadomosc.Subject = "Erestauracja - restet hasła.";
                    wiadomosc.Body = "Nowe hasło: " + newPassword;

                    klient.Port = 587;
                    klient.Credentials = new System.Net.NetworkCredential("erestauracja", "Erestauracja123");
                    klient.EnableSsl = true;
                    klient.Send(wiadomosc);
                }
                catch (Exception ex)
                {
                    // znazcy że nie wysłał wiadomości co robić ??
                    // przywrócić stare ??
                    // może rollback ??

                }
                }
                return newPassword;
            }
            else
            {
                throw new MembershipPasswordException("User not found, or user is locked out. Password not Reset.");
            }
        }

        //
        // MembershipProvider.UpdateUser
        //

        public override void UpdateUser(MembershipUser user)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.UpdateUser);

            CustomMembershipUser cu = (CustomMembershipUser)user;

            command.Parameters.AddWithValue("@name", cu.Name);
            command.Parameters.AddWithValue("@surname", cu.Surname);
            command.Parameters.AddWithValue("@address", cu.Address);
            command.Parameters.AddWithValue("@townID", cu.TownID);
            command.Parameters.AddWithValue("@country", cu.Country);
            command.Parameters.AddWithValue("@birthdate", cu.Birthdate);
            command.Parameters.AddWithValue("@sex", cu.Sex);
            command.Parameters.AddWithValue("@telephone", cu.Telephone);
            command.Parameters.AddWithValue("@comment", user.Comment);
            command.Parameters.AddWithValue("@isApproved", user.IsApproved);
            command.Parameters.AddWithValue("@login", cu.Login);
            command.Parameters.AddWithValue("@applicationName", pApplicationName);
            command.Connection = conn;

            try
            {
                conn.Open();

                command.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "UpdateUser");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }
        }

        //
        // MembershipProvider.ValidateUser
        //

        public override bool ValidateUser(string login, string password)
        {
            bool isValid = false;

            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.ValidateUser);
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@applicationName", pApplicationName);
            command.Parameters.AddWithValue("@isLockedOut", false);
            command.Connection = conn;

            MySqlDataReader reader = null;
            bool isApproved = false;
            string pwd = "";

            try
            {
                conn.Open();

                reader = command.ExecuteReader(CommandBehavior.SingleRow);

                if (reader.HasRows)
                {
                    reader.Read();
                    pwd = reader.GetString(0);
                    isApproved = reader.GetBoolean(1);
                }
                else
                {
                    return false;
                }

                reader.Close();

                if (CheckPassword(password, pwd))
                {
                    if (isApproved)
                    {
                        isValid = true;

                        MySqlCommand updateCmd = new MySqlCommand(Queries.UpdateUserLoginDate);

                        updateCmd.Parameters.Add("@lastLoginDate", DateTime.Now);
                        updateCmd.Parameters.Add("@login", login);
                        updateCmd.Parameters.Add("@applicationName", pApplicationName);
                        updateCmd.Connection = conn;

                        updateCmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    conn.Close();

                    UpdateFailureCount(login, "password");
                }
            }
            catch (MySqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ValidateUser");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            return isValid;
        }

        //
        // UpdateFailureCount
        //   A helper method that performs the checks and updates associated with
        // password failure tracking.
        //

        private void UpdateFailureCount(string login, string failureType)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.GetFailureCount);
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@applicationName", pApplicationName);
            command.Connection = conn;

            MySqlDataReader reader = null;
            DateTime windowStart = new DateTime();
            int failureCount = 0;

            try
            {
                conn.Open();

                reader = command.ExecuteReader(CommandBehavior.SingleRow);

                if (reader.HasRows)
                {
                    reader.Read();

                    if (failureType == "password")
                    {
                        failureCount = reader.GetInt32(0);
                        windowStart = reader.GetDateTime(1);
                    }

                    if (failureType == "passwordAnswer")
                    {
                        failureCount = reader.GetInt32(2);
                        windowStart = reader.GetDateTime(3);
                    }
                }

                reader.Close();

                DateTime windowEnd = windowStart.AddMinutes(PasswordAttemptWindow);

                if (failureCount == 0 || DateTime.Now > windowEnd)
                {
                    // First password failure or outside of PasswordAttemptWindow. 
                    // Start a new password failure count from 1 and a new window starting now.

                    if (failureType == "password")
                        command.CommandText = Queries.UpdateFailedPasswordAttempt;

                    if (failureType == "passwordAnswer")
                        command.CommandText = Queries.UpdateFailedPasswordAnswerAttempt;

                    command.Parameters.Clear();

                    command.Parameters.Add("@count", 1);
                    command.Parameters.Add("@windowStart", DateTime.Now);
                    command.Parameters.Add("@login", login);
                    command.Parameters.Add("@applicationName", pApplicationName);

                    if (command.ExecuteNonQuery() < 0)
                        throw new ProviderException("Unable to update failure count and window start.");
                }
                else
                {
                    if (failureCount++ >= MaxInvalidPasswordAttempts)
                    {
                        // Password attempts have exceeded the failure threshold. Lock out
                        // the user.

                        command.CommandText = Queries.LockOutUser;

                        command.Parameters.Clear();

                        command.Parameters.Add("@isLockedOut", true);
                        command.Parameters.Add("@lastLockedOutDate", DateTime.Now);
                        command.Parameters.Add("@login", login);
                        command.Parameters.Add("@applicationName", pApplicationName);

                        if (command.ExecuteNonQuery() < 0)
                            throw new ProviderException("Unable to lock out user.");
                    }
                    else
                    {
                        // Password attempts have not exceeded the failure threshold. Update
                        // the failure counts. Leave the window the same.

                        if (failureType == "password")
                            command.CommandText = Queries.SetFailedPasswordAttemptCount;

                        if (failureType == "passwordAnswer")
                            command.CommandText = Queries.SetFailedPasswordAnswerAttemptCount;

                        command.Parameters.Clear();

                        command.Parameters.Add("@count", failureCount);
                        command.Parameters.Add("@login", login);
                        command.Parameters.Add("@applicationName", pApplicationName);

                        if (command.ExecuteNonQuery() < 0)
                            throw new ProviderException("Unable to update failure count.");
                    }
                }
            }
            catch (MySqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "UpdateFailureCount");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }
        }

        //
        // CheckPassword
        //   Compares password values based on the MembershipPasswordFormat.
        //

        private bool CheckPassword(string password, string dbpassword)
        {
            string pass1 = password;
            string pass2 = dbpassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Encrypted:
                    pass2 = UnEncodePassword(dbpassword);
                    break;
                case MembershipPasswordFormat.Hashed:
                    pass1 = EncodePassword(password);
                    break;
                default:
                    break;
            }

            if (pass1 == pass2)
            {
                return true;
            }

            return false;
        }

        //
        // EncodePassword
        //   Encrypts, Hashes, or leaves the password clear based on the PasswordFormat.
        //

        private string EncodePassword(string password)
        {
            string encodedPassword = password;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    encodedPassword =
                      Convert.ToBase64String(EncryptPassword(Encoding.Unicode.GetBytes(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    HMACSHA1 hash = new HMACSHA1();
                    hash.Key = HexToByte(machineKey.ValidationKey);
                    encodedPassword =
                      Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                    break;
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return encodedPassword;
        }

        //
        // UnEncodePassword
        //   Decrypts or leaves the password clear based on the PasswordFormat.
        //

        private string UnEncodePassword(string encodedPassword)
        {
            string password = encodedPassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    password =
                      Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    throw new ProviderException("Cannot unencode a hashed password.");
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return password;
        }

        //
        // HexToByte
        //   Converts a hexadecimal string to a byte array. Used to convert encryption
        // key values from the configuration.
        //

        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        //
        // MembershipProvider.FindUsersByName
        //

        public override MembershipUserCollection FindUsersByName(string loginToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.GetUserCountByLogin);
            command.Parameters.AddWithValue("@login", loginToMatch);
            command.Parameters.AddWithValue("@applicationName", pApplicationName);
            command.Connection = conn;

            MembershipUserCollection users = new MembershipUserCollection();

            MySqlDataReader reader = null;

            try
            {
                conn.Open();
                totalRecords = (int)command.ExecuteScalar();

                if (totalRecords <= 0) { return users; }

                command.CommandText = Queries.FindUsersByName;
                command.Connection = conn;
                reader = command.ExecuteReader();

                int counter = 0;
                int startIndex = pageSize * pageIndex;
                int endIndex = startIndex + pageSize - 1;

                while (reader.Read())
                {
                    if (counter >= startIndex)
                    {
                        CustomMembershipUser u = GetUserFromReader(reader);
                        users.Add(u);
                    }

                    if (counter >= endIndex) { command.Cancel(); }

                    counter++;
                }
            }
            catch (MySqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "FindUsersByName");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }

                conn.Close();
            }

            return users;
        }

        //
        // MembershipProvider.FindUsersByEmail
        //

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.GetUserCountByEmail);
            command.Parameters.AddWithValue("@email", emailToMatch);
            command.Parameters.AddWithValue("@applicationName", pApplicationName);
            command.Connection = conn;

            MembershipUserCollection users = new MembershipUserCollection();

            MySqlDataReader reader = null;
            totalRecords = 0;

            try
            {
                conn.Open();
                totalRecords = (int)command.ExecuteScalar();

                if (totalRecords <= 0) { return users; }

                command.CommandText = Queries.FindUsersByEmail;

                reader = command.ExecuteReader();

                int counter = 0;
                int startIndex = pageSize * pageIndex;
                int endIndex = startIndex + pageSize - 1;

                while (reader.Read())
                {
                    if (counter >= startIndex)
                    {
                        CustomMembershipUser u = GetUserFromReader(reader);
                        users.Add(u);
                    }

                    if (counter >= endIndex) { command.Cancel(); }

                    counter++;
                }
            }
            catch (MySqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "FindUsersByEmail");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }

                conn.Close();
            }

            return users;
        }

        //
        // WriteToEventLog
        //   A helper function that writes exception detail to the event log. Exceptions
        // are written to the event log as a security measure to avoid private database
        // details from being returned to the browser. If a method does not return a status
        // or boolean indicating the action succeeded or failed, a generic exception is also 
        // thrown by the caller.
        //

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

            log.WriteEntry(message);
        }

        public string GetUserQuestion(string login)
        {
            if (!EnablePasswordReset)
            {
                throw new ProviderException("Password Reset Not Enabled.");
            }

            MySqlConnection conn = new MySqlConnection(connectionString);
            MySqlCommand command = new MySqlCommand(Queries.GetUserQuestion);
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@applicationName", pApplicationName);
            command.Connection = conn;

            string passwordQuestion = "";
            MySqlDataReader reader = null;

            try
            {
                conn.Open();

                reader = command.ExecuteReader(CommandBehavior.SingleRow);

                if (reader.HasRows)
                {
                    reader.Read();

                    if (reader.GetBoolean(1))
                        throw new MembershipPasswordException("The supplied user is locked out.");

                    passwordQuestion = reader.GetString(0);
                }
                else
                {
                    throw new MembershipPasswordException("The supplied user name is not found.");
                }
            }
            catch (MySqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUserQuestion");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            return passwordQuestion;
        }

    }
}