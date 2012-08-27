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

        private string ConnectionString = "SERVER=" + "5.32.56.82" + ";DATABASE=" + "dbo" + ";UID=" + "erestauracja" + ";PASSWORD=" + "Erestauracja123" + ";";
      
        //zabezpieczyć connectionString
        public Database()
        {
            //ConnectionStringSettingsCollection settings =
            //    ConfigurationManager.ConnectionStrings;

            //this.ConnectionString = settings[2].ConnectionString;
        }

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

        public bool CreateUser(string login, string password, string email, string name, string surname, string address, string townID, string country, DateTime birthdate, string sex, string telephone, string passwordQuestion, string passwordAnswer, bool isApproved)
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
            command.Parameters.AddWithValue("@sex", sex);
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
            string townID = reader.GetString(6);
            string country = reader.GetString(7);
            DateTime birthdate = Convert.ToDateTime(reader["birthdate"].ToString());//Convert.ToDateTime(reader.GetDateTime(8) );// GetString(8)); //Convert.ToDateTime(reader["date"].ToString());//reader.GetDateTime(8);
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

            if (!(String.IsNullOrEmpty(rowsaffected)))
            {
                return rowsaffected;
            }
            return null;
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
            command.Parameters.AddWithValue("@sex", user.Sex);
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
    }


}
