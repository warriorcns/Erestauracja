using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Diagnostics;

namespace Contract
{
    public class Database
    {
        private string eventSource = "EresWindowsService";
        private string eventLog = "Erestauracja";
        private string message = "Wystąpił błąd związany z mySql podczas komunikacji z bazą danych.\n\n";
        private string message2 = "Wystąpił błąd podczas komunikacji z bazą danych.\n\n";

        private string ConnectionString = "SERVER=" + "5.32.56.82" + ";DATABASE=" + "dbo" + ";UID=" + "erestauracja" + ";PASSWORD=" + "Erestauracja123" + ";";
      
        //zabezpieczyć connectionString
        public Database()
        {
            //ConnectionStringSettingsCollection settings =
            //    ConfigurationManager.ConnectionStrings;

            //this.ConnectionString = settings[2].ConnectionString;
        }

        private DataSet ExecuteQuery(MySqlCommand command)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();
            command.Connection = conn;

            DataSet myDS = new DataSet();

            try
            {
                MySqlDataAdapter da = new MySqlDataAdapter(command);
                da.Fill(myDS);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            finally
            {
                //conn.Clone();
                conn.Close();
            }

            return myDS;
        }

        private int ExecuteNonQuery(MySqlCommand command, string action)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();
            command.Connection = conn;

            int rowsaffected = 0;

            try
            {
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
        
        
            //////try
            //////{
            //////    MySqlCommand command = new MySqlCommand(Queries.Test);

            //////    DataSet ds = new DataSet();
            //////    ds = ExecuteQuery(command);
            //////    string test = "";


            //////    if (ds.Tables.Count > 0)
            //////    {
            //////        foreach (DataRow row in ds.Tables[0].Rows)
            //////        {
            //////            if (row["Nazwa"] != DBNull.Value) test = row["Nazwa"].ToString();
            //////        }
            //////    }
            //////    return test;

            //////}
            //////catch (MySqlException ex)
            //////{
            //////    Console.WriteLine(ex.ToString());
            //////    return "";
            //////}   
    }


}
