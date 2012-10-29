using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;

namespace Contract
{
    public class Database
    {
        private string eventSource = "EresWindowsService";
        private string eventLog = "Erestauracja";
        private string message = "Wystąpił błąd związany z MySql podczas komunikacji z bazą danych.\n\n";
        private string message2 = "Wystąpił błąd podczas komunikacji z bazą danych.\n\n";

       //private string ConnectionString = "SERVER=" + "5.153.38.77" + ";DATABASE=" + "eres" + ";UID=" + "erestauracja" + ";PASSWORD=" + "Erestauracja123" + ";charset=utf8";
        private string ConnectionString = "SERVER=5.153.38.77;DATABASE=eres;UID=erestauracja;PASSWORD=Erestauracja123;charset=utf8;Encrypt=true;Connection Timeout=60";
       // private string ConnectionString = "SERVER=localhost;DATABASE=eres;UID=root;charset=utf8;Encrypt=true;Connection Timeout=60";
      
        //zabezpieczyć connectionString
        public Database()
        {
            //ConnectionStringSettingsCollection settings =
            //    ConfigurationManager.ConnectionStrings;

            //this.ConnectionString = settings[2].ConnectionString;
        }

        //
        //dorobić wewnętrzne ternzakcje ??
        //
        private DataSet ExecuteQuery(MySqlCommand command, string action)
        {
            MySqlConnection conn = new MySqlConnection();
            DataSet myDS = new DataSet();

            try
            {
                conn = new MySqlConnection(ConnectionString);
                conn.Open();
                command.Connection = conn;

                MySqlDataAdapter da = new MySqlDataAdapter(command);
                da.Fill(myDS);
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + action + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);
            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + action + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);
            }
            finally
            {
                conn.Close();
            }

            return myDS;
        }

        private  int ExecuteNonQuery(MySqlCommand command, string action)
        {
            MySqlConnection conn = new MySqlConnection();
            int rowsaffected = 0;
            try
            {
                conn = new MySqlConnection(ConnectionString);
                conn.Open();
                command.Connection = conn;

                rowsaffected = command.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + action + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);
            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + action + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);
            }
            finally
            {
                conn.Close();
            }

            return rowsaffected;
        }

        private object ExecuteScalar(MySqlCommand command, string action)
        {
            MySqlConnection conn = new MySqlConnection();
            object objekt = null;

            try
            {
                conn = new MySqlConnection(ConnectionString);
                conn.Open();
                command.Connection = conn;
                objekt = command.ExecuteScalar();
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + action + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);
            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + action + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);
            }
            finally
            {
                conn.Close();
            }
            return objekt;
        }

        #region membership
        public bool ChangePassword(string login, string password)
        {
            MySqlCommand command = new MySqlCommand(Queries.ChangePassword);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@lastPasswordChangedDate", DateTime.Now);
            command.Parameters.AddWithValue("@login", login);
            //command.Parameters.AddWithValue("@applicationName", pApplicationName);

            int rowsaffected = ExecuteNonQuery(command, "ChangePassword");

            if (rowsaffected > 0)
            {
                return true;
            }
            return false;  
        }

        public bool ChangePasswordQuestionAndAnswer(string login, string newPwdQuestion, string newPwdAnswer)
        {
            MySqlCommand command = new MySqlCommand(Queries.ChangePasswordQuestionAndAnswer);
            command.Parameters.AddWithValue("@question", newPwdQuestion);
            command.Parameters.AddWithValue("@answer", newPwdAnswer);
            command.Parameters.AddWithValue("@login", login);
            //command.Parameters.AddWithValue("@applicationName", pApplicationName);

            int rowsaffected = ExecuteNonQuery(command, "ChangePasswordQuestionAndAnswer");

            if (rowsaffected > 0)
            {
                return true;
            }
            return false;
        }

        public PasswordAndAnswer GetPassword(string login)
        {
            MySqlCommand command = new MySqlCommand(Queries.GetPassword);
            command.Parameters.AddWithValue("@login", login);

            PasswordAndAnswer value = null;

            DataSet ds = new DataSet();
            ds = ExecuteQuery(command, "GetPassword");

            if (ds.Tables.Count > 0)
            {
                value = new PasswordAndAnswer();
                value.Password = null;
                value.PasswordAnswer = null;
                value.IsLockedOut = false;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["password"] != DBNull.Value) value.Password = row["password"].ToString();
                    if (row["passwordAnswer"] != DBNull.Value) value.PasswordAnswer = row["passwordAnswer"].ToString();
                    if (row["isLockedOut"] != DBNull.Value) value.IsLockedOut = Convert.ToBoolean(row["isLockedOut"]);
                }
            }
            return value;
        }

        public PasswordAnswer GetPasswordAnswer(string login)
        {
            MySqlCommand command = new MySqlCommand(Queries.GetPasswordAnswer);
            command.Parameters.AddWithValue("@login", login);
            ////command.Parameters.AddWithValue("@applicationName", pApplicationName);

            PasswordAnswer value = null;

            DataSet ds = new DataSet();
            ds = ExecuteQuery(command, "GetPasswordAnswer");

            if (ds.Tables.Count > 0)
            {
                value = new PasswordAnswer();
                value.Answer = null;
                value.IsLockedOut = false;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["passwordAnswer"] != DBNull.Value) value.Answer = row["passwordAnswer"].ToString();
                    if (row["isLockedOut"] != DBNull.Value) value.IsLockedOut = Convert.ToBoolean(row["isLockedOut"]);
                }
            }
            return value;
        }

        public bool ResetPassword(string login, string password)
        {
            string email=null;
            try
            {
                MySqlConnection conn = new MySqlConnection(ConnectionString);
                conn.Open();
                MySqlTransaction trans;
                trans = conn.BeginTransaction();

                int rowsAffected = 0;

                MySqlCommand reset = new MySqlCommand(Queries.ResetPassword);
                reset.Parameters.AddWithValue("@password", password);
                reset.Parameters.AddWithValue("@lastPasswordChangedDate", DateTime.Now);
                reset.Parameters.AddWithValue("@login", login);
                reset.Parameters.AddWithValue("@isLockedOut", false);
                reset.Connection = conn;
                reset.Transaction = trans;
                try
                {
                    MySqlCommand getcommand = new MySqlCommand(Queries.GetEmailByLogin);
                    getcommand.Parameters.AddWithValue("@login", login);
                    getcommand.Connection = conn;

                    DataSet ds = new DataSet();
                    ds = ExecuteQuery(getcommand, "GetEmailByLogin");

                    if (ds.Tables.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (row["email"] != DBNull.Value) email = row["email"].ToString();
                            else return false;
                        }
                    }
                    else return false;

                    rowsAffected = reset.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        if (!String.IsNullOrEmpty(email))
                        {
                            SmtpClient klient = new SmtpClient("smtp.gmail.com");
                            MailMessage wiadomosc = new MailMessage();
                            try
                            {
                                wiadomosc.From = new MailAddress("erestauracja@gmail.com");
                                wiadomosc.To.Add(email);
                                wiadomosc.Subject = "Erestauracja - restet hasła.";
                                wiadomosc.Body = "Nowe hasło: " + password;

                                klient.Port = 587;
                                klient.Credentials = new System.Net.NetworkCredential("erestauracja", "Erestauracja123");
                                klient.EnableSsl = true;
                                klient.Send(wiadomosc);

                                trans.Commit();
                                return true;
                            }
                            catch (Exception ex)
                            {
                                EventLog log = new EventLog();
                                log.Source = eventSource;
                                log.Log = eventLog;

                                string info = "Błąd podczas wysyłania wiadomości email";
                                info += "Action: " + "Email sending" + "\n\n";
                                info += "Exception: " + ex.ToString();

                                trans.Rollback();
                                return false;
                            }
                        }
                        else return false;
                    }
                    else
                    {
                        trans.Rollback();
                        return false;
                    }
                }
                catch (Exception e)
                {
                    EventLog log = new EventLog();
                    log.Source = eventSource;
                    log.Log = eventLog;

                    string info = "Błąd podczas resetowania hasła";
                    info += "Action: " + "ResetPassword" + "\n\n";
                    info += "Exception: " + e.ToString();
                    trans.Rollback();
                    return false;
                }
                finally
                {
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string info = "Błąd podczas resetowania hasła";
                info += "Action: " + "ResetPassword" + "\n\n";
                info += "Exception: " + ex.ToString();
            }

            return false;
        }

        public PasswordQuestion GetUserQuestion(string login)
        {
            MySqlCommand command = new MySqlCommand(Queries.GetUserQuestion);
            command.Parameters.AddWithValue("@login", login);
            ////command.Parameters.AddWithValue("@applicationName", pApplicationName);

            PasswordQuestion value = null;

            DataSet ds = new DataSet();
            ds = ExecuteQuery(command, "GetUserQuestion");

            if (ds.Tables.Count > 0)
            {
                value = new PasswordQuestion();
                value.Question = null;
                value.IsLockedOut = false;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["passwordQuestion"] != DBNull.Value) value.Question = row["passwordQuestion"].ToString();
                    if (row["isLockedOut"] != DBNull.Value) value.IsLockedOut = Convert.ToBoolean(row["isLockedOut"]);
                }
            }
            return value;
        }

        public bool CreateUser(string login, string password, string email, string name, string surname, string address, int townID, string country, DateTime birthdate, string sex, string telephone, string passwordQuestion, string passwordAnswer, bool isApproved)
        {
            DateTime createDate = DateTime.Now;

            MySqlCommand command = new MySqlCommand(Queries.CreateUser);
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@surname", surname);
            command.Parameters.AddWithValue("@address", address);
            command.Parameters.AddWithValue("@townID", townID);
            command.Parameters.AddWithValue("@country", country);
            command.Parameters.AddWithValue("@birthdate", birthdate);
            if (sex == "Kobieta")
                command.Parameters.AddWithValue("@sex", true);
            else
                command.Parameters.AddWithValue("@sex", false);
            command.Parameters.AddWithValue("@telephone", telephone);
            command.Parameters.AddWithValue("@comment", "");
            command.Parameters.AddWithValue("@passwordQuestion", passwordQuestion);
            command.Parameters.AddWithValue("@passwordAnswer", passwordAnswer);
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

            int rowsaffected = ExecuteNonQuery(command, "CreateUser");

            if (rowsaffected > 0)
            {
                return true;
            }
            return false;
        }

        public bool DeleteUser(string login, bool deleteAllRelatedData)
        {
            if (deleteAllRelatedData)
            {
                // Process commands to delete all data for the user in the database.
                //dopisać
                //co zmienić jak sie usera usuwa
                // + tranzakcje
            }

            MySqlCommand command = new MySqlCommand(Queries.DeleteUser);
            command.Parameters.AddWithValue("@login", login);

            int rowsaffected = ExecuteNonQuery(command, "DeleteUser");

            if (rowsaffected > 0)
            {
                return true;
            }
            return false;  
        }

        public List<User> GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlCommand command = new MySqlCommand(Queries.AllUsersCount);
            command.Connection = conn;

            List<User> users = new List<User>();

            MySqlDataReader reader = null;
            totalRecords = 0;

            try
            {
                conn.Open();
                totalRecords = Convert.ToInt32(command.ExecuteScalar());

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
                        User u = GetUserFromReader(reader);
                        users.Add(u);
                    }

                    if (counter >= endIndex) { command.Cancel(); }

                    counter++;
                }
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetAllUsers" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;

            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "GetAllUsers" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            return users;
            
        }

        private User GetUserFromReader(MySqlDataReader reader)
        {
            int id = reader.GetInt32(0);
            string login = reader.GetString(1);
            string email = reader.GetString(2);
            string name = reader.GetString(3);
            string surname = reader.GetString(4);
            string address = reader.GetString(5);
            string town = reader.GetString(6);
            string postalCode = reader.GetString(7);
            string country = reader.GetString(8);
            DateTime birthdate = Convert.ToDateTime(reader["birthdate"].ToString());//Convert.ToDateTime(reader.GetDateTime(8) );// GetString(8)); //Convert.ToDateTime(reader["date"].ToString());//reader.GetDateTime(8);
            string sex = "Mężczyzna";
           // bool plec = false;
            if (reader.GetBoolean(10) == true)
                sex = "Kobieta";
           // else
           //     command.Parameters.AddWithValue("@sex", false);
           //     reader.GetString(9);
            string telephone = reader.GetString(11);
            string comment = "";
            if (reader.GetValue(12) != DBNull.Value)
                comment = reader.GetString(12);
            string passwordQuestion = "";
            if (reader.GetValue(13) != DBNull.Value)
                passwordQuestion = reader.GetString(13);
            bool isApproved = reader.GetBoolean(14);
            DateTime lastActivityDate = Convert.ToDateTime(reader.GetString(15)); //reader.GetDateTime(14);
            DateTime lastLoginDate = new DateTime();
            if (reader.GetValue(16) != DBNull.Value)
                lastLoginDate = Convert.ToDateTime(reader.GetString(16)); //reader.GetDateTime(15);
            DateTime lastPasswordChangedDate = Convert.ToDateTime(reader.GetString(17)); //reader.GetDateTime(16);
            DateTime creationDate = Convert.ToDateTime(reader.GetString(18)); //reader.GetDateTime(17);
            bool isLockedOut = reader.GetBoolean(19);
            DateTime lastLockedOutDate = new DateTime();
            if (reader.GetValue(20) != DBNull.Value)
                lastLockedOutDate = Convert.ToDateTime(reader.GetString(20)); //reader.GetDateTime(19);
            
            User u = new User();
            u.Email = email;
            u.PasswordQuestion = passwordQuestion;
            u.Comment = comment;
            u.IsApproved = isApproved;
            u.IsLockedOut = isLockedOut;
            u.CreationDate = creationDate;
            u.LastLoginDate = lastLoginDate;
            u.LastActivityDate = lastActivityDate;
            u.LastPasswordChangedDate = lastPasswordChangedDate;
            u.LastLockedOutDate = lastLockedOutDate;
            u.ID = id;
            u.Login = login;
            u.Name = name;
            u.Surname = surname;
            u.Address = address;
            u.Town = town;
            u.PostalCode = postalCode;
            u.Country = country;
            u.Birthdate = birthdate;
            u.Sex = sex;
            u.Telephone = telephone;

            return u;
        }

        public int GetNumberOfUsersOnline(TimeSpan onlineSpan)
        {
            DateTime compareTime = DateTime.Now.Subtract(onlineSpan);

            MySqlCommand command = new MySqlCommand(Queries.GetNumberOfUsersOnline);
            command.Parameters.AddWithValue("@lastActivityDate", compareTime);

            int rowsaffected = Convert.ToInt32(ExecuteScalar(command, "GetNumberOfUsersOnline"));

            if (rowsaffected > 0)
            {
                return rowsaffected;
            }
            return 0;  
    }

        public User GetUser(string login, bool userIsOnline)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlCommand command = new MySqlCommand(Queries.GetUserByLogin);
            command.Parameters.AddWithValue("@login", login);
            command.Connection = conn;

            User u = null;
            MySqlDataReader reader = null;

            try
            {
                conn.Open();

                reader = command.ExecuteReader(CommandBehavior.SingleRow);

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
                        updateCmd.Connection = conn;

                        updateCmd.ExecuteNonQuery();
                    }
                }

            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetUser" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;

            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "GetUser" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;
            }
            finally
            {
                if (reader != null) { reader.Close(); }

                conn.Close();
            }

            return u;
        }

        public User GetUser(int id, bool userIsOnline)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlCommand command = new MySqlCommand(Queries.GetUserByID);
            command.Parameters.AddWithValue("@id", (int)id);
            command.Connection = conn;

            User u = null;
            MySqlDataReader reader = null;

            try
            {
                conn.Open();

                reader = command.ExecuteReader(CommandBehavior.SingleRow);

                if (reader.HasRows)
                {
                    reader.Read();
                    u = GetUserFromReader(reader);
                    reader.Close();
                    if (userIsOnline)
                    {
                        MySqlCommand updateCmd = new MySqlCommand(Queries.UpdateUserActivityByID);

                        updateCmd.Parameters.AddWithValue("@lastActivityDate", DateTime.Now);
                        updateCmd.Parameters.AddWithValue("@id", (int)id);
                        updateCmd.Connection = conn;

                        updateCmd.ExecuteNonQuery();
                    }
                }

            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetUser" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;

            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "GetUser" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;
            }
            finally
            {
                if (reader != null) { reader.Close(); }

                conn.Close();
            }

            return u;
        }

        public bool UnlockUser(string login)
        {
            MySqlCommand command = new MySqlCommand(Queries.UnlockUser);
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@isLockedOut", false);
            command.Parameters.AddWithValue("@lastLockedOutDate", DateTime.Now);

            int rowsaffected = ExecuteNonQuery(command, "UnlockUser");

            if (rowsaffected > 0)
            {
                return true;
            }
            return false;
        }

        public string GetUserNameByEmail(string email)
        {
            MySqlCommand command = new MySqlCommand(Queries.GetUserNameByEmail);
            command.Parameters.AddWithValue("@email", email);

            string rowsaffected = (string)(ExecuteScalar(command, "GetUserNameByEmail"));

            if (!( String.IsNullOrEmpty(rowsaffected)))
            {
                return rowsaffected;
            }
            else
            {
                return null;
            }
        }

        public bool UpdateUser(User user)
        {
            MySqlCommand command = new MySqlCommand(Queries.UpdateUser);
            command.Parameters.AddWithValue("@name", user.Name);
            command.Parameters.AddWithValue("@surname", user.Surname);
            command.Parameters.AddWithValue("@address", user.Address);
            command.Parameters.AddWithValue("@town_name", user.Town);
            command.Parameters.AddWithValue("@postal_code", user.PostalCode);
            command.Parameters.AddWithValue("@country", user.Country);
            command.Parameters.AddWithValue("@birthdate", user.Birthdate);
            if (user.Sex == "Kobieta")
                command.Parameters.AddWithValue("@sex", true);
            else
                command.Parameters.AddWithValue("@sex", false);
           // command.Parameters.AddWithValue("@sex", user.Sex);
            command.Parameters.AddWithValue("@telephone", user.Telephone);
            command.Parameters.AddWithValue("@comment", user.Comment);
            command.Parameters.AddWithValue("@isApproved", user.IsApproved);
            command.Parameters.AddWithValue("@login", user.Login);

            int rowsaffected = ExecuteNonQuery(command, "UpdateUser");

            if (rowsaffected > 0)
            {
                return true;
            }
            return false;
        }

        public ValidateUser ValidateUser(string login)
        {
            MySqlCommand command = new MySqlCommand(Queries.ValidateUser);
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@isLockedOut", false);

            ValidateUser value = null;

            DataSet ds = new DataSet();
            ds = ExecuteQuery(command, "ValidateUser");

            if (ds.Tables.Count > 0)
            {
                value = new ValidateUser();
                value.Password = null;
                value.IsApproved = false;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["password"] != DBNull.Value) value.Password = row["password"].ToString();
                    if (row["isApproved"] != DBNull.Value) value.IsApproved = Convert.ToBoolean(row["isApproved"]);
                }
            }
            return value;
        }

        public bool UpdateUserLoginDate(string login)
        {
            MySqlCommand command = new MySqlCommand(Queries.UpdateUserLoginDate);
            command.Parameters.AddWithValue("@lastLoginDate", DateTime.Now);
            command.Parameters.AddWithValue("@login", login);

            int rowsaffected = ExecuteNonQuery(command, "UpdateUserLoginDate");

            if (rowsaffected > 0)
            {
                return true;
            }
            return false;
        }

        public bool UpdateFailureCount(string login, string failureType, int PasswordAttemptWindow, int MaxInvalidPasswordAttempts)
        {
            MySqlCommand command = new MySqlCommand(Queries.GetFailureCount);
            command.Parameters.AddWithValue("@login", login);

            DateTime windowStart = new DateTime();
            int failureCount = 0;

            DataSet ds = new DataSet();
            ds = ExecuteQuery(command, "UpdateFailureCount");

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (failureType == "password")
                    {
                        if (row["failedPasswordAttemptCount"] != DBNull.Value) failureCount = Convert.ToInt32(row["failedPasswordAttemptCount"]);
                        if (row["failedPasswordAttemptWindowStart"] != DBNull.Value) windowStart = Convert.ToDateTime(row["failedPasswordAttemptWindowStart"]);
                    }

                    if (failureType == "passwordAnswer")
                    {

                        if (row["failedPasswordAnswerAttemptCount"] != DBNull.Value) failureCount = Convert.ToInt32(row["failedPasswordAnswerAttemptCount"]);
                        if (row["failedPasswordAnswerAttemptWindowStart"] != DBNull.Value) windowStart = Convert.ToDateTime(row["failedPasswordAnswerAttemptWindowStart"]);
                    }
                }
            }

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

                command.Parameters.AddWithValue("@count", 1);
                command.Parameters.AddWithValue("@windowStart", DateTime.Now);
                command.Parameters.AddWithValue("@login", login);

                int rowsaffected = ExecuteNonQuery(command, "UpdateFailed" + failureType + "Attempt");

                if (rowsaffected < 0)
                {
                    return false;
                }
            }
            else
            {
                if (failureCount++ >= MaxInvalidPasswordAttempts)
                {
                    // Password attempts have exceeded the failure threshold. Lock out
                    // the user.

                    command.CommandText = Queries.LockOutUser;

                    command.Parameters.Clear();

                    command.Parameters.AddWithValue("@isLockedOut", true);
                    command.Parameters.AddWithValue("@lastLockedOutDate", DateTime.Now);
                    command.Parameters.AddWithValue("@login", login);

                    int rowsaffected = ExecuteNonQuery(command, "LockOutUser");

                    if (rowsaffected < 0)
                    {
                        return false;
                    }
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

                    command.Parameters.AddWithValue("@count", failureCount);
                    command.Parameters.AddWithValue("@login", login);

                    int rowsaffected = ExecuteNonQuery(command, "Unable to update failure count.");

                    if (rowsaffected < 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion

        #region role

        public bool AddUsersToRoles(string[] logins, string[] rolenames)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlCommand command = new MySqlCommand(Queries.AddUsersToRoles);
            command.Connection = conn;

            MySqlTransaction tran=null;

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

                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "AddUsersToRoles" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                return false;
            }
            catch (Exception ex)
            {
                try
                {
                    tran.Rollback();
                }
                catch { }

                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "AddUsersToRoles" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                return false;
            }
            finally
            {
                conn.Close();
            }
            return true;
        }

        public bool CreateRole(string rolename)
        {
            MySqlCommand command = new MySqlCommand(Queries.CreateRole);
            command.Parameters.AddWithValue("@rolename", rolename);

            int rowsaffected = ExecuteNonQuery(command, "CreateRole");

            if (rowsaffected > 0)
            {
                return true;
            }
            return false;
        }

        public bool DeleteRole(string rolename)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlCommand command = new MySqlCommand(Queries.DeleteRole);
            command.Parameters.AddWithValue("@rolename", rolename);
            command.Connection = conn;

            MySqlCommand command2 = new MySqlCommand(Queries.DeleteUsersInRole);
            command2.Parameters.AddWithValue("@rolename", rolename);
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

                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "DeleteRole" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                return false;
            }
            catch (Exception ex)
            {
                try
                {
                    tran.Rollback();
                }
                catch { }

                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "DeleteRole" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                return false;
            }
            finally
            {
                conn.Close();
            }
            return true;
        }

        public string GetAllRoles()
        {
            string tmpRoleNames = "";

            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlCommand command = new MySqlCommand(Queries.GetAllRoles);
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
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetAllRoles" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                return null;
            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "GetAllRoles" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                return null;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }
            return tmpRoleNames;
        }

        public string GetRolesForUser(string login)
        {
            string tmpRoleNames = "";

            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlCommand command = new MySqlCommand(Queries.GetRolesForUser);
            command.Parameters.AddWithValue("@login", login);

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
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetRolesForUser" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                return null;
            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "GetRolesForUser" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                return null;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            return tmpRoleNames;
        }

        public string GetUsersInRole(string rolename)
        {
            string tmpUserNames = "";

            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlCommand command = new MySqlCommand(Queries.GetUsersInRole);
            command.Parameters.AddWithValue("@rolename", rolename);

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
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetUsersInRole" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                return null;
            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "GetUsersInRole" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                return null;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            return tmpUserNames;
        }

        public bool IsUserInRole(string login, string rolename)
        {
            MySqlCommand command = new MySqlCommand(Queries.IsUserInRole);
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@rolename", rolename);

            object value = ExecuteScalar(command, "IsUserInRole");
            int numRecs = (value == null ? 0 : Convert.ToInt32(value.ToString()));

            if (numRecs > 0)
            {
                return true;
            }
            return false;
        }

        public bool RemoveUsersFromRoles(string[] logins, string[] rolenames)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlCommand command = new MySqlCommand(Queries.RemoveUsersFromRoles);
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

                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "RemoveUsersFromRoles" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                return false;
            }
            catch (Exception ex)
            {
                try
                {
                    tran.Rollback();
                }
                catch { }

                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "RemoveUsersFromRoles" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                return false;
            }
            finally
            {
                conn.Close();
            }
            return true;
        }

        public bool RoleExists(string rolename)
        {
            MySqlCommand command = new MySqlCommand(Queries.RoleExists);
            command.Parameters.AddWithValue("@rolename", rolename);

            object value = ExecuteScalar(command, "RoleExists");
            int numRecs = (value == null ? 0 : Convert.ToInt32(value.ToString()));

            if (numRecs > 0)
            {
                return true;
            }
            return false;
        }

        public string FindUsersInRole(string rolename, string loginToMatch)
        {
            string tmpUserNames = "";

            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlCommand command = new MySqlCommand(Queries.FindUsersInRole);
            if (String.IsNullOrWhiteSpace(loginToMatch))
            {
                command.Parameters.AddWithValue("@login", "%");
            }
            else
            {
                command.Parameters.AddWithValue("@login", loginToMatch+"%");
            }
            command.Parameters.AddWithValue("@rolename", rolename);
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
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "FindUsersInRole" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                return null;
            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "FindUsersInRole" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                return null;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            return tmpUserNames;
        }

        #endregion

        #region Manage restaurant

        public bool AddRestaurant(string name, string displayName, string address, int townId, string country, string telephone, string email, string nip, string regon, string password, string managerLogin, string deliveryTime)
        {
            DateTime createDate = DateTime.Now;

            MySqlCommand command = new MySqlCommand(Queries.AddRestaurant);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@displayName", displayName);
            command.Parameters.AddWithValue("@address", address);
            command.Parameters.AddWithValue("@townId", townId);
            command.Parameters.AddWithValue("@country", country);
            command.Parameters.AddWithValue("@telephone", telephone);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@nip", nip);
            command.Parameters.AddWithValue("@regon", regon);
            command.Parameters.AddWithValue("@creationData", createDate);
            command.Parameters.AddWithValue("@inputsCount", 0);
            command.Parameters.AddWithValue("@averageRating", 0);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@menager", managerLogin);
            command.Parameters.AddWithValue("@deliveryTime", deliveryTime);
            command.Parameters.AddWithValue("@currentDeliveryTime", "00:00:00");
            command.Parameters.AddWithValue("@isApproved", true);
            command.Parameters.AddWithValue("@lastActivityDate", createDate);
            command.Parameters.AddWithValue("@isOnLine", false);
            command.Parameters.AddWithValue("@isLockedOut", false);
            command.Parameters.AddWithValue("@lastLockedOutDate", createDate);

            int rowsaffected = ExecuteNonQuery(command, "AddRestaurant");

            if (rowsaffected > 0)
            {
                return true;
            }
            return false;
        }

        public bool EditRestaurant(string name, string displayName, string address, int townId, string country, string telephone, string email, string nip, string regon, string deliveryTime, string managerLogin, int id)
        {
            MySqlCommand command = new MySqlCommand(Queries.EditRestaurant);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@displayName", displayName);
            command.Parameters.AddWithValue("@address", address);
            command.Parameters.AddWithValue("@townId", townId);
            command.Parameters.AddWithValue("@country", country);
            command.Parameters.AddWithValue("@telephone", telephone);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@nip", nip);
            command.Parameters.AddWithValue("@regon", regon);
            command.Parameters.AddWithValue("@deliveryTime", deliveryTime);

            command.Parameters.AddWithValue("@menager", managerLogin);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@isLockedOut", false);

            int rowsaffected = ExecuteNonQuery(command, "EditRestaurant");

            if (rowsaffected > 0)
            {
                return true;
            }
            return false;
        }

        public List<Restaurant> GetRestaurantsByManagerLogin(string managerLogin)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            List<Restaurant> rest = null;
            try
            {
                MySqlCommand command = new MySqlCommand(Queries.GetRestaurantsByManagerLogin);
                command.Parameters.AddWithValue("@managerLogin", managerLogin);
                command.Connection = conn;
                rest = new List<Restaurant>();
                conn.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Restaurant r = GetRestaurantsFromReader(reader);
                    rest.Add(r);
                }
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetRestaurantsByManagerLogin" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;

            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "GetRestaurantsByManagerLogin" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            return rest;
        }

        public RestaurantInfo GetRestaurant(string managerLogin, int id)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            RestaurantInfo rest = null;
            try
            {
                MySqlCommand command = new MySqlCommand(Queries.GetRestaurant);
                command.Parameters.AddWithValue("@managerLogin", managerLogin);
                command.Parameters.AddWithValue("@id", id);
                command.Connection = conn;
                rest = new RestaurantInfo();
                conn.Open();

                reader = command.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rest.ID = reader.GetInt32(0);
                        rest.Name = reader.GetString(1);
                        rest.DisplayName = reader.GetString(2);
                        rest.Address = reader.GetString(3);
                        rest.Town = reader.GetString(4);
                        rest.PostalCode = reader.GetString(5);
                        rest.Country = reader.GetString(6);
                        rest.Telephone = reader.GetString(7);
                        rest.Email = reader.GetString(8);
                        rest.Nip = reader.GetString(9);
                        rest.Regon = reader.GetString(10);
                        rest.DeliveryTime = reader.GetString(11);
                    }
                }
                else
                    return null;
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetRestaurant" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;

            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "GetRestaurant" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            return rest;
        }

        private Restaurant GetRestaurantsFromReader(MySqlDataReader reader)
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            string displayName = reader.GetString(2);
            string address = reader.GetString(3);
            string town = reader.GetString(4);
            string postalCode = reader.GetString(5);
            string country = reader.GetString(6);
            string telephone = reader.GetString(7);
            string email = reader.GetString(8);
            string nip = reader.GetString(9);
            string regon = reader.GetString(10);
            DateTime creationDate = Convert.ToDateTime(reader.GetString(11));
            int inputsCount = reader.GetInt32(12);
            int averageRating = reader.GetInt32(13);
            string password = reader.GetString(14);
            int menagerId = reader.GetInt32(15);
            string deliveryTime = reader.GetString(16);
            string currentDeliveryTime = reader.GetString(17);
            bool isApproved = reader.GetBoolean(18);
            DateTime lastActivityDate = Convert.ToDateTime(reader.GetString(19));
            bool isLockedOut = reader.GetBoolean(20);
            DateTime lastLockedOutDate = new DateTime();
            if (reader.GetValue(21) != DBNull.Value)
                lastLockedOutDate = Convert.ToDateTime(reader.GetString(21));

            Restaurant u = new Restaurant();
            u.ID = id;
            u.Name = name;
            u.DisplayName = displayName;
            u.Address = address;
            u.Town = town;
            u.PostalCode = postalCode;
            u.Country = country;
            u.Telephone = telephone;
            u.Email = email;
            u.Nip = nip;
            u.Regon = regon;
            u.CreationDate = creationDate;
            u.InputsCount = inputsCount;
            u.AverageRating = averageRating;
            u.Password = password;
            u.MenagerId = menagerId;
            u.DeliveryTime = deliveryTime;
            u.CurrentDeliveryTime = currentDeliveryTime;
            u.IsApproved = isApproved;
            u.LastActivityDate = lastActivityDate;
            u.IsLockedOut = isLockedOut;
            u.LastLockedOutDate = lastLockedOutDate;

            return u;
        }

        public MainPageContent GetMainPage(string managerLogin, int id)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            MainPageContent rest = null;
            try
            {
                MySqlCommand command = new MySqlCommand(Queries.GetMainPage);
                command.Parameters.AddWithValue("@managerLogin", managerLogin);
                command.Parameters.AddWithValue("@id", id);
                command.Connection = conn;
                rest = new MainPageContent();
                conn.Open();

                reader = command.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rest.Description = reader.GetString(0);
                        rest.Foto = reader.GetString(1);
                        rest.SpecialOffers = reader.GetString(2);
                    }
                }
                else
                    return null;
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetMainPage" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;

            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "GetMainPage" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            return rest;
        }

        public bool EditMainPage(string description, string foto, string specialOffers, int id, string managerLogin)
        {
            MySqlCommand command = new MySqlCommand(Queries.EditMainPage);
            command.Parameters.AddWithValue("@description", description);
            command.Parameters.AddWithValue("@foto", foto);
            command.Parameters.AddWithValue("@specialOffers", specialOffers);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@managerLogin", managerLogin);

            int rowsaffected = ExecuteNonQuery(command, "EditMainPage");

            if (rowsaffected > 0)
            {
                return true;
            }
            return false;
        }

        public DeliveryPageContent GetDeliveryPage(string managerLogin, int id)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            DeliveryPageContent rest = null;
            try
            {
                MySqlCommand command = new MySqlCommand(Queries.GetDeliveryPage);
                command.Parameters.AddWithValue("@managerLogin", managerLogin);
                command.Parameters.AddWithValue("@id", id);
                command.Connection = conn;
                rest = new DeliveryPageContent();
                conn.Open();

                reader = command.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rest.Delivery = reader.GetString(0);
                    }
                }
                else
                    return null;
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetDeliveryPage" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;

            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "GetDeliveryPage" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            return rest;
        }

        public bool EditDeliveryPage(string delivery, int id, string managerLogin)
        {
            MySqlCommand command = new MySqlCommand(Queries.EditDeliveryPage);
            command.Parameters.AddWithValue("@delivery", delivery);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@managerLogin", managerLogin);

            int rowsaffected = ExecuteNonQuery(command, "EditDeliveryPage");

            if (rowsaffected > 0)
            {
                return true;
            }
            return false;

        }

        public EventsPageContent GetEventsPage(string managerLogin, int id)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            EventsPageContent rest = null;
            try
            {
                MySqlCommand command = new MySqlCommand(Queries.GetEventsPage);
                command.Parameters.AddWithValue("@managerLogin", managerLogin);
                command.Parameters.AddWithValue("@id", id);
                command.Connection = conn;
                rest = new EventsPageContent();
                conn.Open();

                reader = command.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rest.Events = reader.GetString(0);
                    }
                }
                else
                    return null;
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetEventsPage" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;

            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "GetEventsPage" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            return rest;
        }

        public bool EditEventsPage(string events, int id, string managerLogin)
        {
            MySqlCommand command = new MySqlCommand(Queries.EditEventsPage);
            command.Parameters.AddWithValue("@events", events);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@managerLogin", managerLogin);

            int rowsaffected = ExecuteNonQuery(command, "EditEventsPage");

            if (rowsaffected > 0)
            {
                return true;
            }
            return false;

        }

        public ContactPageContent GetContactPage(string managerLogin, int id)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            ContactPageContent rest = null;
            try
            {
                MySqlCommand command = new MySqlCommand(Queries.GetContactPage);
                command.Parameters.AddWithValue("@managerLogin", managerLogin);
                command.Parameters.AddWithValue("@id", id);
                command.Connection = conn;
                rest = new ContactPageContent();
                conn.Open();

                reader = command.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rest.Contact = reader.GetString(0);
                    }
                }
                else
                    return null;
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetContactPage" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;

            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "GetContactPage" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            return rest;
        }

        public bool EditContactPage(string contact, int id, string managerLogin)
        {
            MySqlCommand command = new MySqlCommand(Queries.EditContactPage);
            command.Parameters.AddWithValue("@contact", contact);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@managerLogin", managerLogin);

            int rowsaffected = ExecuteNonQuery(command, "EditContactPage");

            if (rowsaffected > 0)
            {
                return true;
            }
            return false;

        }

        public bool AddCategory(int restaurantID, string categoryName, string categoryDescription, string priceOption, string nonPriceOption, string nonPriceOption2, string managerLogin)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand commandTest = new MySqlCommand(Queries.IsRestaurantOwner);
                commandTest.Parameters.AddWithValue("@managerLogin", managerLogin);
                commandTest.Parameters.AddWithValue("@restaurantID", restaurantID);
                commandTest.Connection = conn;
                conn.Open();

                reader = commandTest.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.HasRows)
                {
                    MySqlCommand command = new MySqlCommand(Queries.AddCategory);
                    command.Parameters.AddWithValue("@restaurantID", restaurantID);
                    command.Parameters.AddWithValue("@categoryName", categoryName);
                    command.Parameters.AddWithValue("@categoryDescription", categoryDescription);
                    command.Parameters.AddWithValue("@priceOption", priceOption);
                    command.Parameters.AddWithValue("@nonPriceOption", nonPriceOption);
                    command.Parameters.AddWithValue("@nonPriceOption2", nonPriceOption2);

                    int rowsaffected = ExecuteNonQuery(command, "AddCategory");

                    if (rowsaffected > 0)
                    {
                        return true;
                    }
                }
                else
                    return false;
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "IsRestaurantOwner" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return false;

            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "IsRestaurantOwner" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            return false;
        }

        public List<Category> GetCategories(string managerLogin, int restaurantID)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand commandTest = new MySqlCommand(Queries.IsRestaurantOwner);
                commandTest.Parameters.AddWithValue("@managerLogin", managerLogin);
                commandTest.Parameters.AddWithValue("@restaurantID", restaurantID);
                commandTest.Connection = conn;
                conn.Open();

                reader = commandTest.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.HasRows)
                {
                    reader.Close();
                    conn.Close();
                    ///////////////////////////////
                    MySqlDataReader reader2 = null;
                    List<Category> rest = null;
                    try
                    {
                        MySqlCommand command = new MySqlCommand(Queries.GetCategories);
                        command.Parameters.AddWithValue("@restaurantID", restaurantID);
                        command.Connection = conn;
                        rest = new List<Category>();
                        conn.Open();

                        reader2 = command.ExecuteReader();

                        while (reader2.Read())
                        {
                            Category r = GetCategoriesFromReader(reader2);
                            rest.Add(r);
                        }
                    }
                    catch (MySqlException e)
                    {
                        EventLog log = new EventLog();
                        log.Source = eventSource;
                        log.Log = eventLog;

                        string wiadomosc = message;
                        wiadomosc += "Action: " + "GetCategories" + "\n\n";
                        wiadomosc += "Exception: " + e.ToString();

                        log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                        if (reader2 != null) { reader2.Close(); }
                        conn.Close();
                        return null;

                    }
                    catch (Exception ex)
                    {
                        EventLog log = new EventLog();
                        log.Source = eventSource;
                        log.Log = eventLog;

                        string wiadomosc = message2;
                        wiadomosc += "Action: " + "GetCategories" + "\n\n";
                        wiadomosc += "Exception: " + ex.ToString();

                        log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                        if (reader2 != null) { reader2.Close(); }
                        conn.Close();
                        return null;
                    }
                    finally
                    {
                        if (reader2 != null) { reader2.Close(); }
                        conn.Close();
                    }

                    return rest;
                    //////////////////////////
                }
                else
                    return null;
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "IsRestaurantOwner" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;

            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "IsRestaurantOwner" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }
            return null;
        }

        private Category GetCategoriesFromReader(MySqlDataReader reader)
        {
            int id = reader.GetInt32(0);
            int restaurantID = reader.GetInt32(1);
            string categoryName = reader.GetString(2);
            string categoryDescription = null;
            if (!reader.IsDBNull(3)) categoryDescription = reader.GetString(3);
            string priceOption = null;
            if (!reader.IsDBNull(4)) priceOption = reader.GetString(4);
            string nonPriceOption = null;
            if (!reader.IsDBNull(5)) nonPriceOption = reader.GetString(5);
            string nonPriceOption2 = null;
            if (!reader.IsDBNull(6)) nonPriceOption2 = reader.GetString(6);

            Category u = new Category();
            u.CategoryID = id;
            u.RestaurantID = restaurantID;
            u.CategoryName = categoryName;
            u.CategoryDescription = categoryDescription;
            u.PriceOption = priceOption;
            u.NonPriceOption = nonPriceOption;
            u.NonPriceOption2 = nonPriceOption2;

            return u;
        }

        public Category GetCategory(string managerLogin, int restaurantID, int categoryID)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand commandTest = new MySqlCommand(Queries.IsRestaurantOwner);
                commandTest.Parameters.AddWithValue("@managerLogin", managerLogin);
                commandTest.Parameters.AddWithValue("@restaurantID", restaurantID);
                commandTest.Connection = conn;
                conn.Open();

                reader = commandTest.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.HasRows)
                {
                    reader.Close();
                    conn.Close();

                    MySqlDataReader reader2 = null;
                    Category rest = null;
                    try
                    {
                        MySqlCommand command = new MySqlCommand(Queries.GetCategory);
                        command.Parameters.AddWithValue("@restaurantID", restaurantID);
                        command.Parameters.AddWithValue("@id", categoryID);
                        command.Connection = conn;

                        conn.Open();

                        reader2 = command.ExecuteReader();

                        while (reader2.Read())
                        {
                            rest = GetCategoriesFromReader(reader2);
                        }
                    }
                    catch (MySqlException e)
                    {
                        EventLog log = new EventLog();
                        log.Source = eventSource;
                        log.Log = eventLog;

                        string wiadomosc = message;
                        wiadomosc += "Action: " + "GetCategory" + "\n\n";
                        wiadomosc += "Exception: " + e.ToString();

                        log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                        if (reader2 != null) { reader2.Close(); }
                        conn.Close();
                        return null;

                    }
                    catch (Exception ex)
                    {
                        EventLog log = new EventLog();
                        log.Source = eventSource;
                        log.Log = eventLog;

                        string wiadomosc = message2;
                        wiadomosc += "Action: " + "GetCategory" + "\n\n";
                        wiadomosc += "Exception: " + ex.ToString();

                        log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                        if (reader2 != null) { reader2.Close(); }
                        conn.Close();
                        return null;
                    }
                    finally
                    {
                        if (reader2 != null) { reader2.Close(); }
                        conn.Close();
                    }

                    return rest;
                }
                else
                    return null;
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "IsRestaurantOwner" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;

            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "IsRestaurantOwner" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }
            return null;

        }

        public bool EditCategory(string managerLogin, int restaurantID, int categoryID, string categoryName, string categoryDescription, string priceOption, string nonPriceOption, string nonPriceOption2)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand commandTest = new MySqlCommand(Queries.IsRestaurantOwner);
                commandTest.Parameters.AddWithValue("@managerLogin", managerLogin);
                commandTest.Parameters.AddWithValue("@restaurantID", restaurantID);
                commandTest.Connection = conn;
                conn.Open();

                reader = commandTest.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.HasRows)
                {
                    reader.Close();
                    conn.Close();
/////////////////////////////////////////////////////////////////////////////////////////////////////
                 //   MySqlConnection conn = new MySqlConnection(ConnectionString);
                    MySqlCommand command = new MySqlCommand(Queries.EditCategory);
                    command.Parameters.AddWithValue("@categoryName", categoryName);
                    command.Parameters.AddWithValue("@categoryDescription", categoryDescription);
                    command.Parameters.AddWithValue("@priceOption", priceOption);
                    command.Parameters.AddWithValue("@nonPriceOption", nonPriceOption);
                    command.Parameters.AddWithValue("@nonPriceOption2", nonPriceOption2);
                    command.Parameters.AddWithValue("@restaurantId", restaurantID);
                    command.Parameters.AddWithValue("@id", categoryID);
                    command.Connection = conn;

                    MySqlCommand command2 = new MySqlCommand(Queries.DisableProduct);
                    command2.Parameters.AddWithValue("@restaurantId", restaurantID);
                    command2.Parameters.AddWithValue("@categoryId", categoryID);
                    command2.Parameters.AddWithValue("@isEnabled", false);
                    command2.Parameters.AddWithValue("@isAvailable", false);
                    command2.Connection = conn;

                    MySqlTransaction tran = null;

                    try
                    {
                        conn.Open();
                        tran = conn.BeginTransaction();
                        command.Transaction = tran;
                        command2.Transaction = tran;

                        command2.ExecuteNonQuery();
                        int rows = command.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            tran.Commit();
                            return true;
                        }
                        else
                            tran.Rollback();
                    }
                    catch (MySqlException e)
                    {
                        try
                        {
                            tran.Rollback();
                        }
                        catch { }

                        EventLog log = new EventLog();
                        log.Source = eventSource;
                        log.Log = eventLog;

                        string wiadomosc = message;
                        wiadomosc += "Action: " + "EditCategory" + "\n\n";
                        wiadomosc += "Exception: " + e.ToString();

                        log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                        return false;
                    }
                    catch (Exception ex)
                    {
                        try
                        {
                            tran.Rollback();
                        }
                        catch { }

                        EventLog log = new EventLog();
                        log.Source = eventSource;
                        log.Log = eventLog;

                        string wiadomosc = message2;
                        wiadomosc += "Action: " + "EditCategory" + "\n\n";
                        wiadomosc += "Exception: " + ex.ToString();

                        log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                        return false;
                    }
                    finally
                    {
                        conn.Close();
                    }
                    //MySqlCommand command = new MySqlCommand(Queries.EditCategory);
                    //command.Parameters.AddWithValue("@categoryName", @categoryName);
                    //command.Parameters.AddWithValue("@categoryDescription", categoryDescription);
                    //command.Parameters.AddWithValue("@priceOption", priceOption);
                    //command.Parameters.AddWithValue("@nonPriceOption", nonPriceOption);
                    //command.Parameters.AddWithValue("@nonPriceOption2", nonPriceOption2);
                    //command.Parameters.AddWithValue("@restaurantId", restaurantID);
                    //command.Parameters.AddWithValue("@id", categoryID);

                    //int rowsaffected = ExecuteNonQuery(command, "EditCategory");

                    //if (rowsaffected > 0)
                    //{
                    //    return true;
                    //}
////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    return false;
                }
                else
                    return false;
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "IsRestaurantOwner" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return false;

            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "IsRestaurantOwner" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }
            return false;
        }

        public bool DeleteCategory(string managerLogin, int restaurantID, int categoryID)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand commandTest = new MySqlCommand(Queries.IsRestaurantOwner);
                commandTest.Parameters.AddWithValue("@managerLogin", managerLogin);
                commandTest.Parameters.AddWithValue("@restaurantID", restaurantID);
                commandTest.Connection = conn;
                conn.Open();

                reader = commandTest.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.HasRows)
                {
                    reader.Close();
                    conn.Close();

                    MySqlCommand command = new MySqlCommand(Queries.DeleteCategory);
                    command.Parameters.AddWithValue("@restaurantId", restaurantID);
                    command.Parameters.AddWithValue("@id", categoryID);

                    int rowsaffected = ExecuteNonQuery(command, "DeleteCategory");

                    if (rowsaffected > 0)
                    {
                        return true;
                    }
                    return false;
                }
                else
                    return false;
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "IsRestaurantOwner" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return false;

            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "IsRestaurantOwner" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }
            return false;
        }

        public bool AddProduct(int restaurantID, int categoryID, string productName, string productDescription, string price, string managerLogin)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            try
            {

                MySqlCommand commandTest = new MySqlCommand(Queries.IsRestaurantOwner);
                commandTest.Parameters.AddWithValue("@managerLogin", managerLogin);
                commandTest.Parameters.AddWithValue("@restaurantID", restaurantID);
                commandTest.Connection = conn;
                conn.Open();

                reader = commandTest.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.HasRows)
                {
                    reader.Close();
                    conn.Close();
                    Category category = GetCategory(managerLogin, restaurantID, categoryID);
                    if (category == null)
                    {
                        return false;
                    }
                    else
                    {
                        try
                        {
                            DateTime creationDate = DateTime.Now;
                     //   MySqlTransaction trans;
                     //   trans = conn.BeginTransaction();
// command.Transaction = trans;
                        MySqlCommand command = new MySqlCommand(Queries.AddProduct);
                        command.Parameters.AddWithValue("@price", price);
                        command.Parameters.AddWithValue("@priceOption", category.PriceOption);
                        command.Parameters.AddWithValue("@restaurantId", restaurantID);
                        command.Parameters.AddWithValue("@categoryId", categoryID);
                        command.Parameters.AddWithValue("@name", productName);
                        command.Parameters.AddWithValue("@description", productDescription);
                        command.Parameters.AddWithValue("@creationDate", creationDate);
                        command.Parameters.AddWithValue("@isAvailable", false);
                        command.Parameters.AddWithValue("@isEnabled", true);
                      //  command.Parameters.AddWithValue("@isSubproduct", false);

                        command.Connection = conn;
                        conn.Open();
                            //string[] option = category.PriceOption.Split(',');
                            //string[] prices = price.Split('|');
                            //for (int i = 0; i < option.Length; i++)
                            //{
                                int rowsAffected = 0;
                            //    command.Parameters.Clear();
                            //   // command.Parameters.AddWithValue("@price", prices[i]);
                            //   // command.Parameters.AddWithValue("@priceOption", option[i]);
                            //    if (i == 0)
                            //    {
                            //        command.Parameters.AddWithValue("@price", prices[i]);
                            //        command.Parameters.AddWithValue("@priceOption", option[i]);
                            //        command.Parameters.AddWithValue("@restaurantId", restaurantID);
                            //        command.Parameters.AddWithValue("@categoryId", categoryID);
                            //        command.Parameters.AddWithValue("@name", productName);
                            //        command.Parameters.AddWithValue("@description", productDescription);
                            //        command.Parameters.AddWithValue("@creationDate", creationDate);
                            //        command.Parameters.AddWithValue("@isAvailable", false);
                            //        command.Parameters.AddWithValue("@isEnabled", true);
                            //        command.Parameters.AddWithValue("@isSubproduct", false);
                            //    }
                            //    else
                            //    {
                            //        command.Parameters.AddWithValue("@price", prices[i]);
                            //        command.Parameters.AddWithValue("@priceOption", option[i]);
                            //        command.Parameters.AddWithValue("@restaurantId", restaurantID);
                            //        command.Parameters.AddWithValue("@categoryId", categoryID);
                            //        command.Parameters.AddWithValue("@name", productName);
                            //        command.Parameters.AddWithValue("@description", productDescription);
                            //        command.Parameters.AddWithValue("@creationDate", creationDate);
                            //        command.Parameters.AddWithValue("@isAvailable", false);
                            //        command.Parameters.AddWithValue("@isEnabled", true);
                            //        command.Parameters.AddWithValue("@isSubproduct", true);
                            //    }

                                rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected <= 0)
                                {
                            //        trans.Rollback();
                                    return false;
                                }
                            //}
                        //    trans.Commit();
                            return true;
                        }
                        catch (MySqlException e)
                        {
                            EventLog log = new EventLog();
                            log.Source = eventSource;
                            log.Log = eventLog;

                            string wiadomosc = message;
                            wiadomosc += "Action: " + "AddProduct" + "\n\n";
                            wiadomosc += "Exception: " + e.ToString();

                            log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                         //   trans.Rollback();
                        }
                        catch (Exception ex)
                        {
                            EventLog log = new EventLog();
                            log.Source = eventSource;
                            log.Log = eventLog;

                            string wiadomosc = message2;
                            wiadomosc += "Action: " + "AddProduct" + "\n\n";
                            wiadomosc += "Exception: " + ex.ToString();

                            log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                        //    trans.Rollback();
                        }
                        finally
                        {
                            if (reader != null) { reader.Close(); }
                            conn.Close();
                        }
                    }
////////////////////////////////////////////////////////////
                    //try
                    //{
                    // //   MySqlConnection conn = new MySqlConnection(ConnectionString);
                    //    conn.Close();
                    //    conn.Open();
                    //    MySqlTransaction trans;
                    //    trans = conn.BeginTransaction();

                    //    Category category = null;
                    //    MySqlCommand command = new MySqlCommand(Queries.GetCategory);
                    //    command.Parameters.AddWithValue("@restaurantID", restaurantID);
                    //    command.Parameters.AddWithValue("@id", categoryID);
                    //    command.Connection = conn;
                    //    command.Transaction = trans;
                    //    //  rest = new Category();
                    //    //conn.Open();

                    //    reader2 = command.ExecuteReader();

                    //    while (reader2.Read())
                    //    {
                    //        rest = GetCategoriesFromReader(reader2);
                    //        // rest.Add(r);
                    //    }
                    //    ///
                    //    MySqlCommand reset = new MySqlCommand(Queries.ResetPassword);
                    //    reset.Parameters.AddWithValue("@password", password);
                    //    reset.Parameters.AddWithValue("@lastPasswordChangedDate", DateTime.Now);
                    //    reset.Parameters.AddWithValue("@login", login);
                    //    reset.Parameters.AddWithValue("@isLockedOut", false);
                    //    reset.Connection = conn;
                    //    reset.Transaction = trans;
                    //    try
//    {
                    //        MySqlCommand getcommand = new MySqlCommand(Queries.GetEmailByLogin);
                    //        getcommand.Parameters.AddWithValue("@login", login);
                    //        getcommand.Connection = conn;

                    //        DataSet ds = new DataSet();
                    //        ds = ExecuteQuery(getcommand, "GetEmailByLogin");

                    //        if (ds.Tables.Count > 0)
                    //        {
                    //            foreach (DataRow row in ds.Tables[0].Rows)
                    //            {
                    //                if (row["email"] != DBNull.Value) email = row["email"].ToString();
                    //                else return false;
                    //            }
                                        //        }
                    //        else return false;

                    //        rowsAffected = reset.ExecuteNonQuery();

                    //        if (rowsAffected > 0)
                    //        {
                    //            if (!String.IsNullOrEmpty(email))
                    //            {
                    //                SmtpClient klient = new SmtpClient("smtp.gmail.com");
                    //                MailMessage wiadomosc = new MailMessage();
                    //                try
                    //                {
                    //                    wiadomosc.From = new MailAddress("erestauracja@gmail.com");
                    //                    wiadomosc.To.Add(email);
                    //                    wiadomosc.Subject = "Erestauracja - restet hasła.";
                    //                    wiadomosc.Body = "Nowe hasło: " + password;

                    //                    klient.Port = 587;
                    //                    klient.Credentials = new System.Net.NetworkCredential("erestauracja", "Erestauracja123");
                    //                    klient.EnableSsl = true;
                    //                    klient.Send(wiadomosc);

                    //                    trans.Commit();
                    //                    return true;
                    //                }
                    //                catch (Exception ex)
                    //                {
                    //                    EventLog log = new EventLog();
                    //                    log.Source = eventSource;
                    //                    log.Log = eventLog;

                    //                    string info = "Błąd podczas wysyłania wiadomości email";
                    //                    info += "Action: " + "Email sending" + "\n\n";
                    //                    info += "Exception: " + ex.ToString();

                    //                    trans.Rollback();
                    //                    return false;
                    //                }
                    //            }
                    //            else return false;
                    //        }
                    //        else
                    //        {
                    //            trans.Rollback();
                    //            return false;
                    //        }
                    //    }
                    //    catch (Exception e)
                    //    {
                    //        EventLog log = new EventLog();
                    //        log.Source = eventSource;
                    //        log.Log = eventLog;

                    //        string info = "Błąd podczas dodawania produktu";
                    //        info += "Action: " + "AddProduct" + "\n\n";
                    //        info += "Exception: " + e.ToString();
                    //        trans.Rollback();
                    //        return false;
                    //    }
                    //    finally
                    //    {
                    //        conn.Close();
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    EventLog log = new EventLog();
                    //    log.Source = eventSource;
                    //    log.Log = eventLog;

                    //    string info = "Błąd podczas dodawania produktu";
                    //    info += "Action: " + "AddProduct" + "\n\n";
                    //    info += "Exception: " + ex.ToString();
                    //}
/////////////////////////////////////////
                    //DateTime creationDate = DateTime.Now;

                    //MySqlCommand command = new MySqlCommand(Queries.AddProduct);
                    //command.Parameters.AddWithValue("@restaurantID", restaurantID);
                    //command.Parameters.AddWithValue("@categoryName", categoryID);
                    //command.Parameters.AddWithValue("@categoryDescription", productName);
                    //command.Parameters.AddWithValue("@priceOption", productDescription);
                    //command.Parameters.AddWithValue("@nonPriceOption", price);
                    //// command.Parameters.AddWithValue("@restaurantID", priceOption);
                    //command.Parameters.AddWithValue("@categoryName", creationDate);
                    //command.Parameters.AddWithValue("@categoryDescription", false);
                    ////command.Parameters.AddWithValue("@priceOption", subproduct);
                    //command.Parameters.AddWithValue("@nonPriceOption", true);

                    //int rowsaffected = ExecuteNonQuery(command, "AddCategory");

                    //if (rowsaffected > 0)
                    //{
                    //    return true;
                    //}
//////////////////////////////////////////////
                }
                else
                    return false;
/////////////////////////////////////////////////////
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "IsRestaurantOwner" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return false;

            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "IsRestaurantOwner" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            return false;
        }

        public List<Menu> GetMenuManager(string managerLogin, int restaurantID)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand commandTest = new MySqlCommand(Queries.IsRestaurantOwner);
                commandTest.Parameters.AddWithValue("@managerLogin", managerLogin);
                commandTest.Parameters.AddWithValue("@restaurantID", restaurantID);
                commandTest.Connection = conn;
                conn.Open();

                reader = commandTest.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.HasRows)
                {
                    reader.Close();
                    conn.Close();

                    MySqlDataReader reader2 = null;
                    List<Menu> rest = null;
                    try
                    {
                        MySqlCommand command = new MySqlCommand(Queries.GetCategories);
                        command.Parameters.AddWithValue("@restaurantID", restaurantID);
                        command.Connection = conn;
                        rest = new List<Menu>();
                        conn.Open();

                        reader2 = command.ExecuteReader();

                        while (reader2.Read())
                        {
                            Menu r = GetMenuFromReader(reader2);
                            rest.Add(r);
                        }
                    }
                    catch (MySqlException e)
                    {
                        EventLog log = new EventLog();
                        log.Source = eventSource;
                        log.Log = eventLog;

                        string wiadomosc = message;
                        wiadomosc += "Action: " + "GetCategories" + "\n\n";
                        wiadomosc += "Exception: " + e.ToString();

                        log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                        if (reader2 != null) { reader2.Close(); }
                        conn.Close();
                        return null;

                    }
                    catch (Exception ex)
                    {
                        EventLog log = new EventLog();
                        log.Source = eventSource;
                        log.Log = eventLog;

                        string wiadomosc = message2;
                        wiadomosc += "Action: " + "GetCategories" + "\n\n";
                        wiadomosc += "Exception: " + ex.ToString();

                        log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                        if (reader2 != null) { reader2.Close(); }
                        conn.Close();
                        return null;
                    }
                    finally
                    {
                        if (reader2 != null) { reader2.Close(); }
                        conn.Close();
                    }

                    Menu item = new Menu();
                    item.CategoryID = 0;
                    item.RestaurantID = restaurantID;
                    item.CategoryName = "Nieprzypisane";
                    item.CategoryDescription = "Produkty nieprzydzielone do kategorii, wymagają edycji";
                    item.PriceOption = null;
                    item.NonPriceOption = null;
                    item.NonPriceOption2 = null;
                    
                    Menu item2 = new Menu();
                    item2.CategoryID = -1;
                    item2.RestaurantID = restaurantID;
                    item2.CategoryName = "Wszystkie";
                    item2.CategoryDescription = null;
                    item2.PriceOption = null;
                    item2.NonPriceOption = null;
                    item2.NonPriceOption2 = null;
                    
                    rest.Add(item);
                    rest.Add(item2);

////////////////////////////////////////////////////
                    foreach (Menu menu in rest)
                    {
                        List<Product> prod = null;
                        MySqlDataReader reader3 = null;
                        try
                        {
                        if(menu.CategoryID == -1)
                        {
                            MySqlCommand command = new MySqlCommand(Queries.GetAllProducts);
                            command.Parameters.AddWithValue("@restaurantId", restaurantID);
                           // command.Parameters.AddWithValue("@categoryId", menu.CategoryID);

                            command.Connection = conn;
                            prod = new List<Product>();
                            conn.Open();

                            reader3 = command.ExecuteReader();

                            while (reader3.Read())
                            {
                                Product r = GetProductFromReader(reader3);
                                prod.Add(r);
                            }
                        }
                        else
                        {
                        
                            MySqlCommand command = new MySqlCommand(Queries.GetProducts);
                            command.Parameters.AddWithValue("@restaurantId", restaurantID);
                            command.Parameters.AddWithValue("@categoryId", menu.CategoryID);

                            command.Connection = conn;
                            prod = new List<Product>();
                            conn.Open();

                            reader3 = command.ExecuteReader();

                            while (reader3.Read())
                            {
                                Product r = GetProductFromReader(reader3);
                                prod.Add(r);
                            }
                        }
                        }
                        catch (MySqlException e)
                        {
                            EventLog log = new EventLog();
                            log.Source = eventSource;
                            log.Log = eventLog;

                            string wiadomosc = message;
                            wiadomosc += "Action: " + "GetProducts" + "\n\n";
                            wiadomosc += "Exception: " + e.ToString();

                            log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                            if (reader3 != null) { reader3.Close(); }
                            conn.Close();
                            return null;

                        }
                        catch (Exception ex)
                        {
                            EventLog log = new EventLog();
                            log.Source = eventSource;
                            log.Log = eventLog;

                            string wiadomosc = message2;
                            wiadomosc += "Action: " + "GetProducts" + "\n\n";
                            wiadomosc += "Exception: " + ex.ToString();

                            log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                            if (reader3 != null) { reader3.Close(); }
                            conn.Close();
                            return null;
                        }
                        finally
                        {
                            if (reader3 != null) { reader3.Close(); }
                            conn.Close();
                        }

                        menu.Products = prod;

                        
                    }
//////////////////////////////////////////////
                    return rest;
                }
                else
                    return null;
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "IsRestaurantOwner" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;

            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "IsRestaurantOwner" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }
            return null;
        }

        private Product GetProductFromReader(MySqlDataReader reader)
        {
            int id = reader.GetInt32(0);
            int restaurantID = reader.GetInt32(1);
            int categoryID = reader.GetInt32(2);
            string name = reader.GetString(3);
            string description = null;
            if (!reader.IsDBNull(4)) description = reader.GetString(4);
            string price = null;
            if (!reader.IsDBNull(5)) price = reader.GetString(5);
            string priceOption = null;
            if (!reader.IsDBNull(6)) priceOption = reader.GetString(6);
            DateTime creationDate = reader.GetDateTime(7);
            bool isAvailable = reader.GetBoolean(8);
            bool isEnabled = reader.GetBoolean(9);
           

            Product u = new Product();
            u.ProductId = id;
            u.RestaurantId = restaurantID;
            u.CategoryId = categoryID;
            u.ProductName = name;
            u.ProductDescription = description;
            u.Price = price;
            u.PriceOption = priceOption;
            u.CreationDate = creationDate;
            u.IsAvailable = isAvailable;
            u.IsEnabled = isEnabled;

            return u;
        }

        private Menu GetMenuFromReader(MySqlDataReader reader)
        {
            int id = reader.GetInt32(0);
            int restaurantID = reader.GetInt32(1);
            string categoryName = reader.GetString(2);
            string categoryDescription = null;
            if (!reader.IsDBNull(3)) categoryDescription = reader.GetString(3);
            string priceOption = null;
            if (!reader.IsDBNull(4)) priceOption = reader.GetString(4);
            string nonPriceOption = null;
            if (!reader.IsDBNull(5)) nonPriceOption = reader.GetString(5);
            string nonPriceOption2 = null;
            if (!reader.IsDBNull(6)) nonPriceOption2 = reader.GetString(6);

            Menu u = new Menu();
            u.CategoryID = id;
            u.RestaurantID = restaurantID;
            u.CategoryName = categoryName;
            u.CategoryDescription = categoryDescription;
            u.PriceOption = priceOption;
            u.NonPriceOption = nonPriceOption;
            u.NonPriceOption2 = nonPriceOption2;
            u.Products = null;

            return u;
        }

        public Product GetProduct(string managerLogin, int restaurantID, int productID)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand commandTest = new MySqlCommand(Queries.IsRestaurantOwner);
                commandTest.Parameters.AddWithValue("@managerLogin", managerLogin);
                commandTest.Parameters.AddWithValue("@restaurantID", restaurantID);
                commandTest.Connection = conn;
                conn.Open();

                reader = commandTest.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.HasRows)
                {
                    reader.Close();
                    conn.Close();

                    MySqlDataReader reader2 = null;
                    Product rest = null;
                    try
                    {
                        MySqlCommand command = new MySqlCommand(Queries.GetProduct);
                        command.Parameters.AddWithValue("@restaurantID", restaurantID);
                        command.Parameters.AddWithValue("@id", productID);
                        command.Connection = conn;

                        conn.Open();

                        reader2 = command.ExecuteReader(CommandBehavior.SingleRow);

                        while (reader2.Read())
                        {
                            rest = GetProductFromReader(reader2);
                        }
                    }
                    catch (MySqlException e)
                    {
                        EventLog log = new EventLog();
                        log.Source = eventSource;
                        log.Log = eventLog;

                        string wiadomosc = message;
                        wiadomosc += "Action: " + "GetProduct" + "\n\n";
                        wiadomosc += "Exception: " + e.ToString();

                        log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                        if (reader2 != null) { reader2.Close(); }
                        conn.Close();
                        return null;

                    }
                    catch (Exception ex)
                    {
                        EventLog log = new EventLog();
                        log.Source = eventSource;
                        log.Log = eventLog;

                        string wiadomosc = message2;
                        wiadomosc += "Action: " + "GetProduct" + "\n\n";
                        wiadomosc += "Exception: " + ex.ToString();

                        log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                        if (reader2 != null) { reader2.Close(); }
                        conn.Close();
                        return null;
                    }
                    finally
                    {
                        if (reader2 != null) { reader2.Close(); }
                        conn.Close();
                    }

                    return rest;
                }
                else
                    return null;
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "IsRestaurantOwner" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;

            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "IsRestaurantOwner" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }
            return null;

        }

        public bool EditProduct(string managerLogin, int restaurantID, int id, int categoryID, string productName, string productDescription, string price, bool isAvailable)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand commandTest = new MySqlCommand(Queries.IsRestaurantOwner);
                commandTest.Parameters.AddWithValue("@managerLogin", managerLogin);
                commandTest.Parameters.AddWithValue("@restaurantID", restaurantID);
                commandTest.Connection = conn;
                conn.Open();

                reader = commandTest.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.HasRows)
                {
                    reader.Close();
                    conn.Close();
                    Category category = GetCategory(managerLogin, restaurantID, categoryID);
                    if (category == null)
                    {
                        return false;
                    }
                    else
                    {

                        MySqlCommand command = new MySqlCommand(Queries.EditProduct);
                        command.Parameters.AddWithValue("@categoryId", categoryID);
                        command.Parameters.AddWithValue("@name", productName);
                        command.Parameters.AddWithValue("@description", productDescription);
                        command.Parameters.AddWithValue("@price", price);
                        command.Parameters.AddWithValue("@priceOption", category.PriceOption);
                        command.Parameters.AddWithValue("@isAvailable", isAvailable);
                        command.Parameters.AddWithValue("@isEnabled", true);
                        command.Parameters.AddWithValue("@restaurantId", restaurantID);
                        command.Parameters.AddWithValue("@id", id);

                        int rowsaffected = ExecuteNonQuery(command, "EditProduct");

                        if (rowsaffected > 0)
                        {
                            return true;
                        }
                        return false;
                    }
                }
                else
                    return false;
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "IsRestaurantOwner" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return false;

            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "IsRestaurantOwner" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return false;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }
            return false;
        }

        #endregion

        #region ogólne
        public List<string> GetCountriesList()
        {
            MySqlCommand command = new MySqlCommand(Queries.GetCountriesList);

            List<string> value = new List<string>();

            DataSet ds = new DataSet();
            ds = ExecuteQuery(command, "GetCountriesList");

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["name"] != DBNull.Value) value.Add(row["name"].ToString());
                }
            }
            else
                value = null;
            return value;
        }

        public List<Town> GetTowns(string townName, string postalCode, out string status)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            List<Town> towns = null;
            try
            {
                MySqlCommand command = new MySqlCommand(Queries.GetTowns);
                command.Parameters.AddWithValue("@townName", townName);
                command.Parameters.AddWithValue("@postalCode", postalCode);
                command.Connection = conn;
                towns = new List<Town>();
                conn.Open();

                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Town r = GetTownFromReader(reader);
                    towns.Add(r);
                }

            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetTowns" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                status = "Błąd sql: "+e.Message;
                return null;

            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "GetTowns" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                status = "Błąd: " + ex.Message;
                return null;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            if (towns.Count == 1)
            {
                status = "OK";
                return towns;
            }
            else if (towns.Count == 0)
            {
                try
                {
                    MySqlCommand command = new MySqlCommand(Queries.GetMoreTowns);
                    command.Parameters.AddWithValue("@townName", "%" + townName + "%");
                    command.Parameters.AddWithValue("@postalCode", postalCode);
                    command.Connection = conn;
                    towns = new List<Town>();
                    conn.Open();

                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Town r = GetTownFromReader(reader);
                        towns.Add(r);
                    }
                }
                catch (MySqlException e)
                {
                    EventLog log = new EventLog();
                    log.Source = eventSource;
                    log.Log = eventLog;

                    string wiadomosc = message;
                    wiadomosc += "Action: " + "GetTowns" + "\n\n";
                    wiadomosc += "Exception: " + e.ToString();

                    log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                    if (reader != null) { reader.Close(); }
                    conn.Close();
                    status = "Błąd sql: " + e.Message;
                    return null;

                }
                catch (Exception ex)
                {
                    EventLog log = new EventLog();
                    log.Source = eventSource;
                    log.Log = eventLog;

                    string wiadomosc = message2;
                    wiadomosc += "Action: " + "GetTowns" + "\n\n";
                    wiadomosc += "Exception: " + ex.ToString();

                    log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                    if (reader != null) { reader.Close(); }
                    conn.Close();
                    status = "Błąd: " + ex.Message;
                    return null;
                }
                finally
                {
                    if (reader != null) { reader.Close(); }
                    conn.Close();
                }
                if (towns.Count > 0)
                {
                    status = "Nie znaleziono podanego miasta, ale znaleziono podobne.";
                    return towns;
                }
                else
                {
                    try
                    {
                        MySqlCommand command = new MySqlCommand(Queries.GetMoreMoreTowns);
                        string regex = "^";
                        for (int i = 0; i < townName.Length; i++)
                        {
                            regex += "[" + townName + "]";
                        }

                        command.Parameters.AddWithValue("@townName", regex);
                        command.Parameters.AddWithValue("@postalCode", postalCode);
                        command.Connection = conn;
                        towns = new List<Town>();
                        conn.Open();

                        reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            Town r = GetTownFromReader(reader);
                            towns.Add(r);
                        }
                    }
                    catch (MySqlException e)
                    {
                        EventLog log = new EventLog();
                        log.Source = eventSource;
                        log.Log = eventLog;

                        string wiadomosc = message;
                        wiadomosc += "Action: " + "GetTowns" + "\n\n";
                        wiadomosc += "Exception: " + e.ToString();

                        log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                        if (reader != null) { reader.Close(); }
                        conn.Close();
                        status = "Błąd sql: " + e.Message;
                        return null;

                    }
                    catch (Exception ex)
                    {
                        EventLog log = new EventLog();
                        log.Source = eventSource;
                        log.Log = eventLog;

                        string wiadomosc = message2;
                        wiadomosc += "Action: " + "GetTowns" + "\n\n";
                        wiadomosc += "Exception: " + ex.ToString();

                        log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                        if (reader != null) { reader.Close(); }
                        conn.Close();
                        status = "Błąd: " + ex.Message;
                        return null;
                    }
                    finally
                    {
                        if (reader != null) { reader.Close(); }
                        conn.Close();
                    }

                    if (towns.Count > 0)
                    {
                        status = "Nie znaleziono podanego miasta, ale znaleziono podobne (po szukaniu literówek).";
                        return towns;
                    }
                    else
                    {
                        status = "Nie znaleziono żadnego pasującego miasta.";
                        return null;
                    }
                }
            }
            else
            {
                status = "Błąd ";
               // return null;
                return towns;
            }
        }

        private Town GetTownFromReader(MySqlDataReader reader)
        {
            int id = reader.GetInt32(0);
            string townName = reader.GetString(2);
            string postalCode = reader.GetString(1);
            string province = reader.GetString(3);
            string district = reader.GetString(4);
            string community = reader.GetString(5);
            double latitude = reader.GetDouble(6);
            double longtitude = reader.GetDouble(7);

            Town u = new Town();
            u.ID = id;
            u.TownName = townName;
            u.PostalCode = postalCode;
            u.Province = province;
            u.District = district;
            u.Community = community;
            u.Latitude = latitude;
            u.Longtitude = longtitude;

            return u;
        }

        public List<RestaurantInTown> GetRestaurantByTown(string townName)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            List<RestaurantInTown> rest = null;
            try
            {
                MySqlCommand command = new MySqlCommand(Queries.GetRestaurantByTown);
                command.Parameters.AddWithValue("@townName", townName+"%");
                command.Connection = conn;
                rest = new List<RestaurantInTown>();
                conn.Open();

                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        RestaurantInTown u = new RestaurantInTown();
                        u.ResId = reader.GetInt32(0);
                        u.Name = reader.GetString(1) + " (" + reader.GetString(2) + ")";
                        u.TownId = reader.GetInt32(3);
                        rest.Add(u);
                    }
                }
                else
                {
                    if (reader != null) { reader.Close(); }
                    conn.Close();
                    return null;
                }
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetRestaurantByTown" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;

            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "GetRestaurantByTown" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            return rest;
        }

        public List<Menu> GetMenu(int restaurantID)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);

            MySqlDataReader reader2 = null;
            List<Menu> rest = null;
            try
            {
                MySqlCommand command = new MySqlCommand(Queries.GetCategories);
                command.Parameters.AddWithValue("@restaurantID", restaurantID);
                command.Connection = conn;
                rest = new List<Menu>();
                conn.Open();

                reader2 = command.ExecuteReader();

                while (reader2.Read())
                {
                    Menu r = GetMenuFromReader(reader2);
                    rest.Add(r);
                }
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetCategories" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader2 != null) { reader2.Close(); }
                conn.Close();
                return null;

            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "GetCategories" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader2 != null) { reader2.Close(); }
                conn.Close();
                return null;
            }
            finally
            {
                if (reader2 != null) { reader2.Close(); }
                conn.Close();
            }

            foreach (Menu menu in rest)
            {
                List<Product> prod = null;
                MySqlDataReader reader3 = null;
                try
                {

                    MySqlCommand command = new MySqlCommand(Queries.GetClientProducts);
                    command.Parameters.AddWithValue("@restaurantId", restaurantID);
                    command.Parameters.AddWithValue("@categoryId", menu.CategoryID);

                    command.Connection = conn;
                    prod = new List<Product>();
                    conn.Open();

                    reader3 = command.ExecuteReader();

                    while (reader3.Read())
                    {
                        Product r = GetProductFromReader(reader3);
                        prod.Add(r);
                    }
                }
                catch (MySqlException e)
                {
                    EventLog log = new EventLog();
                    log.Source = eventSource;
                    log.Log = eventLog;

                    string wiadomosc = message;
                    wiadomosc += "Action: " + "GetProducts" + "\n\n";
                    wiadomosc += "Exception: " + e.ToString();

                    log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                    if (reader3 != null) { reader3.Close(); }
                    conn.Close();
                    return null;

                }
                catch (Exception ex)
                {
                    EventLog log = new EventLog();
                    log.Source = eventSource;
                    log.Log = eventLog;

                    string wiadomosc = message2;
                    wiadomosc += "Action: " + "GetProducts" + "\n\n";
                    wiadomosc += "Exception: " + ex.ToString();

                    log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                    if (reader3 != null) { reader3.Close(); }
                    conn.Close();
                    return null;
                }
                finally
                {
                    if (reader3 != null) { reader3.Close(); }
                    conn.Close();
                }

                menu.Products = prod;
            }

            return rest;

        }

        public MainPageContent GetMainPageUser(int id)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            MainPageContent rest = null;
            try
            {
                MySqlCommand command = new MySqlCommand(Queries.GetMainPageUser);
               // command.Parameters.AddWithValue("@managerLogin", managerLogin);
                command.Parameters.AddWithValue("@id", id);
                command.Connection = conn;
                rest = new MainPageContent();
                conn.Open();

                reader = command.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rest.Description = reader.GetString(0);
                        rest.Foto = reader.GetString(1);
                        rest.SpecialOffers = reader.GetString(2);
                    }
                }
                else
                    return null;
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetMainPageUser" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;

            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "GetMainPageUser" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            return rest;
        }

        public DeliveryPageContent GetDeliveryPageUser(int id)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            DeliveryPageContent rest = null;
            try
            {
                MySqlCommand command = new MySqlCommand(Queries.GetDeliveryPageUser);
              //  command.Parameters.AddWithValue("@managerLogin", managerLogin);
                command.Parameters.AddWithValue("@id", id);
                command.Connection = conn;
                rest = new DeliveryPageContent();
                conn.Open();

                reader = command.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rest.Delivery = reader.GetString(0);
                    }
                }
                else
                    return null;
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetDeliveryPageUser" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;

            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "GetDeliveryPageUser" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            return rest;
        }

        public EventsPageContent GetEventsPageUser(int id)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            EventsPageContent rest = null;
            try
            {
                MySqlCommand command = new MySqlCommand(Queries.GetEventsPageUser);
             //   command.Parameters.AddWithValue("@managerLogin", managerLogin);
                command.Parameters.AddWithValue("@id", id);
                command.Connection = conn;
                rest = new EventsPageContent();
                conn.Open();

                reader = command.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rest.Events = reader.GetString(0);
                    }
                }
                else
                    return null;
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetEventsPageUser" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;

            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "GetEventsPageUser" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            return rest;
        }

        public ContactPageContent GetContactPageUser(int id)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            ContactPageContent rest = null;
            try
            {
                MySqlCommand command = new MySqlCommand(Queries.GetContactPageUser);
               // command.Parameters.AddWithValue("@managerLogin", managerLogin);
                command.Parameters.AddWithValue("@id", id);
                command.Connection = conn;
                rest = new ContactPageContent();
                conn.Open();

                reader = command.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rest.Contact = reader.GetString(0);
                    }
                }
                else
                    return null;
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetContactPageUser" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;

            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "GetContactPageUser" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
                return null;
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            return rest;
        }

        #endregion
    }


}
