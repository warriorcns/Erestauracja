﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Contract
{
    [ServiceContract]
    public interface IEresService
    {
        #region membership

        [OperationContract]
        bool ChangePassword(string login, string password);

        [OperationContract]
        bool ChangePasswordQuestionAndAnswer(string login, string newPwdQuestion, string newPwdAnswer);

        [OperationContract]
        PasswordAndAnswer GetPassword(string login);

        [OperationContract]
        PasswordAnswer GetPasswordAnswer(string login);

        [OperationContract]
        bool ResetPassword(string login, string password);

        [OperationContract]
        PasswordQuestion GetUserQuestion(string login);

        [OperationContract]
        bool CreateUser(string login, string password, string email, string name, string surname, string address, int townID, string country, DateTime birthdate, string sex, string telephone, string passwordQuestion, string passwordAnswer, bool isApproved);

        [OperationContract]
        bool DeleteUser(string login, bool deleteAllRelatedData);

        [OperationContract]
        List<User> GetAllUsers(int pageIndex, int pageSize, out int totalRecords);

        [OperationContract]
        int GetNumberOfUsersOnline(TimeSpan onlineSpan);

        [OperationContract]
        User GetUser(string login, bool userIsOnline);

        [OperationContract]
        User GetUserById(int id, bool userIsOnline);

        [OperationContract]
        bool UnlockUser(string login);

        [OperationContract]
        string GetUserNameByEmail(string email);

        [OperationContract]
        bool UpdateUser(User user);

        [OperationContract]
        ValidateUser ValidateUser(string login);

        [OperationContract]
        ValidateUser ValidateEmployee(string login, string rest);

        [OperationContract]
        bool UpdateUserLoginDate(string login);

        [OperationContract]
        bool UpdateEmployeeLoginDate(string login, string rest);

        [OperationContract]
        bool UpdateFailureCount(string login, string failureType, int PasswordAttemptWindow, int MaxInvalidPasswordAttempts);

        #endregion

        #region role

        [OperationContract]
        bool AddUsersToRoles(string[] logins, string[]rolenames);

        [OperationContract]
        bool CreateRole(string rolename);

        [OperationContract]
        bool DeleteRole(string rolename);

        [OperationContract]
        string GetAllRoles();

        [OperationContract]
        string GetRolesForUser(string login);

        [OperationContract]
        string GetUsersInRole(string rolename);

        [OperationContract]
        bool IsUserInRole(string login, string rolename);

        [OperationContract]
        bool RemoveUsersFromRoles(string[] logins, string[] rolenames);

        [OperationContract]
        bool RoleExists(string rolename);

        [OperationContract]
        string FindUsersInRole(string rolename, string loginToMatch);

        #endregion

        #region manage restaurant

        [OperationContract]
        bool AddRestaurant(string login, string email, string password, string passwordQuestion, string passwordAnswer, string name, string displayName, string address, int townID, string country, string telephone, string nip, string regon, string deliveryTime, string managerLogin);

        [OperationContract]
        bool EditRestaurant(string name, string displayName, string address, int townId, string country, string telephone, string nip, string regon, string deliveryTime, bool isEnabled, string managerLogin, int id);

        [OperationContract]
        List<Restaurant> GetRestaurantsByManagerLogin(string managerLogin);

        [OperationContract]
        RestaurantInfo GetRestaurant(string managerLogin, int id);

        [OperationContract]
        MainPageContent GetMainPage(string managerLogin, int id);

        [OperationContract]
        bool EditMainPage(string description, string foto, string specialOffers, int id, string managerLogin);

        [OperationContract]
        DeliveryPageContent GetDeliveryPage(string managerLogin, int id);

        [OperationContract]
        bool EditDeliveryPage(string delivery, int id, string managerLogin);

        [OperationContract]
        EventsPageContent GetEventsPage(string managerLogin, int id);

        [OperationContract]
        bool EditEventsPage(string events, int id, string managerLogin);

        [OperationContract]
        ContactPageContent GetContactPage(string managerLogin, int id);

        [OperationContract]
        bool EditContactPage(string contact, int id, string managerLogin);

        [OperationContract]
        bool AddCategory(int restaurantID, string categoryName, string categoryDescription, string priceOption, string nonPriceOption, string nonPriceOption2, string managerLogin);

        [OperationContract]
        List<Category> GetCategories(string managerLogin, int restaurantID);

        [OperationContract]
        Category GetCategory(string managerLogin, int restaurantID, int categoryID);

        [OperationContract]
        bool EditCategory(string managerLogin, int restaurantID, int categoryID, string categoryName, string categoryDescription, string priceOption, string nonPriceOption, string nonPriceOption2);
        
        [OperationContract]
        bool DeleteCategory(string managerLogin, int restaurantID, int categoryID);

        [OperationContract]
        bool AddProduct(int restaurantID, int categoryID, string productName, string productDescription, string price, string managerLogin);

        [OperationContract]
        List<Menu> GetMenuManager(string managerLogin, int restaurantID);

        [OperationContract]
        Product GetProduct(string managerLogin, int restaurantID, int productID);

        [OperationContract]
        bool EditProduct(string managerLogin, int restaurantID, int id, int categoryID, string productName, string productDescription, string price, bool isAvailable);

        [OperationContract]
        List<Presonnel> GetPersonnel(string managerLogin);

        [OperationContract]
        bool AddUserToRestaurant(int userId, int restaurantId);

        #endregion

        #region ogólne

        [OperationContract]
        List<string> GetCountriesList();

        [OperationContract]
        List<Town> GetTowns(string townName, string postalCode, out string status);

        [OperationContract]
        List<RestaurantInTown> GetRestaurantByTown(string townName);

        [OperationContract]
        List<Menu> GetMenu(int restaurantID);

        [OperationContract]
        MainPageContent GetMainPageUser(int id);

        [OperationContract]
        DeliveryPageContent GetDeliveryPageUser(int id);

        [OperationContract]
        EventsPageContent GetEventsPageUser(int id);

        [OperationContract]
        ContactPageContent GetContactPageUser(int id);

        [OperationContract]
        bool SendError(string email, string text);

        [OperationContract]
        RestaurantsFromCity RestaurantsFromCity(string cityName);

        [OperationContract]
        List<RestaurantTop> GetTopRestaurant();

        [OperationContract]
        Statistics GetStatistics();

        [OperationContract]
        List<string> GetEmployeesInRestaurant(string login);

        #endregion
    }


    #region membership dataContract

    [DataContract]
    public class PasswordAndAnswer
    {
        private string password = null;
        private string passwordAnswer = null;
        private bool isLockedOut = false;

        [DataMember]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        [DataMember]
        public string PasswordAnswer
        {
            get { return passwordAnswer; }
            set { passwordAnswer = value; }
        }

        [DataMember]
        public bool IsLockedOut
        {
            get { return isLockedOut; }
            set { isLockedOut = value; }
        }
    }

    [DataContract]
    public class PasswordAnswer
    {
        private string passwordAnswer = null;
        private bool isLockedOut = false;

        [DataMember]
        public string Answer
        {
            get { return passwordAnswer; }
            set { passwordAnswer = value; }
        }

        [DataMember]
        public bool IsLockedOut
        {
            get { return isLockedOut; }
            set { isLockedOut = value; }
        }
    }

    [DataContract]
    public class PasswordQuestion
    {
        private string passwordQuestion = null;
        private bool isLockedOut = false;

        [DataMember]
        public string Question
        {
            get { return passwordQuestion; }
            set { passwordQuestion = value; }
        }

        [DataMember]
        public bool IsLockedOut
        {
            get { return isLockedOut; }
            set { isLockedOut = value; }
        }
    }

    [DataContract]
    public class User
    {
        private int id = -1;
        private string login = null;
        private string email = null;
        private string name = null;
        private string surname = null;
        private string address = null;
        private string town = null;
        private string postalCode = null;
        private string country = null;
        private DateTime birthdate = new DateTime();
        private string sex = null;
        private string telephone = null;
        private string comment = null;
        private string passwordQuestion = null;
        private bool isApproved = false;
        private DateTime lastActivityDate = new DateTime();
        private DateTime lastLoginDate = new DateTime();
        private DateTime lastPasswordChangedDate = new DateTime();
        private DateTime creationDate = new DateTime();
        private bool isLockedOut = false;
        private DateTime lastLockedOutDate = new DateTime();

        [DataMember]
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public string Login
        {
            get { return login; }
            set { login = value; }
        }

        [DataMember]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        public string Surname
        {
            get { return surname; }
            set { surname = value; }
        }

        [DataMember]
        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        [DataMember]
        public string Town
        {
            get { return town; }
            set { town = value; }
        }

        [DataMember]
        public string PostalCode
        {
            get { return postalCode; }
            set { postalCode = value; }
        }

        [DataMember]
        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        [DataMember]
        public DateTime Birthdate
        {
            get { return birthdate; }
            set { birthdate = value; }
        }

        [DataMember]
        public string Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        [DataMember]
        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }

        [DataMember]
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        [DataMember]
        public string PasswordQuestion
        {
            get { return passwordQuestion; }
            set { passwordQuestion = value; }
        }

        [DataMember]
        public bool IsApproved
        {
            get { return isApproved; }
            set { isApproved = value; }
        }

        [DataMember]
        public DateTime LastActivityDate
        {
            get { return lastActivityDate; }
            set { lastActivityDate = value; }
        }

        [DataMember]
        public DateTime LastLoginDate
        {
            get { return lastLoginDate; }
            set { lastLoginDate = value; }
        }

        [DataMember]
        public DateTime LastPasswordChangedDate
        {
            get { return lastPasswordChangedDate; }
            set { lastPasswordChangedDate = value; }
        }

        [DataMember]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        [DataMember]
        public bool IsLockedOut
        {
            get { return isLockedOut; }
            set { isLockedOut = value; }
        }

        [DataMember]
        public DateTime LastLockedOutDate
        {
            get { return lastLockedOutDate; }
            set { lastLockedOutDate = value; }
        }
    }

    [DataContract]
    public class ValidateUser
    {
        private string password = null;
        private bool isApproved = false;

        [DataMember]
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        [DataMember]
        public bool IsApproved
        {
            get { return isApproved; }
            set { isApproved = value; }
        }
    }

    #endregion

    #region restaurant dataContract

    [DataContract]
    public class Restaurant
    { 
        private int id = -1;
        private string name = null;
        private string displayName = null;
        private string address = null;
        private string town = null;
        private string postalCode = null;
        private string country = null;
        private string telephone = null;
        private string nip = null;
        private string regon = null;
        private int inputsCount = -1;
        private int averageRating = -1;
        private int menagerId = -1;
        private string deliveryTime = null;
        private int userId = -1;
        private bool isEnabled = false;
        private string login = null;
        private string email = null;
        private bool isApproved = false;
        private DateTime lastActivityDate = new DateTime();
        private DateTime creationDate = new DateTime();
        private bool isLockedOut = false;
        private DateTime lastLockedOutDate = new DateTime();

        [DataMember]
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        [DataMember]
        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        [DataMember]
        public string Town
        {
            get { return town; }
            set { town = value; }
        }

        [DataMember]
        public string PostalCode
        {
            get { return postalCode; }
            set { postalCode = value; }
        }

        [DataMember]
        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        [DataMember]
        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }

        [DataMember]
        public string Nip
        {
            get { return nip; }
            set { nip = value; }
        }

        [DataMember]
        public string Regon
        {
            get { return regon; }
            set { regon = value; }
        }

        [DataMember]
        public int InputsCount
        {
            get { return inputsCount; }
            set { inputsCount = value; }
        }

        [DataMember]
        public int AverageRating
        {
            get { return averageRating; }
            set { averageRating = value; }
        }

        [DataMember]
        public int MenagerId
        {
            get { return menagerId; }
            set { menagerId = value; }
        }

        [DataMember]
        public string DeliveryTime
        {
            get { return deliveryTime; }
            set { deliveryTime = value; }
        }

        [DataMember]
        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        [DataMember]
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        [DataMember]
        public string Login
        {
            get { return login; }
            set { login = value; }
        }

        [DataMember]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        
        [DataMember]
        public bool IsApproved
        {
            get { return isApproved; }
            set { isApproved = value; }
        }

        [DataMember]
        public DateTime LastActivityDate
        {
            get { return lastActivityDate; }
            set { lastActivityDate = value; }
        }

        [DataMember]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        [DataMember]
        public bool IsLockedOut
        {
            get { return isLockedOut; }
            set { isLockedOut = value; }
        }

        [DataMember]
        public DateTime LastLockedOutDate
        {
            get { return lastLockedOutDate; }
            set { lastLockedOutDate = value; }
        }
    }

    [DataContract]
    public class RestaurantInfo
    {
        private int id = -1;
        private string name = null;
        private string displayName = null;
        private string address = null;
        private string town = null;
        private string postalCode = null;
        private string country = null;
        private string telephone = null;
        private string nip = null;
        private string regon = null;
        private string deliveryTime = null;
        private bool isEnabled = false;

        [DataMember]
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        [DataMember]
        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        [DataMember]
        public string Town
        {
            get { return town; }
            set { town = value; }
        }

        [DataMember]
        public string PostalCode
        {
            get { return postalCode; }
            set { postalCode = value; }
        }

        [DataMember]
        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        [DataMember]
        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }

        [DataMember]
        public string Nip
        {
            get { return nip; }
            set { nip = value; }
        }

        [DataMember]
        public string Regon
        {
            get { return regon; }
            set { regon = value; }
        }

        [DataMember]
        public string DeliveryTime
        {
            get { return deliveryTime; }
            set { deliveryTime = value; }
        }
        
        [DataMember]
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }
    }

    [DataContract]
    public class MainPageContent
    {
        private string description = null;
        private string foto = null;
        private string specialOffers = null;

        [DataMember]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        [DataMember]
        public string Foto
        {
            get { return foto; }
            set { foto = value; }
        }

        [DataMember]
        public string SpecialOffers
        {
            get { return specialOffers; }
            set { specialOffers = value; }
        }
    }
    
    [DataContract]
    public class DeliveryPageContent
    {
        private string delivery = null;

        [DataMember]
        public string Delivery
        {
            get { return delivery; }
            set { delivery = value; }
        }
    }

    [DataContract]
    public class EventsPageContent
    {
        private string events = null;

        [DataMember]
        public string Events
        {
            get { return events; }
            set { events = value; }
        }
    }

    [DataContract]
    public class ContactPageContent
    {
        private string contact = null;

        [DataMember]
        public string Contact
        {
            get { return contact; }
            set { contact = value; }
        }
    }

    [DataContract]
    public class Category
    {
        private int restaurantID = -1;
        private int categoryID = -1;
        private string categoryName = null;
        private string categoryDescription = null;
        private string priceOption = null;
        private string nonPriceOption = null;
        private string nonPriceOption2 = null;

        [DataMember]
        public int RestaurantID
        {
            get { return restaurantID; }
            set { restaurantID = value; }
        }

        [DataMember]
        public int CategoryID
        {
            get { return categoryID; }
            set { categoryID = value; }
        }

        [DataMember]
        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }

        [DataMember]
        public string CategoryDescription
        {
            get { return categoryDescription; }
            set { categoryDescription = value; }
        }

        [DataMember]
        public string PriceOption
        {
            get { return priceOption; }
            set { priceOption = value; }
        }

        [DataMember]
        public string NonPriceOption
        {
            get { return nonPriceOption; }
            set { nonPriceOption = value; }
        }

        [DataMember]
        public string NonPriceOption2
        {
            get { return nonPriceOption2; }
            set { nonPriceOption2 = value; }
        }
    }    
        
    [DataContract]
    public class Menu
    {
        private int restaurantID = -1;
        private int categoryID = -1;
        private string categoryName = null;
        private string categoryDescription = null;
        private string priceOption = null;
        private string nonPriceOption = null;
        private string nonPriceOption2 = null;
        private List<Product> products = null;

        [DataMember]
        public int RestaurantID
        {
            get { return restaurantID; }
            set { restaurantID = value; }
        }

        [DataMember]
        public int CategoryID
        {
            get { return categoryID; }
            set { categoryID = value; }
        }

        [DataMember]
        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }

        [DataMember]
        public string CategoryDescription
        {
            get { return categoryDescription; }
            set { categoryDescription = value; }
        }

        [DataMember]
        public string PriceOption
        {
            get { return priceOption; }
            set { priceOption = value; }
        }

        [DataMember]
        public string NonPriceOption
        {
            get { return nonPriceOption; }
            set { nonPriceOption = value; }
        }

        [DataMember]
        public string NonPriceOption2
        {
            get { return nonPriceOption2; }
            set { nonPriceOption2 = value; }
        }

        [DataMember]
        public List<Product> Products
        {
            get { return products; }
            set { products = value; }
        }
    }

    [DataContract]
    public class Product
    {
        private int productId = -1;
        private int restaurantId = -1;
        private int categoryId = -1;
        private string productName = null;
        private string productDescription = null;
        private string price = null;
        private string priceOption = null;
        private DateTime creationDate = new DateTime();
        private bool isAvailable = false;
        private bool isEnabled = false;

        [DataMember]
        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        [DataMember]
        public int RestaurantId
        {
            get { return restaurantId; }
            set { restaurantId = value; }
        }

        [DataMember]
        public int CategoryId
        {
            get { return categoryId; }
            set { categoryId = value; }
        }

        [DataMember]
        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        [DataMember]
        public string ProductDescription
        {
            get { return productDescription; }
            set { productDescription = value; }
        }

        [DataMember]
        public string Price
        {
            get { return price; }
            set { price = value; }
        }

        [DataMember]
        public string PriceOption
        {
            get { return priceOption; }
            set { priceOption = value; }
        }

        [DataMember]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        [DataMember]
        public bool IsAvailable
        {
            get { return isAvailable; }
            set { isAvailable = value; }
        }

        [DataMember]
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }
    }

    [DataContract]
    public class Presonnel
    {
        private int restaurantId = -1;
        private string restaurantName = null;
        private string restaurantAddress = null;
        private string restaurantTown = null;
        private List<User> employees = new List<User>();

        [DataMember]
        public int RestaurantId
        {
            get { return restaurantId; }
            set { restaurantId = value; }
        }
        [DataMember]
        public string RestaurantName
        {
            get { return restaurantName; }
            set { restaurantName = value; }
        }
        [DataMember]
        public string RestaurantAddress
        {
            get { return restaurantAddress; }
            set { restaurantAddress = value; }
        }
        [DataMember]
        public string RestaurantTown
        {
            get { return restaurantTown; }
            set { restaurantTown = value; }
        }
        [DataMember]
        public List<User> Employees
        {
            get { return employees; }
            set { employees = value; }
        }
    }

    #endregion

    [DataContract]
    public class Town
    {
        private int id = -1;
        private string postalCode = null;
        private string townName = null;
        private string province = null;
        private string district = null;
        private string community = null;
        private double latitude = 0;
        private double longtitude = 0;
        private string infoWindowContent = "";

        [DataMember]
        public string InfoWindowContent
        {
            get { return infoWindowContent; }
            set { infoWindowContent = value; }
        }

        [DataMember]
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public string PostalCode
        {
            get { return postalCode; }
            set { postalCode = value; }
        }

        [DataMember]
        public string TownName
        {
            get { return townName; }
            set { townName = value; }
        }

        [DataMember]
        public string Province
        {
            get { return province; }
            set { province = value; }
        }

        [DataMember]
        public string District
        {
            get { return district; }
            set { district = value; }
        }

        [DataMember]
        public string Community
        {
            get { return community; }
            set { community = value; }
        }

        [DataMember]
        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        [DataMember]
        public double Longtitude
        {
            get { return longtitude; }
            set { longtitude = value; }
        }
    }

    [DataContract]
    public class RestaurantInTown
    {
        private int resId = -1;
        private string name = null;
        private int townId = -1;

        [DataMember]
        public int ResId
        {
            get { return resId; }
            set { resId = value; }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [DataMember]
        public int TownId
        {
            get { return townId; }
            set { townId = value; }
        }
    }

    [DataContract]
    public class RestaurantsFromCity
    {
        private string cityName = null;
        private List<RestaurantInCity> restaurants = new List<RestaurantInCity>();

        [DataMember]
        public string CityName
        {
            get { return cityName; }
            set { cityName = value; }
        }

        [DataMember]
        public List<RestaurantInCity> Restaurants
        {
            get { return restaurants; }
            set { restaurants = value; }
        }
    }

    [DataContract]
    public class RestaurantInCity
    {
        private int id = -1;
        private string displayName = null;
        private string address = null;
        private string town = null;
        private string postalCode = null;
        private string country = null;
        private string telephone = null;
        private int inputsCount = -1;
        private int averageRating = -1;
        private string deliveryTime = null;
        private DateTime creationDate = new DateTime();
        private double latitude = 0;
        private double longtitude = 0;
        private string infoWindowContent = String.Empty;
       

        [DataMember]
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        [DataMember]
        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        [DataMember]
        public string Town
        {
            get { return town; }
            set { town = value; }
        }

        [DataMember]
        public string PostalCode
        {
            get { return postalCode; }
            set { postalCode = value; }
        }

        [DataMember]
        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        [DataMember]
        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }

        [DataMember]
        public int InputsCount
        {
            get { return inputsCount; }
            set { inputsCount = value; }
        }

        [DataMember]
        public int AverageRating
        {
            get { return averageRating; }
            set { averageRating = value; }
        }

        [DataMember]
        public string DeliveryTime
        {
            get { return deliveryTime; }
            set { deliveryTime = value; }
        }

        [DataMember]
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
        }

        [DataMember]
        public double Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        [DataMember]
        public double Longtitude
        {
            get { return longtitude; }
            set { longtitude = value; }
        }

        [DataMember]
        public string InfoWindowContent
        {
            get { return infoWindowContent; }
            set { infoWindowContent = value; }
        }
    }

    [DataContract]
    public class RestaurantTop
    {
        private int id = -1;
        private string displayName = null;
        private string address = null;
        private string town = null;
        private string postalCode = null;
        private string telephone = null;

        [DataMember]
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        [DataMember]
        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        [DataMember]
        public string Town
        {
            get { return town; }
            set { town = value; }
        }

        [DataMember]
        public string PostalCode
        {
            get { return postalCode; }
            set { postalCode = value; }
        }

        [DataMember]
        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }
    }

    [DataContract]
    public class Statistics
    {
        private int productsCount = -1;
        private int restaurantsCount = -1;
        private int usersCount = -1;
        private int activeUsers = -1;
        private int activeRestaurants = -1;

        [DataMember]
        public int ProductsCount
        {
            get { return productsCount; }
            set { productsCount = value; }
        }

        [DataMember]
        public int RestaurantsCount
        {
            get { return restaurantsCount; }
            set { restaurantsCount = value; }
        }

        [DataMember]
        public int UsersCount
        {
            get { return usersCount; }
            set { usersCount = value; }
        }

        [DataMember]
        public int ActiveUsers
        {
            get { return activeUsers; }
            set { activeUsers = value; }
        }

        [DataMember]
        public int ActiveRestaurants
        {
            get { return activeRestaurants; }
            set { activeRestaurants = value; }
        }
    }

}
