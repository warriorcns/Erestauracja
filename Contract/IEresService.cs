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
        bool CreateUser(string login, string password, string email, string name, string surname, string address, string townID, string country, DateTime birthdate, string sex, string telephone, string passwordQuestion, string passwordAnswer, bool isApproved);

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
        bool UpdateUserLoginDate(string login);

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
        bool AddRestaurant(string name, string displayName, string address, string townId, string country, string telephone, string email, string nip, string regon, string password, string managerLogin, string deliveryTime);

        [OperationContract]
        List<Restaurant> GetRestaurantsByManagerLogin(string managerLogin);

        #endregion

        #region ogólne

        [OperationContract]
        List<string> GetCountriesList();

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
        private string townID = null;
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
        public string TownID
        {
            get { return townID; }
            set { townID = value; }
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

    [DataContract]
    public class Restaurant
    { 
        private int id = -1;
        private string name = null;
        private string displayName = null;
        private string address = null;
        private string townID = null;
        private string country = null;
        private string telephone = null;
        private string email = null;
        private string nip = null;
        private string regon = null;
        private DateTime creationDate = new DateTime();
        private int inputsCount = -1;
        private int averageRating = -1;
        private string password = null;
        private int menagerId = -1;
        private string deliveryTime = null;
        private string currentDeliveryTime = null;
        private bool isApproved = false;
        private DateTime lastActivityDate = new DateTime();
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
        public string TownID
        {
            get { return townID; }
            set { townID = value; }
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
        public string Email
        {
            get { return email; }
            set { email = value; }
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
        public DateTime CreationDate
        {
            get { return creationDate; }
            set { creationDate = value; }
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
        public string Password
        {
            get { return password; }
            set { password = value; }
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
        public string CurrentDeliveryTime
        {
            get { return currentDeliveryTime; }
            set { currentDeliveryTime = value; }
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
}
