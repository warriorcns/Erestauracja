using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using MySql.Data.MySqlClient;

namespace Contract
{
    public class Database
    {
        //stałe dla informacji wyświetlanych w dzienniku zdarzeń
        private string eventSource = "EresWindowsService";
        private string eventLog = "Erestauracja";
        private string message = "Wystąpił błąd związany z MySql podczas komunikacji z bazą danych.\n\n";
        private string message2 = "Wystąpił błąd podczas komunikacji z bazą danych.\n\n";

        public string ConnectionString = "SERVER=localhost;DATABASE=eres;UID=erestauracja;PASSWORD=Erestauracja123;charset=utf8;Encrypt=true;Connection Timeout=60;";//String.Empty;
      
        /// <summary>
        /// Wczytuje wartość connectionString z app.config 
        /// </summary>
        public Database()
        {
           // ConnectionString = ConfigurationManager.ConnectionStrings["MySqlConnection"].ConnectionString;
        }

        /// <summary>
        /// Własny ExecuteQuery
        /// </summary>
        /// <param name="command">Komenda MySqlCommand</param>
        /// <param name="action">Nazwa wykonywanej czynności - info dla dziennika zdarzeń</param>
        /// <returns>Obiekt typu DataSet</returns>
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

        /// <summary>
        /// Własny ExecuteNonQuery
        /// </summary>
        /// <param name="command">Komenda MySqlCommand</param>
        /// <param name="action">Nazwa wykonywanej czynności - info dla dziennika zdarzeń</param>
        /// <returns>int jako liczba wierszy, na których zostało wykonane zapytanie.</returns>
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

        /// <summary>
        /// Własny ExecuteScalar
        /// </summary>
        /// <param name="command">Komenda MySqlCommand</param>
        /// <param name="action">Nazwa wykonywanej czynności - info dla dziennika zdarzeń</param>
        /// <returns>object - pierwsza kolumna pierwszego wiersza z wyniku zapytania</returns>
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


        #region pomocnicze - wczytywanie danych z MySqlDataReader

        /// <summary>
        /// Pobiera dane z MySqlDataReader i zwraca obiekt typu User
        /// </summary>
        /// <param name="reader">MySqlDataReader</param>
        /// <returns>User</returns>
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
            DateTime birthdate = Convert.ToDateTime(reader["birthdate"].ToString());
            string sex = "Mężczyzna";
            if (reader.GetBoolean(10) == true)
                sex = "Kobieta";
            string telephone = reader.GetString(11);
            string comment = "";
            if (reader.GetValue(12) != DBNull.Value)
                comment = reader.GetString(12);
            string passwordQuestion = "";
            if (reader.GetValue(13) != DBNull.Value)
                passwordQuestion = reader.GetString(13);
            bool isApproved = reader.GetBoolean(14);
            DateTime lastActivityDate = Convert.ToDateTime(reader.GetString(15));
            DateTime lastLoginDate = new DateTime();
            if (reader.GetValue(16) != DBNull.Value)
                lastLoginDate = Convert.ToDateTime(reader.GetString(16));
            DateTime lastPasswordChangedDate = Convert.ToDateTime(reader.GetString(17));
            DateTime creationDate = Convert.ToDateTime(reader.GetString(18));
            bool isLockedOut = reader.GetBoolean(19);
            DateTime lastLockedOutDate = new DateTime();
            if (reader.GetValue(20) != DBNull.Value)
                lastLockedOutDate = Convert.ToDateTime(reader.GetString(20));
            
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

        /// <summary>
        /// Pobiera dane z MySqlDataReader i zwraca obiekt typu Restaurant
        /// </summary>
        /// <param name="reader">MySqlDataReader</param>
        /// <returns>Restaurant</returns>
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
            string nip = reader.GetString(8);
            string regon = reader.GetString(9);
            int inputsCount = reader.GetInt32(10);
            double averageRating = 0.0;
            if (reader.GetValue(11) != DBNull.Value) averageRating = reader.GetDouble(11);
            int menagerId = reader.GetInt32(12);
            string deliveryTime = reader.GetString(13);
            int userId = reader.GetInt32(14);
            bool isEnabled = reader.GetBoolean(15);
            string login = reader.GetString(16);
            string email = reader.GetString(17);
            bool isApproved = reader.GetBoolean(18);
            DateTime lastActivityDate = Convert.ToDateTime(reader.GetString(19));
            DateTime creationDate = Convert.ToDateTime(reader.GetString(20));
            bool isLockedOut = reader.GetBoolean(21);
            DateTime lastLockedOutDate = new DateTime();
            if (reader.GetValue(22) != DBNull.Value)
                lastLockedOutDate = Convert.ToDateTime(reader.GetString(22));
            decimal deliveryPrice = reader.GetDecimal(23);

            Restaurant u = new Restaurant();
            u.ID = id;
            u.Name = name;
            u.DisplayName = displayName;
            u.Address = address;
            u.Town = town;
            u.PostalCode = postalCode;
            u.Country = country;
            u.Telephone = telephone;
            u.Nip = nip;
            u.Regon = regon;
            u.InputsCount = inputsCount;
            u.AverageRating = averageRating;
            u.MenagerId = menagerId;
            u.DeliveryTime = deliveryTime;
            u.DeliveryPrice = deliveryPrice;
            u.UserId = userId;
            u.IsEnabled = isEnabled;
            u.Login = login;
            u.Email = email;
            u.IsApproved = isApproved;
            u.LastActivityDate = lastActivityDate;
            u.CreationDate = creationDate;
            u.IsLockedOut = isLockedOut;
            u.LastLockedOutDate = lastLockedOutDate;

            return u;
        }

        /// <summary>
        /// Pobiera dane z MySqlDataReader i zwraca obiekt typu Product
        /// </summary>
        /// <param name="reader">MySqlDataReader</param>
        /// <returns>Product</returns>
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

        /// <summary>
        /// Pobiera dane z MySqlDataReader i zwraca obiekt typu Menu
        /// </summary>
        /// <param name="reader">MySqlDataReader</param>
        /// <returns>Menu</returns>
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

        /// <summary>
        /// Pobiera dane z MySqlDataReader i zwraca obiekt typu Order
        /// </summary>
        /// <param name="reader">MySqlDataReader</param>
        /// <returns>Order</returns>
        private Order GetOrderFromReader(MySqlDataReader reader)
        {
            int orderId = reader.GetInt32(0);
            string userName = reader.GetString(1);
            string userSurname = reader.GetString(2);
            string userAdderss = reader.GetString(3);
            string userTown = reader.GetString(4);
            string userPostal = reader.GetString(5);
            string userTelephone = reader.GetString(6);
            string status = reader.GetString(7);
            decimal price = reader.GetDecimal(8);
            string comment = reader.GetString(9);
            DateTime orderDate = reader.GetDateTime(10);
            string payment = reader.GetString(11);
            DateTime finishDate = new DateTime(9999, 12, DateTime.DaysInMonth(9999, 12));
            if (!reader.IsDBNull(12))
                finishDate = reader.GetDateTime(12);

            Order u = new Order();
            u.OrderId = orderId;
            u.UserName = userName;
            u.UserSurname = userSurname;
            u.UserAdderss = userAdderss;
            u.UserTown = userTown;
            u.UserPostal = userPostal;
            u.UserTelephone = userTelephone;
            u.Status = status;
            u.Price = price;
            u.Comment = comment;
            u.OrderDate = orderDate;
            u.Payment = payment;
            u.FinishDate = finishDate;

            return u;
        }

        /// <summary>
        /// Pobiera dane z MySqlDataReader i zwraca obiekt typu OrderedProduct
        /// </summary>
        /// <param name="reader">MySqlDataReader</param>
        /// <returns>OrderedProduct</returns>
        private OrderedProduct GetOrderedProductFromReader(MySqlDataReader reader)
        {
            int productId = reader.GetInt32(0);
            string productName = reader.GetString(1);
            string priceOption = reader.GetString(2);
            int count = reader.GetInt32(3);
            string nonPriceOption = reader.GetString(4);
            string nonPriceOption2 = reader.GetString(5);
            string comment = reader.GetString(6);

            OrderedProduct u = new OrderedProduct();
            u.ProductId = productId;
            u.ProductName = productName;
            u.PriceOption = priceOption;
            u.Count = count;
            u.NonPriceOption = nonPriceOption;
            u.NonPriceOption2 = nonPriceOption2;
            u.Comment = comment;

            return u;
        }

        /// <summary>
        /// Pobiera dane z MySqlDataReader i zwraca obiekt typu UserOrder
        /// </summary>
        /// <param name="reader">MySqlDataReader</param>
        /// <returns>UserOrder</returns>
        private UserOrder GetUserOrderFromReader(MySqlDataReader reader)
        {
            int orderId = reader.GetInt32(0);
            string displayName = reader.GetString(1);
            string address = reader.GetString(2);
            string town = reader.GetString(3);
            string postal = reader.GetString(4);
            string telephone = reader.GetString(5);
            string deliveryTime = reader.GetString(6);
            decimal deliveryPrice = reader.GetDecimal(7);
            string status = reader.GetString(8);
            decimal price = reader.GetDecimal(9);
            string comment = reader.GetString(10);
            DateTime orderDate = reader.GetDateTime(11);
            string payment = reader.GetString(12);
            DateTime finishDate = new DateTime(9999, 12, DateTime.DaysInMonth(9999, 12));
            if (!reader.IsDBNull(13))
                finishDate = reader.GetDateTime(13);

            UserOrder u = new UserOrder();
            u.OrderId = orderId;
            u.DisplayName = displayName;
            u.Address = address;
            u.Town = town;
            u.Postal = postal;
            u.Telephone = telephone;
            u.DeliveryTime = deliveryTime;
            u.DeliveryPrice = deliveryPrice;
            u.Status = status;
            u.Price = price;
            u.Comment = comment;
            u.OrderDate = orderDate;
            u.Payment = payment;
            u.FinishDate = finishDate;

            return u;
        }

        /// <summary>
        /// Pobiera dane z MySqlDataReader i zwraca obiekt typu Comment
        /// </summary>
        /// <param name="reader">MySqlDataReader</param>
        /// <returns>Comment</returns>
        private Comment GetCommentFromReader(MySqlDataReader reader)
        {
            int id = reader.GetInt32(0);
            string login = reader.GetString(1);
            string displayName = reader.GetString(2);
            string address = reader.GetString(3);
            string town = reader.GetString(4);
            string postal = reader.GetString(5);
            double rating = reader.GetDouble(6);
            string comment = reader.GetString(7);
            DateTime date = reader.GetDateTime(8);

            Comment u = new Comment();
            u.Id = id;
            u.UserLogin = login;
            u.DisplayName = displayName;
            u.Address = address;
            u.Town = town;
            u.Postal = postal;
            u.Rating = rating;
            u.CommentText = comment;
            u.Date = date;

            return u;
        }

        /// <summary>
        /// Pobiera dane z MySqlDataReader i zwraca obiekt typu Town
        /// </summary>
        /// <param name="reader">MySqlDataReader</param>
        /// <returns>Town</returns>
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

        /// <summary>
        /// Pobiera dane z MySqlDataReader i zwraca obiekt typu RestaurantInCity
        /// </summary>
        /// <param name="reader">MySqlDataReader</param>
        /// <returns>RestaurantInCity</returns>
        private RestaurantInCity GetRestaurantsFromCity(MySqlDataReader reader)
        {
            int id = reader.GetInt32(0);
            string displayName = reader.GetString(1);
            string address = reader.GetString(2);
            string town = reader.GetString(3);
            string postalCode = reader.GetString(4);
            string country = reader.GetString(5);
            string telephone = reader.GetString(6);
            int inputsCount = reader.GetInt32(7);
            double averageRating = 0.0;
            if (reader.GetValue(8) != DBNull.Value) averageRating = reader.GetDouble(8);
            string deliveryTime = reader.GetString(9);
            DateTime creationDate = reader.GetDateTime(10);
            double latitude = reader.GetDouble(11);
            double longtitude = reader.GetDouble(12);

            RestaurantInCity u = new RestaurantInCity();
            u.ID = id;
            u.DisplayName = displayName;
            u.Address = address;
            u.Town = town;
            u.PostalCode = postalCode;
            u.Country = country;
            u.Telephone = telephone;
            u.InputsCount = inputsCount;
            u.AverageRating = averageRating;
            u.DeliveryTime = deliveryTime;
            u.CreationDate = creationDate;
            u.Latitude = latitude;
            u.Longtitude = longtitude;

            return u;
        }

        /// <summary>
        /// Pobiera dane z MySqlDataReader i zwraca obiekt typu Category
        /// </summary>
        /// <param name="reader">MySqlDataReader</param>
        /// <returns>Category</returns>
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

        #endregion

        #region dodatkowe klasy pomocnicze

        #region Geocoding

        /// <summary>
        /// Współrzędne do google maps
        /// </summary>
        public class Coordinate
        {
            public double Latitude;
            public double Longitude;

            public Coordinate(double Latitude, double Longitude)
            {
                this.Latitude = Latitude;
                this.Longitude = Longitude;
            }
        }

        /// <summary>
        /// Pobiera współrzędne określonego miejsca według map google
        /// </summary>
        /// <param name="region">Adres z którego zostaną pobrane współrzędne</param>
        /// <returns>Współrzędne jako Coordinate</returns>
        public static Coordinate GetCoordinates(string region)
        {
            using (var client = new WebClient())
            {
                string uri = "http://maps.google.com/maps/geo?q='" + region +
                  "'&output=csv&key=ABQIAAAAzr2EBOXUKnm_jVnk0OJI7xSosDVG8KKPE1-m51RBrvYughuyMxQ-i1QfUnH94QxWIa6N4U6MouMmBA";

                string[] geocodeInfo = client.DownloadString(uri).Split(',');

                NumberStyles style;
                CultureInfo culture;

                style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
                culture = CultureInfo.CreateSpecificCulture("en-CA");

                double lat;
                double lng;

                double.TryParse(geocodeInfo[2].ToString(), style, culture, out lat);
                double.TryParse(geocodeInfo[3].ToString(), style, culture, out lng);
                return new Coordinate(lat, lng);
            }
        }

        #endregion

        /// <summary>
        /// Klasa pomocnicza - zawiera dane o produktach z koszyka
        /// </summary>
        public class Basket
        {
            private int restaurantId = -1;// id restauracji
            private List<string> data = new List<string>();//lista danych w koszyku

            public int RestaurantId
            {
                get { return restaurantId; }
                set { restaurantId = value; }
            }

            public List<string> Data
            {
                get { return data; }
                set { data = value; }
            }
        }

        #endregion


        #region Membership

        /// <summary>
        /// Zamienia hasło u użytkownika o danym loginie.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="password">Hasło do ustawienia</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public bool ChangePassword(string login, string password)
        {
            MySqlCommand command = new MySqlCommand(Queries.ChangePassword);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@lastPasswordChangedDate", DateTime.Now);
            command.Parameters.AddWithValue("@login", login);

            int rowsaffected = ExecuteNonQuery(command, "ChangePassword");

            if (rowsaffected > 0)
            {
                return true;
            }
            return false;  
        }

        /// <summary>
        /// Zamienia pytanie oraz odpowiedź do odzyskiwania hasła u użytkownika o danym loginie.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="newPwdQuestion">Nowe pytanie</param>
        /// <param name="newPwdAnswer">Nowa odpowiedź</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public bool ChangePasswordQuestionAndAnswer(string login, string newPwdQuestion, string newPwdAnswer)
        {
            MySqlCommand command = new MySqlCommand(Queries.ChangePasswordQuestionAndAnswer);
            command.Parameters.AddWithValue("@question", newPwdQuestion);
            command.Parameters.AddWithValue("@answer", newPwdAnswer);
            command.Parameters.AddWithValue("@login", login);

            int rowsaffected = ExecuteNonQuery(command, "ChangePasswordQuestionAndAnswer");

            if (rowsaffected > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Zwraca hasło, odpowiedź do odzyskiwania hasła oraz informacje czy użytkownik jest zablokowany, użytkownika o danym loginie.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>obiekt typu PasswordAndAnswer</returns>
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

        /// <summary>
        /// Zwraca odpowiedź do odzyskiwania hasła oraz informacje czy użytkownik jest zablokowany, użytkownika o danym loginie.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>obiekt typu PasswordAnswer</returns>
        public PasswordAnswer GetPasswordAnswer(string login)
        {
            MySqlCommand command = new MySqlCommand(Queries.GetPasswordAnswer);
            command.Parameters.AddWithValue("@login", login);

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

        /// <summary>
        /// Ustawia nowe hasło, aktualizuje date zmiany hasła oraz wysyła email z nowym hasłem na adres emali użytkownika
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="password">Hasło</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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
                            bool ok = false;
                            Email em = new Email();
                            ok = em.SendPassword(email, password);

                            if (ok == true)
                            {
                                trans.Commit();
                                return true;
                            }
                            else
                            {
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

        /// <summary>
        /// Zwraca pytanie do odzyskiwania hasła oraz informacje czy użytkownik jest zablokowany, użytkownika o danym loginie.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>obiekt typu PasswordQuestion</returns>
        public PasswordQuestion GetUserQuestion(string login)
        {
            MySqlCommand command = new MySqlCommand(Queries.GetUserQuestion);
            command.Parameters.AddWithValue("@login", login);

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

        /// <summary>
        /// Zapisuje nowego użytkownika w bazie danych.
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
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Usuwa użytkownika z bazy
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="deleteAllRelatedData">Czy usunąć powiązane dane</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Zwraca wszystkich użytkowników, z podziałem na strony
        /// </summary>
        /// <param name="pageIndex">Indeks strony</param>
        /// <param name="pageSize">Rozmiar strony</param>
        /// <param name="totalRecords">Out ilość pobranych użytkowników</param>
        /// <returns>Lista typu User</returns>
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
 
        /// <summary>
        /// Zwraca ilość użytkowników, których aktywonść jest późniejsza niż onlineSpan
        /// </summary>
        /// <param name="onlineSpan">Okres czasu do porównania</param>
        /// <returns>int jako liczba użytkowników</returns>
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

        /// <summary>
        /// Zwraca użytkownika o danym loginie
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="userIsOnline">Czy użytkownik jest online - aktualizacja daty ostatniej aktywności</param>
        /// <returns>obiekt typu User</returns>
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

        /// <summary>
        /// Zwraca użytkownika o danym id
        /// </summary>
        /// <param name="id">Id użytkownika</param>
        /// <param name="userIsOnline">Czy użytkownik jest online - aktualizacja daty ostatniej aktywności</param>
        /// <returns>obiekt typu User</returns>
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

        /// <summary>
        /// Odblokowuje konto użytkownika.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Pobiera nazwe użytkownika na podstawie adresu email.
        /// </summary>
        /// <param name="email">Adres email użytkownika</param>
        /// <returns>Zwraca login użytkownika.</returns>
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

        /// <summary>
        /// Aktualizuje dane użytkownika
        /// </summary>
        /// <param name="user">Dane użytkownika jako obiekt typu User</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public bool UpdateUser(User user)
        {
            MySqlCommand command = new MySqlCommand(Queries.UpdateUser);
            command.Parameters.AddWithValue("@email", user.Email);
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

        /// <summary>
        /// Zwraca hasło, oraz informacje czy użytkownik jest zatwierdzony, użytkownika o danym loginie.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>obiekt typu ValidateUser</returns>
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

        /// <summary>
        /// Zwraca hasło, oraz informacje czy użytkownik jest zatwierdzony, pracownika o danym loginie oraz restauracji.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="rest">Login reatauracji</param>
        /// <returns>obiekt typu ValidateUser</returns>
        public ValidateUser ValidateEmployee(string login, string rest)
        {
            MySqlCommand command = new MySqlCommand(Queries.ValidateEmployee);
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@isLockedOut", false);
            command.Parameters.AddWithValue("@rest", rest);

            ValidateUser value = null;

            DataSet ds = new DataSet();
            ds = ExecuteQuery(command, "ValidateEmployee");

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

        /// <summary>
        /// Atkualizuje date ostatniego logowania użytkownika
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Atkualizuje date ostatniego logowania pracownika  z danej restauracji
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="rest">Login reatauracji</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public bool UpdateEmployeeLoginDate(string login, string rest)
        {
            DateTime czas = DateTime.Now;

            MySqlCommand command = new MySqlCommand(Queries.UpdateUserLoginDate);
            command.Parameters.AddWithValue("@lastLoginDate", czas);
            command.Parameters.AddWithValue("@login", login);

            MySqlCommand command2 = new MySqlCommand(Queries.UpdateEmployeeLoginDate);
            command2.Parameters.AddWithValue("@lastLoginDate", czas);
            command2.Parameters.AddWithValue("@login", login);
            command2.Parameters.AddWithValue("@rest", rest);

            int rowsaffected = ExecuteNonQuery(command, "UpdateUserLoginDate");
            int rowsaffected2 = ExecuteNonQuery(command2, "UpdateEmployeeLoginDate");

            if (rowsaffected > 0 && rowsaffected2 > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Aktualizuje licznik błędnie wprowadzonego hasła lub odpowiedzi na pytanie do przywracania hasła.
        /// Jeśli licznik sie wyczerpie blokuje użytkownika.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="failureType">Typ błędu - password lub passwordAnswer</param>
        /// <param name="PasswordAttemptWindow">Ilość minut po jakich użytkownik będzie mógł ponowić próby</param>
        /// <param name="MaxInvalidPasswordAttempts">Możliwa liczba pomyłek</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public bool UpdateFailureCount(string login, string failureType, int PasswordAttemptWindow, int MaxInvalidPasswordAttempts)
        {
            MySqlCommand command = new MySqlCommand(Queries.GetFailureCount);
            command.Parameters.AddWithValue("@login", login);

            DateTime windowStart = new DateTime();
            int failureCount = 0;

            DataSet ds = new DataSet();
            ds = ExecuteQuery(command, "GetFailureCount");

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

        #region Role

        /// <summary>
        /// Dodaje użytkowników o danym loginie do ról
        /// </summary>
        /// <param name="logins">Loginy użytkowników</param>
        /// <param name="rolenames">Role</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Tworzy nową role
        /// </summary>
        /// <param name="rolename">Nazwa roli</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Usuwa role oraz przypisania użytkowników do tej roli
        /// </summary>
        /// <param name="rolename">Nazwa roli</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Pobiera wszystkie role
        /// </summary>
        /// <returns>Nazwy ról oddzielonych przecinkami</returns>
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

        /// <summary>
        /// Pobiera role przypisane do danego użytkownika
        /// </summary>
        /// <param name="login">Login uzytkownika</param>
        /// <returns>Nazwy ról oddzielone przecinkami</returns>
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

        /// <summary>
        /// Pobiera loginy użytkowników przypisanych do danej roli
        /// </summary>
        /// <param name="rolename">Nazwa roli</param>
        /// <returns>Loginy użytkowników oddzielone przecinkami</returns>
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

        /// <summary>
        /// Sprawdza czy użytkownik posiada daną role
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="rolename">Nazwa roli</param>
        /// <returns>True jeśli użytkownik posiada daną role</returns>
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

        /// <summary>
        /// Usuwa przypisania użytkowników do ról
        /// </summary>
        /// <param name="logins">Loginy użytkowników</param>
        /// <param name="rolenames">Nazwy ról</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Sprawdza istnienie danej roli
        /// </summary>
        /// <param name="rolename">Nazwa roli</param>
        /// <returns>True jeśli rola istnieje</returns>
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

        /// <summary>
        /// Pobiera loginy użytkowników przypisanych do danej roli
        /// </summary>
        /// <remarks>
        /// Jeśli loginToMatch jest pusty lub null to metoda zwróci wszystkich użytkowników w danej roli
        /// Jeśli loginToMatch jest podany metoda zwróci uzytkowników których login rozpoczyna się do loginToMatch
        /// </remarks>
        /// <param name="rolename">Nazwa roli</param>
        /// <param name="loginToMatch">Login do dopasowania</param>
        /// <returns>Loginy użytkowników oddzielone przecinkami</returns>
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

        /// <summary>
        /// Dodawanie nowej restauracji
        /// </summary>
        /// <param name="login">Login restauracji</param>
        /// <param name="email">Emali</param>
        /// <param name="password">Hasło</param>
        /// <param name="passwordQuestion">Pytanie do odzyskiwania hasła</param>
        /// <param name="passwordAnswer">Odpowiedz do pytania do odzyskiwania hasła</param>
        /// <param name="name">Nazwa firmy</param>
        /// <param name="displayName">Nazwa wyświetlana</param>
        /// <param name="address">Adres lokalu</param>
        /// <param name="townID">Id miasta</param>
        /// <param name="country">Kraj</param>
        /// <param name="telephone">Telefon</param>
        /// <param name="nip">Numer NIP</param>
        /// <param name="regon">Numer REGON</param>
        /// <param name="deliveryTime">Czas dostawy</param>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="deliveryPrice">Cena dostawy</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public bool AddRestaurant(string login, string email, string password, string passwordQuestion, string passwordAnswer, string name, string displayName, string address, int townID, string country, string telephone, string nip, string regon, string deliveryTime, string managerLogin, decimal deliveryPrice)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlTransaction tran = null;
            Coordinate wspol = new Coordinate(0.0, 0.0);

            //dodawanie restauracji jako usera
            DateTime createDate = DateTime.Now;

            MySqlCommand command = new MySqlCommand(Queries.CreateUser);
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@password", password);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@name", String.Empty);
            command.Parameters.AddWithValue("@surname", String.Empty);
            command.Parameters.AddWithValue("@address", String.Empty);
            command.Parameters.AddWithValue("@townID", townID);
            command.Parameters.AddWithValue("@country", country);
            command.Parameters.AddWithValue("@birthdate", new DateTime());
            command.Parameters.AddWithValue("@sex", false);
            command.Parameters.AddWithValue("@telephone", String.Empty);
            command.Parameters.AddWithValue("@comment", "R");
            command.Parameters.AddWithValue("@passwordQuestion", passwordQuestion);
            command.Parameters.AddWithValue("@passwordAnswer", passwordAnswer);
            command.Parameters.AddWithValue("@isApproved", true);
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

            //dodawanie roli do restauracji jako user
            MySqlCommand command2 = new MySqlCommand(Queries.AddUsersToRoles);
            command2.Parameters.AddWithValue("@login", login);
            command2.Parameters.AddWithValue("@rolename", "Restauracja");
            command2.Connection = conn;

            //pobranie współrzędnych restauracji
            string region = String.Empty;

            MySqlDataReader reader = null;
            try
            {
                MySqlCommand wspolrzedne = new MySqlCommand(Queries.GetDataForGeolock);
                wspolrzedne.Parameters.AddWithValue("@id", townID);
                wspolrzedne.Connection = conn;
                conn.Open();

                string miejscowosc = String.Empty;
                string kod = String.Empty;
                string gmina = String.Empty;
                string powiat = String.Empty;
                string wojewodztwo = String.Empty;

                reader = wspolrzedne.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        miejscowosc = reader.GetString(0);
                        kod = reader.GetString(1);
                        gmina = reader.GetString(2);
                        powiat = reader.GetString(3);
                        wojewodztwo = reader.GetString(4);
                    }
                }
                reader.Close();
                region= address + ", " + miejscowosc + ", " + kod + ", " + gmina + ", " + powiat + ", " + wojewodztwo + ", " + country;
                wspol = GetCoordinates(region);
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "Pobieranie danych do współrzędnych" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "Pobieranie danych do współrzędnych" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            //dodawanie restauracji
            MySqlCommand command3 = new MySqlCommand(Queries.AddRestaurant);
            command3.Parameters.AddWithValue("@name", name);
            command3.Parameters.AddWithValue("@displayName", displayName);
            command3.Parameters.AddWithValue("@address", address);
            command3.Parameters.AddWithValue("@townId", townID);
            command3.Parameters.AddWithValue("@country", country);
            command3.Parameters.AddWithValue("@telephone", telephone);
            command3.Parameters.AddWithValue("@nip", nip);
            command3.Parameters.AddWithValue("@regon", regon);
            command3.Parameters.AddWithValue("@inputsCount", 0);
            command3.Parameters.AddWithValue("@menager", managerLogin);
            command3.Parameters.AddWithValue("@deliveryTime", deliveryTime);
            command3.Parameters.AddWithValue("@login", login);
            command3.Parameters.AddWithValue("@isEnabled", false);
            command3.Parameters.AddWithValue("@latitude", wspol.Latitude);
            command3.Parameters.AddWithValue("@longitude", wspol.Longitude);
            command3.Parameters.AddWithValue("@price", deliveryPrice);
            command3.Connection = conn;

            //dodawanie pustej zawartości strony restauracji
            MySqlCommand command4 = new MySqlCommand(Queries.AddEmptyContent);
            command4.Parameters.AddWithValue("@menager", managerLogin);
            command4.Parameters.AddWithValue("@login", login);
            command4.Connection = conn;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                command.Transaction = tran;
                command2.Transaction = tran;
                command3.Transaction = tran;
                command4.Transaction = tran;

                int com1 = command.ExecuteNonQuery();
                int com2 = command2.ExecuteNonQuery();
                int com3 = command3.ExecuteNonQuery();
                int com4 = command4.ExecuteNonQuery();

                if (com1 > 0 && com2 > 0 && com3 > 0 && com4 > 0)
                {
                    tran.Commit();
                    return true;
                }
                else
                {
                    tran.Rollback();
                    return false;
                }
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
                wiadomosc += "Action: " + "AddRestaurant" + "\n\n";
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
                wiadomosc += "Action: " + "AddRestaurant" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                return false;
            }
            finally
            {
                conn.Close();
            }
            return false;
        }

        /// <summary>
        /// Edycja danych restauracji
        /// </summary>
        /// <param name="name">Nazwa firmy</param>
        /// <param name="displayName">Nazwa wyświetlana</param>
        /// <param name="address">Adres lokalu</param>
        /// <param name="townId">Id miasta</param>
        /// <param name="country">Kraj</param>
        /// <param name="telephone">Telefon</param>
        /// <param name="nip">Numer NIP</param>
        /// <param name="regon">Numer REGON</param>
        /// <param name="deliveryTime">Czas dostawy</param>
        /// <param name="isEnabled">Czy restauracja jest widoczna dla klientów</param>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="id">Id restauracji</param>
        /// <param name="deliveryPrice">Cena dostawy</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public bool EditRestaurant(string name, string displayName, string address, int townId, string country, string telephone, string nip, string regon, string deliveryTime, bool isEnabled, string managerLogin, int id, decimal deliveryPrice)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            Coordinate wspol = new Coordinate(0.0, 0.0);

            //pobranie współrzędnych restauracji
            string region = String.Empty;

            MySqlDataReader reader = null;
            try
            {
                MySqlCommand wspolrzedne = new MySqlCommand(Queries.GetDataForGeolock);
                wspolrzedne.Parameters.AddWithValue("@id", townId);
                wspolrzedne.Connection = conn;
                conn.Open();

                string miejscowosc = String.Empty;
                string kod = String.Empty;
                string gmina = String.Empty;
                string powiat = String.Empty;
                string wojewodztwo = String.Empty;

                reader = wspolrzedne.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        miejscowosc = reader.GetString(0);
                        kod = reader.GetString(1);
                        gmina = reader.GetString(2);
                        powiat = reader.GetString(3);
                        wojewodztwo = reader.GetString(4);
                    }
                }
                reader.Close();
                region = address + ", " + miejscowosc + ", " + kod + ", " + gmina + ", " + powiat + ", " + wojewodztwo + ", " + country;
                wspol = GetCoordinates(region);
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "Pobieranie danych do współrzędnych" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
            }
            catch (Exception ex)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message2;
                wiadomosc += "Action: " + "Pobieranie danych do współrzędnych" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                if (reader != null) { reader.Close(); }
                conn.Close();
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            MySqlCommand command = new MySqlCommand(Queries.EditRestaurant);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@displayName", displayName);
            command.Parameters.AddWithValue("@address", address);
            command.Parameters.AddWithValue("@townId", townId);
            command.Parameters.AddWithValue("@country", country);
            command.Parameters.AddWithValue("@telephone", telephone);
            command.Parameters.AddWithValue("@nip", nip);
            command.Parameters.AddWithValue("@regon", regon);
            command.Parameters.AddWithValue("@deliveryTime", deliveryTime);
            command.Parameters.AddWithValue("@isEnabled", isEnabled);
            command.Parameters.AddWithValue("@menager", managerLogin);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@latitude", wspol.Latitude);
            command.Parameters.AddWithValue("@longitude", wspol.Longitude);
            command.Parameters.AddWithValue("@price", deliveryPrice);

            int rowsaffected = ExecuteNonQuery(command, "EditRestaurant");

            if (rowsaffected > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Pobiera restauracje przypisane do danego menadżera
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <returns>Lista typu Restaurant</returns>
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

        /// <summary>
        /// Pobiera restauracje o danym id, przypisaną do danego menadżera
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu RestaurantInfo</returns>
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
                        rest.Nip = reader.GetString(8);
                        rest.Regon = reader.GetString(9);
                        rest.DeliveryTime = reader.GetString(10);
                        rest.IsEnabled = reader.GetBoolean(11);
                        rest.DeliveryPrice = reader.GetDecimal(12);
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

        /// <summary>
        /// Pobiera zawartość strony głównej restauracji dla menadżera 
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu MainPageContent</returns>
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

        /// <summary>
        /// Edycja zawartości strony głównej restauracji dla menadżera 
        /// </summary>
        /// <param name="description">Treść opisu</param>
        /// <param name="foto">Pole nie jest używane, zostanie usunięte przy okazji updateu</param>
        /// <param name="specialOffers">Treść ofert specjalnych</param>
        /// <param name="id">Id restauracji</param>
        /// <param name="managerLogin">Login menadżera</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Pobiera zawartość strony dowóz restauracji dla menadżera 
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu DeliveryPageContent</returns>
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

        /// <summary>
        /// Edycja zawartości strony dowóz restauracji dla menadżera
        /// </summary>
        /// <param name="delivery">Treść dowozu</param>
        /// <param name="id">Id restauracji</param>
        /// <param name="managerLogin">Login menadżera</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Pobiera zawartość strony wydarzenia restauracji dla menadżera 
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu EventsPageContent</returns>
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

        /// <summary>
        /// Edycja zawartości strony wydarzenia restauracji dla menadżera
        /// </summary>
        /// <param name="events">Treść wydarzeń</param>
        /// <param name="id">Id restauracji</param>
        /// <param name="managerLogin">Login menadżera</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Pobiera zawartość strony kontakt restauracji dla menadżera 
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu ContactPageContent</returns>
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

        /// <summary>
        /// Edycja zawartości strony kontakt restauracji dla menadżera
        /// </summary>
        /// <param name="contact">Treść kontaktu</param>
        /// <param name="id">Id restauracji</param>
        /// <param name="managerLogin">Login menadżera</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Dodaje nową kategorię
        /// </summary>
        /// <param name="restaurantID">Id restauracji</param>
        /// <param name="categoryName">Nazwa kategorii</param>
        /// <param name="categoryDescription">Opis kategorii</param>
        /// <param name="priceOption">Opcje cenowe</param>
        /// <param name="nonPriceOption">Opcje nie cenowe</param>
        /// <param name="nonPriceOption2">Opcje nie cenowe 2</param>
        /// <param name="managerLogin">Login menadżera</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Pobiera kategorie danej restauracji
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="restaurantID">Id restauracji</param>
        /// <returns>Lista typu Category</returns>
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

        /// <summary>
        /// Pobiera dane kategori dla menadżera
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="restaurantID">Id restauracji</param>
        /// <param name="categoryID">Id kategorii</param>
        /// <returns>Obiekt typu Category</returns>
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

        /// <summary>
        /// Edytuje kategorie
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="restaurantID">Id restauracji</param>
        /// <param name="categoryID">Id kategorii</param>
        /// <param name="categoryName">Nazwa kategorii</param>
        /// <param name="categoryDescription">Opis kategorii</param>
        /// <param name="priceOption">Opcja cenowa</param>
        /// <param name="nonPriceOption">Opcja niecenowa</param>
        /// <param name="nonPriceOption2">Opcja niecenowa 2</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Usuwa kategorie
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="restaurantID">Id restauracji</param>
        /// <param name="categoryID">Id kategorii</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Dodawanie nowego produktu
        /// </summary>
        /// <param name="restaurantID">Id restauracji</param>
        /// <param name="categoryID">Id kategorii</param>
        /// <param name="productName">Nazwa produktu</param>
        /// <param name="productDescription">Opis produktu</param>
        /// <param name="price">Cena</param>
        /// <param name="managerLogin">Login menadżera</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Pobiera menu restauracji dla menadżera
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="restaurantID">Id restauracji</param>
        /// <returns>Lista typu Menu</returns>
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

        /// <summary>
        /// Pobiera dane produktu dla menadżera
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="restaurantID">Id restauracji</param>
        /// <param name="productID">Id produktu</param>
        /// <returns>Obiekt typu Product</returns>
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

        /// <summary>
        /// Edytuje produkt
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="restaurantID">Id restauracji</param>
        /// <param name="id">Id produktu</param>
        /// <param name="categoryID">Id kategorii</param>
        /// <param name="productName">Nazwa produktu</param>
        /// <param name="productDescription">Opis produktu</param>
        /// <param name="price">Cena</param>
        /// <param name="isAvailable">Czy widoczny dla klientów</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Pobiera pracowników przypisanych do restauracji danego menadżera
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <returns>Lista typu Presonnel</returns>
        public List<Presonnel> GetPersonnel(string managerLogin)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            List<Presonnel> rest = null;
            try
            {
                MySqlCommand command = new MySqlCommand(Queries.GetRestaurantsByManagerLogin);
                command.Parameters.AddWithValue("@managerLogin", managerLogin);
                command.Connection = conn;
                rest = new List<Presonnel>();
                conn.Open();

                reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Restaurant r = GetRestaurantsFromReader(reader);
                    Presonnel per = new Presonnel();
                    per.RestaurantId = r.ID;
                    per.RestaurantName = r.Name;
                    per.RestaurantAddress = r.Address;
                    per.RestaurantTown = r.Town;
                    rest.Add(per);
                }
                conn.Close();
                reader.Close();

                if (rest.Count > 0)
                {
                    MySqlCommand command2 = new MySqlCommand(Queries.GetPersonnel);
                    command2.Connection = conn;
                    foreach (Presonnel item in rest)
                    {
                        command2.Parameters.Clear();
                        command2.Parameters.AddWithValue("@restaurantId", item.RestaurantId);
                        conn.Open();
                        reader = command2.ExecuteReader();
                        while (reader.Read())
                        {
                            User u = GetUserFromReader(reader);
                            item.Employees.Add(u);
                        }
                        conn.Close();
                        reader.Close();
                    }
                }
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetPersonnel" + "\n\n";
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
                wiadomosc += "Action: " + "GetPersonnel" + "\n\n";
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

        /// <summary>
        /// Dodaje pracownika do restauracji
        /// </summary>
        /// <param name="userId">Id użytkownika</param>
        /// <param name="restaurantId">Id restauracji</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public bool AddUserToRestaurant(int userId, int restaurantId)
        {
            MySqlCommand command = new MySqlCommand(Queries.AddUserToRestaurant);
            command.Parameters.AddWithValue("@userId", userId);
            command.Parameters.AddWithValue("@restaurantId", restaurantId);

            int rowsaffected = ExecuteNonQuery(command, "AddUserToRestaurant");

            if (rowsaffected > 0)
            {
                return true;
            }

            else
                return false;
        }

        /// <summary>
        /// Pobiera dane o zawartości koszyka
        /// </summary>
        /// <param name="koszyk">Zawartość koszyka z cookie</param>
        /// <returns>Obiekt typu BasketOut</returns>
        public BasketOut GetBasket(string koszyk)
        {
            BasketOut basket = new BasketOut();
            List<Basket> kosz = new List<Basket>();
            foreach (string item in koszyk.Split('|'))
            {
                bool flaga = false;
                string[] dane = item.Split('~');
                if (kosz.Count == 0)
                {
                    Basket bas = new Basket();
                    bas.RestaurantId = Int32.Parse(dane[1]);
                    bas.Data.Add(item);
                    kosz.Add(bas);
                }
                else
                {
                    foreach (Basket xxx in kosz)
                    {
                        if (xxx.RestaurantId == Int32.Parse(dane[1]))
                        {
                            xxx.Data.Add(item);
                            flaga = true;
                            break;
                        }  
                    }
                    if(flaga==false)
                        {
                            Basket bas = new Basket();
                            bas.RestaurantId = Int32.Parse(dane[1]);
                            bas.Data.Add(item);
                            kosz.Add(bas);
                           // break;
                        }
                }
            }
            foreach (Basket item in kosz)
            {
                //info o restauracji
                MySqlCommand command = new MySqlCommand(Queries.GetBasketRestaurant);
                command.Parameters.AddWithValue("@id", item.RestaurantId);

                BasketRest value = null;

                DataSet ds = new DataSet();
                ds = ExecuteQuery(command, "GetBasketRestaurant");

                if (ds.Tables.Count > 0)
                {
                    value = new BasketRest();
                    value.RestaurantId = -1;
                    value.DisplayName = String.Empty;
                    value.Telephone = String.Empty;
                    value.DeliveryTime = String.Empty;
                    value.DeliveryPrice = 0.00M;
                    value.TotalPriceRest = 0.00M;
                    value.Products = new List<BasketProduct>();

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (row["id"] != DBNull.Value) value.RestaurantId = Convert.ToInt32(row["id"]);
                        if (row["displayName"] != DBNull.Value) value.DisplayName = row["displayName"].ToString();
                        if (row["telephone"] != DBNull.Value) value.Telephone = row["telephone"].ToString();
                        if (row["deliveryTime"] != DBNull.Value) value.DeliveryTime = row["deliveryTime"].ToString();
                        if (row["deliveryPrice"] != DBNull.Value) value.DeliveryPrice = Convert.ToDecimal(row["deliveryPrice"]);
                    }
                    value.TotalPriceRest = value.DeliveryPrice;
                    //
                    //pobrać dane produktu
                    foreach (String produkt in item.Data)
                    {
                        string[] dane = produkt.Split('~');

                        MySqlCommand command2 = new MySqlCommand(Queries.GetBasketProduct);
                        command2.Parameters.AddWithValue("@id", dane[2]);

                        BasketProduct value2 = null;

                        DataSet ds2 = new DataSet();
                        ds2 = ExecuteQuery(command2, "GetBasketProduct");

                        if (ds2.Tables.Count > 0)
                        {
                            //pomocnicze
                            string price = String.Empty;
                            string priceOption = String.Empty;

                            value2 = new BasketProduct();
                            value2.BasketId = Convert.ToInt32( dane[0] );
                            value2.ProductId = -1;
                            value2.ProductName = String.Empty;
                            value2.Comment = dane[7];
                            value2.PriceOption = dane[3];
                            value2.NonPriceOption = dane[4];
                            value2.NonPriceOption2 = dane[5];
                            value2.Price = 0.00M;
                            value2.Count = Convert.ToInt32( dane[6] );
                            value2.TotalPriceProd = 0.00M;

                            foreach (DataRow row in ds2.Tables[0].Rows)
                            {
                                if (row["id"] != DBNull.Value) value2.ProductId = Convert.ToInt32( row["id"] );
                                if (row["name"] != DBNull.Value) value2.ProductName = row["name"].ToString();
                                if (row["price"] != DBNull.Value) price =row["price"].ToString();
                                if (row["priceOption"] != DBNull.Value) priceOption = row["priceOption"].ToString();
                            }

                            //pobieranie caeny
                            string[] priceT = price.Split('|');
                            string[] priceOptionT = priceOption.Split(',');
                            if (priceT.Length == priceOptionT.Length)
                            {
                                for (int i = 0; i < priceT.Length; i++)
                                {
                                    if (priceOptionT[i] == value2.PriceOption)
                                    {
                                     //   NumberStyles style = NumberStyles.AllowDecimalPoint;
                                        //price = Decimal.Parse(model.DeliveryPrice, style);
                                        //value2.Price = Decimal.Parse(model.DeliveryPrice, style);
                                        value2.Price = Convert.ToDecimal(priceT[i].Replace('.',','));
                                        value2.TotalPriceProd = value2.Price * value2.Count;
                                        value.TotalPriceRest += value2.TotalPriceProd;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                //ceny nie są adekwatne do opcji więc zwrócone dane nie będą prawidłowe
                                return null;
                            }
                            value.Products.Add(value2);
                        }
                        else
                        {
                            //nie pobrało danych więc zwrócone dane nie będą prawidłowe
                            return null;
                        }
                    }
                    //
                    basket.Basket.Add(value);
                }
                else
                {
                    //nie pobrało danych więc zwrócone dane nie będą prawidłowe
                    return null;
                }
            }

            return basket;
        }

        /// <summary>
        /// Sprawdza czy menadżer jest właścicielem restauracji
        /// </summary>
        /// <param name="login">Login menadżera</param>
        /// <param name="id">Id restauracji</param>
        /// <returns>True jeśli jest właścicielem</returns>
        public bool IsRestaurantOwner(string login, int id)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            try
            {
                MySqlCommand commandTest = new MySqlCommand(Queries.IsRestaurantOwner);
                commandTest.Parameters.AddWithValue("@managerLogin", login);
                commandTest.Parameters.AddWithValue("@restaurantID", id);
                commandTest.Connection = conn;
                conn.Open();

                reader = commandTest.ExecuteReader(CommandBehavior.SingleRow);
                if (reader.HasRows)
                {
                    return true;
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

        #region Ogólne

        /// <summary>
        /// Pobiera liste państw
        /// </summary>
        /// <returns>Lista typu string</returns>
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

        /// <summary>
        /// Pobiera liste miast pasujących do kryteriów
        /// </summary>
        /// <param name="townName">Nazwa miasta</param>
        /// <param name="postalCode">Kod pocztowy</param>
        /// <param name="status">out status - informacja o statusie pobierania miast</param>
        /// <returns>Lista typu Town</returns>
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
                        status = "Nie znaleziono podanego miasta, ale znaleziono podobne.";
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

        /// <summary>
        /// Pobiera liste restauracji z danego miasta
        /// </summary>
        /// <param name="townName">Nazwa miasta</param>
        /// <returns>Lista typu RestaurantInTown</returns>
        public List<RestaurantInTown> GetRestaurantByTown(string townName)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            List<RestaurantInTown> rest = null;
            try
            {
                MySqlCommand command = new MySqlCommand(Queries.GetRestaurantByTown);
                command.Parameters.AddWithValue("@townName", townName+"%");
                command.Parameters.AddWithValue("@isEnabled", true);
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

        /// <summary>
        /// Pobiera menu restauracji dla klienta
        /// </summary>
        /// <param name="restaurantID">Id restauracji</param>
        /// <returns>Lista typu menu</returns>
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

        /// <summary>
        /// Pobiera zawartość strony głównej restauracji dla klienta
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu MainPageContent</returns>
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

        /// <summary>
        /// Pobiera zawartość strony dowozu restauracji dla klienta
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu DeliveryPageContent</returns>
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

        /// <summary>
        /// Pobiera zawartość strony wydarzeń restauracji dla klienta
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu EventsPageContent</returns>
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

        /// <summary>
        /// Pobiera zawartość strony kontaktu restauracji dla klienta
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu ContactPageContent</returns>
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

        /// <summary>
        /// Pobiera liste restauracji z danego miasta
        /// </summary>
        /// <param name="cityName">Nazwa miasta</param>
        /// <returns>Obiekt typu RestaurantsFromCity</returns>
        public RestaurantsFromCity RestaurantsFromCity(string cityName)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            RestaurantsFromCity rest = null;
            try
            {
                MySqlCommand command = new MySqlCommand(Queries.RestaurantsFromCity);
                command.Parameters.AddWithValue("@cityName", cityName+"%");
                command.Parameters.AddWithValue("@isEnabled", true);
                command.Connection = conn;
                rest = new RestaurantsFromCity();
                conn.Open();

                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    rest.CityName = cityName.ToUpper();
                    while (reader.Read())
                    {
                        RestaurantInCity r = GetRestaurantsFromCity(reader);
                        rest.Restaurants.Add(r);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "RestaurantsFromCity" + "\n\n";
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
                wiadomosc += "Action: " + "RestaurantsFromCity" + "\n\n";
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

        /// <summary>
        /// Pobiera 10 najnowszych restauracji
        /// </summary>
        /// <returns>Lista typu RestaurantTop</returns>
        public List<RestaurantTop> GetTopRestaurant()
        {
            MySqlCommand command = new MySqlCommand(Queries.GetTopRestaurant);

            List<RestaurantTop> value = null;

            DataSet ds = new DataSet();
            ds = ExecuteQuery(command, "GetTopRestaurant");

            if (ds.Tables.Count > 0)
            {
                value = new List<RestaurantTop>();

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    RestaurantTop rest = new RestaurantTop();
                    if (row["id"] != DBNull.Value) rest.ID = Convert.ToInt32(row["id"]);
                    if (row["displayName"] != DBNull.Value) rest.DisplayName = row["displayName"].ToString();
                    if (row["address"] != DBNull.Value) rest.Address = row["address"].ToString();
                    if (row["town_name"] != DBNull.Value) rest.Town = row["town_name"].ToString();
                    if (row["postal_code"] != DBNull.Value) rest.PostalCode = row["postal_code"].ToString();
                    if (row["telephone"] != DBNull.Value) rest.Telephone = row["telephone"].ToString();
                    value.Add(rest);
                }
            }
            return value;
        }

        /// <summary>
        /// Pobiera statystyki
        /// </summary>
        /// <returns>Obiekt typu Statistics</returns>
        public Statistics GetStatistics()
        {
            DateTime compareUser = DateTime.Now.Subtract(new TimeSpan(0,15,0));
            DateTime compareRestaurant = DateTime.Now.Subtract(new TimeSpan(0,5,0));

            MySqlCommand command = new MySqlCommand(Queries.GetStatistics);
            command.Parameters.AddWithValue("@compareUser",compareUser);
            command.Parameters.AddWithValue("@compareRestaurant",compareRestaurant);
            Statistics value = null;

            DataSet ds = new DataSet();
            ds = ExecuteQuery(command, "GetStatistics");

            if (ds.Tables.Count > 0)
            {
                value = new Statistics();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["produkty"] != DBNull.Value) value.ProductsCount = Convert.ToInt32(row["produkty"]);
                    if (row["restauracje"] != DBNull.Value) value.RestaurantsCount = Convert.ToInt32(row["restauracje"]);
                    if (row["osoby"] != DBNull.Value) value.UsersCount = Convert.ToInt32(row["osoby"]);
                    if (row["klienciAkt"] != DBNull.Value) value.ActiveUsers = Convert.ToInt32(row["klienciAkt"]);
                    if (row["restauAkt"] != DBNull.Value) value.ActiveRestaurants = Convert.ToInt32(row["restauAkt"]);
                }
            }
            return value;
        }

        /// <summary>
        /// Sprawdza czy restauracja jest online
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>True jeśli online</returns>
        public bool IsRestaurantOnline(int id)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;

            try
            {
                MySqlCommand command = new MySqlCommand(Queries.IsRestaurantOnline);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@activity", DateTime.Now.Subtract(new TimeSpan(0, 0, 5, 0)));
                command.Connection = conn;

                conn.Open();

                reader = command.ExecuteReader();
                if (reader.HasRows)
                    return true;
                else
                    return false;

                reader.Close();
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "IsRestaurantOnline" + "\n\n";
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
                wiadomosc += "Action: " + "IsRestaurantOnline" + "\n\n";
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
        }

        /// <summary>
        /// Pobiera liste restauracji spełniających tryteria miasta i nazwy restauracji
        /// </summary>
        /// <param name="town">Nazwa miasta</param>
        /// <param name="res">Nazwa restauracji</param>
        /// <returns>Lista tylu RestaurantInCity</returns>
        public List<RestaurantInCity> GetSearchResult(string town, string res)
        {
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            List<RestaurantInCity> rest = new List<RestaurantInCity>();
            try
            {
                MySqlCommand command = new MySqlCommand(Queries.GetSearchResult);
                if(!String.IsNullOrWhiteSpace(town))
                    command.Parameters.AddWithValue("@town", "%" + town + "%");
                else
                    command.Parameters.AddWithValue("@town", "%");
                if (!String.IsNullOrWhiteSpace(res))
                    command.Parameters.AddWithValue("@res", "%" + res + "%");
                else
                    command.Parameters.AddWithValue("@res", "%");
                command.Parameters.AddWithValue("@isEnabled", true);
                command.Connection = conn;
                conn.Open();

                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        RestaurantInCity r = GetRestaurantsFromCity(reader);
                        rest.Add(r);
                    }
                }
                else
                {
                    return rest;
                }
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetSearchResult" + "\n\n";
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
                wiadomosc += "Action: " + "GetSearchResult" + "\n\n";
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

        /// <summary>
        /// Zwiękasz licznik wejść na strone restauracji
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public bool IncInputsCount(int id)
        {
            MySqlCommand command = new MySqlCommand(Queries.IncInputsCount);
            command.Parameters.AddWithValue("@id", id);

            int rowsaffected = ExecuteNonQuery(command, "IncInputsCount");

            if (rowsaffected > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Ustawia nową date aktywności użytkownika
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public bool SetUserActivity(string login)
        {
            MySqlCommand command = new MySqlCommand(Queries.SetUserActivity);
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@activity", DateTime.Now);

            int rowsaffected = ExecuteNonQuery(command, "SetUserActivity");

            if (rowsaffected > 0)
            {
                return true;
            }

            return false;
        }

        #endregion

        #region Komentarze

        /// <summary>
        /// Pobiera komentarze danej restauracji
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>Lista typu Comment</returns>
        public List<Comment> GetRestaurantComments(int id)
        {
            List<Comment> comments = null;
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;

            try
            {
                comments = new List<Comment>();

                MySqlCommand command = new MySqlCommand(Queries.GetRestaurantComments);
                command.Parameters.AddWithValue("@id", id);
                command.Connection = conn;

                conn.Open();

                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Comment r = GetCommentFromReader(reader);
                    comments.Add(r);
                }
                reader.Close();
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetRestaurantComments" + "\n\n";
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
                wiadomosc += "Action: " + "GetRestaurantComments" + "\n\n";
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

            return comments;
        }

        /// <summary>
        /// Dodaje komenatarz
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="id">Id restauracji</param>
        /// <param name="stars">Ocena</param>
        /// <param name="comment">KOmentarz</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public bool AddComment(string login, int id, double stars, string comment)
        {
            MySqlCommand command = new MySqlCommand(Queries.AddComment);
            command.Parameters.AddWithValue("@date", DateTime.Now);
            command.Parameters.AddWithValue("@comment", comment);
            command.Parameters.AddWithValue("@rating", stars);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@login", login);

            int rowsaffected = ExecuteNonQuery(command, "AddComment");

            if (rowsaffected > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Pobiera komentarze wystawione przez użytkownika
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>Lista typu Comment</returns>
        public List<Comment> GetUserComments(string login)
        {
            List<Comment> comments = null;
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;

            try
            {
                comments = new List<Comment>();

                MySqlCommand command = new MySqlCommand(Queries.GetUserComments);
                command.Parameters.AddWithValue("@login", login);
                command.Connection = conn;

                conn.Open();

                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Comment r = GetCommentFromReader(reader);
                    comments.Add(r);
                }
                reader.Close();
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetUserComments" + "\n\n";
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
                wiadomosc += "Action: " + "GetUserComments" + "\n\n";
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

            return comments;
        }

        /// <summary>
        /// Usuwanie komentarza
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="id">Id restauracji</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public bool DeleteComment(string login, int id)
        {
            MySqlCommand command = new MySqlCommand(Queries.DeleteComment);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@login", login);

            int rowsaffected = ExecuteNonQuery(command, "DeleteComment");

            if (rowsaffected > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Pobiera komentarz i danym id
        /// </summary>
        /// <param name="id">Id komentarza</param>
        /// <returns>Obiekt typu Comment</returns>
        public Comment GetComments(int id)
        {
            Comment comments = null;
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;

            try
            {
                comments = new Comment();

                MySqlCommand command = new MySqlCommand(Queries.GetComments);
                command.Parameters.AddWithValue("@id", id);
                command.Connection = conn;

                conn.Open();

                reader = command.ExecuteReader(CommandBehavior.SingleRow);
                while (reader.Read())
                {
                    comments = GetCommentFromReader(reader);
                }
                reader.Close();
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetComments" + "\n\n";
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
                wiadomosc += "Action: " + "GetComments" + "\n\n";
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

            return comments;
        }

        #endregion
        
        #region Zamówienia
        
        /// <summary>
        /// Zapisuje status zamówienia
        /// </summary>
        /// <param name="id">Id zamówienia</param>
        /// <param name="login">Login użytkownika</param>
        /// <param name="status">Status</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public bool SetOrderStatus(int id, string login, string status)
        {
            if (login.Contains('|'))
            {
                string[] logins = login.Split('|');

                MySqlCommand command = new MySqlCommand(Queries.SetOrderStatus);
                command.Parameters.AddWithValue("@id", id);
                command.Parameters.AddWithValue("@login", logins[0]);
                command.Parameters.AddWithValue("@status", status);
                if (status == "Zakończone")
                {
                    command.Parameters.AddWithValue("@finish", DateTime.Now);
                }
                else
                {
                    command.Parameters.AddWithValue("@finish", null);
                }

                int rowsaffected = ExecuteNonQuery(command, "SetOrderStatus");

                if (rowsaffected > 0)
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
            
            return false;
        }

        /// <summary>
        /// Pobiera dane dotyczące zamówienia potrzebne do PayPala
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <param name="order">Id zamówienia</param>
        /// <returns>string</returns>
        public string GetPayPalData(int id, int order)
        {
            MySqlCommand command = new MySqlCommand(Queries.GetPayPalData);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@order", order);

            string value = String.Empty;
            decimal price = 0.0M;
            decimal delivery = 0.0M;

            DataSet ds = new DataSet();
            ds = ExecuteQuery(command, "GetPayPalData");

            if (ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["totalPrice"] != DBNull.Value)
                    {
                        price = Decimal.Parse(row["totalPrice"].ToString());
                    }
                    else
                    {
                        return null;
                    }

                    if (row["deliveryPrice"] != DBNull.Value)
                    {
                        delivery = Decimal.Parse(row["deliveryPrice"].ToString());
                    }
                    else
                    {
                        return null;
                    }
                    value = (price - delivery).ToString() + "|" + delivery.ToString();
                    break;
                }
            }
            else
            {
                value = null;
            }

            return value;
        }

        /// <summary>
        /// Pobiera aders email restauracji
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>Adres emiali typu string</returns>
        public string GetRestaurantEmail(int id)
        {
            MySqlCommand command = new MySqlCommand(Queries.GetRestaurantEmail);
            command.Parameters.AddWithValue("@id", id);

            string rowsaffected = null;
            rowsaffected = Convert.ToString(ExecuteScalar(command, "GetRestaurantEmail"));

            if (!String.IsNullOrWhiteSpace(rowsaffected))
            {
                return rowsaffected;
            }
            return null;  
        }

        /// <summary>
        /// Pobiera adres email restauracji na podstawie id zamówienia
        /// </summary>
        /// <param name="id">Id zamówienia</param>
        /// <returns>Emili jako string</returns>
        private string GetRestaurantEmailByOrderId(int id)
        {
            MySqlCommand command = new MySqlCommand(Queries.GetRestaurantEmailByOrderId);
            command.Parameters.AddWithValue("@id", id);

            string rowsaffected = null;
            rowsaffected = Convert.ToString(ExecuteScalar(command, "GetRestaurantEmailByOrderId"));

            if (!String.IsNullOrWhiteSpace(rowsaffected))
            {
                return rowsaffected;
            }
            return null;  
        }

        /// <summary>
        /// Pobiera aktywne zamówienia danego użytkownika
        /// </summary>
        /// <param name="login">LOgin użytkownika</param>
        /// <returns>Lista typu UserOrder</returns>
        public List<UserOrder> GetUserActiveOrder(string login)
        {
            List<UserOrder> orders = null;
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            MySqlDataReader reader2 = null;
            //pobiera zamówienia
            try
            {
                orders = new List<UserOrder>();

                MySqlCommand command = new MySqlCommand(Queries.GetUserActiveOrders);
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@date", DateTime.Now.Subtract(new TimeSpan(0, 4, 0, 0)));
                command.Connection = conn;

                conn.Open();

                reader = command.ExecuteReader();
                while(reader.Read())
                {
                    UserOrder r = GetUserOrderFromReader(reader);
                    orders.Add(r);
                }
                reader.Close();
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetUserActiveOrder" + "\n\n";
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
                wiadomosc += "Action: " + "GetUserActiveOrder" + "\n\n";
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
            //pobieranie produktów w zamówieniach
            try
            {
                MySqlCommand command = new MySqlCommand(Queries.GetOrderedProducts);
                command.Connection = conn;

                conn.Open();

                foreach (UserOrder list in orders)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@id", list.OrderId);

                    reader2 = command.ExecuteReader();
                    while (reader2.Read())
                    {
                        OrderedProduct r = GetOrderedProductFromReader(reader2);
                        list.Products.Add(r);
                    }
                    reader2.Close();
                }
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetOrderedProducts" + "\n\n";
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
                wiadomosc += "Action: " + "GetOrderedProducts" + "\n\n";
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

            return orders;
        }

        /// <summary>
        /// Pobiera zamówienia użytkownika z danego okresy czasu
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="from">Data od</param>
        /// <param name="to">Data do</param>
        /// <returns>Lista typu UserOrder</returns>
        public List<UserOrder> GetOrderHistory(string login, DateTime from, DateTime to)
        {
            List<UserOrder> orders = null;
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            MySqlDataReader reader2 = null;
            //pobiera zamówienia
            try
            {
                orders = new List<UserOrder>();

                MySqlCommand command = new MySqlCommand(Queries.GetUserOrderHistory);
                command.Parameters.AddWithValue("@login", login);
                command.Parameters.AddWithValue("@from", from);
                command.Parameters.AddWithValue("@to", to);
                command.Connection = conn;

                conn.Open();

                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    UserOrder r = GetUserOrderFromReader(reader);
                    orders.Add(r);
                }
                reader.Close();
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetOrderHistory" + "\n\n";
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
                wiadomosc += "Action: " + "GetOrderHistory" + "\n\n";
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
            //pobieranie produktów w zamówieniach
            try
            {
                MySqlCommand command = new MySqlCommand(Queries.GetOrderedProducts);
                command.Connection = conn;

                conn.Open();

                foreach (UserOrder list in orders)
                {
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@id", list.OrderId);

                    reader2 = command.ExecuteReader();
                    while (reader2.Read())
                    {
                        OrderedProduct r = GetOrderedProductFromReader(reader2);
                        list.Products.Add(r);
                    }
                    reader2.Close();
                }
            }
            catch (MySqlException e)
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string wiadomosc = message;
                wiadomosc += "Action: " + "GetOrderedProducts" + "\n\n";
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
                wiadomosc += "Action: " + "GetOrderedProducts" + "\n\n";
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

            return orders;
        }

        /// <summary>
        /// Zapisuje zamówienie
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="basket">Dane koszyka</param>
        /// <returns>Id zamówienia</returns>
        public int SaveOrder(string login, BasketRest basket)
        {
            int id = -1;
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlCommand command = new MySqlCommand(Queries.SaveOrder);
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@restaurantId", basket.RestaurantId);
            command.Parameters.AddWithValue("@price", basket.TotalPriceRest);
            command.Parameters.AddWithValue("@comment", basket.Comment);
            command.Parameters.AddWithValue("@date", DateTime.Now);

            MySqlCommand command2 = new MySqlCommand(Queries.AddProductToOrder);

            command.Connection = conn;
            command2.Connection = conn;

            MySqlTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                command.Transaction = tran;
                command2.Transaction = tran;

                id =  Convert.ToInt32(command.ExecuteScalar());

                if (id > 0)
                {
                    foreach (BasketProduct item in basket.Products)
                    {
                        command2.Parameters.Clear();
                        command2.Parameters.AddWithValue("@orderId", id);
                        command2.Parameters.AddWithValue("@productId", item.ProductId);
                        command2.Parameters.AddWithValue("@priceOption", item.PriceOption);
                        command2.Parameters.AddWithValue("@count", item.Count);
                        command2.Parameters.AddWithValue("@priceXcount", item.TotalPriceProd);
                        command2.Parameters.AddWithValue("@nonPriceOption", item.NonPriceOption);
                        command2.Parameters.AddWithValue("@nonPriceOption2", item.NonPriceOption2);
                        command2.Parameters.AddWithValue("@comment", item.Comment);

                        int rowsaffected = 0;
                        rowsaffected = command2.ExecuteNonQuery();
                        if (rowsaffected < 1)
                        {
                            tran.Rollback();
                            return -1;
                        }
                    }
                    
                }
                else
                {
                    tran.Rollback();
                    return -1;
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
                wiadomosc += "Action: " + "SaveOrder" + "\n\n";
                wiadomosc += "Exception: " + e.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                return -1;
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
                wiadomosc += "Action: " + "SaveOrder" + "\n\n";
                wiadomosc += "Exception: " + ex.ToString();

                log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                return -1;
            }
            finally
            {
                conn.Close();
            }
            return id;
        }

        /// <summary>
        /// Realizuj zamówienie
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="id">Id zamówienia</param>
        /// <param name="comment">KOmentarz do zamówienia</param>
        /// <param name="payment">Typ płatności</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public bool Pay(string login, int id, string comment, string payment)
        {
            MySqlCommand command = new MySqlCommand(Queries.Pay);
            command.Parameters.AddWithValue("@status", "Oczekujące");
            command.Parameters.AddWithValue("@comment", comment);
            command.Parameters.AddWithValue("@orderDate", DateTime.Now);
            command.Parameters.AddWithValue("@payment", payment);
            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@login", login);

            int rowsaffected = ExecuteNonQuery(command, "Pay");

            if (rowsaffected > 0)
            {
                //wyślij eamil
                string email = GetRestaurantEmailByOrderId(id);
                Order order = GetOrder(id);

                Email em = new Email();
                bool value = em.SendOrder(email, order);

                return true;
            }
            return false;  
        }

        /// <summary>
        /// Pobiera zamówienia dla restauracji
        /// </summary>
        /// <param name="login">Login zalogowanego pracownika</param>
        /// <returns>Obiekt typu AllOrders</returns>
        public AllOrders GetOrders(string login)
        {
            AllOrders allOrders = null;
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            MySqlDataReader reader2 = null;
            MySqlDataReader reader3 = null;
            MySqlDataReader reader4 = null;
            MySqlDataReader reader5 = null;
            MySqlDataReader reader6 = null;

            if (login.Contains('|'))
            {
                string[] logins = login.Split('|');

                allOrders = new AllOrders();

                try
                {
                    MySqlCommand command = new MySqlCommand(Queries.GetWaitingOrders);
                    command.Parameters.AddWithValue("@login", logins[0]);
                    command.Connection = conn;

                    MySqlCommand command2 = new MySqlCommand(Queries.GetActiveOrders);
                    command2.Parameters.AddWithValue("@login", logins[0]);
                    command2.Connection = conn;

                    MySqlCommand command3 = new MySqlCommand(Queries.GetFinishOrders);
                    command3.Parameters.AddWithValue("@login", logins[0]);
                    command3.Parameters.AddWithValue("@date", DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)));
                    command3.Connection = conn;

                    conn.Open();

                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Order r = GetOrderFromReader(reader);
                        allOrders.Waiting.Add(r);
                    }
                    reader.Close();

                    reader2 = command2.ExecuteReader();
                    while (reader2.Read())
                    {
                        Order r = GetOrderFromReader(reader2);
                        allOrders.Active.Add(r);
                    }
                    reader2.Close();

                    reader3 = command3.ExecuteReader();
                    while (reader3.Read())
                    {
                        Order r = GetOrderFromReader(reader3);
                        allOrders.Finish.Add(r);
                    }
                    reader3.Close();
                }
                catch (MySqlException e)
                {
                    EventLog log = new EventLog();
                    log.Source = eventSource;
                    log.Log = eventLog;

                    string wiadomosc = message;
                    wiadomosc += "Action: " + "GetOrders" + "\n\n";
                    wiadomosc += "Exception: " + e.ToString();

                    log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                    if (reader != null) { reader.Close(); }
                    if (reader2 != null) { reader2.Close(); }
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
                    wiadomosc += "Action: " + "GetOrders" + "\n\n";
                    wiadomosc += "Exception: " + ex.ToString();

                    log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                    if (reader != null) { reader.Close(); }
                    if (reader2 != null) { reader2.Close(); }
                    if (reader3 != null) { reader3.Close(); }
                    conn.Close();
                    return null;
                }
                finally
                {
                    if (reader != null) { reader.Close(); }
                    if (reader2 != null) { reader2.Close(); }
                    if (reader3 != null) { reader3.Close(); }
                    conn.Close();
                }
///////////////////////////////////////////////////////////////////////
                try
                {
                    MySqlCommand command = new MySqlCommand(Queries.GetOrderedProducts);
                    command.Connection = conn;

                    conn.Open();

                    foreach(Order list in allOrders.Active)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@id", list.OrderId);

                        reader4 = command.ExecuteReader();
                        while (reader4.Read())
                        {
                            OrderedProduct r = GetOrderedProductFromReader(reader4);
                            list.Products.Add(r);
                        }
                        reader4.Close();
                    }

                    foreach (Order list in allOrders.Finish)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@id", list.OrderId);

                        reader5 = command.ExecuteReader();
                        while (reader5.Read())
                        {
                            OrderedProduct r = GetOrderedProductFromReader(reader5);
                            list.Products.Add(r);
                        }
                        reader5.Close();
                    }

                    foreach (Order list in allOrders.Waiting)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@id", list.OrderId);

                        reader6 = command.ExecuteReader();
                        while (reader6.Read())
                        {
                            OrderedProduct r = GetOrderedProductFromReader(reader6);
                            list.Products.Add(r);
                        }
                        reader6.Close();
                    }
                }
                catch (MySqlException e)
                {
                    EventLog log = new EventLog();
                    log.Source = eventSource;
                    log.Log = eventLog;

                    string wiadomosc = message;
                    wiadomosc += "Action: " + "GetOrderedProducts" + "\n\n";
                    wiadomosc += "Exception: " + e.ToString();

                    log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                    if (reader4 != null) { reader4.Close(); }
                    if (reader5 != null) { reader5.Close(); }
                    if (reader6 != null) { reader6.Close(); }
                    conn.Close();
                    return null;

                }
                catch (Exception ex)
                {
                    EventLog log = new EventLog();
                    log.Source = eventSource;
                    log.Log = eventLog;

                    string wiadomosc = message2;
                    wiadomosc += "Action: " + "GetOrderedProducts" + "\n\n";
                    wiadomosc += "Exception: " + ex.ToString();

                    log.WriteEntry(wiadomosc, EventLogEntryType.Error);

                    if (reader4 != null) { reader4.Close(); }
                    if (reader5 != null) { reader5.Close(); }
                    if (reader6 != null) { reader6.Close(); }
                    conn.Close();
                    return null;
                }
                finally
                {
                    if (reader4 != null) { reader4.Close(); }
                    if (reader5 != null) { reader5.Close(); }
                    if (reader6 != null) { reader6.Close(); }
                    conn.Close();
                }
            }
            else
            {
                return null;
            }

            return allOrders;
        }

        /// <summary>
        /// Pobiera zamówienia dla restauracji z określonego czasu
        /// </summary>
        /// <param name="login">Login zalogowanego pracownika</param>
        /// <param name="from">Data od</param>
        /// <param name="to">Data do</param>
        /// <returns>Lista typu Order</returns>
        public List<Order> GetAllOrders(string login, DateTime from, DateTime to)
        {
            List<Order> allOrders = null;
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            MySqlDataReader reader2 = null;

            if (login.Contains('|'))
            {
                string[] logins = login.Split('|');

                allOrders = new List<Order>();

                try
                {
                    MySqlCommand command = new MySqlCommand(Queries.GetAllOrders);
                    command.Parameters.AddWithValue("@login", logins[0]);
                    command.Parameters.AddWithValue("@from", from);
                    command.Parameters.AddWithValue("@to", to);
                    command.Connection = conn;

                    conn.Open();

                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Order r = GetOrderFromReader(reader);
                        allOrders.Add(r);
                    }
                    reader.Close();
                }
                catch (MySqlException e)
                {
                    EventLog log = new EventLog();
                    log.Source = eventSource;
                    log.Log = eventLog;

                    string wiadomosc = message;
                    wiadomosc += "Action: " + "GetAllOrders" + "\n\n";
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
                    wiadomosc += "Action: " + "GetAllOrders" + "\n\n";
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
                ///////////////////////////////////////////////////////////////////////
                try
                {
                    MySqlCommand command = new MySqlCommand(Queries.GetOrderedProducts);
                    command.Connection = conn;

                    conn.Open();

                    foreach (Order list in allOrders)
                    {
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@id", list.OrderId);

                        reader2 = command.ExecuteReader();
                        while (reader2.Read())
                        {
                            OrderedProduct r = GetOrderedProductFromReader(reader2);
                            list.Products.Add(r);
                        }
                        reader2.Close();
                    }
                }
                catch (MySqlException e)
                {
                    EventLog log = new EventLog();
                    log.Source = eventSource;
                    log.Log = eventLog;

                    string wiadomosc = message;
                    wiadomosc += "Action: " + "GetOrderedProducts" + "\n\n";
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
                    wiadomosc += "Action: " + "GetOrderedProducts" + "\n\n";
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
            }
            else
            {
                return null;
            }

            return allOrders;
        }

        /// <summary>
        /// Pobiera zamówienie o danym id
        /// </summary>
        /// <param name="id">Id zamówienia</param>
        /// <returns>Obiekt typu Order</returns>
        private Order GetOrder(int id)
        {
            Order order = null;
            MySqlConnection conn = new MySqlConnection(ConnectionString);
            MySqlDataReader reader = null;
            MySqlDataReader reader2 = null;

            order = new Order();

                try
                {
                    MySqlCommand command = new MySqlCommand(Queries.GetOrder);
                    command.Parameters.AddWithValue("@id", id);
                    command.Connection = conn;

                    conn.Open();

                    reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        order = GetOrderFromReader(reader);
                        //allOrders.Add(r);
                    }
                    reader.Close();
                }
                catch (MySqlException e)
                {
                    EventLog log = new EventLog();
                    log.Source = eventSource;
                    log.Log = eventLog;

                    string wiadomosc = message;
                    wiadomosc += "Action: " + "GetOrder" + "\n\n";
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
                    wiadomosc += "Action: " + "GetOrder" + "\n\n";
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
                ///////////////////////////////////////////////////////////////////////
                try
                {
                    MySqlCommand command = new MySqlCommand(Queries.GetOrderedProducts);
                    command.Connection = conn;

                    conn.Open();

                  //  foreach (Order list in allOrders)
                  //  {
                //        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@id", id);

                        reader2 = command.ExecuteReader();
                        while (reader2.Read())
                        {
                            OrderedProduct r = GetOrderedProductFromReader(reader2);
                            order.Products.Add(r);
                        }
                        reader2.Close();
                 //   }
                }
                catch (MySqlException e)
                {
                    EventLog log = new EventLog();
                    log.Source = eventSource;
                    log.Log = eventLog;

                    string wiadomosc = message;
                    wiadomosc += "Action: " + "GetOrderedProducts" + "\n\n";
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
                    wiadomosc += "Action: " + "GetOrderedProducts" + "\n\n";
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
        

            return order;
        }

        #endregion

        #region POS

        /// <summary>
        /// Pobiera liste pracowników z danej restauracji
        /// </summary>
        /// <param name="login">Login restauracji</param>
        /// <returns>Lista typu string</returns>
        public List<string> GetEmployeesInRestaurant(string login)
        {
            MySqlCommand command = new MySqlCommand(Queries.GetEmployeesInRestaurant);
            command.Parameters.AddWithValue("@login", login);
            command.Parameters.AddWithValue("@isLockedOut", false);

            List<string> value = null;

            DataSet ds = new DataSet();
            ds = ExecuteQuery(command, "GetEmployeesInRestaurant");

            if (ds.Tables.Count > 0)
            {
                value = new List<string>();
                
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    String log = String.Empty;
                    if (row["login"] != DBNull.Value)
                    {
                        log = row["login"].ToString();
                        value.Add(log);
                    }
                }
            }
            return value;
        }

        /// <summary>
        /// Ustawia status online restauracji
        /// </summary>
        /// <param name="login">Login zalogowanego pracownika</param>
        /// <param name="online">Status online lub offline</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public bool SetRestaurantOnline(string login, string online)
        {
            if (login.Contains('|'))
            {
                string[] logins = login.Split('|');

                MySqlCommand command = new MySqlCommand(Queries.SetRestaurantOnline);
                command.Parameters.AddWithValue("@login", logins[0]);
                if (online == "Online")
                {
                    command.Parameters.AddWithValue("@online", true);
                }
                else
                {
                    command.Parameters.AddWithValue("@online", false);
                }

                int rowsaffected = ExecuteNonQuery(command, "SetRestaurantOnline");

                if (rowsaffected > 0)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Pobiera status online restauracji
        /// </summary>
        /// <param name="login">Login zalogowanego pracownika</param>
        /// <returns>True jeśli restauracja jest online</returns>
        public bool RestaurantOnlineStatus(string login)
        {
            if (login.Contains('|'))
            {
                string[] logins = login.Split('|');

                MySqlCommand command = new MySqlCommand(Queries.RestaurantOnlineStatus);
                command.Parameters.AddWithValue("@login", logins[0]);

                DataSet ds = new DataSet();
                ds = ExecuteQuery(command, "RestaurantOnlineStatus");

                if (ds.Tables.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    if (row["isOnLine"] != DBNull.Value)
                    {
                        bool value = Convert.ToBoolean((row["isOnLine"]));
                        if (value == true) return true;
                    }
                }

                else
                {
                    return false;
                }

            }
            return false;
        }

        /// <summary>
        /// Zapisuje aktywność restauracji
        /// </summary>
        /// <param name="login">Login zalogowanego pracownika lub restauracji</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public bool SetRestaurantActivity(string login)
        {
            MySqlCommand command = new MySqlCommand(Queries.SetRestaurantActivity);
            if (login.Contains('|'))
            {
                string[] logins = login.Split('|');
                command.Parameters.AddWithValue("@login", logins[0]);
            }
            else
            {
                command.Parameters.AddWithValue("@login", login);
            }
            command.Parameters.AddWithValue("@activity", DateTime.Now);

            int rowsaffected = ExecuteNonQuery(command, "SetRestaurantActivity");

            if (rowsaffected > 0)
            {
                return true;
            }

            return false;
        }

        #endregion
    }
}
