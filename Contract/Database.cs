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
            int townID = reader.GetInt32(6);
            string country = reader.GetString(7);
            DateTime birthdate = Convert.ToDateTime(reader["birthdate"].ToString());//Convert.ToDateTime(reader.GetDateTime(8) );// GetString(8)); //Convert.ToDateTime(reader["date"].ToString());//reader.GetDateTime(8);
            string sex = "Mężczyzna";
           // bool plec = false;
            if (reader.GetBoolean(9) == true)
                sex = "Kobieta";
           // else
           //     command.Parameters.AddWithValue("@sex", false);
           //     reader.GetString(9);
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
            u.TownID = townID;
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
            command.Parameters.AddWithValue("@townID", user.TownID);
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
            command.Parameters.Add("@lastLoginDate", DateTime.Now);
            command.Parameters.Add("@login", login);

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

                command.Parameters.Add("@count", 1);
                command.Parameters.Add("@windowStart", DateTime.Now);
                command.Parameters.Add("@login", login);

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

                    command.Parameters.Add("@isLockedOut", true);
                    command.Parameters.Add("@lastLockedOutDate", DateTime.Now);
                    command.Parameters.Add("@login", login);

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

                    command.Parameters.Add("@count", failureCount);
                    command.Parameters.Add("@login", login);

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

        public bool AddRestaurant(string name, string displayName, string address, string townId, string country, string telephone, string email, string nip, string regon, string password, string managerLogin, string deliveryTime)
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

        public bool EditRestaurant(string name, string displayName, string address, string townId, string country, string telephone, string email, string nip, string regon, string deliveryTime, string managerLogin, int id)
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

        private Restaurant GetRestaurantsFromReader(MySqlDataReader reader)
        {
            int id = reader.GetInt32(0);
            string name = reader.GetString(1);
            string displayName = reader.GetString(2);
            string address = reader.GetString(3);
            string townId = reader.GetString(4);
            string country = reader.GetString(5);
            string telephone = reader.GetString(6);
            string email = reader.GetString(7);
            string nip = reader.GetString(8);
            string regon = reader.GetString(9);
            DateTime creationDate = Convert.ToDateTime(reader.GetString(10));
            int inputsCount = reader.GetInt32(11);
            int averageRating = reader.GetInt32(12);
            string password = reader.GetString(13);
            int menagerId = reader.GetInt32(14);
            string deliveryTime = reader.GetString(15);
            string currentDeliveryTime = reader.GetString(16);
            bool isApproved = reader.GetBoolean(17);
            DateTime lastActivityDate = Convert.ToDateTime(reader.GetString(18));
            bool isLockedOut = reader.GetBoolean(19);
            DateTime lastLockedOutDate = new DateTime();
            if (reader.GetValue(20) != DBNull.Value)
                lastLockedOutDate = Convert.ToDateTime(reader.GetString(20));

            Restaurant u = new Restaurant();
            u.ID = id;
            u.Name = name;
            u.DisplayName = displayName;
            u.Address = address;
            u.TownID = townId;
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

        #endregion

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
                return null;
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

            Town u = new Town();
            u.ID = id;
            u.TownName = townName;
            u.PostalCode = postalCode;
            u.Province = province;
            u.District = district;
            u.Community = community;

            return u;
        }
    }


}
