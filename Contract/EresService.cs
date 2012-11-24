using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Contract
{
    public class EresService : IEresService
    {
        #region membershipProvider

        public bool ChangePassword(string login, string password)
        {
            if (!(String.IsNullOrEmpty(login) || String.IsNullOrEmpty(password)))
            {
                Database db = new Database();
                bool value = db.ChangePassword(login, password);
                return value;
            }
            else
            {
                return false;
            }
        }

        public bool ChangePasswordQuestionAndAnswer(string login, string newPwdQuestion, string newPwdAnswer)
        {
            if (!(String.IsNullOrEmpty(login) || String.IsNullOrEmpty(newPwdQuestion) || String.IsNullOrEmpty(newPwdAnswer)))
            {
                Database db = new Database();
                bool value = db.ChangePasswordQuestionAndAnswer(login, newPwdQuestion, newPwdAnswer);
                return value;
            }
            else
            {
                return false;
            }
        }

        public PasswordAndAnswer GetPassword(string login)
        {
            if (!(String.IsNullOrEmpty(login)))
            {
                Database db = new Database();
                PasswordAndAnswer value = db.GetPassword(login);
                return value;
            }
            else
            {
                return null;
            }
        }

        public PasswordAnswer GetPasswordAnswer(string login)
        {
            if (!(String.IsNullOrEmpty(login)))
            {
                Database db = new Database();
                PasswordAnswer value = db.GetPasswordAnswer(login);
                return value;
            }
            else
            {
                return null;
            }
        }

        public bool ResetPassword(string login, string password)
        {
            if (!(String.IsNullOrEmpty(login) || String.IsNullOrEmpty(password)))
            {
                Database db = new Database();
                bool value = db.ResetPassword(login, password);
                return value;
            }
            else
            {
                return false;
            }
        }

        public PasswordQuestion GetUserQuestion(string login)
        {
            if (!(String.IsNullOrEmpty(login)))
            {
                Database db = new Database();
                PasswordQuestion value = db.GetUserQuestion(login);
                return value;
            }
            else
            {
                return null;
            }
        }

        public bool CreateUser(string login, string password, string email, string name, string surname, string address, int townID, string country, DateTime birthdate, string sex, string telephone, string passwordQuestion, string passwordAnswer, bool isApproved)
        {
            if (!(String.IsNullOrEmpty(login) || String.IsNullOrEmpty(password) || String.IsNullOrEmpty(email) || String.IsNullOrEmpty(name)
                || String.IsNullOrEmpty(surname) || String.IsNullOrEmpty(address) || townID==null || String.IsNullOrEmpty(country)
                || String.IsNullOrEmpty(birthdate.ToString()) || String.IsNullOrEmpty(sex) || String.IsNullOrEmpty(telephone) || String.IsNullOrEmpty(passwordQuestion)
                || String.IsNullOrEmpty(passwordAnswer) || String.IsNullOrEmpty(isApproved.ToString())))
            {
                Database db = new Database();
                bool value = db.CreateUser(login, password, email, name, surname, address, townID, country, birthdate, sex, telephone, passwordQuestion, passwordAnswer, isApproved);
                return value;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteUser(string login, bool deleteAllRelatedData)
        {
            if (!(String.IsNullOrEmpty(login) || String.IsNullOrEmpty(deleteAllRelatedData.ToString())))
            {
                Database db = new Database();
                bool value = db.DeleteUser(login, deleteAllRelatedData);
                return value;
            }
            else
            {
                return false;
            }
        }

        public List<User> GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            totalRecords = 0;

            if (!(pageIndex < 0 || pageSize < 0))
            {
                Database db = new Database();
                List<User> value = db.GetAllUsers(pageIndex, pageSize, out totalRecords);
                return value;
            }
            else
            {
                return null;
            }
        }

        public int GetNumberOfUsersOnline(TimeSpan onlineSpan)
        {
            if (onlineSpan != null)
            {
                Database db = new Database();
                int value = db.GetNumberOfUsersOnline(onlineSpan);
                return value;
            }
            else
                return -1;
        }

        public User GetUser(string login, bool userIsOnline)
        {
            if (!(String.IsNullOrEmpty(login)))
            {
                Database db = new Database();
                User value = db.GetUser(login, userIsOnline);
                return value;
            }
            else
            {
                return null;
            }
        }

        public User GetUserById(int id, bool userIsOnline)
        {
            if (!(id<0))
            {
                Database db = new Database();
                User value = db.GetUser(id, userIsOnline);
                return value;
            }
            else
            {
                return null;
            }
        }

        public bool UnlockUser(string login)
        {
            if (!(String.IsNullOrEmpty(login)))
            {
                Database db = new Database();
                bool value = db.UnlockUser(login);
                return value;
            }
            else
            {
                return false;
            }
        }

        public string GetUserNameByEmail(string email)
        {
            if (!(String.IsNullOrEmpty(email)))
            {
                Database db = new Database();
                string value = db.GetUserNameByEmail(email);
                return value;
            }
            else
                return null;
        }

        public bool UpdateUser(User user)
        {
            if (!(user == null))
            {
                Database db = new Database();
                bool value = db.UpdateUser(user);
                return value;
            }
            else
                return false;
        }

        public ValidateUser ValidateUser(string login)
        {
            if (!(String.IsNullOrEmpty(login)))
            {
                Database db = new Database();
                ValidateUser value = db.ValidateUser(login);
                return value;
            }
            else
            {
                return null;
            }
        }

        public ValidateUser ValidateEmployee(string login, string rest)
        {
            if (!(String.IsNullOrWhiteSpace(login) || String.IsNullOrWhiteSpace(rest)))
            {
                Database db = new Database();
                ValidateUser value = db.ValidateEmployee(login, rest);
                return value;
            }
            else
            {
                return null;
            }
        }

        public bool UpdateUserLoginDate(string login)
        {
            if (!(String.IsNullOrEmpty(login)))
            {
                Database db = new Database();
                bool value = db.UpdateUserLoginDate(login);
                return value;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateEmployeeLoginDate(string login, string rest)
        {
            if (!(String.IsNullOrEmpty(login) || String.IsNullOrEmpty(rest)))
            {
                Database db = new Database();
                bool value = db.UpdateEmployeeLoginDate(login, rest);
                return value;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateFailureCount(string login, string failureType, int PasswordAttemptWindow, int MaxInvalidPasswordAttempts)
        {
            if (!(String.IsNullOrEmpty(login) || String.IsNullOrEmpty(failureType)))
            {
                Database db = new Database();
                bool value = db.UpdateFailureCount(login, failureType, PasswordAttemptWindow, MaxInvalidPasswordAttempts);
                return value;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region roleProvider

        public bool AddUsersToRoles(string[] logins, string[] rolenames)
        {
            if (logins!=null && rolenames!=null)
            {
                Database db = new Database();
                bool value = db.AddUsersToRoles(logins, rolenames);
                return value;
            }
            else
            {
                return false;
            }
        }

        public bool CreateRole(string rolename)
        {
            if ( !(String.IsNullOrEmpty(rolename)) )
            {
                Database db = new Database();
                bool value = db.CreateRole(rolename);
                return value;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteRole(string rolename)
        {
            if (!(String.IsNullOrEmpty(rolename)))
            {
                Database db = new Database();
                bool value = db.DeleteRole(rolename);
                return value;
            }
            else
            {
                return false;
            }
        }

        public string GetAllRoles()
        {
            Database db = new Database();
            string value = db.GetAllRoles();
            return value;
        }

        public string GetRolesForUser(string login)
        {
            if (!(String.IsNullOrEmpty(login)))
            {
                Database db = new Database();
                string value = db.GetRolesForUser(login);
                return value;
            }
            else
            {
                return null;
            }
        }

        public string GetUsersInRole(string rolename)
        {
            if (!(String.IsNullOrEmpty(rolename)))
            {
                Database db = new Database();
                string value = db.GetUsersInRole(rolename);
                return value;
            }
            else
            {
                return null;
            }
        }

        public bool IsUserInRole(string login, string rolename)
        {
            if (!(String.IsNullOrEmpty(rolename) || String.IsNullOrEmpty(login)))
            {
                Database db = new Database();
                bool value = db.IsUserInRole(login, rolename);
                return value;
            }
            else
            {
                return false;
            }
        }

        public bool RemoveUsersFromRoles(string[] logins, string[] rolenames)
        {
            if (logins != null && rolenames != null)
            {
                Database db = new Database();
                bool value = db.RemoveUsersFromRoles(logins, rolenames);
                return value;
            }
            else
            {
                return false;
            }
        }

        public bool RoleExists(string rolename)
        {
            if (!(String.IsNullOrEmpty(rolename)))
            {
                Database db = new Database();
                bool value = db.RoleExists(rolename);
                return value;
            }
            else
            {
                return false;
            }
        }

        public string FindUsersInRole(string rolename, string loginToMatch)
        {
            if (!(String.IsNullOrEmpty(rolename)))
            {
                Database db = new Database();
                string value = db.FindUsersInRole(rolename, loginToMatch);
                return value;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Manage restaurant

        public bool AddRestaurant(string login, string email, string password, string passwordQuestion, string passwordAnswer, string name, string displayName, string address, int townID, string country, string telephone, string nip, string regon, string deliveryTime, string managerLogin, decimal deliveryPrice)
        {
            if (!(String.IsNullOrEmpty(login) || String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password) 
                || String.IsNullOrEmpty(passwordQuestion) || String.IsNullOrEmpty(passwordAnswer) || String.IsNullOrEmpty(name) 
                || String.IsNullOrEmpty(displayName) || String.IsNullOrEmpty(address)
                || townID==null || townID < 0 || String.IsNullOrEmpty(country) || String.IsNullOrEmpty(telephone)
                || String.IsNullOrEmpty(nip) || String.IsNullOrEmpty(regon)
                || String.IsNullOrEmpty(managerLogin) || String.IsNullOrEmpty(deliveryTime) || deliveryPrice == null || deliveryPrice < 0.00M))
            {
                Database db = new Database();
                bool value = db.AddRestaurant(login, email, password, passwordQuestion, passwordAnswer, name, displayName, address, townID, country, telephone, nip, regon, deliveryTime, managerLogin, deliveryPrice);
                return value;
            }
            else
            {
                return false;
            }
        }

        public bool EditRestaurant(string name, string displayName, string address, int townId, string country, string telephone, string nip, string regon, string deliveryTime, bool isEnabled, string managerLogin, int id, decimal deliveryPrice)
        {
            if (!(String.IsNullOrEmpty(name) || String.IsNullOrEmpty(displayName) || String.IsNullOrEmpty(address)
                || townId == null || townId < 0 || String.IsNullOrEmpty(country) || String.IsNullOrEmpty(telephone)
                || isEnabled==null || String.IsNullOrEmpty(nip) || String.IsNullOrEmpty(regon)
                || id == null || id < 0 || String.IsNullOrEmpty(managerLogin) || String.IsNullOrEmpty(deliveryTime) || deliveryPrice == null || deliveryPrice < 0.00M))
            {
                Database db = new Database();
                bool value = db.EditRestaurant(name, displayName, address, townId, country, telephone, nip, regon, deliveryTime, isEnabled, managerLogin, id, deliveryPrice);
                return value;
            }
            else
            {
                return false;
            }
        }

        public List<Restaurant> GetRestaurantsByManagerLogin(string managerLogin)
        {
            if (!(String.IsNullOrEmpty(managerLogin)))
            {
                Database db = new Database();
                List<Restaurant> value = db.GetRestaurantsByManagerLogin(managerLogin);
                return value;
            }
            else
            {
                return null;
            }
        }

        public RestaurantInfo GetRestaurant(string managerLogin, int id)
        {
            if (!(String.IsNullOrEmpty(managerLogin) || id==null || id<0))
            {
                Database db = new Database();
                RestaurantInfo value = db.GetRestaurant(managerLogin, id);
                return value;
            }
            else
            {
                return null;
            }
        }

        public MainPageContent GetMainPage(string managerLogin, int id)
        {
            if (!(String.IsNullOrEmpty(managerLogin) || id == null || id < 0))
            {
                Database db = new Database();
                MainPageContent value = db.GetMainPage(managerLogin, id);
                return value;
            }
            else
            {
                return null;
            }
        }

        public bool EditMainPage(string description, string foto, string specialOffers, int id, string managerLogin)
        {
            if (!(String.IsNullOrEmpty(managerLogin) || description==null || foto==null || specialOffers==null || id == null || id < 0))
            {
                Database db = new Database();
                bool value = db.EditMainPage(description, foto, specialOffers, id, managerLogin);
                return value;
            }
            else
            {
                return false;
            }
        }

        public DeliveryPageContent GetDeliveryPage(string managerLogin, int id)
        {
            if (!(String.IsNullOrEmpty(managerLogin) || id == null || id < 0))
            {
                Database db = new Database();
                DeliveryPageContent value = db.GetDeliveryPage(managerLogin, id);
                return value;
            }
            else
            {
                return null;
            }
        }

        public bool EditDeliveryPage(string delivery, int id, string managerLogin)
        {
            if (!(String.IsNullOrEmpty(managerLogin) || delivery == null || id == null || id < 0))
            {
                Database db = new Database();
                bool value = db.EditDeliveryPage(delivery, id, managerLogin);
                return value;
            }
            else
            {
                return false;
            }
        }

        public EventsPageContent GetEventsPage(string managerLogin, int id)
        {
            if (!(String.IsNullOrEmpty(managerLogin) || id == null || id < 0))
            {
                Database db = new Database();
                EventsPageContent value = db.GetEventsPage(managerLogin, id);
                return value;
            }
            else
            {
                return null;
            }
        }

        public bool EditEventsPage(string events, int id, string managerLogin)
        {
            if (!(String.IsNullOrEmpty(managerLogin) || events == null || id == null || id < 0))
            {
                Database db = new Database();
                bool value = db.EditEventsPage(events, id, managerLogin);
                return value;
            }
            else
            {
                return false;
            }
        }

        public ContactPageContent GetContactPage(string managerLogin, int id)
        {
            if (!(String.IsNullOrEmpty(managerLogin) || id == null || id < 0))
            {
                Database db = new Database();
                ContactPageContent value = db.GetContactPage(managerLogin, id);
                return value;
            }
            else
            {
                return null;
            }
        }

        public bool EditContactPage(string contact, int id, string managerLogin)
        {
            if (!(String.IsNullOrEmpty(managerLogin) || contact == null || id == null || id < 0))
            {
                Database db = new Database();
                bool value = db.EditContactPage(contact, id, managerLogin);
                return value;
            }
            else
            {
                return false;
            }
        }

        public bool AddCategory(int restaurantID, string categoryName, string categoryDescription, string priceOption, string nonPriceOption, string nonPriceOption2, string managerLogin)
        {
            if (!(String.IsNullOrEmpty(categoryName) || String.IsNullOrEmpty(managerLogin) || restaurantID == null || restaurantID < 1))
            {
                Database db = new Database();
                bool value = db.AddCategory(restaurantID, categoryName, categoryDescription, priceOption, nonPriceOption, nonPriceOption2, managerLogin);
                return value;
            }
            else
            {
                return false;
            }
        }

        public List<Category> GetCategories(string managerLogin, int restaurantID)
        {
            if (!(String.IsNullOrEmpty(managerLogin) || restaurantID == null || restaurantID < 1))
            {
                Database db = new Database();
                List<Category> value = db.GetCategories(managerLogin, restaurantID);
                return value;
            }
            else
            {
                return null;
            }
        }

        public Category GetCategory(string managerLogin, int restaurantID, int categoryID)
        {
            if (!(String.IsNullOrEmpty(managerLogin) || restaurantID == null || restaurantID < 1 || categoryID == null || categoryID < 1))
            {
                Database db = new Database();
                Category value = db.GetCategory(managerLogin, restaurantID, categoryID);
                return value;
            }
            else
            {
                return null;
            }
        }

        public bool EditCategory(string managerLogin, int restaurantID, int categoryID, string categoryName, string categoryDescription, string priceOption, string nonPriceOption, string nonPriceOption2)
        {
            if (!(String.IsNullOrEmpty(categoryName) || String.IsNullOrEmpty(managerLogin) || restaurantID == null || restaurantID < 1 || categoryID == null || categoryID < 1))
            {
                Database db = new Database();
                bool value = db.EditCategory(managerLogin, restaurantID, categoryID, categoryName, categoryDescription, priceOption, nonPriceOption, nonPriceOption2);
                return value;
            }
            else
            {
                return false;
            }
        }

        public bool DeleteCategory(string managerLogin, int restaurantID, int categoryID)
        {
            if (!(String.IsNullOrEmpty(managerLogin) || restaurantID == null || restaurantID < 1 || categoryID == null || categoryID < 1))
            {
                Database db = new Database();
                bool value = db.DeleteCategory(managerLogin, restaurantID, categoryID);
                return value;
            }
            else
            {
                return false;
            }
        }

        public bool AddProduct(int restaurantID, int categoryID, string productName, string productDescription, string price, string managerLogin)
        {
            if (!(String.IsNullOrEmpty(productName) || String.IsNullOrEmpty(price) || String.IsNullOrEmpty(managerLogin) || restaurantID == null || restaurantID < 1 || categoryID == null || categoryID < 1))
            {
                Database db = new Database();
                bool value = db.AddProduct(restaurantID, categoryID, productName, productDescription, price, managerLogin);
                return value;
            }
            else
            {
                return false;
            }
        }

        public List<Menu> GetMenuManager(string managerLogin, int restaurantID)
        {
            if (!(String.IsNullOrEmpty(managerLogin) || restaurantID == null || restaurantID < 1))
            {
                Database db = new Database();
                List<Menu> value = db.GetMenuManager(managerLogin, restaurantID);
                return value;
            }
            else
            {
                return null;
            }
        }

        public Product GetProduct(string managerLogin, int restaurantID, int productID)
        {
            if (!(String.IsNullOrEmpty(managerLogin) || restaurantID == null || restaurantID < 1 || productID == null || productID < 1))
            {
                Database db = new Database();
                Product value = db.GetProduct(managerLogin, restaurantID, productID);
                return value;
            }
            else
            {
                return null;
            }
        }

        public bool EditProduct(string managerLogin, int restaurantID, int id, int categoryID, string productName, string productDescription, string price, bool isAvailable)
        {
            if (!(String.IsNullOrEmpty(price) || String.IsNullOrEmpty(managerLogin) || String.IsNullOrEmpty(productName) || restaurantID == null || restaurantID < 1 || categoryID == null || categoryID < 1 || id == null || id < 1))
            {
                Database db = new Database();
                bool value = db.EditProduct(managerLogin, restaurantID, id, categoryID, productName, productDescription, price, isAvailable);
                return value;
            }
            else
            {
                return false;
            }
        }

        public List<Presonnel> GetPersonnel(string managerLogin)
        {
            if (!(String.IsNullOrEmpty(managerLogin)))
            {
                Database db = new Database();
                List<Presonnel> value = db.GetPersonnel(managerLogin);
                return value;
            }
            else
            {
                return null;
            }
        }

        public bool AddUserToRestaurant(int userId, int restaurantId)
        {
            if (!(restaurantId == null || restaurantId < 1 || userId == null || userId < 1))
            {
                Database db = new Database();
                bool value = db.AddUserToRestaurant(userId, restaurantId);
                return value;
            }
            else
            {
                return false;
            }
        }

        public BasketOut GetBasket(string basket)
        {
            if (!(String.IsNullOrWhiteSpace(basket)))
            {
                Database db = new Database();
                BasketOut value = db.GetBasket(basket);
                return value;
            }
            else
            {
                return null;
            }
        }

        public bool IsRestaurantOwner(string login, int id)
        {
            if (!(String.IsNullOrEmpty(login) || id == null || id < 1))
            {
                Database db = new Database();
                bool value = db.IsRestaurantOwner(login, id);
                return value;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region ogólne
        
        public List<string> GetCountriesList()
        {
            List<string> lista = null;
            Database db = new Database();
            lista = db.GetCountriesList();
            return lista;
        }

        public List<Town> GetTowns(string townName, string postalCode, out string status)
        {
            if (!(String.IsNullOrEmpty(townName) || String.IsNullOrEmpty(postalCode)))
            {
                Database db = new Database();
                List<Town> value = db.GetTowns(townName, postalCode, out status);
                return value;
            }
            else
            {
                status = "Błędne dane wejściowe";
                return null;
            }
        }

        public List<RestaurantInTown> GetRestaurantByTown(string townName)
        {
            if (!(String.IsNullOrEmpty(townName)))
            {
                Database db = new Database();
                List<RestaurantInTown> value = db.GetRestaurantByTown(townName);
                return value;
            }
            else
            {
                return null;
            }
        }

        public List<Menu> GetMenu(int restaurantID)
        {
            if (!(restaurantID == null || restaurantID < 1))
            {
                Database db = new Database();
                List<Menu> value = db.GetMenu(restaurantID);
                return value;
            }
            else
            {
                return null;
            }
        }

        public MainPageContent GetMainPageUser(int id)
        {
            if (!(id == null || id < 0))
            {
                Database db = new Database();
                MainPageContent value = db.GetMainPageUser(id);
                return value;
            }
            else
            {
                return null;
            }
        }

        public DeliveryPageContent GetDeliveryPageUser(int id)
        {
            if (!(id == null || id < 0))
            {
                Database db = new Database();
                DeliveryPageContent value = db.GetDeliveryPageUser(id);
                return value;
            }
            else
            {
                return null;
            }
        }

        public EventsPageContent GetEventsPageUser(int id)
        {
            if (!(id == null || id < 0))
            {
                Database db = new Database();
                EventsPageContent value = db.GetEventsPageUser(id);
                return value;
            }
            else
            {
                return null;
            }
        }

        public ContactPageContent GetContactPageUser(int id)
        {
            if (!(id == null || id < 0))
            {
                Database db = new Database();
                ContactPageContent value = db.GetContactPageUser(id);
                return value;
            }
            else
            {
                return null;
            }
        }

        public bool SendError(string email, string text)
        {
            if (!(String.IsNullOrEmpty(email) || String.IsNullOrEmpty(text)))
            {
                Email em = new Email();
                bool value = em.SendError(email, text);
                return value;
            }
            else
            {
                return false;
            }
        }

        public RestaurantsFromCity RestaurantsFromCity(string cityName)
        {
            if (!(String.IsNullOrEmpty(cityName)))
            {
                Database db = new Database();
                RestaurantsFromCity value = db.RestaurantsFromCity(cityName);
                return value;
            }
            else
            {
                return null;
            }
        }

        public List<RestaurantTop> GetTopRestaurant()
        {
            Database db = new Database();
            List<RestaurantTop> value = db.GetTopRestaurant();
            return value;
        }

        public Statistics GetStatistics()
        {
            Database db = new Database();
            Statistics value = db.GetStatistics();
            return value;
        }

        public List<string> GetEmployeesInRestaurant(string login)
        {
            if (!(String.IsNullOrEmpty(login)))
            {
                Database db = new Database();
                List<string> value = db.GetEmployeesInRestaurant(login);
                return value;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
