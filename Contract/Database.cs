using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//using MySql.Data;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace Contract
{
    public class Database
    {
        private string ConnectionString = "SERVER=" + "5.32.56.82" + ";DATABASE=" + "dbo" + ";UID=" + "erestauracja" + ";PASSWORD=" + "Erestauracja123" + ";";
      
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

        private int ExecuteNonQuery(MySqlCommand command)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            conn.Open();
            command.Connection = conn;

            int rowsaffected = 0;

            try
            {
                rowsaffected = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            finally
            {
                conn.Close();
            }

            return rowsaffected;
        }

        public string GetString()
        {

            try
            {
                MySqlCommand command = new MySqlCommand(Queries.Test);
                // command.Parameters.AddWithValue("@liczba", liczba);
                DataSet ds = new DataSet();
                ds = ExecuteQuery(command);
                string test = "";


                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["Nazwa"] != DBNull.Value) test = row["Nazwa"].ToString();
                    }
                }
                return test;

            }
            catch (MySqlException ex)
            {
                Console.WriteLine(ex.ToString());
                return "";
            }   
        }
    }


}
