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
            if (!(String.IsNullOrEmpty(login) && String.IsNullOrEmpty(password)))
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
            if (!(String.IsNullOrEmpty(login) && String.IsNullOrEmpty(newPwdQuestion) && String.IsNullOrEmpty(newPwdAnswer)))
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
            if (!(String.IsNullOrEmpty(login) && String.IsNullOrEmpty(password)))
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

        #endregion
    }
}
