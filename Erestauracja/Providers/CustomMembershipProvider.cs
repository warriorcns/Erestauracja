using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Web.Security;
using Erestauracja.ServiceReference;

/*
This provider works with the following schema for the table of user data.

CREATE  TABLE  `dbo`.`users` (  `id` int( 10  )  NOT  NULL  AUTO_INCREMENT , 
`login` varchar( 20  )  COLLATE utf8_polish_ci NOT  NULL , 
`password` varchar( 90  )  COLLATE utf8_polish_ci NOT  NULL , 
`email` varchar( 50  )  COLLATE utf8_polish_ci NOT  NULL , 
`name` varchar( 50  )  COLLATE utf8_polish_ci NOT  NULL , 
`surname` varchar( 50  )  COLLATE utf8_polish_ci NOT  NULL , 
`address` varchar( 100  )  COLLATE utf8_polish_ci NOT  NULL , 
`townID` int( 10  )  NOT  NULL , 
`country` varchar( 50  )  COLLATE utf8_polish_ci NOT  NULL , 
`birthdate` date NOT  NULL , 
`sex` varchar( 20  )  COLLATE utf8_polish_ci NOT  NULL , 
`telephone` varchar( 20  )  COLLATE utf8_polish_ci NOT  NULL , 
`applicationName` mediumtext COLLATE utf8_polish_ci NOT  NULL , 
`comment` mediumtext COLLATE utf8_polish_ci, 
`passwordQuestion` mediumtext COLLATE utf8_polish_ci, 
`passwordAnswer` mediumtext COLLATE utf8_polish_ci, 
`isApproved` bit( 1  )  DEFAULT NULL , 
`lastActivityDate` datetime  DEFAULT NULL , 
`lastLoginDate` datetime  DEFAULT NULL , 
`lastPasswordChangedDate` datetime  DEFAULT NULL , 
`creationDate` datetime  DEFAULT NULL , 
`isOnLine` bit( 1  )  DEFAULT NULL , 
`isLockedOut` bit( 1  )  DEFAULT NULL , 
`lastLockedOutDate` datetime  DEFAULT NULL , 
`failedPasswordAttemptCount` int( 11  )  DEFAULT NULL , 
`failedPasswordAttemptWindowStart` datetime  DEFAULT NULL , 
`failedPasswordAnswerAttemptCount` int( 11  )  DEFAULT NULL , 
`failedPasswordAnswerAttemptWindowStart` datetime  DEFAULT NULL , 
PRIMARY  KEY (  `id`  )  ) ENGINE  = InnoDB  DEFAULT CHARSET  = utf8 COLLATE  = utf8_polish_ci; 
*/

namespace Erestauracja.Providers
{
    public sealed class CustomMembershipProvider : MembershipProvider
    {
        /// <summary>
        /// Generated password length.
        /// </summary>
        private int newPasswordLength = 8;
        
        /// <summary>
        /// Generic exception event.
        /// </summary>
        private string eventSource = "CustomMembershipProvider";
        
        /// <summary>
        /// Generic exception log info.
        /// </summary>
        private string eventLog = "Erestauracja";
        
        /// <summary>
        /// Generic exception message.
        /// </summary>
        private string exceptionMessage = "An exception occurred. Please check the Event Log.";

        /// <summary>
        /// Used when determining encryption key values.
        /// </summary>
        private MachineKeySection machineKey;
        
        private bool pWriteExceptionsToEventLog;
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

            // Get encryption and decryption key information from the configuration.
            Configuration cfg =
              WebConfigurationManager.OpenWebConfiguration(System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            machineKey = (MachineKeySection)cfg.GetSection("system.web/machineKey");

            if (machineKey.ValidationKey.Contains("AutoGenerate"))
                if (PasswordFormat != MembershipPasswordFormat.Clear)
                    throw new ProviderException("Hashed or Encrypted passwords " +
                                                "are not supported with auto-generated keys.");
        }
        
        /// <summary>
        /// A helper function to retrieve config values from the configuration file.
        /// </summary>
        /// <param name="configValue">Value from config</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns></returns>
        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (String.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }

        #endregion

        #region System.Web.Security.MembershipProvider properties.

        private string pApplicationName;
        public override string ApplicationName
        {
            get { return pApplicationName; }
            set { pApplicationName = value; }
        }

        private bool pEnablePasswordReset;
        public override bool EnablePasswordReset
        {
            get { return pEnablePasswordReset; }
        }
 
        private bool pEnablePasswordRetrieval;
        public override bool EnablePasswordRetrieval
        {
            get { return pEnablePasswordRetrieval; }
        }

        private bool pRequiresQuestionAndAnswer;
        public override bool RequiresQuestionAndAnswer
        {
            get { return pRequiresQuestionAndAnswer; }
        }

        private bool pRequiresUniqueEmail;
        public override bool RequiresUniqueEmail
        {
            get { return pRequiresUniqueEmail; }
        }

        private int pMaxInvalidPasswordAttempts;
        public override int MaxInvalidPasswordAttempts
        {
            get { return pMaxInvalidPasswordAttempts; }
        }

        private int pPasswordAttemptWindow;
        public override int PasswordAttemptWindow
        {
            get { return pPasswordAttemptWindow; }
        }

        private MembershipPasswordFormat pPasswordFormat;
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

        #endregion

        #region System.Web.Security.MembershipProvider methods.

        #region Password methods:

        /// <summary>
        /// Zamienia stare hasło na nowe u użytkownika o danym loginie.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="oldPwd">Stare hasło</param>
        /// <param name="newPwd">Nowe hasło</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

            bool value = false;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value = client.ChangePassword(login, EncodePassword(newPwd));
                }
                client.Close();
            }
            catch (Exception e)
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
            
            return value;
        }

        /// <summary>
        /// Zamienia stare hasło na nowe u pracownika o danym loginie.
        /// </summary>
        /// <param name="login">Login pracownika</param>
        /// <param name="newPwd">Nowe hasło</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public bool ChangeEmployeePassword(string login, string newPwd)
        {
            ValidatePasswordEventArgs args =
              new ValidatePasswordEventArgs(login, newPwd, true);

            OnValidatingPassword(args);

            if (args.Cancel)
                if (args.FailureInformation != null)
                    throw args.FailureInformation;
                else
                    throw new MembershipPasswordException("Change password canceled due to new password validation failure.");

            bool value = false;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value = client.ChangePassword(login, EncodePassword(newPwd));
                }
                client.Close();
            }
            catch (Exception e)
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

            return value;
        }

        /// <summary>
        /// Zamienia pytanie oraz odpowiedź do odzyskiwania hasła u użytkownika o danym loginie.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="password">Hasło użytkownika</param>
        /// <param name="newPwdQuestion">Nowe pytanie</param>
        /// <param name="newPwdAnswer">Nowa odpowiedź</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public override bool ChangePasswordQuestionAndAnswer(string login, string password, string newPwdQuestion, string newPwdAnswer)
        {
            if (!ValidateUser(login, password))
                return false;

            bool value = false;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                { 
                    value = client.ChangePasswordQuestionAndAnswer(login, newPwdQuestion, EncodePassword(newPwdAnswer));
                }
                client.Close();
            }
            catch (Exception e)
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

            return value;
        }

        /// <summary>
        /// Zwraca hasło, jeżeli odpowiedz na pytanie do przywaracania hasła jest poprawna.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="answer">Odpowiedź na pytanie</param>
        /// <returns>Hasło jeśli odpowiedź jest poprawna.</returns>
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

            string password = "";
            PasswordAndAnswer value = null;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value = client.GetPassword(login);
                }
                client.Close();
            }
            catch (Exception e)
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

            if (value != null)
            {
                if(value.IsLockedOut == false)
                {
                    if (RequiresQuestionAndAnswer && !CheckPassword(answer, value.PasswordAnswer))
                    {
                        UpdateFailureCount(login, "passwordAnswer");

                        throw new MembershipPasswordException("Incorrect password answer.");
                    }

                    if (PasswordFormat == MembershipPasswordFormat.Encrypted)
                    {
                        password = UnEncodePassword(value.Password);
                    }
                }
                else
                {
                    throw new MembershipPasswordException("The supplied user is locked out.");
                }
            }
            else
            {
                throw new MembershipPasswordException("The supplied user name is not found.");
            }

            return password;
        }

        /// <summary>
        /// Jeśli odpowiedź na pytanie do przywracania hasła jest poprawna ustawia nowe hasło dla danego użytkownika oraz wysyła nowe hasło na jego aders email.
        /// </summary>
        /// /// <remarks>
        /// Aby metoda zwracała nowe hasło trzeba odkomentować return.
        /// </remarks>
        /// <param name="login">Login użytkownika</param>
        /// <param name="answer">Odpowiedź na pytanie</param>
        /// <returns>OK - jeśli metoda wykonała się poprawnie.</returns>
        public override string ResetPassword(string login, string answer)
        {
            if (!EnablePasswordReset)
            {
               // throw new NotSupportedException("Resetowanie hasła nie jest dostępne.");
                return "Resetowanie hasła nie jest dostępne.";
            }

            if (answer == null && RequiresQuestionAndAnswer)
            {
                //UpdateFailureCount(login, "passwordAnswer");

                //throw new ProviderException("Odpowiedź na pytanie do restetowania hasła jest wymagana.");
                return "Odpowiedź na pytanie do restetowania hasła jest wymagana.";
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

            PasswordAnswer value = null;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value = client.GetPasswordAnswer(login);
                }
                client.Close();
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetPasswordAnswer");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }

            if (value != null)
            {
                if (value.IsLockedOut == false)
                {
                    if (RequiresQuestionAndAnswer && !CheckPassword(answer, value.Answer))
                    {
                       // UpdateFailureCount(login, "passwordAnswer");
                      //  throw new MembershipPasswordException("Nieprawidłowa odpowiedź.");
                        return "Nieprawidłowa odpowiedź.";
                    }

                    bool reset = false;
                    try
                    {
                        ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                        using (client)
                        {
                            reset = client.ResetPassword(login, EncodePassword(newPassword));
                        }
                        client.Close();
                    }
                    catch (Exception e)
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

                    if (reset == true)
                    {
                        //return newPassword;
                        return "OK";
                    }
                    else
                    {
                        //throw new MembershipPasswordException("Restetowanie hasła nie powiodło się.");
                        return "Restetowanie hasła nie powiodło się.";
                    }
                }
                else
                {
                   // throw new MembershipPasswordException("Użytkownik o podanym loginie jest zablokowany.");
                    return "Użytkownik o podanym loginie jest zablokowany.";
                }
            }
            else
            {
               // throw new MembershipPasswordException("Nieprawidłowy login.");
                return "Nieprawidłowy login.";
            }
        }

        /// <summary>
        /// Zwraca pytanie do odzyskiwania hasła danego użytkownika.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>Pytanie do odzyskiwania hasła.</returns>
        public string GetUserQuestion(string login)
        {
            if (!EnablePasswordReset)
            {
                throw new ProviderException("Password Reset Not Enabled.");
            }

            string passwordQuestion = "";
            PasswordQuestion value = null;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value = client.GetUserQuestion(login);
                }
                client.Close();
            }
            catch (Exception e)
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

            if (value != null)
            {
                if (value.IsLockedOut == false)
                {
                    passwordQuestion = value.Question;
                }
                else
                {
                    throw new MembershipPasswordException("The supplied user is locked out.");
                }
            }
            else
            {
                throw new MembershipPasswordException("The supplied user name is not found.");
            }

            return passwordQuestion;
        } 

        /// <summary>
        /// Porównuje wartości haseł na podstawie MembershipPasswordFormat.
        /// </summary>
        /// <param name="password">Hasło podane przez użytkownika</param>
        /// <param name="dbpassword">Hasło z bazy</param>
        /// <returns>True jeśli hasła zgodne.</returns>
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

        /// <summary>
        /// Szyfruje, hashuje, lub pozostawia hasło nie zmienione na podstawie PasswordFormat.
        /// </summary>
        /// <param name="password">Hasło</param>
        /// <returns>Przerobione hasło.</returns>
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

        /// <summary>
        /// Odszyfrowuje lub pozostawia hasło nie zmienione na podstawie PasswordFormat.
        /// </summary>
        /// <param name="encodedPassword">Zakodowane hasło</param>
        /// <returns>Odkodowane hasło.</returns>
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

        /// <summary>
        /// Konwertuje szesnastkowy łańcuch znaków do tablicy bajtów.
        /// </summary>
        /// <remarks>
        /// Służy do konwersji wartości klucza szyfrowania z pliku konfiguracji.
        /// </remarks>
        /// <param name="hexString">Łańcuch znaków do przerobienia</param>
        /// <returns>Zwraca tablice bajtów.</returns>
        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        
        #endregion

        #region User methods:

        /*CreateUser dla Guid - nie używana
        //
        //
        // MembershipProvider.CreateUser
        //
        public override MembershipUser CreateUser(string login,
            string password,
            string email,
            string name,
            string surname,
            string address,
            int townID,//tu zmienić na nazwe miasta lub kod i select do zapytania
            string country,
            DateTime birthdate,
            string sex,
            string telephone,
            string passwordQuestion,
            string passwordAnswer,
            bool isApproved,
            object providerUserKey,
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
                command.Parameters.AddWithValue("@login ", login);
                command.Parameters.AddWithValue("@password ", EncodePassword(password));
                command.Parameters.AddWithValue("@email ", email);
                command.Parameters.AddWithValue("@name ", name);
                command.Parameters.AddWithValue("@surname ", surname);
                command.Parameters.AddWithValue("@address ", address);
                command.Parameters.AddWithValue("@townID ", townID);
                command.Parameters.AddWithValue("@country ", country);
                command.Parameters.AddWithValue("@birthdate ", birthdate);
                command.Parameters.AddWithValue("@sex ", sex);
                command.Parameters.AddWithValue("@telephone ", telephone);
                command.Parameters.AddWithValue("@applicationName ", pApplicationName);
                command.Parameters.AddWithValue("@comment ", "");
                command.Parameters.AddWithValue("@passwordQuestion ", passwordQuestion);
                command.Parameters.AddWithValue("@passwordAnswer ", EncodePassword(passwordAnswer));
                command.Parameters.AddWithValue("@isApproved ", isApproved);
                command.Parameters.AddWithValue("@lastActivityDate ", createDate);
                command.Parameters.AddWithValue("@lastLoginDate ", createDate);
                command.Parameters.AddWithValue("@lastPasswordChangedDate ", createDate);
                command.Parameters.AddWithValue("@creationDate ", createDate);
                command.Parameters.AddWithValue("@isOnLine ", false);
                command.Parameters.AddWithValue("@isLockedOut ", false);
                command.Parameters.AddWithValue("@lastLockedOutDate ", createDate);
                command.Parameters.AddWithValue("@failedPasswordAttemptCount ", 0);
                command.Parameters.AddWithValue("@failedPasswordAttemptWindowStart ", createDate);
                command.Parameters.AddWithValue("@failedPasswordAnswerAttemptCount ", 0);
                command.Parameters.AddWithValue("@failedPasswordAnswerAttemptWindowStart ", createDate);
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
                return GetUser(login, false);
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }
            return null;
        }
        */

        /// <summary>
        /// Bazowy MembershipUser CreateUser - nie stosować jako samodzielna metoda
        /// </summary>
        /// <param name="username">Pusty string - zawsze ""</param>
        /// <param name="password">Hasło użytkownika</param>
        /// <param name="email">Adres email</param>
        /// <param name="passwordQuestion">Pytanie do odzyskiwania hasła</param>
        /// <param name="passwordAnswer">Odpowiedź do odzyskiwania hasła</param>
        /// <param name="isApproved">Czy zatwierdzony</param>
        /// <param name="providerUserKey">Guid - zawsze null</param>
        /// <param name="status">out MembershipCreateStatus</param>
        /// <returns>MembershipUser</returns>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            return this.CreateUser("", password, email, "", "", "", 0, "", DateTime.Now, "", "", passwordQuestion, passwordAnswer, isApproved, out status);
        }

        /// <summary>
        /// Tworzy nowego użytkownika oraz zapisuje go w bazie danych.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="password">Hasło użytkownika</param>
        /// <param name="email">Adres email</param>
        /// <param name="name">Imię użytkownika</param>
        /// <param name="surname">Nazwisko użytkownika</param>
        /// <param name="address">Adres</param>
        /// <param name="townID">Id miasta</param>
        /// <param name="country">Kraj</param>
        /// <param name="birthdate">Data urodzenia</param>
        /// <param name="sex">Płeć</param>
        /// <param name="telephone">Numer telefonu</param>
        /// <param name="passwordQuestion">Pytanie do odzyskiwania hasła</param>
        /// <param name="passwordAnswer">Odpowiedź do odzyskiwania hasła</param>
        /// <param name="isApproved">Czy zatwierdzony</param>
        /// <param name="status">out MembershipCreateStatus</param>
        /// <returns>CustomMembershipUser</returns>
        public CustomMembershipUser CreateUser(string login, string password, string email, string name, string surname, string address, int townID, string country, DateTime birthdate, string sex, string telephone, string passwordQuestion, string passwordAnswer, bool isApproved, out MembershipCreateStatus status)
        {
            ValidatePasswordEventArgs args =
              new ValidatePasswordEventArgs(login, password, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (RequiresUniqueEmail && !(String.IsNullOrEmpty(GetUserNameByEmail(email))))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            MembershipUser u = GetUser(login, false);

            if (u == null)
            {
                #region //tworzenie Guid wymaga - object providerUserKey argumentach
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
                #endregion

                bool value = false;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {       
                        value = client.CreateUser(login, EncodePassword(password), email, name, surname, address, townID, country, birthdate, sex, telephone, passwordQuestion, EncodePassword(passwordAnswer), isApproved);
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    if (WriteExceptionsToEventLog)
                    {
                        WriteToEventLog(e, "CreateUser");
                        status = MembershipCreateStatus.ProviderError;
                        throw new ProviderException(exceptionMessage);
                    }
                    else
                    {
                        status = MembershipCreateStatus.ProviderError;
                        throw e;
                    }
                }

                if (value == true)
                {
                    status = MembershipCreateStatus.Success;
                }
                else
                {
                    status = MembershipCreateStatus.UserRejected;
                }

                return (CustomMembershipUser)GetUser(login, false);
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }


            return null;
        }

        /// <summary>
        /// Usuwa użytkownika o danym loginie.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="deleteAllRelatedData">Czy usunąć powiązane dane</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public override bool DeleteUser(string login, bool deleteAllRelatedData)
        {
            bool value = false;

            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value = client.DeleteUser(login, deleteAllRelatedData);
                }
                client.Close();
            }
            catch (Exception e)
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

            return value;
        }

        /// <summary>
        /// Pobiera kolekcje typu MembershipUserCollection zawierającą określoną ilość użytkowników
        /// </summary>
        /// <param name="pageIndex">Indeks strony</param>
        /// <param name="pageSize">Rozmiar strony</param>
        /// <param name="totalRecords">Out ilość pobranych użytkowników</param>
        /// <returns>MembershipUserCollection</returns>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection users = new MembershipUserCollection();
            users.Clear();
            List<User> lista = new List<User>();

            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    lista.AddRange(client.GetAllUsers(out totalRecords, pageIndex, pageSize));
                }
                client.Close();
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetAllUsers");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }

            foreach (User user in lista)
            {
                CustomMembershipUser u = GetCustomMembershipUserFromUser(user);
                MembershipUser us = (MembershipUser)u;
                users.Add(u);
            }
            
            return users;   
        }

        /// <summary>
        /// Konwertuje typ User na CustomMembershipUser
        /// </summary>
        /// <param name="reader">User</param>
        /// <returns>CustomMembershipUser</returns>
        private CustomMembershipUser GetCustomMembershipUserFromUser(User reader)
        {
            //  object providerUserKey = reader.GetValue(0);
            int id = reader.ID;
            string login = reader.Login;
            string email = reader.Email;
            string name = reader.Name;
            string surname = reader.Surname;
            string address = reader.Address;
            string town = reader.Town;
            string postalCode = reader.PostalCode;
            string country = reader.Country;
            DateTime birthdate = reader.Birthdate;
            string sex = reader.Sex;
            string telephone = reader.Telephone;
            string comment = reader.Comment;
            string passwordQuestion = reader.PasswordQuestion;
            bool isApproved = reader.IsApproved;
            DateTime lastActivityDate = reader.LastActivityDate;
            DateTime lastLoginDate = reader.LastLoginDate;
            DateTime lastPasswordChangedDate = reader.LastPasswordChangedDate;
            DateTime creationDate = reader.CreationDate;
            bool isLockedOut = reader.IsLockedOut;
            DateTime lastLockedOutDate = reader.LastLockedOutDate;


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
                                                  town,
                                                  postalCode,
                                                  country,
                                                  birthdate,
                                                  sex,
                                                  telephone);

            return u;
        }

        /// <summary>
        /// Konwertuje typ CustomMembershipUser na User
        /// </summary>
        /// <param name="reader">CustomMembershipUser</param>
        /// <returns>User</returns>
        private User GetUserFromCustomMembershipUser(CustomMembershipUser reader)
        {
            User user = new User();

            user.ID = reader.Id;
            user.Login = reader.Login;
            user.Email = reader.Email;
            user.Name = reader.Name;
            user.Surname = reader.Surname;
            user.Address = reader.Address;
            user.Town = reader.Town;
            user.PostalCode = reader.PostalCode;
            user.Country = reader.Country;
            user.Birthdate = reader.Birthdate;
            user.Sex = reader.Sex;
            user.Telephone = reader.Telephone;
            user.Comment = reader.Comment;
            user.PasswordQuestion = reader.PasswordQuestion;
            user.IsApproved = reader.IsApproved;
            user.LastActivityDate = reader.LastActivityDate;
            user.LastLoginDate = reader.LastLoginDate;
            user.LastPasswordChangedDate = reader.LastPasswordChangedDate;
            user.CreationDate = reader.CreationDate;
            user.IsLockedOut = reader.IsLockedOut;
            user.LastLockedOutDate = reader.LastLockoutDate;

            return user;
        }

        /// <summary>
        /// Zwraca ilość użytkowników, których ostatnia aktywność jest późniejsza niż czas ustalony w configu
        /// </summary>
        /// <returns>Int</returns>
        public override int GetNumberOfUsersOnline()
        {
            TimeSpan onlineSpan = new TimeSpan(0, System.Web.Security.Membership.UserIsOnlineTimeWindow, 0);
            int value = 0;
            
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value = client.GetNumberOfUsersOnline(onlineSpan);
                }
                client.Close();
            }
            catch (Exception e)
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

            return value;  
        }

        /// <summary>
        /// Pobiera dane użytkownika o danym loginie.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="userIsOnline">Czy użytkownik jest online - aktualizacja daty ostatniej aktywności</param>
        /// <returns>MembershipUser - CustomMembershipUser</returns>
        public override MembershipUser GetUser(string login, bool userIsOnline)
        {
            User lista = new User();

            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    lista = client.GetUser(login, userIsOnline);
                }
                client.Close();
            }
            catch (Exception e)
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

            if (lista != null)
            {
                CustomMembershipUser u = GetCustomMembershipUserFromUser(lista);
                MembershipUser us = (MembershipUser)u;
                return us;
            }
            else
            {
                return null;
            }
            
        }

        /// <summary>
        /// Pobiera dane użytkownika o danym id.
        /// </summary>
        /// <param name="login">Id użytkownika</param>
        /// <param name="userIsOnline">Czy użytkownik jest online - aktualizacja daty ostatniej aktywności</param>
        /// <returns>MembershipUser - CustomMembershipUser</returns>
        public override MembershipUser GetUser(object id, bool userIsOnline)
        {
            User lista = new User();

            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    lista = client.GetUserById((int)id, userIsOnline);
                }
                client.Close();
            }
            catch (Exception e)
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

            CustomMembershipUser u = GetCustomMembershipUserFromUser(lista);
            MembershipUser us = (MembershipUser)u;

            return us;
        }

        ////Nie używana
        ////
        //// GetUserFromReader
        ////    A helper function that takes the current row from the OdbcDataReader
        //// and hydrates a MembershiUser from the values. Called by the 
        //// MembershipUser.GetUser implementation.
        ////
        //private CustomMembershipUser GetUserFromReader(MySqlDataReader reader)
        //{
        //    //  object providerUserKey = reader.GetValue(0);
        //    int id = reader.GetInt32(0);
        //    string login = reader.GetString(1);
        //    string email = reader.GetString(2);
        //    string name = reader.GetString(3);
        //    string surname = reader.GetString(4);
        //    string address = reader.GetString(5);
        //    string townID = reader.GetString(6);
        //    string country = reader.GetString(7);
        //    DateTime birthdate = Convert.ToDateTime(reader.GetString(8)); //Convert.ToDateTime(reader["date"].ToString());//reader.GetDateTime(8);
        //    string sex = reader.GetString(9);
        //    string telephone = reader.GetString(10);
        //    string comment = "";
        //    if (reader.GetValue(11) != DBNull.Value)
        //        comment = reader.GetString(11);
        //    string passwordQuestion = "";
        //    if (reader.GetValue(12) != DBNull.Value)
        //        passwordQuestion = reader.GetString(12);
        //    bool isApproved = reader.GetBoolean(13);
        //    DateTime lastActivityDate = Convert.ToDateTime(reader.GetString(14)); //reader.GetDateTime(14);
        //    DateTime lastLoginDate = new DateTime();
        //    if (reader.GetValue(15) != DBNull.Value)
        //        lastLoginDate = Convert.ToDateTime(reader.GetString(15)); //reader.GetDateTime(15);
        //    DateTime lastPasswordChangedDate = Convert.ToDateTime(reader.GetString(16)); //reader.GetDateTime(16);
        //    DateTime creationDate = Convert.ToDateTime(reader.GetString(17)); //reader.GetDateTime(17);
        //    bool isLockedOut = reader.GetBoolean(18);
        //    DateTime lastLockedOutDate = new DateTime();
        //    if (reader.GetValue(19) != DBNull.Value)
        //        lastLockedOutDate = Convert.ToDateTime(reader.GetString(19)); //reader.GetDateTime(19);
        //    CustomMembershipUser u = new CustomMembershipUser(this.Name,
        //        //             login,
        //        //  providerUserKey,
        //                                          email,
        //                                          passwordQuestion,
        //                                          comment,
        //                                          isApproved,
        //                                          isLockedOut,
        //                                          creationDate,
        //                                          lastLoginDate,
        //                                          lastActivityDate,
        //                                          lastPasswordChangedDate,
        //                                          lastLockedOutDate,
        //                                          id,
        //                                          login,
        //                                          name,
        //                                          surname,
        //                                          address,
        //                                          townID,
        //                                          country,
        //                                          birthdate,
        //                                          sex,
        //                                          telephone);
        //    return u;
        //}

        /// <summary>
        /// Odblokowuje konto użytkownika.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public override bool UnlockUser(string login)
        {
            bool value = false;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value = client.UnlockUser(login);
                }
                client.Close();
            }
            catch (Exception e)
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

            return value;
        }

        /// <summary>
        /// Pobiera nazwe użytkownika na podstawie adresu email.
        /// </summary>
        /// <param name="email">Adres email użytkownika</param>
        /// <returns>Zwraca login użytkownika.</returns>
        public override string GetUserNameByEmail(string email)
        {
            string value = null;

            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value = client.GetUserNameByEmail(email);
                }
                client.Close();
            }
            catch (Exception e)
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

            return value;  
        }

        /// <summary>
        /// Pobiera id użytkownika na podstawie adresu email.
        /// </summary>
        /// <param name="email">Adres email użytkownika</param>
        /// <returns>Zwraca id użytkownika.</returns>
        public int GetRestaurantIdByEmail(string email)
        {
            int value = -1;

            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value = client.GetRestaurantIdByEmail(email);
                }
                client.Close();
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetRestaurantIdByEmail");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }

            return value;
        }

        /// <summary>
        /// Metoda override - zawsze zwraca NotImplementedException
        /// </summary>
        /// <param name="muser">MembershipUser - CustomMembershipUser</param>
        public override void UpdateUser(MembershipUser muser)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Aktualizuje dane użytkownika.
        /// </summary>
        /// <param name="muser">MembershipUser - CustomMembershipUser</param>
        public void CustomUpdateUser(MembershipUser muser, out string status )
        {
            status = null;
            User user = GetUserFromCustomMembershipUser((CustomMembershipUser)muser);

            string useremail = GetUserNameByEmail(user.Email);
            if (RequiresUniqueEmail && !(String.IsNullOrEmpty(useremail)))
            {
                if (useremail != user.Login)
                {
                    status = "Podany adres email jest zajęty";
                }
            }

            if (String.IsNullOrWhiteSpace(status))
            {
                bool value = false;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.UpdateUser(user);
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    value = false;
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
                
                if (value == false)
                {
                    status = "Nieznany błąd podczas zapisywania danych";
                }
            }

            
        }

        /// <summary>
        /// Sprawdza czy dany użytkownik posiada konto.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="password">Hasło użytkownika</param>
        /// <returns>True - jeśli użytkownik posiada konto.</returns>
        public override bool ValidateUser(string login, string password)
        {
            bool isValid = false;

            ValidateUser value = null;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value = client.ValidateUser(login);
                }
                client.Close();
            }
            catch (Exception e)
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

            if (value == null) return false;
            else
            {
                if (CheckPassword(password, value.Password))
                {
                    if (value.IsApproved)
                    {
                        isValid = true;

                        bool update = false;
                        try
                        {
                            ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                            using (client)
                            {
                                update = client.UpdateUserLoginDate(login);
                            }
                            client.Close();
                        }
                        catch (Exception e)
                        {
                            if (WriteExceptionsToEventLog)
                            {
                                WriteToEventLog(e, "UpdateUserLoginDate");
                                throw new ProviderException(exceptionMessage);
                            }
                            else
                            {
                                throw e;
                            }
                        }
                        if (update == false)
                        {
                            if (WriteExceptionsToEventLog)
                            {
                                WriteToEventLog(new Exception("Aktualizacja daty logowania nie powiodła się!"), "UpdateUserLoginDate");
                                throw new ProviderException(exceptionMessage);
                            }
                            else
                            {
                                throw new ProviderException(exceptionMessage);
                            }
                        }
                    }
                }
                else
                {
                 //   UpdateFailureCount(login, "password");
                }
            }

            return isValid;
        }

        /// <summary>
        /// Sprawdza czy dany pracownik posiada konto.
        /// </summary>
        /// <param name="login">Login pracownika</param>
        /// <param name="password">Hasło pracownika</param>
        /// /// <param name="rest">Login restauracji</param>
        /// <returns>True - jeśli pracownik posiada konto.</returns>
        public bool ValidateEmployee(string login, string password, string rest)
        {
            bool isValid = false;

            ValidateUser value = null;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value = client.ValidateEmployee(login, rest);
                }
                client.Close();
            }
            catch (Exception e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ValidateEmployee");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }

            if (value == null) return false;
            else
            {
                if (CheckPassword(password, value.Password))
                {
                    if (value.IsApproved)
                    {
                        isValid = true;

                        bool update = false;
                        try
                        {
                            ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                            using (client)
                            {
                                update = client.UpdateEmployeeLoginDate(login, rest);
                            }
                            client.Close();
                        }
                        catch (Exception e)
                        {
                            if (WriteExceptionsToEventLog)
                            {
                                WriteToEventLog(e, "UpdateEmployeeLoginDate");
                                throw new ProviderException(exceptionMessage);
                            }
                            else
                            {
                                throw e;
                            }
                        }
                        if (update == false)
                        {
                            if (WriteExceptionsToEventLog)
                            {
                                WriteToEventLog(new Exception("Aktualizacja daty logowania nie powiodła się!"), "UpdateEmployeeLoginDate");
                                throw new ProviderException(exceptionMessage);
                            }
                            else
                            {
                                throw new ProviderException(exceptionMessage);
                            }
                        }
                    }
                }
                else
                {
                    UpdateFailureCount(login, "password");
                }
            }

            return isValid;
        }

        /// <summary>
        /// Nie używać ze względu na niepowtarzalność loginów!
        /// </summary>
        /// <param name="loginToMatch"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords">Zawsze -1</param>
        /// <returns>Zawsze null</returns>
        public override MembershipUserCollection FindUsersByName(string loginToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = -1;
            return null;

            //MySqlConnection conn = new MySqlConnection(connectionString);
            //MySqlCommand command = new MySqlCommand(Queries.GetUserCountByLogin);
            //command.Parameters.AddWithValue("@login", loginToMatch);
            //command.Parameters.AddWithValue("@applicationName", pApplicationName);
            //command.Connection = conn;
            //MySqlDataReader reader = null;
            //try
            //{
            //    conn.Open();
            //    totalRecords = (int)command.ExecuteScalar();
            //    if (totalRecords <= 0) { return users; }
            //    command.CommandText = Queries.FindUsersByName;
            //    command.Connection = conn;
            //    reader = command.ExecuteReader();
            //    int counter = 0;
            //    int startIndex = pageSize * pageIndex;
            //    int endIndex = startIndex + pageSize - 1;
            //    while (reader.Read())
            //    {
            //        if (counter >= startIndex)
            //        {
            //            CustomMembershipUser u = GetUserFromReader(reader);
            //            users.Add(u);
            //        }
            //        if (counter >= endIndex) { command.Cancel(); }
            //        counter++;
            //    }
            //}
            //catch (MySqlException e)
            //{
            //    if (WriteExceptionsToEventLog)
            //    {
            //        WriteToEventLog(e, "FindUsersByName");

            //        throw new ProviderException(exceptionMessage);
            //    }
            //    else
            //    {
            //        throw e;
            //    }
            //}
            //finally
            //{
            //    if (reader != null) { reader.Close(); }

            //    conn.Close();
            //}

            //return users;
        }

        /// <summary>
        /// Nie używać ze względu na niepowtarzalność adresów email!
        /// </summary>
        /// <param name="loginToMatch"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords">Zawsze -1</param>
        /// <returns>Zawsze null</returns>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = -1;
            return null;

            //MySqlConnection conn = new MySqlConnection(connectionString);
            //MySqlCommand command = new MySqlCommand(Queries.GetUserCountByEmail);
            //command.Parameters.AddWithValue("@email", emailToMatch);
            //command.Parameters.AddWithValue("@applicationName", pApplicationName);
            //command.Connection = conn;
            //MembershipUserCollection users = new MembershipUserCollection();
            //MySqlDataReader reader = null;
            //totalRecords = 0;
            //try
            //{
            //    conn.Open();
            //    totalRecords = (int)command.ExecuteScalar();
            //    if (totalRecords <= 0) { return users; }
            //    command.CommandText = Queries.FindUsersByEmail;
            //    reader = command.ExecuteReader();
            //    int counter = 0;
            //    int startIndex = pageSize * pageIndex;
            //    int endIndex = startIndex + pageSize - 1;
            //    while (reader.Read())
            //    {
            //        if (counter >= startIndex)
            //        {
            //            CustomMembershipUser u = GetUserFromReader(reader);
            //            users.Add(u);
            //        }
            //        if (counter >= endIndex) { command.Cancel(); }
            //        counter++;
            //    }
            //}
            //catch (MySqlException e)
            //{
            //    //if (WriteExceptionsToEventLog)
            //    //{
            //    //    WriteToEventLog(e, "FindUsersByEmail");
            //    //    throw new ProviderException(exceptionMessage);
            //    //}
            //    //else
            //    {
            //        throw e;
            //    }
            //}
            //finally
            //{
            //    if (reader != null) { reader.Close(); }
            //    conn.Close();
            //}
            //return users;
        }

        /// <summary>
        /// Tworzy nową restaurację oraz zapisuje ją w bazie danych. 
        /// </summary>
        /// <param name="login">Login restauracji</param>
        /// <param name="email">Adres email</param>
        /// <param name="password">Hasło restauracji</param>
        /// <param name="passwordQuestion">Pytanie do odzyskiwania hasła</param>
        /// <param name="passwordAnswer">Odpowiedź do odzyskiwania hasła</param>
        /// <param name="name">Nazwa restauracji</param>
        /// <param name="displayName">Nazwa wyświetlana</param>
        /// <param name="address">Adres lokalu</param>
        /// <param name="townID">Id miasta</param>
        /// <param name="country">Kraj</param>
        /// <param name="telephone">Numer telefonu</param>
        /// <param name="nip">Nip</param>
        /// <param name="regon">REGON</param>
        /// <param name="deliveryTime">Czas dostawy</param>
        /// <param name="status">out MembershipCreateStatus</param>
        /// <returns>True jeśli restauracja została utworzona pomyslnie</returns>
        public bool CreateRestaurant(string login, string email, string password, string passwordQuestion, string passwordAnswer, string name, string displayName, string address, int townID, string country, string telephone, string nip, string regon, string deliveryTime, string managerLogin, out MembershipCreateStatus status, decimal deliveryPrice)
        {
            ValidatePasswordEventArgs args =
              new ValidatePasswordEventArgs(login, password, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return false;
            }

            if (RequiresUniqueEmail && !(String.IsNullOrEmpty(GetUserNameByEmail(email))))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return false;
            }

            MembershipUser u = GetUser(login, false);

            if (u == null)
            {
                bool value = false;
                try
                {
                    ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                    using (client)
                    {
                        value = client.AddRestaurant(login, email, EncodePassword(password), passwordQuestion, EncodePassword(passwordAnswer), name, displayName, address, townID, country, telephone, nip, regon, deliveryTime, managerLogin, deliveryPrice);
                    }
                    client.Close();
                }
                catch (Exception e)
                {
                    if (WriteExceptionsToEventLog)
                    {
                        WriteToEventLog(e, "CreateRestaurant");
                        status = MembershipCreateStatus.ProviderError;
                        throw new ProviderException(exceptionMessage);
                    }
                    else
                    {
                        status = MembershipCreateStatus.ProviderError;
                        throw e;
                    }
                }

                if (value == true)
                {
                    status = MembershipCreateStatus.Success;
                    return true;
                }
                else
                {
                    status = MembershipCreateStatus.UserRejected;
                    return false;
                }
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return false;
            }

            return false;
        }


        #endregion

        /// <summary>
        /// A helper method that performs the checks and updates associated with password failure tracking.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="failureType">Typ niepowodzenia</param>
        private void UpdateFailureCount(string login, string failureType)
        {
            bool value = false;
            try
            {
                ServiceReference.EresServiceClient client = new ServiceReference.EresServiceClient();
                using (client)
                {
                    value = client.UpdateFailureCount(login, failureType, PasswordAttemptWindow, MaxInvalidPasswordAttempts);
                }
                client.Close();
            }
            catch (Exception e)
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
            if (value == false)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(new Exception("UpdateFailureCount nie wykonał się poprawnie."), "UpdateFailureCount");
                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw new Exception("UpdateFailureCount nie wykonał się poprawnie.");
                }
            }
        }

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