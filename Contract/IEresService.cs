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
        string GetData(int value);

        [OperationContract]
        CompositeType GetDataUsingDataContract(CompositeType composite);

        [OperationContract]
        string Test();

        //[OperationContract]
        //User GetUserData(User user);
        //// TODO: Add your service operations here
    }

    // Use a data contract as illustrated in the sample below to add composite types to service operations
    [DataContract]
    public class CompositeType
    {
        bool boolValue = true;
        string stringValue = "Hello ";

        [DataMember]
        public bool BoolValue
        {
            get { return boolValue; }
            set { boolValue = value; }
        }

        [DataMember]
        public string StringValue
        {
            get { return stringValue; }
            set { stringValue = value; }
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
