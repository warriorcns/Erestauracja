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

        private int ExecuteNonQuery(MySqlCommand command, string action)
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

        // jeszcze execute scalar

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
            ////command.Parameters.AddWithValue("@applicationName", pApplicationName);

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
    }


}
