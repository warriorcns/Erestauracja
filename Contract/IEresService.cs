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
    }

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
}
