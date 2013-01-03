using System;
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
        #region Membership

        /// <summary>
        /// Zamienia hasło u użytkownika o danym loginie.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="password">Hasło do ustawienia</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool ChangePassword(string login, string password);

        /// <summary>
        /// Zamienia pytanie oraz odpowiedź do odzyskiwania hasła u użytkownika o danym loginie.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="newPwdQuestion">Nowe pytanie</param>
        /// <param name="newPwdAnswer">Nowa odpowiedź</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool ChangePasswordQuestionAndAnswer(string login, string newPwdQuestion, string newPwdAnswer);

        /// <summary>
        /// Zwraca hasło, odpowiedź do odzyskiwania hasła oraz informacje czy użytkownik jest zablokowany, użytkownika o danym loginie.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>obiekt typu PasswordAndAnswer</returns>
        [OperationContract]
        PasswordAndAnswer GetPassword(string login);

        /// <summary>
        /// Zwraca odpowiedź do odzyskiwania hasła oraz informacje czy użytkownik jest zablokowany, użytkownika o danym loginie.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>obiekt typu PasswordAnswer</returns>
        [OperationContract]
        PasswordAnswer GetPasswordAnswer(string login);

        /// <summary>
        /// Ustawia nowe hasło, aktualizuje date zmiany hasła oraz wysyła email z nowym hasłem na adres emali użytkownika
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="password">Hasło</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool ResetPassword(string login, string password);

        /// <summary>
        /// Zwraca pytanie do odzyskiwania hasła oraz informacje czy użytkownik jest zablokowany, użytkownika o danym loginie.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>obiekt typu PasswordQuestion</returns>
        [OperationContract]
        PasswordQuestion GetUserQuestion(string login);

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
        [OperationContract]
        bool CreateUser(string login, string password, string email, string name, string surname, string address, int townID, string country, DateTime birthdate, string sex, string telephone, string passwordQuestion, string passwordAnswer, bool isApproved);

        /// <summary>
        /// Usuwa użytkownika z bazy
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="deleteAllRelatedData">Czy usunąć powiązane dane</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool DeleteUser(string login, bool deleteAllRelatedData);

        /// <summary>
        /// Zwraca wszystkich użytkowników, z podziałem na strony
        /// </summary>
        /// <param name="pageIndex">Indeks strony</param>
        /// <param name="pageSize">Rozmiar strony</param>
        /// <param name="totalRecords">Out ilość pobranych użytkowników</param>
        /// <returns>Lista typu User</returns>
        [OperationContract]
        List<User> GetAllUsers(int pageIndex, int pageSize, out int totalRecords);

        /// <summary>
        /// Zwraca ilość użytkowników, których aktywonść jest późniejsza niż onlineSpan
        /// </summary>
        /// <param name="onlineSpan">Okres czasu do porównania</param>
        /// <returns>int jako liczba użytkowników</returns>
        [OperationContract]
        int GetNumberOfUsersOnline(TimeSpan onlineSpan);

        /// <summary>
        /// Zwraca użytkownika o danym loginie
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="userIsOnline">Czy użytkownik jest online - aktualizacja daty ostatniej aktywności</param>
        /// <returns>obiekt typu User</returns>
        [OperationContract]
        User GetUser(string login, bool userIsOnline);

        /// <summary>
        /// Zwraca użytkownika o danym id
        /// </summary>
        /// <param name="id">Id użytkownika</param>
        /// <param name="userIsOnline">Czy użytkownik jest online - aktualizacja daty ostatniej aktywności</param>
        /// <returns>obiekt typu User</returns>
        [OperationContract]
        User GetUserById(int id, bool userIsOnline);

        /// <summary>
        /// Odblokowuje konto użytkownika.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool UnlockUser(string login);

        /// <summary>
        /// Pobiera nazwe użytkownika na podstawie adresu email.
        /// </summary>
        /// <param name="email">Adres email użytkownika</param>
        /// <returns>Zwraca login użytkownika.</returns>
        [OperationContract]
        string GetUserNameByEmail(string email);

        /// <summary>
        /// Aktualizuje dane użytkownika
        /// </summary>
        /// <param name="user">Dane użytkownika jako obiekt typu User</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool UpdateUser(User user);

        /// <summary>
        /// Zwraca hasło, oraz informacje czy użytkownik jest zatwierdzony, użytkownika o danym loginie.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>obiekt typu ValidateUser</returns>
        [OperationContract]
        ValidateUser ValidateUser(string login);

        /// <summary>
        /// Zwraca hasło, oraz informacje czy użytkownik jest zatwierdzony, pracownika o danym loginie oraz restauracji.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="rest">Login reatauracji</param>
        /// <returns>obiekt typu ValidateUser</returns>
        [OperationContract]
        ValidateUser ValidateEmployee(string login, string rest);

        /// <summary>
        /// Atkualizuje date ostatniego logowania użytkownika
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool UpdateUserLoginDate(string login);

        /// <summary>
        /// Atkualizuje date ostatniego logowania pracownika  z danej restauracji
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="rest">Login reatauracji</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool UpdateEmployeeLoginDate(string login, string rest);

        /// <summary>
        /// Aktualizuje licznik błędnie wprowadzonego hasła lub odpowiedzi na pytanie do przywracania hasła.
        /// Jeśli licznik sie wyczerpie blokuje użytkownika.
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="failureType">Typ błędu - password lub passwordAnswer</param>
        /// <param name="PasswordAttemptWindow">Ilość minut po jakich użytkownik będzie mógł ponowić próby</param>
        /// <param name="MaxInvalidPasswordAttempts">Możliwa liczba pomyłek</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool UpdateFailureCount(string login, string failureType, int PasswordAttemptWindow, int MaxInvalidPasswordAttempts);

        #endregion

        #region Role

        /// <summary>
        /// Dodaje użytkowników o danym loginie do ról
        /// </summary>
        /// <param name="logins">Loginy użytkowników</param>
        /// <param name="rolenames">Role</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool AddUsersToRoles(string[] logins, string[]rolenames);

        /// <summary>
        /// Tworzy nową role
        /// </summary>
        /// <param name="rolename">Nazwa roli</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool CreateRole(string rolename);

        /// <summary>
        /// Usuwa role oraz przypisania użytkowników do tej roli
        /// </summary>
        /// <param name="rolename">Nazwa roli</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool DeleteRole(string rolename);

        /// <summary>
        /// Pobiera wszystkie role
        /// </summary>
        /// <returns>Nazwy ról oddzielonych przecinkami</returns>
        [OperationContract]
        string GetAllRoles();

        /// <summary>
        /// Pobiera role przypisane do danego użytkownika
        /// </summary>
        /// <param name="login">Login uzytkownika</param>
        /// <returns>Nazwy ról oddzielone przecinkami</returns>
        [OperationContract]
        string GetRolesForUser(string login);

        /// <summary>
        /// Pobiera loginy użytkowników przypisanych do danej roli
        /// </summary>
        /// <param name="rolename">Nazwa roli</param>
        /// <returns>Loginy użytkowników oddzielone przecinkami</returns>
        [OperationContract]
        string GetUsersInRole(string rolename);

        /// <summary>
        /// Sprawdza czy użytkownik posiada daną role
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="rolename">Nazwa roli</param>
        /// <returns>True jeśli użytkownik posiada daną role</returns>
        [OperationContract]
        bool IsUserInRole(string login, string rolename);

        /// <summary>
        /// Usuwa przypisania użytkowników do ról
        /// </summary>
        /// <param name="logins">Loginy użytkowników</param>
        /// <param name="rolenames">Nazwy ról</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool RemoveUsersFromRoles(string[] logins, string[] rolenames);

        /// <summary>
        /// Sprawdza istnienie danej roli
        /// </summary>
        /// <param name="rolename">Nazwa roli</param>
        /// <returns>True jeśli rola istnieje</returns>
        [OperationContract]
        bool RoleExists(string rolename);

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
        [OperationContract]
        string FindUsersInRole(string rolename, string loginToMatch);

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
        [OperationContract]
        bool AddRestaurant(string login, string email, string password, string passwordQuestion, string passwordAnswer, string name, string displayName, string address, int townID, string country, string telephone, string nip, string regon, string deliveryTime, string managerLogin, decimal deliveryPrice);

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
        [OperationContract]
        bool EditRestaurant(string name, string displayName, string address, int townId, string country, string telephone, string nip, string regon, string deliveryTime, bool isEnabled, string managerLogin, int id, decimal deliveryPrice);

        /// <summary>
        /// Pobiera restauracje przypisane do danego menadżera
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <returns>Lista typu Restaurant</returns>
        [OperationContract]
        List<Restaurant> GetRestaurantsByManagerLogin(string managerLogin);

        /// <summary>
        /// Pobiera restauracje o danym id, przypisaną do danego menadżera
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu RestaurantInfo</returns>
        [OperationContract]
        RestaurantInfo GetRestaurant(string managerLogin, int id);

        /// <summary>
        /// Pobiera zawartość strony głównej restauracji dla menadżera 
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu MainPageContent</returns>
        [OperationContract]
        MainPageContent GetMainPage(string managerLogin, int id);

        /// <summary>
        /// Edycja zawartości strony głównej restauracji dla menadżera 
        /// </summary>
        /// <param name="description">Treść opisu</param>
        /// <param name="foto">Pole nie jest używane, zostanie usunięte przy okazji updateu</param>
        /// <param name="specialOffers">Treść ofert specjalnych</param>
        /// <param name="id">Id restauracji</param>
        /// <param name="managerLogin">Login menadżera</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool EditMainPage(string description, string foto, string specialOffers, int id, string managerLogin);

        /// <summary>
        /// Pobiera zawartość strony dowóz restauracji dla menadżera 
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu DeliveryPageContent</returns>
        [OperationContract]
        DeliveryPageContent GetDeliveryPage(string managerLogin, int id);

        /// <summary>
        /// Edycja zawartości strony dowóz restauracji dla menadżera
        /// </summary>
        /// <param name="delivery">Treść dowozu</param>
        /// <param name="id">Id restauracji</param>
        /// <param name="managerLogin">Login menadżera</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool EditDeliveryPage(string delivery, int id, string managerLogin);

        /// <summary>
        /// Pobiera zawartość strony wydarzenia restauracji dla menadżera 
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu EventsPageContent</returns>
        [OperationContract]
        EventsPageContent GetEventsPage(string managerLogin, int id);

        /// <summary>
        /// Edycja zawartości strony wydarzenia restauracji dla menadżera
        /// </summary>
        /// <param name="events">Treść wydarzeń</param>
        /// <param name="id">Id restauracji</param>
        /// <param name="managerLogin">Login menadżera</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool EditEventsPage(string events, int id, string managerLogin);

        /// <summary>
        /// Pobiera zawartość strony kontakt restauracji dla menadżera 
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu ContactPageContent</returns>
        [OperationContract]
        ContactPageContent GetContactPage(string managerLogin, int id);

        /// <summary>
        /// Edycja zawartości strony kontakt restauracji dla menadżera
        /// </summary>
        /// <param name="contact">Treść kontaktu</param>
        /// <param name="id">Id restauracji</param>
        /// <param name="managerLogin">Login menadżera</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool EditContactPage(string contact, int id, string managerLogin);

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
        [OperationContract]
        bool AddCategory(int restaurantID, string categoryName, string categoryDescription, string priceOption, string nonPriceOption, string nonPriceOption2, string managerLogin);

        /// <summary>
        /// Pobiera kategorie danej restauracji
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="restaurantID">Id restauracji</param>
        /// <returns>Lista typu Category</returns>
        [OperationContract]
        List<Category> GetCategories(string managerLogin, int restaurantID);

        /// <summary>
        /// Pobiera dane kategori dla menadżera
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="restaurantID">Id restauracji</param>
        /// <param name="categoryID">Id kategorii</param>
        /// <returns>Obiekt typu Category</returns>
        [OperationContract]
        Category GetCategory(string managerLogin, int restaurantID, int categoryID);

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
        [OperationContract]
        bool EditCategory(string managerLogin, int restaurantID, int categoryID, string categoryName, string categoryDescription, string priceOption, string nonPriceOption, string nonPriceOption2);

        /// <summary>
        /// Usuwa kategorie
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="restaurantID">Id restauracji</param>
        /// <param name="categoryID">Id kategorii</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool DeleteCategory(string managerLogin, int restaurantID, int categoryID);

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
        [OperationContract]
        bool AddProduct(int restaurantID, int categoryID, string productName, string productDescription, string price, string managerLogin);

        /// <summary>
        /// Pobiera menu restauracji dla menadżera
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="restaurantID">Id restauracji</param>
        /// <returns>Lista typu Menu</returns>
        [OperationContract]
        List<Menu> GetMenuManager(string managerLogin, int restaurantID);

        /// <summary>
        /// Pobiera dane produktu dla menadżera
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <param name="restaurantID">Id restauracji</param>
        /// <param name="productID">Id produktu</param>
        /// <returns>Obiekt typu Product</returns>
        [OperationContract]
        Product GetProduct(string managerLogin, int restaurantID, int productID);

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
        [OperationContract]
        bool EditProduct(string managerLogin, int restaurantID, int id, int categoryID, string productName, string productDescription, string price, bool isAvailable);

        /// <summary>
        /// Pobiera pracowników przypisanych do restauracji danego menadżera
        /// </summary>
        /// <param name="managerLogin">Login menadżera</param>
        /// <returns>Lista typu Presonnel</returns>
        [OperationContract]
        List<Presonnel> GetPersonnel(string managerLogin);

        /// <summary>
        /// Dodaje pracownika do restauracji
        /// </summary>
        /// <param name="userId">Id użytkownika</param>
        /// <param name="restaurantId">Id restauracji</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool AddUserToRestaurant(int userId, int restaurantId);

        /// <summary>
        /// Pobiera dane o zawartości koszyka
        /// </summary>
        /// <param name="koszyk">Zawartość koszyka z cookie</param>
        /// <returns>Obiekt typu BasketOut</returns>
        [OperationContract]
        BasketOut GetBasket(string basket);

        /// <summary>
        /// Sprawdza czy menadżer jest właścicielem restauracji
        /// </summary>
        /// <param name="login">Login menadżera</param>
        /// <param name="id">Id restauracji</param>
        /// <returns>True jeśli jest właścicielem</returns>
        [OperationContract]
        bool IsRestaurantOwner(string login, int id);

        #endregion

        #region Ogólne

        /// <summary>
        /// Pobiera liste państw
        /// </summary>
        /// <returns>Lista typu string</returns>
        [OperationContract]
        List<string> GetCountriesList();

        /// <summary>
        /// Pobiera liste miast pasujących do kryteriów
        /// </summary>
        /// <param name="townName">Nazwa miasta</param>
        /// <param name="postalCode">Kod pocztowy</param>
        /// <param name="status">out status - informacja o statusie pobierania miast</param>
        /// <returns>Lista typu Town</returns>
        [OperationContract]
        List<Town> GetTowns(string townName, string postalCode, out string status);

        /// <summary>
        /// Pobiera liste restauracji z danego miasta
        /// </summary>
        /// <param name="townName">Nazwa miasta</param>
        /// <returns>Lista typu RestaurantInTown</returns>
        [OperationContract]
        List<RestaurantInTown> GetRestaurantByTown(string townName);

        /// <summary>
        /// Pobiera menu restauracji dla klienta
        /// </summary>
        /// <param name="restaurantID">Id restauracji</param>
        /// <returns>Lista typu menu</returns>
        [OperationContract]
        List<Menu> GetMenu(int restaurantID);

        /// <summary>
        /// Pobiera zawartość strony głównej restauracji dla klienta
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu MainPageContent</returns>
        [OperationContract]
        MainPageContent GetMainPageUser(int id);

        /// <summary>
        /// Pobiera zawartość strony dowozu restauracji dla klienta
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu DeliveryPageContent</returns>
        [OperationContract]
        DeliveryPageContent GetDeliveryPageUser(int id);

        /// <summary>
        /// Pobiera zawartość strony wydarzeń restauracji dla klienta
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu EventsPageContent</returns>
        [OperationContract]
        EventsPageContent GetEventsPageUser(int id);

        /// <summary>
        /// Pobiera zawartość strony kontaktu restauracji dla klienta
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>Obiekt typu ContactPageContent</returns>
        [OperationContract]
        ContactPageContent GetContactPageUser(int id);

        /// <summary>
        /// Pobiera liste restauracji z danego miasta
        /// </summary>
        /// <param name="cityName">Nazwa miasta</param>
        /// <returns>Obiekt typu RestaurantsFromCity</returns>
        [OperationContract]
        RestaurantsFromCity RestaurantsFromCity(string cityName);

        /// <summary>
        /// Pobiera 10 najnowszych restauracji
        /// </summary>
        /// <returns>Lista typu RestaurantTop</returns>
        [OperationContract]
        List<RestaurantTop> GetTopRestaurant();

        /// <summary>
        /// Pobiera statystyki
        /// </summary>
        /// <returns>Obiekt typu Statistics</returns>
        [OperationContract]
        Statistics GetStatistics();

        /// <summary>
        /// Sprawdza czy restauracja jest online
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>True jeśli online</returns>
        [OperationContract]
        bool IsRestaurantOnline(int id);

        /// <summary>
        /// Pobiera liste restauracji spełniających tryteria miasta i nazwy restauracji
        /// </summary>
        /// <param name="town">Nazwa miasta</param>
        /// <param name="res">Nazwa restauracji</param>
        /// <returns>Lista tylu RestaurantInCity</returns>
        [OperationContract]
        List<RestaurantInCity> GetSearchResult(string town, string res);

        /// <summary>
        /// Zwiękasz licznik wejść na strone restauracji
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool IncInputsCount(int id);

        /// <summary>
        /// Ustawia nową date aktywności użytkownika
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool SetUserActivity(string login);

        /// <summary>
        /// Wysyła zgłoszenie błędu
        /// </summary>
        /// <param name="email">Informacja o użytkowniku, który wysłał zgłoszenie (email lub login)</param>
        /// <param name="text">Treść zgłoszenia</param>
        /// <returns>True jeśli metoda wykonała się poprawnie</returns>
        [OperationContract]
        bool SendError(string email, string text);

        #endregion

        #region Komentarze

        /// <summary>
        /// Pobiera komentarze danej restauracji
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>Lista typu Comment</returns>
        [OperationContract]
        List<Comment> GetRestaurantComments(int id);

        /// <summary>
        /// Dodaje komenatarz
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="id">Id restauracji</param>
        /// <param name="stars">Ocena</param>
        /// <param name="comment">KOmentarz</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool AddComment(string login, int id, double stars, string comment);

        /// <summary>
        /// Pobiera komentarze wystawione przez użytkownika
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <returns>Lista typu Comment</returns>
        [OperationContract]
        List<Comment> GetUserComments(string login);

        /// <summary>
        /// Usuwanie komentarza
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="id">Id restauracji</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool DeleteComment(string login, int id);

        /// <summary>
        /// Pobiera komentarz o danym id
        /// </summary>
        /// <param name="id">Id komentarza</param>
        /// <returns>Obiekt typu Comment</returns>
        [OperationContract]
        Comment GetComments(int id);

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
        [OperationContract]
        bool ReportComment(int id, int resId, string userLogin, string comment, string report, string login);
        
        #endregion

        #region Zamówienia

        /// <summary>
        /// Zapisuje status zamówienia
        /// </summary>
        /// <param name="id">Id zamówienia</param>
        /// <param name="login">Login użytkownika</param>
        /// <param name="status">Status</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool SetOrderStatus(int id, string login, string status);

        /// <summary>
        /// Pobiera dane dotyczące zamówienia potrzebne do PayPala
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <param name="order">Id zamówienia</param>
        /// <returns>string</returns>
        [OperationContract]
        string GetPayPalData(int id, int order);

        /// <summary>
        /// Pobiera aders email restauracji
        /// </summary>
        /// <param name="id">Id restauracji</param>
        /// <returns>Adres emiali typu string</returns>
        [OperationContract]
        string GetRestaurantEmail(int id);

        /// <summary>
        /// Pobiera aktywne zamówienia danego użytkownika
        /// </summary>
        /// <param name="login">LOgin użytkownika</param>
        /// <returns>Lista typu UserOrder</returns>
        [OperationContract]
        List<UserOrder> GetUserActiveOrder(string login);

        /// <summary>
        /// Pobiera zamówienia użytkownika z danego okresy czasu
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="from">Data od</param>
        /// <param name="to">Data do</param>
        /// <returns>Lista typu UserOrder</returns>
        [OperationContract]
        List<UserOrder> GetOrderHistory(string login, DateTime from, DateTime to);

        /// <summary>
        /// Zapisuje zamówienie
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="basket">Dane koszyka</param>
        /// <returns>Id zamówienia</returns>
        [OperationContract]
        int SaveOrder(string login, BasketRest basket);

        /// <summary>
        /// Realizuj zamówienie
        /// </summary>
        /// <param name="login">Login użytkownika</param>
        /// <param name="id">Id zamówienia</param>
        /// <param name="comment">KOmentarz do zamówienia</param>
        /// <param name="payment">Typ płatności</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool Pay(string login, int id, string comment, string payment);

        /// <summary>
        /// Pobiera zamówienia dla restauracji
        /// </summary>
        /// <param name="login">Login zalogowanego pracownika</param>
        /// <returns>Obiekt typu AllOrders</returns>
        [OperationContract]
        AllOrders GetOrders(string login);

        /// <summary>
        /// Pobiera zamówienia dla restauracji z określonego czasu
        /// </summary>
        /// <param name="login">Login zalogowanego pracownika</param>
        /// <param name="from">Data od</param>
        /// <param name="to">Data do</param>
        /// <returns>Lista typu Order</returns>
        [OperationContract]
        List<Order> GetAllOrders(string login, DateTime from, DateTime to);
  
        #endregion

        #region POS

        /// <summary>
        /// Pobiera liste pracowników z danej restauracji
        /// </summary>
        /// <param name="login">Login restauracji</param>
        /// <returns>Lista typu string</returns>
        [OperationContract]
        List<string> GetEmployeesInRestaurant(string login);

        /// <summary>
        /// Ustawia status online restauracji
        /// </summary>
        /// <param name="login">Login zalogowanego pracownika</param>
        /// <param name="online">Status online lub offline</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool SetRestaurantOnline(string login, string online);

        /// <summary>
        /// Pobiera status online restauracji
        /// </summary>
        /// <param name="login">Login zalogowanego pracownika</param>
        /// <returns>True jeśli restauracja jest online</returns>
        [OperationContract]
        bool RestaurantOnlineStatus(string login);

        /// <summary>
        /// Zapisuje aktywność restauracji
        /// </summary>
        /// <param name="login">Login zalogowanego pracownika lub restauracji</param>
        /// <returns>True jeśli metoda wykonała się poprawnie.</returns>
        [OperationContract]
        bool SetRestaurantActivity(string login);

        #endregion
    }


    #region membership dataContract

    /// <summary>
    /// 
    /// </summary>
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
        private double averageRating = -1;
        private int menagerId = -1;
        private string deliveryTime = null;
        private decimal deliveryPrice = 0.00M;
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
        public double AverageRating
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
        public decimal DeliveryPrice
        {
            get { return deliveryPrice; }
            set { deliveryPrice = value; }
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
        private decimal deliveryPrice = 0.00M;
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
        public decimal DeliveryPrice
        {
            get { return deliveryPrice; }
            set { deliveryPrice = value; }
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

    [DataContract]
    public class BasketOut
    {
       // public decimal totalPrice = 0.00M;
        public List<BasketRest> basket = new List<BasketRest>();

        //[DataMember]
        //public decimal TotalPrice
        //{
        //    get { return totalPrice; }
        //    set { totalPrice = value; }
        //}

        [DataMember]
        public List<BasketRest> Basket
        {
            get { return basket; }
            set { basket = value; }
        }

    }

    [DataContract]
    public class BasketRest
    {
        public int restaurantId = -1;
        public string displayName = String.Empty;
        public string telephone = String.Empty;
        public string deliveryTime = String.Empty;
        public decimal deliveryPrice = 0.00M;
        public decimal totalPriceRest = 0.00M;
        public string comment = String.Empty;
        public List<BasketProduct> products = new List<BasketProduct>();

        [DataMember]
        public int RestaurantId
        {
            get { return restaurantId; }
            set { restaurantId = value; }
        }

        [DataMember]
        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        [DataMember]
        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }

        [DataMember]
        public string DeliveryTime
        {
            get { return deliveryTime; }
            set { deliveryTime = value; }
        }

        [DataMember]
        public decimal DeliveryPrice
        {
            get { return deliveryPrice; }
            set { deliveryPrice = value; }
        }

        [DataMember]
        public decimal TotalPriceRest
        {
            get { return totalPriceRest; }
            set { totalPriceRest = value; }
        }

        [DataMember]
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        [DataMember]
        public List<BasketProduct> Products
        {
            get { return products; }
            set { products = value; }
        }

    }

    [DataContract]
    public class BasketProduct
    {
        public int basketId = -1;
        public int productId = -1;
        public string productName = String.Empty;
        public string comment = String.Empty;
        public string priceOption = String.Empty;
        public string nonPriceOption = String.Empty;
        public string nonPriceOption2 = String.Empty;
        public decimal price = 0.00M;
        public int count = -1;
        public decimal totalPriceProd = 0.00M;
      //  public bool isSelected = true;

        [DataMember]
        public int BasketId
        {
            get { return basketId; }
            set { basketId = value; }
        }

        [DataMember]
        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        [DataMember]
        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        [DataMember]
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
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
        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        [DataMember]
        public int Count
        {
            get { return count; }
            set { count = value; }
        }

        [DataMember]
        public decimal TotalPriceProd
        {
            get { return totalPriceProd; }
            set { totalPriceProd = value; }
        }

        //[DataMember]
        //public bool IsSelected
        //{
        //    get { return isSelected; }
        //    set { isSelected = value; }
        //}

    }

    #endregion

    #region ogólne

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
        private double averageRating = -1;
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
        public double AverageRating
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

    #endregion

    [DataContract]
    public class AllOrders
    {
        private List<Order> waiting = new List<Order>();
        private List<Order> active = new List<Order>();
        private List<Order> finish = new List<Order>();

        [DataMember]
        public List<Order> Waiting
        {
            get { return waiting; }
            set { waiting = value; }
        }

        [DataMember]
        public List<Order> Active
        {
            get { return active; }
            set { active = value; }
        }

        [DataMember]
        public List<Order> Finish
        {
            get { return finish; }
            set { finish = value; }
        }
    }

    [DataContract]
    public class Order
    {
        private int orderId = -1;
        private string userName = null;
        private string userSurname = null;
        private string userAdderss = null;
        private string userTown = null;
        private string userPostal = null;
        private string userTelephone = null;
        private string status = null;
        private decimal price = 0.00M;
        private string comment = null;
        private DateTime orderDate = new DateTime(1,1,1);
        private string payment = null;
        private DateTime finishDate = new DateTime(1, 1, 1);
        private List<OrderedProduct> products = new List<OrderedProduct>();

        [DataMember]
        public int OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }

        [DataMember]
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        [DataMember]
        public string UserSurname
        {
            get { return userSurname; }
            set { userSurname = value; }
        }

        [DataMember]
        public string UserAdderss
        {
            get { return userAdderss; }
            set { userAdderss = value; }
        }

        [DataMember]
        public string UserTown
        {
            get { return userTown; }
            set { userTown = value; }
        }

        [DataMember]
        public string UserPostal
        {
            get { return userPostal; }
            set { userPostal = value; }
        }

        [DataMember]
        public string UserTelephone
        {
            get { return userTelephone; }
            set { userTelephone = value; }
        }

        [DataMember]
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        [DataMember]
        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        [DataMember]
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        [DataMember]
        public DateTime OrderDate
        {
            get { return orderDate; }
            set { orderDate = value; }
        }

        [DataMember]
        public string Payment
        {
            get { return payment; }
            set { payment = value; }
        }

        [DataMember]
        public DateTime FinishDate
        {
            get { return finishDate; }
            set { finishDate = value; }
        }

        [DataMember]
        public List<OrderedProduct> Products
        {
            get { return products; }
            set { products = value; }
        }
    }

    [DataContract]
    public class OrderedProduct
    {
        public int productId = -1;
        public string productName = null;
        public string priceOption = null;
        public int count = -1;
        public string nonPriceOption = null;
        public string nonPriceOption2 = null;
        public string comment = null;
       
        [DataMember]
        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

        [DataMember]
        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        [DataMember]
        public string PriceOption
        {
            get { return priceOption; }
            set { priceOption = value; }
        }

        [DataMember]
        public int Count
        {
            get { return count; }
            set { count = value; }
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
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }
    }

    //zamówienia widziane przez klienta
    [DataContract]
    public class UserOrder
    {
        private int orderId = -1;
        public string displayName = String.Empty;
        private string address = null;
        private string town = null;
        private string postal = null;
        public string telephone = String.Empty;
        public string deliveryTime = String.Empty;
        public decimal deliveryPrice = 0.00M;
        private string status = null;
        private decimal price = 0.00M;
        private string comment = null;
        private DateTime orderDate = new DateTime(1, 1, 1);
        private string payment = null;
        private DateTime finishDate = new DateTime(1, 1, 1);
        private List<OrderedProduct> products = new List<OrderedProduct>();

        [DataMember]
        public int OrderId
        {
            get { return orderId; }
            set { orderId = value; }
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
        public string Postal
        {
            get { return postal; }
            set { postal = value; }
        }

        [DataMember]
        public string Telephone
        {
            get { return telephone; }
            set { telephone = value; }
        }

        [DataMember]
        public string DeliveryTime
        {
            get { return deliveryTime; }
            set { deliveryTime = value; }
        }

        [DataMember]
        public decimal DeliveryPrice
        {
            get { return deliveryPrice; }
            set { deliveryPrice = value; }
        }

        [DataMember]
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        [DataMember]
        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        [DataMember]
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        [DataMember]
        public DateTime OrderDate
        {
            get { return orderDate; }
            set { orderDate = value; }
        }

        [DataMember]
        public string Payment
        {
            get { return payment; }
            set { payment = value; }
        }

        [DataMember]
        public DateTime FinishDate
        {
            get { return finishDate; }
            set { finishDate = value; }
        }

        [DataMember]
        public List<OrderedProduct> Products
        {
            get { return products; }
            set { products = value; }
        }
    }

    [DataContract]
    public class Comment
    {
        private int id = -1;
        private string userLogin = null;
        public string displayName = String.Empty;
        private string address = null;
        private string town = null;
        private string postal = null;
        private double rating = -1;
        private string comment = null;
        private DateTime date = new DateTime(1,1,1);

        [DataMember]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        [DataMember]
        public string UserLogin
        {
            get { return userLogin; }
            set { userLogin = value; }
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
        public string Postal
        {
            get { return postal; }
            set { postal = value; }
        }

        [DataMember]
        public double Rating
        {
            get { return rating; }
            set { rating = value; }
        }

        [DataMember]
        public string CommentText
        {
            get { return comment; }
            set { comment = value; }
        }

        [DataMember]
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }
    }
}
