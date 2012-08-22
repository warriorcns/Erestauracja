﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:4.0.30319.225
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Erestauracja {
    using System;
    
    
    /// <summary>
    ///   Klasa zasobu wymagająca zdefiniowania typu do wyszukiwania zlokalizowanych ciągów itd.
    /// </summary>
    // Ta klasa została automatycznie wygenerowana za pomocą klasy StronglyTypedResourceBuilder
    // przez narzędzie, takie jak ResGen lub Visual Studio.
    // Aby dodać lub usunąć członka, edytuj plik .ResX, a następnie ponownie uruchom ResGen
    // z opcją /str lub ponownie utwórz projekt VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Queries {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Queries() {
        }
        
        /// <summary>
        /// Zwraca buforowane wystąpienie ResourceManager używane przez tę klasę.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Erestauracja.Queries", typeof(Queries).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Zastępuje właściwość CurrentUICulture bieżącego wątku dla wszystkich
        ///   przypadków przeszukiwania zasobów za pomocą tej klasy zasobów wymagającej zdefiniowania typu.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu INSERT INTO `users_in_roles`(`login`, `rolename`, `applicationName`) VALUES (@login,@rolename,@applicationName).
        /// </summary>
        internal static string AddUsersToRoles {
            get {
                return ResourceManager.GetString("AddUsersToRoles", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu SELECT Count(*) FROM `users` WHERE `applicationName` = @applicationName.
        /// </summary>
        internal static string AllUsersCount {
            get {
                return ResourceManager.GetString("AllUsersCount", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu UPDATE `users` SET `password` = @password, `lastPasswordChangedDate` = @lastPasswordChangedDate WHERE `login` = @login AND `applicationName` = @applicationName.
        /// </summary>
        internal static string ChangePassword {
            get {
                return ResourceManager.GetString("ChangePassword", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu UPDATE `users` SET `passwordQuestion` = @question, `passwordAnswer` = @answer WHERE `login` = @login AND `applicationName` = @applicationName.
        /// </summary>
        internal static string ChangePasswordQuestionAndAnswer {
            get {
                return ResourceManager.GetString("ChangePasswordQuestionAndAnswer", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu INSERT INTO `roles`(`rolename`, `applicationName`) VALUES (@rolename, @applicationName).
        /// </summary>
        internal static string CreateRole {
            get {
                return ResourceManager.GetString("CreateRole", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu INSERT INTO `users`(`login`, `password`, `email`, `name`, `surname`, `address`, `townID`, `country`, `birthdate`, `sex`, `telephone`, `applicationName`, `comment`, `passwordQuestion`, `passwordAnswer`, `isApproved`, `lastActivityDate`, `lastLoginDate`, `lastPasswordChangedDate`, `creationDate`, `isOnLine`, `isLockedOut`, `lastLockedOutDate`, `failedPasswordAttemptCount`, `failedPasswordAttemptWindowStart`, `failedPasswordAnswerAttemptCount`, `failedPasswordAnswerAttemptWindowStart`) VALUES(@login, @password [obcięto pozostałą część ciągu]&quot;;.
        /// </summary>
        internal static string CreateUser {
            get {
                return ResourceManager.GetString("CreateUser", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu DELETE FROM `roles` WHERE `rolename` = @rolename AND `applicationName` = @applicationName.
        /// </summary>
        internal static string DeleteRole {
            get {
                return ResourceManager.GetString("DeleteRole", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu DELETE FROM `users`  WHERE `login` = @login AND `applicationname` = @applicationname.
        /// </summary>
        internal static string DeleteUser {
            get {
                return ResourceManager.GetString("DeleteUser", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu DELETE FROM `users_in_roles` WHERE `rolename` = @rolename AND `applicationName` = @applicationName.
        /// </summary>
        internal static string DeleteUsersInRole {
            get {
                return ResourceManager.GetString("DeleteUsersInRole", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu SELECT `login`, `email`, `passwordQuestion`, `comment`, `isApproved`, `isLockedOut`, `creationDate`, `lastLoginDate`,`lastActivityDate`, `lastPasswordChangedDate`, `llastLockedOutDate` FROM `users` WHERE `email` LIKE @email AND `applicationName` = @applicationName ORDER BY `login` Asc.
        /// </summary>
        internal static string FindUsersByEmail {
            get {
                return ResourceManager.GetString("FindUsersByEmail", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu SELECT `id`, `login`,  `email`, `name`, `surname`, `address`, `townID`, `country`, `birthdate`, `sex`, `telephone`,  `comment`, `passwordQuestion`,  `isApproved`, `lastActivityDate`, `lastLoginDate`, `lastPasswordChangedDate`, `creationDate`,  `isLockedOut`, `lastLockedOutDate`   FROM `users` WHERE `login` LIKE @login AND `applicationName` = @applicationName ORDER BY `login` Asc.
        /// </summary>
        internal static string FindUsersByName {
            get {
                return ResourceManager.GetString("FindUsersByName", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu SELECT `login` FROM `users_in_roles` WHERE `login` LIKE @login AND `rolename` = @rolename AND `applicationName` = @applicationName.
        /// </summary>
        internal static string FindUsersInRole {
            get {
                return ResourceManager.GetString("FindUsersInRole", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu SELECT `rolename` FROM `roles`  WHERE `applicationName` = @applicationName.
        /// </summary>
        internal static string GetAllRoles {
            get {
                return ResourceManager.GetString("GetAllRoles", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu SELECT `id`, `login`,  `email`, `name`, `surname`, `address`, `townID`, `country`, `birthdate`, `sex`, `telephone`,  `comment`, `passwordQuestion`,  `isApproved`, `lastActivityDate`, `lastLoginDate`, `lastPasswordChangedDate`, `creationDate`,  `isLockedOut`, `lastLockedOutDate`  FROM `users`  WHERE `applicationName` = @applicationName ORDER BY `login` Asc.
        /// </summary>
        internal static string GetAllUsers {
            get {
                return ResourceManager.GetString("GetAllUsers", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu SELECT `email` FROM `users` WHERE `login` = @login AND `applicationName` = @applicationName.
        /// </summary>
        internal static string GetEmailByLogin {
            get {
                return ResourceManager.GetString("GetEmailByLogin", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu SELECT `failedPasswordAttemptCount`, `failedPasswordAttemptWindowStart`, `failedPasswordAnswerAttemptCount`, `failedPasswordAnswerAttemptWindowStart` FROM `users` WHERE `login` = @login AND `applicationName` = @applicationName.
        /// </summary>
        internal static string GetFailureCount {
            get {
                return ResourceManager.GetString("GetFailureCount", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu SELECT Count(*) FROM `users` WHERE `lastActivityDate` &gt; @lastActivityDate AND `applicationName` = @applicationName.
        /// </summary>
        internal static string GetNumberOfUsersOnline {
            get {
                return ResourceManager.GetString("GetNumberOfUsersOnline", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu SELECT `password`, `passwordAnswer`, `isLockedOut` FROM `users WHERE `login` = @login AND `applicationName` = @applicationName.
        /// </summary>
        internal static string GetPassword {
            get {
                return ResourceManager.GetString("GetPassword", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu SELECT `passwordAnswer`, `isLockedOut` FROM `users` WHERE `login` = @login AND `applicationName` = @applicationName.
        /// </summary>
        internal static string GetPasswordAnswer {
            get {
                return ResourceManager.GetString("GetPasswordAnswer", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu SELECT `rolename` FROM `users_in_roles` WHERE `login` = @login AND `applicationName` = @applicationName.
        /// </summary>
        internal static string GetRolesForUser {
            get {
                return ResourceManager.GetString("GetRolesForUser", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu SELECT `id`, `login`,  `email`, `name`, `surname`, `address`, `townID`, `country`, `birthdate`, `sex`, `telephone`,  `comment`, `passwordQuestion`,  `isApproved`, `lastActivityDate`, `lastLoginDate`, `lastPasswordChangedDate`, `creationDate`,  `isLockedOut`, `lastLockedOutDate` FROM `users` WHERE `id` = @id.
        /// </summary>
        internal static string GetUserByID {
            get {
                return ResourceManager.GetString("GetUserByID", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu SELECT `id`, `login`,  `email`, `name`, `surname`, `address`, `townID`, `country`, `birthdate`, `sex`, `telephone`,  `comment`, `passwordQuestion`,  `isApproved`, `lastActivityDate`, `lastLoginDate`, `lastPasswordChangedDate`, `creationDate`,  `isLockedOut`, `lastLockedOutDate`  FROM `users` WHERE `login` = @login AND `applicationName` = @applicationName.
        /// </summary>
        internal static string GetUserByLogin {
            get {
                return ResourceManager.GetString("GetUserByLogin", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu SELECT Count(*) FROM `users` WHERE `email` LIKE @email AND `applicationName` = @applicationName.
        /// </summary>
        internal static string GetUserCountByEmail {
            get {
                return ResourceManager.GetString("GetUserCountByEmail", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu SELECT Count(*) FROM `users` WHERE `login` LIKE @login AND `applicationName` = @applicationName.
        /// </summary>
        internal static string GetUserCountByLogin {
            get {
                return ResourceManager.GetString("GetUserCountByLogin", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu SELECT `login` FROM `users` WHERE `email` = @email AND `applicationName` = @applicationName.
        /// </summary>
        internal static string GetUserNameByEmail {
            get {
                return ResourceManager.GetString("GetUserNameByEmail", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu SELECT `passwordQuestion`, `isLockedOut` FROM `users` WHERE `login` = @login AND `applicationName` = @applicationName.
        /// </summary>
        internal static string GetUserQuestion {
            get {
                return ResourceManager.GetString("GetUserQuestion", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu SELECT `login` FROM `users_in_roles` WHERE `rolename` = @rolename AND `applicationName` = @applicationName.
        /// </summary>
        internal static string GetUsersInRole {
            get {
                return ResourceManager.GetString("GetUsersInRole", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu SELECT COUNT(*) FROM `users_in_roles` WHERE `login` = @login AND `rolename` = @rolename AND `applicationName` = @applicationName.
        /// </summary>
        internal static string IsUserInRole {
            get {
                return ResourceManager.GetString("IsUserInRole", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu UPDATE `users` SET `isLockedOut` = @isLockedOut, `lastLockedOutDate` = @lastLockedOutDate WHERE `login` = @login AND `applicationName` = @applicationName.
        /// </summary>
        internal static string LockOutUser {
            get {
                return ResourceManager.GetString("LockOutUser", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu DELETE FROM `users_in_roles` WHERE `login` = @login AND `rolename` = @rolename AND `applicationName` = @applicationName.
        /// </summary>
        internal static string RemoveUsersFromRoles {
            get {
                return ResourceManager.GetString("RemoveUsersFromRoles", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu UPDATE `users` SET `password` = @password, `lastPasswordChangedDate` = @lastPasswordChangedDate WHERE `login` = @login AND `applicationName` = @applicationName AND `isLockedOut` = @isLockedOut.
        /// </summary>
        internal static string ResetPassword {
            get {
                return ResourceManager.GetString("ResetPassword", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu SELECT COUNT(*) FROM `roles` WHERE `rolename` = @rolename AND `applicationName` = @applicationName.
        /// </summary>
        internal static string RoleExists {
            get {
                return ResourceManager.GetString("RoleExists", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu UPDATE `users`  SET `failedPasswordAnswerAttemptCount` = @count WHERE `login` = @login AND `applicationName` = @applicationName.
        /// </summary>
        internal static string SetFailedPasswordAnswerAttemptCount {
            get {
                return ResourceManager.GetString("SetFailedPasswordAnswerAttemptCount", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu UPDATE `users` SET `failedPasswordAttemptCount` = @count WHERE `login` = @login AND `applicationName` = @applicationName.
        /// </summary>
        internal static string SetFailedPasswordAttemptCount {
            get {
                return ResourceManager.GetString("SetFailedPasswordAttemptCount", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu UPDATE `users` SET `isLockedOut` = @isLockedOut, `lastLockedOutDate` = @lastLockedOutDate WHERE `login` = @login AND `applicationName` = @applicationName.
        /// </summary>
        internal static string UnlockUser {
            get {
                return ResourceManager.GetString("UnlockUser", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu UPDATE `users` SET `failedPasswordAnswerAttemptCount` = @count, `failedPasswordAnswerAttemptWindowStart` = @windowStart WHERE `login` = @login AND `applicationName` = @applicationName.
        /// </summary>
        internal static string UpdateFailedPasswordAnswerAttempt {
            get {
                return ResourceManager.GetString("UpdateFailedPasswordAnswerAttempt", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu UPDATE `users` SET `failedPasswordAttemptCount` = @count, `failedPasswordAttemptWindowStart` = @windowStart WHERE `login` = @login AND `applicationName` = @applicationName.
        /// </summary>
        internal static string UpdateFailedPasswordAttempt {
            get {
                return ResourceManager.GetString("UpdateFailedPasswordAttempt", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu UPDATE `users` SET `name` = @name, `surname` = @surname, `address` = @address, `townID` = @townID, `country` = @country, `birthdate` = @birthdate, `sex` = @sex, `telephone` = @telephone, `comment` = @comment, `isApproved` = @isApproved  WHERE `login` = @login AND `applicationName` = @applicationName.
        /// </summary>
        internal static string UpdateUser {
            get {
                return ResourceManager.GetString("UpdateUser", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu UPDATE `users` SET `lastActivityDate` = @lastActivityDate WHERE `id` = @id.
        /// </summary>
        internal static string UpdateUserActivityByID {
            get {
                return ResourceManager.GetString("UpdateUserActivityByID", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu UPDATE `users` SET `lastActivityDate` = @lastActivityDate WHERE `login` = @login AND `applicationname` = @applicationname.
        /// </summary>
        internal static string UpdateUserActivityByLogin {
            get {
                return ResourceManager.GetString("UpdateUserActivityByLogin", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu UPDATE `users` SET `lastLoginDate`= @lastLoginDate WHERE `login` = @login AND `applicationName` = @applicationName.
        /// </summary>
        internal static string UpdateUserLoginDate {
            get {
                return ResourceManager.GetString("UpdateUserLoginDate", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu SELECT `password`, `isApproved` FROM `users` WHERE `login` = @login AND `applicationName` = @applicationName AND `isLockedOut` = @isLockedOut.
        /// </summary>
        internal static string ValidateUser {
            get {
                return ResourceManager.GetString("ValidateUser", resourceCulture);
            }
        }
    }
}
