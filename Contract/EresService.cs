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
        #region Membership

        /// <summary>
        /// Zamienia hasło u użytkownika o danym loginie.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="password">Hasło do ustawienia</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Zamienia pytanie oraz odpowiedź do odzyskiwania hasła u użytkownika o danym loginie.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="newPwdQuestion">Nowe pytanie</param>
        /// <param name="newPwdAnswer">Nowa odpowiedź</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Zwraca hasło, odpowiedź do odzyskiwania hasła oraz informacje czy użytkownik jest zablokowany, użytkownika o danym loginie.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>obiekt typu PasswordAndAnswer</returns>
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

        /// <summary>
        /// Zwraca odpowiedź do odzyskiwania hasła oraz informacje czy użytkownik jest zablokowany, użytkownika o danym loginie.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>obiekt typu PasswordAnswer</returns>
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

        /// <summary>
        /// Ustawia nowe hasło, aktualizuje date zmiany hasła oraz wysyła email z nowym hasłem na adres emali użytkownika
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="password">Hasło</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Zwraca pytanie do odzyskiwania hasła oraz informacje czy użytkownik jest zablokowany, użytkownika o danym loginie.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>obiekt typu PasswordQuestion</returns>
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

        /// <summary>
        /// Usuwa użytkownika z bazy
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="deleteAllRelatedData">Czy usunąć powiązane dane</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Zwraca wszystkich użytkowników, z podziałem na strony
        /// </summary>
        /// <param name="pageIndex">Indeks strony</param>
        /// <param name="pageSize">Rozmiar strony</param>
        /// <param name="totalRecords">Out ilość pobranych użytkowników</param>
        /// <returns>Lista typu User</returns>
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

        /// <summary>
        /// Zwraca ilość użytkowników, których aktywonść jest późniejsza niż onlineSpan
        /// </summary>
        /// <param name="onlineSpan">Okres czasu do porównania</param>
        /// <returns>int jako liczba użytkowników</returns>
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

        /// <summary>
        /// Zwraca użytkownika o danym loginie
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="userIsOnline">Czy użytkownik jest online - aktualizacja daty ostatniej aktywności</param>
        /// <returns>obiekt typu User</returns>
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

        /// <summary>
        /// Zwraca użytkownika o danym id
        /// </summary>
        /// <param name="id">Id użytkownika</param>
        /// <param name="userIsOnline">Czy użytkownik jest online - aktualizacja daty ostatniej aktywności</param>
        /// <returns>obiekt typu User</returns>
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

        /// <summary>
        /// Odblokowuje konto użytkownika.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Pobiera nazwe użytkownika na podstawie adresu email.
        /// </summary>
        /// <param name="email">Adres email użytkownika</param>
        /// <returns>Zwraca login użytkownika.</returns>
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

        /// <summary>
        /// Pobiera id użytkownika na podstawie adresu email.
        /// </summary>
        /// <param name="email">Adres email użytkownika</param>
        /// <returns>Zwraca id użytkownika.</returns>
        public int GetRestaurantIdByEmail(string email)
        {
            if (!(String.IsNullOrEmpty(email)))
            {
                Database db = new Database();
                int value = db.GetRestaurantIdByEmail(email);
                return value;
            }
            else
                return -1;
        }

        /// <summary>
        /// Aktualizuje dane użytkownika
        /// </summary>
        /// <param name="user">Dane użytkownika jako obiekt typu User</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Zwraca hasło, oraz informacje czy użytkownik jest zatwierdzony, użytkownika o danym loginie.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>obiekt typu ValidateUser</returns>
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

        /// <summary>
        /// Zwraca hasło, oraz informacje czy użytkownik jest zatwierdzony, pracownika o danym loginie oraz restauracji.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="rest">Login reatauracji</param>
        /// <returns>obiekt typu ValidateUser</returns>
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

        /// <summary>
        /// Atkualizuje date ostatniego logowania użytkownika
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Atkualizuje date ostatniego logowania pracownika  z danej restauracji
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="rest">Login reatauracji</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        #region Role

        /// <summary>
        /// Dodaje użytkowników o danym loginie do ról
        /// </summary>
        /// <param name="logins">Loginy użytkowników</param>
        /// <param name="rolenames">Role</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Tworzy nową role
        /// </summary>
        /// <param name="rolename">Nazwa roli</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Usuwa role oraz przypisania użytkowników do tej roli
        /// </summary>
        /// <param name="rolename">Nazwa roli</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Pobiera wszystkie role
        /// </summary>
        /// <returns>Nazwy ról oddzielonych przecinkami</returns>
        public string GetAllRoles()
        {
            Database db = new Database();
            string value = db.GetAllRoles();
            return value;
        }

        /// <summary>
        /// Pobiera role przypisane do danego użytkownika
        /// </summary>
        /// <param name="login">Login uzytkownika</param>
        /// <returns>Nazwy ról oddzielone przecinkami</returns>
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

        /// <summary>
        /// Pobiera loginy użytkowników przypisanych do danej roli
        /// </summary>
        /// <param name="rolename">Nazwa roli</param>
        /// <returns>Loginy użytkowników oddzielone przecinkami</returns>
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

        /// <summary>
        /// Sprawdza czy użytkownik posiada daną role
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="rolename">Nazwa roli</param>
        /// <returns>True jeśli użytkownik posiada daną role</returns>
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

        /// <summary>
        /// Usuwa przypisania użytkowników do ról
        /// </summary>
        /// <param name="logins">Loginy użytkowników</param>
        /// <param name="rolenames">Nazwy ról</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Sprawdza istnienie danej roli
        /// </summary>
        /// <param name="rolename">Nazwa roli</param>
        /// <returns>True jeśli rola istnieje</returns>
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
        public bool EditRestaurant(string name, string displayName, string address, int townId, string country, string telephone, string nip, string regon, string deliveryTime, bool isEnabled, string managerLogin, int id, decimal deliveryPrice, string email)
        {
            if (!(String.IsNullOrEmpty(name) || String.IsNullOrEmpty(displayName) || String.IsNullOrEmpty(address)
                || townId == null || townId < 0 || String.IsNullOrEmpty(country) || String.IsNullOrEmpty(telephone)
                || isEnabled==null || String.IsNullOrEmpty(nip) || String.IsNullOrEmpty(regon)
                || id == null || id < 0 || String.IsNullOrEmpty(managerLogin) || String.IsNullOrEmpty(deliveryTime) || deliveryPrice == null || deliveryPrice < 0.00M || String.IsNullOrEmpty(email)))
            {
                Database db = new Database();
                bool value = db.EditRestaurant(name, displayName, address, townId, country, telephone, nip, regon, deliveryTime, isEnabled, managerLogin, id, deliveryPrice, email);
                return value;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Pobiera restauracje przypisane do danego menadżera
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <returns>Lista typu Restaurant</returns>
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

        /// <summary>
        /// Pobiera restauracje o danym id, przypisaną do danego menadżera
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu RestaurantInfo</returns>
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

        /// <summary>
        /// Pobiera zawartość strony głównej restauracji dla menadżera 
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu MainPageContent</returns>
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

        /// <summary>
        /// Pobiera zawartość strony dowóz restauracji dla menadżera 
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu DeliveryPageContent</returns>
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

        /// <summary>
        /// Edycja zawartości strony dowóz restauracji dla menadżera
        /// </summary>
        /// <param name="delivery">Treść dowozu</param>
        /// <param name="id">Id restauracji</param>
        /// <param name="managerLogin">Login menadżera</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Pobiera zawartość strony wydarzenia restauracji dla menadżera 
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu EventsPageContent</returns>
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

        /// <summary>
        /// Edycja zawartości strony wydarzenia restauracji dla menadżera
        /// </summary>
        /// <param name="events">Treść wydarzeń</param>
        /// <param name="id">Id restauracji</param>
        /// <param name="managerLogin">Login menadżera</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Pobiera zawartość strony kontakt restauracji dla menadżera 
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu ContactPageContent</returns>
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

        /// <summary>
        /// Edycja zawartości strony kontakt restauracji dla menadżera
        /// </summary>
        /// <param name="contact">Treść kontaktu</param>
        /// <param name="id">Id restauracji</param>
        /// <param name="managerLogin">Login menadżera</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Pobiera kategorie danej restauracji
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="restaurantID">Id restauracji</param>
        /// <returns>Lista typu Category</returns>
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

        /// <summary>
        /// Pobiera dane kategori dla menadżera
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="restaurantID">Id restauracji</param>
        /// <param name="categoryID">Id kategorii</param>
        /// <returns>Obiekt typu Category</returns>
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

        /// <summary>
        /// Usuwa kategorie
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="restaurantID">Id restauracji</param>
        /// <param name="categoryID">Id kategorii</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Pobiera menu restauracji dla menadżera
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="restaurantID">Id restauracji</param>
        /// <returns>Lista typu Menu</returns>
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

        /// <summary>
        /// Pobiera dane produktu dla menadżera
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="restaurantID">Id restauracji</param>
        /// <param name="productID">Id produktu</param>
        /// <returns>Obiekt typu Product</returns>
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

        /// <summary>
        /// Pobiera pracowników przypisanych do restauracji danego menadżera
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <returns>Lista typu Presonnel</returns>
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

        /// <summary>
        /// Dodaje pracownika do restauracji
        /// </summary>
        /// <param name="userId">Id użytkownika</param>
        /// <param name="restaurantId">Id restauracji</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
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

        /// <summary>
        /// Pobiera dane o zawartości koszyka
        /// </summary>
        /// <param name="koszyk">Zawartość koszyka z cookie</param>
        /// <returns>Obiekt typu BasketOut</returns>
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

        /// <summary>
        /// Sprawdza czy menadżer jest właścicielem restauracji
        /// </summary>
        /// <param name="login">Login menadżera</param>
        /// <param name="id">Id restauracji</param>
        /// <returns>True jeśli jest właścicielem</returns>
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

        /// <summary>
        /// Usuwanie produktu
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="res">Id restauracji</param>
        /// <param name="id">Id produktu</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public bool DeleteProduct(string managerLogin, int res, int id)
        {
            if (!(String.IsNullOrEmpty(managerLogin) || res == null || res < 1 || id == null || id < 1))
            {
                Database db = new Database();
                bool value = db.DeleteProduct(managerLogin, res, id);
                return value;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Usuwanie pracownika z restauracji
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="res">Id restauracji</param>
        /// <param name="id">Id pracownika</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public bool DeleteEmployee(string managerLogin, int res, int id)
        {
            if (!(String.IsNullOrEmpty(managerLogin) || res == null || res < 1 || id == null || id < 1))
            {
                Database db = new Database();
                bool value = db.DeleteEmployee(managerLogin, res, id);
                return value;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Usuwanie restauracji
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="id">Id restauracji</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public bool DeleteRestaurant(string managerLogin, int id)
        {
            if (!(String.IsNullOrEmpty(managerLogin) || id == null || id < 1))
            {
                Database db = new Database();
                bool value = db.DeleteRestaurant(managerLogin, id);
                return value;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Ogólne

        /// <summary>
        /// Pobiera liste państw
        /// </summary>
        /// <returns>Lista typu string</returns>
        public List<string> GetCountriesList()
        {
            List<string> lista = null;
            Database db = new Database();
            lista = db.GetCountriesList();
            return lista;
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

        /// <summary>
        /// Pobiera liste restauracji z danego miasta
        /// </summary>
        /// <param name="townName">Nazwa miasta</param>
        /// <returns>Lista typu RestaurantInTown</returns>
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

        /// <summary>
        /// Pobiera menu restauracji dla klienta
        /// </summary>
        /// <param name="restaurantID">Id restauracji</param>
        /// <returns>Lista typu menu</returns>
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

        /// <summary>
        /// Pobiera zawartość strony głównej restauracji dla klienta
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu MainPageContent</returns>
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

        /// <summary>
        /// Pobiera zawartość strony dowozu restauracji dla klienta
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu DeliveryPageContent</returns>
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

        /// <summary>
        /// Pobiera zawartość strony wydarzeń restauracji dla klienta
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu EventsPageContent</returns>
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

        /// <summary>
        /// Pobiera zawartość strony kontaktu restauracji dla klienta
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu ContactPageContent</returns>
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

        /// <summary>
        /// Pobiera liste restauracji z danego miasta
        /// </summary>
        /// <param name="cityName">Nazwa miasta</param>
        /// <returns>Obiekt typu RestaurantsFromCity</returns>
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

        /// <summary>
        /// Pobiera 10 najnowszych restauracji
        /// </summary>
        /// <returns>Lista typu RestaurantTop</returns>
        public List<RestaurantTop> GetTopRestaurant()
        {
            Database db = new Database();
            List<RestaurantTop> value = db.GetTopRestaurant();
            return value;
        }

        /// <summary>
        /// Pobiera statystyki
        /// </summary>
        /// <returns>Obiekt typu Statistics</returns>
        public Statistics GetStatistics()
        {
            Database db = new Database();
            Statistics value = db.GetStatistics();
            return value;
        }

        /// <summary>
        /// Sprawdza czy restauracja jest online
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>True jeśli online</returns>
        public bool IsRestaurantOnline(int id)
        {
            if (!(id == null || id < 1))
            {
                Database db = new Database();
                bool value = db.IsRestaurantOnline(id);
                return value;
            }
            else
            {
                return false;
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
            if (!(String.IsNullOrWhiteSpace(town) && String.IsNullOrWhiteSpace(res)))
            {
                Database db = new Database();
                List<RestaurantInCity> value = db.GetSearchResult(town, res);
                return value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Zwiękasz licznik wejść na strone restauracji
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public bool IncInputsCount(int id)
        {
            if (!(id == null || id < 1))
            {
                Database db = new Database();
                bool value = db.IncInputsCount(id);
                return value;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Ustawia nową date aktywności użytkownika
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public bool SetUserActivity(string login)
        {
            if (!(String.IsNullOrWhiteSpace(login)))
            {
                Database db = new Database();
                bool value = db.SetUserActivity(login);
                return value;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Wysyła zgłoszenie błędu
        /// </summary>
        /// <param name="email">Informacja o użytkowniku, który wysłał zgłoszenie (email lub login)</param>
        /// <param name="text">Treść zgłoszenia</param>
        /// <returns>True jeśli metoda wykonała się poprawnie</returns>
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

        #endregion

        #region Komentarze

        /// <summary>
        /// Pobiera komentarze danej restauracji
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>Lista typu Comment</returns>
        public List<Comment> GetRestaurantComments(int id)
        {
            if (!(id == null || id < 1))
            {
                Database db = new Database();
                List<Comment> value = db.GetRestaurantComments(id);
                return value;
            }
            else
            {
                return null;
            }
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
            if (!(String.IsNullOrWhiteSpace(login) || id == null || id < 1 || stars == null || stars <= 0))
            {
                Database db = new Database();
                bool value = db.AddComment(login, id, stars, comment);
                return value;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Pobiera komentarze wystawione przez użytkownika
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>Lista typu Comment</returns>
        public List<Comment> GetUserComments(string login)
        {
            if (!(String.IsNullOrWhiteSpace(login)))
            {
                Database db = new Database();
                List<Comment> value = db.GetUserComments(login);
                return value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Usuwanie komentarza
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="id">Id restauracji</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public bool DeleteComment(string login, int id)
        {
            if (!(String.IsNullOrWhiteSpace(login) || id == null || id < 1))
            {
                Database db = new Database();
                bool value = db.DeleteComment(login, id);
                return value;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Pobiera komentarz o danym id
        /// </summary>
        /// <param name="id">Id komentarza</param>
        /// <returns>Obiekt typu Comment</returns>
        public Comment GetComments(int id)
        {
            if (!(id == null || id < 1))
            {
                Database db = new Database();
                Comment value = db.GetComments(id);
                return value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Wysyła zgłoszenie nadużycia w komentarzu
        /// </summary>
        /// <param name="id">Id komentarza</param>
        /// <param name="resId">Id restauracji</param>
        /// <param name="userLogin">Login użytkownika</param>
        /// <param name="comment">Treść komentarza</param>
        /// <param name="report">Treść zgłoszenia</param>
        /// <param name="login">Login menadżera</param>
        /// <returns>True jeśli metoda wykonała się poprawnie</returns>
        public bool ReportComment(int id, int resId, string userLogin, string comment, string report, string login)
        {
            if (!(String.IsNullOrWhiteSpace(userLogin) || String.IsNullOrWhiteSpace(comment) || String.IsNullOrWhiteSpace(report) || id == null || id < 1 || resId == null || resId < 1 || String.IsNullOrWhiteSpace(login)))
            {
                Email em = new Email();
                bool value = em.SendReportComment(id, resId, userLogin, comment, report, login);
                return value;
            }
            else
            {
                return false;
            }
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
            if (!(String.IsNullOrWhiteSpace(login) || String.IsNullOrWhiteSpace(status) || id == null || id < 1))
            {
                Database db = new Database();
                bool value = db.SetOrderStatus(id, login, status);
                return value;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Pobiera dane dotyczące zamówienia potrzebne do PayPala
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <param name="order">Id zamówienia</param>
        /// <returns>string</returns>
        public string GetPayPalData(int id, int order)
        {
            if (!(id == null || id < 1 || order == null || order < 1))
            {
                Database db = new Database();
                string value = db.GetPayPalData(id, order);
                return value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Pobiera aders email restauracji
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>Adres emiali typu string</returns>
        public string GetRestaurantEmail(int id)
        {
            if (!(id == null || id < 1))
            {
                Database db = new Database();
                string value = db.GetRestaurantEmail(id);
                return value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Pobiera aktywne zamówienia danego użytkownika
        /// </summary>
        /// <param name="login">LOgin użytkownika</param>
        /// <returns>Lista typu UserOrder</returns>
        public List<UserOrder> GetUserActiveOrder(string login)
        {
            if (!(String.IsNullOrWhiteSpace(login)))
            {
                Database db = new Database();
                List<UserOrder> value = db.GetUserActiveOrder(login);
                return value;
            }
            else
            {
                return null;
            }
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
            if (!(String.IsNullOrWhiteSpace(login) || from == null || to == null))
            {
                Database db = new Database();
                List<UserOrder> value = db.GetOrderHistory(login, from, to);
                return value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Zapisuje zamówienie
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="basket">Dane koszyka</param>
        /// <returns>Id zamówienia</returns>
        public int SaveOrder(string login, BasketRest basket)
        {
            if (!(String.IsNullOrWhiteSpace(login) || basket == null) )
            {
                Database db = new Database();
                int value = db.SaveOrder(login, basket);
                return value;
            }
            else
            {
                return -1;
            }
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
            if (!(String.IsNullOrWhiteSpace(login) || id == null || id<1 || payment == null))
            {
                Database db = new Database();
                bool value = db.Pay(login, id, comment, payment);
                return value;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Pobiera zamówienia dla restauracji
        /// </summary>
        /// <param name="login">Login zalogowanego pracownika</param>
        /// <returns>Obiekt typu AllOrders</returns>
        public AllOrders GetOrders(string login)
        {
            if (!(String.IsNullOrWhiteSpace(login)))
            {
                Database db = new Database();
                AllOrders value = db.GetOrders(login);
                return value;
            }
            else
            {
                return null;
            }
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
            if (!(String.IsNullOrWhiteSpace(login) || from == null || to == null))
            {
                Database db = new Database();
                List<Order> value = db.GetAllOrders(login, from, to);
                return value;
            }
            else
            {
                return null;
            }
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

        /// <summary>
        /// Ustawia status online restauracji
        /// </summary>
        /// <param name="login">Login zalogowanego pracownika</param>
        /// <param name="online">Status online lub offline</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public bool SetRestaurantOnline(string login, string online)
        {
            if (!(String.IsNullOrWhiteSpace(login) && String.IsNullOrWhiteSpace(online)))
            {
                Database db = new Database();
                bool value = db.SetRestaurantOnline(login, online);
                return value;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Pobiera status online restauracji
        /// </summary>
        /// <param name="login">Login zalogowanego pracownika</param>
        /// <returns>True jeśli restauracja jest online</returns>
        public bool RestaurantOnlineStatus(string login)
        {
            if (!(String.IsNullOrWhiteSpace(login)))
            {
                Database db = new Database();
                bool value = db.RestaurantOnlineStatus(login);
                return value;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Zapisuje aktywność restauracji
        /// </summary>
        /// <param name="login">Login zalogowanego pracownika lub restauracji</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        public bool SetRestaurantActivity(string login)
        {
            if (!(String.IsNullOrWhiteSpace(login)))
            {
                Database db = new Database();
                bool value = db.SetRestaurantActivity(login);
                return value;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
