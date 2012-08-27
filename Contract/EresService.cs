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

        public bool CreateUser(string login, string password, string email, string name, string surname, string address, string townID, string country, DateTime birthdate, string sex, string telephone, string passwordQuestion, string passwordAnswer, bool isApproved)
        {
            if (!(String.IsNullOrEmpty(login) || String.IsNullOrEmpty(password) || String.IsNullOrEmpty(email) || String.IsNullOrEmpty(name)
                || String.IsNullOrEmpty(surname) || String.IsNullOrEmpty(address) || String.IsNullOrEmpty(townID) || String.IsNullOrEmpty(country)
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

        #endregion
    }
}
