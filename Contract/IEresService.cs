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

    //[DataContract]
    //public class User
    //{
    //    string imie = "Hello ";
    //    string nazwisko = "Hello ";
    //    string adres = "Hello ";
    //    string kraj = "Hello ";
    //    string tel = "Hello ";
    //    string email = "Hello ";
    //    string plec = "Hello ";
    //    string login = "Hello ";
    //    string haslo = "Hello ";
    //    string uprawnienia = "Hello ";
    //    string miasto = "Hello ";
    //    string kodpocztowy = "Hello ";

    //    [DataMember]
    //    public string Imie
    //    {
    //        get { return imie; }
    //        set { imie = value; }
    //    }
    //    [DataMember]
    //    public string Nazwisko
    //    {
    //        get { return nazwisko; }
    //        set { nazwisko = value; }
    //    }
    //    [DataMember]
    //    public string Adres
    //    {
    //        get { return adres; }
    //        set { adres = value; }
    //    }
    //    [DataMember]
    //    public string Kraj
    //    {
    //        get { return kraj; }
    //        set { kraj = value; }
    //    }
    //    [DataMember]
    //    public string Tel
    //    {
    //        get { return tel; }
    //        set { tel = value; }
    //    }
    //    [DataMember]
    //    public string Email
    //    {
    //        get { return email; }
    //        set { email = value; }
    //    }
    //    [DataMember]
    //    public string Plec
    //    {
    //        get { return plec; }
    //        set { plec = value; }
    //    }
    //    [DataMember]
    //    public string Login
    //    {
    //        get { return login; }
    //        set { login = value; }
    //    }
    //    [DataMember]
    //    public string Haslo
    //    {
    //        get { return haslo; }
    //        set { haslo = value; }

    //    }
    //    [DataMember]
    //    public string Uprawnienia
    //    {
    //        get { return uprawnienia; }
    //        set { uprawnienia = value; }
    //    }
    //    [DataMember]
    //    public string Miasto
    //    {
    //        get { return miasto; }
    //        set { miasto = value; }
    //    }
    //    [DataMember]
    //    public string Kodpocztowy
    //    {
    //        get { return kodpocztowy; }
    //        set { kodpocztowy = value; }
    //    }
    //}
}
