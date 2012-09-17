using System;
using System.Web.Security;

namespace Erestauracja.Providers
{
    public class CustomMembershipUser : MembershipUser
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
        public int TownID { get; set; }
        public string Country { get; set; }
        public DateTime Birthdate { get; set; }
        public string Sex { get; set; }
        public string Telephone { get; set; }

        public CustomMembershipUser(
            string providername,
            //string userName,
            //object providerUserKey,
            string email,
            string passwordQuestion,
            string comment,
            bool isApproved,
            bool isLockedOut,
            DateTime creationDate,
            DateTime lastLoginDate,
            DateTime lastActivityDate,
            DateTime lastPasswordChangedDate,
            DateTime lastLockedOutDate,
            int id,
            string login,
            string name,
            string surname,
            string address,
            int townID,
            string country,
            DateTime birthdate,
            string sex,
            string telephone
            ) :
            base(providername,
                login,
                Guid.NewGuid(),
                email,
                passwordQuestion,
                comment,
                isApproved,
                isLockedOut,
                creationDate,
                lastLoginDate,
                lastPasswordChangedDate,
                lastActivityDate,
                lastLockedOutDate)
        {
            Id = id;
            Login = login;
            Name = name;
            Surname = surname;
            Address = address;
            TownID = townID;
            Country = country;
            Birthdate = birthdate;
            Sex = sex;
            Telephone = telephone;
        }
    }
}