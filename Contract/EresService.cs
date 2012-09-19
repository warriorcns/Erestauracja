﻿using System;
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

        public bool AddRestaurant(string name, string displayName, string address, string townId, string country, string telephone, string email, string nip, string regon, string password, string managerLogin, string deliveryTime)
        {
            if (!(String.IsNullOrEmpty(name) || String.IsNullOrEmpty(displayName) || String.IsNullOrEmpty(address)
                || String.IsNullOrEmpty(townId) || String.IsNullOrEmpty(country) || String.IsNullOrEmpty(telephone)
                || String.IsNullOrEmpty(email) || String.IsNullOrEmpty(nip) || String.IsNullOrEmpty(regon)
                || String.IsNullOrEmpty(password) || String.IsNullOrEmpty(managerLogin) || String.IsNullOrEmpty(deliveryTime)))
            {
                Database db = new Database();
                bool value = db.AddRestaurant(name, displayName, address, townId, country, telephone, email, nip, regon, password, managerLogin, deliveryTime);
                return value;
            }
            else
            {
                return false;
            }
        }

        public bool EditRestaurant(string name, string displayName, string address, string townId, string country, string telephone, string email, string nip, string regon, string deliveryTime, string managerLogin, int id)
        {
            if (!(String.IsNullOrEmpty(name) || String.IsNullOrEmpty(displayName) || String.IsNullOrEmpty(address)
                || String.IsNullOrEmpty(townId) || String.IsNullOrEmpty(country) || String.IsNullOrEmpty(telephone)
                || String.IsNullOrEmpty(email) || String.IsNullOrEmpty(nip) || String.IsNullOrEmpty(regon)
                || id==null || id<0 || String.IsNullOrEmpty(managerLogin) || String.IsNullOrEmpty(deliveryTime)))
            {
                Database db = new Database();
                bool value = db.EditRestaurant(name, displayName, address, townId, country, telephone, email, nip, regon, deliveryTime, managerLogin, id);
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

        #endregion
    }
}
