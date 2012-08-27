﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Contract {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
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
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Contract.Queries", typeof(Queries).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
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
        ///   Looks up a localized string similar to INSERT INTO `users_in_roles`(`login`, `rolename`, `applicationName`) VALUES (@login,@rolename,@applicationName).
        /// </summary>
        internal static string AddUsersToRoles {
            get {
                return ResourceManager.GetString("AddUsersToRoles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Count(*) FROM `users`.
        /// </summary>
        internal static string AllUsersCount {
            get {
                return ResourceManager.GetString("AllUsersCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE `users` SET `password` = @password, `lastPasswordChangedDate` = @lastPasswordChangedDate WHERE `login` = @login.
        /// </summary>
        internal static string ChangePassword {
            get {
                return ResourceManager.GetString("ChangePassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE `users` SET `passwordQuestion` = @question, `passwordAnswer` = @answer WHERE `login` = @login.
        /// </summary>
        internal static string ChangePasswordQuestionAndAnswer {
            get {
                return ResourceManager.GetString("ChangePasswordQuestionAndAnswer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO `roles`(`rolename`, `applicationName`) VALUES (@rolename, @applicationName).
        /// </summary>
        internal static string CreateRole {
            get {
                return ResourceManager.GetString("CreateRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO `users`(`login`, `password`, `email`, `name`, `surname`, `address`, `townID`, `country`, `birthdate`, `sex`, `telephone`, `comment`, `passwordQuestion`, `passwordAnswer`, `isApproved`, `lastActivityDate`, `lastLoginDate`, `lastPasswordChangedDate`, `creationDate`, `isOnLine`, `isLockedOut`, `lastLockedOutDate`, `failedPasswordAttemptCount`, `failedPasswordAttemptWindowStart`, `failedPasswordAnswerAttemptCount`, `failedPasswordAnswerAttemptWindowStart`) VALUES(@login, @password, @email, @name, @s [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string CreateUser {
            get {
                return ResourceManager.GetString("CreateUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM `roles` WHERE `rolename` = @rolename AND `applicationName` = @applicationName.
        /// </summary>
        internal static string DeleteRole {
            get {
                return ResourceManager.GetString("DeleteRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM `users`  WHERE `login` = @login.
        /// </summary>
        internal static string DeleteUser {
            get {
                return ResourceManager.GetString("DeleteUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM `users_in_roles` WHERE `rolename` = @rolename AND `applicationName` = @applicationName.
        /// </summary>
        internal static string DeleteUsersInRole {
            get {
                return ResourceManager.GetString("DeleteUsersInRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `login`, `email`, `passwordQuestion`, `comment`, `isApproved`, `isLockedOut`, `creationDate`, `lastLoginDate`,`lastActivityDate`, `lastPasswordChangedDate`, `llastLockedOutDate` FROM `users` WHERE `email` LIKE @email AND `applicationName` = @applicationName ORDER BY `login` Asc.
        /// </summary>
        internal static string FindUsersByEmail {
            get {
                return ResourceManager.GetString("FindUsersByEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `id`, `login`,  `email`, `name`, `surname`, `address`, `townID`, `country`, `birthdate`, `sex`, `telephone`,  `comment`, `passwordQuestion`,  `isApproved`, `lastActivityDate`, `lastLoginDate`, `lastPasswordChangedDate`, `creationDate`,  `isLockedOut`, `lastLockedOutDate`   FROM `users` WHERE `login` LIKE @login AND `applicationName` = @applicationName ORDER BY `login` Asc.
        /// </summary>
        internal static string FindUsersByName {
            get {
                return ResourceManager.GetString("FindUsersByName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `login` FROM `users_in_roles` WHERE `login` LIKE @login AND `rolename` = @rolename AND `applicationName` = @applicationName.
        /// </summary>
        internal static string FindUsersInRole {
            get {
                return ResourceManager.GetString("FindUsersInRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `rolename` FROM `roles`  WHERE `applicationName` = @applicationName.
        /// </summary>
        internal static string GetAllRoles {
            get {
                return ResourceManager.GetString("GetAllRoles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `id`, `login`,  `email`, `name`, `surname`, `address`, `townID`, `country`, `birthdate`, `sex`, `telephone`,  `comment`, `passwordQuestion`,  `isApproved`, `lastActivityDate`, `lastLoginDate`, `lastPasswordChangedDate`, `creationDate`,  `isLockedOut`, `lastLockedOutDate`  FROM `users`  ORDER BY `login` Asc.
        /// </summary>
        internal static string GetAllUsers {
            get {
                return ResourceManager.GetString("GetAllUsers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `email` FROM `users` WHERE `login` = @login.
        /// </summary>
        internal static string GetEmailByLogin {
            get {
                return ResourceManager.GetString("GetEmailByLogin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `failedPasswordAttemptCount`, `failedPasswordAttemptWindowStart`, `failedPasswordAnswerAttemptCount`, `failedPasswordAnswerAttemptWindowStart` FROM `users` WHERE `login` = @login.
        /// </summary>
        internal static string GetFailureCount {
            get {
                return ResourceManager.GetString("GetFailureCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Count(*) FROM `users` WHERE `lastActivityDate` &gt; @lastActivityDate.
        /// </summary>
        internal static string GetNumberOfUsersOnline {
            get {
                return ResourceManager.GetString("GetNumberOfUsersOnline", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `password`, `passwordAnswer`, `isLockedOut` FROM `users` WHERE `login` = @login.
        /// </summary>
        internal static string GetPassword {
            get {
                return ResourceManager.GetString("GetPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `passwordAnswer`, `isLockedOut` FROM `users` WHERE `login` = @login.
        /// </summary>
        internal static string GetPasswordAnswer {
            get {
                return ResourceManager.GetString("GetPasswordAnswer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `rolename` FROM `users_in_roles` WHERE `login` = @login AND `applicationName` = @applicationName.
        /// </summary>
        internal static string GetRolesForUser {
            get {
                return ResourceManager.GetString("GetRolesForUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `id`, `login`,  `email`, `name`, `surname`, `address`, `townID`, `country`, `birthdate`, `sex`, `telephone`,  `comment`, `passwordQuestion`,  `isApproved`, `lastActivityDate`, `lastLoginDate`, `lastPasswordChangedDate`, `creationDate`,  `isLockedOut`, `lastLockedOutDate` FROM `users` WHERE `id` = @id.
        /// </summary>
        internal static string GetUserByID {
            get {
                return ResourceManager.GetString("GetUserByID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `id`, `login`,  `email`, `name`, `surname`, `address`, `townID`, `country`, `birthdate`, `sex`, `telephone`,  `comment`, `passwordQuestion`,  `isApproved`, `lastActivityDate`, `lastLoginDate`, `lastPasswordChangedDate`, `creationDate`,  `isLockedOut`, `lastLockedOutDate`  FROM `users` WHERE `login` = @login.
        /// </summary>
        internal static string GetUserByLogin {
            get {
                return ResourceManager.GetString("GetUserByLogin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Count(*) FROM `users` WHERE `email` LIKE @email AND `applicationName` = @applicationName.
        /// </summary>
        internal static string GetUserCountByEmail {
            get {
                return ResourceManager.GetString("GetUserCountByEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Count(*) FROM `users` WHERE `login` LIKE @login AND `applicationName` = @applicationName.
        /// </summary>
        internal static string GetUserCountByLogin {
            get {
                return ResourceManager.GetString("GetUserCountByLogin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `login` FROM `users` WHERE `email` = @email.
        /// </summary>
        internal static string GetUserNameByEmail {
            get {
                return ResourceManager.GetString("GetUserNameByEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `passwordQuestion`, `isLockedOut` FROM `users` WHERE `login` = @login.
        /// </summary>
        internal static string GetUserQuestion {
            get {
                return ResourceManager.GetString("GetUserQuestion", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `login` FROM `users_in_roles` WHERE `rolename` = @rolename AND `applicationName` = @applicationName.
        /// </summary>
        internal static string GetUsersInRole {
            get {
                return ResourceManager.GetString("GetUsersInRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT COUNT(*) FROM `users_in_roles` WHERE `login` = @login AND `rolename` = @rolename AND `applicationName` = @applicationName.
        /// </summary>
        internal static string IsUserInRole {
            get {
                return ResourceManager.GetString("IsUserInRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE `users` SET `isLockedOut` = @isLockedOut, `lastLockedOutDate` = @lastLockedOutDate WHERE `login` = @login.
        /// </summary>
        internal static string LockOutUser {
            get {
                return ResourceManager.GetString("LockOutUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM `users_in_roles` WHERE `login` = @login AND `rolename` = @rolename AND `applicationName` = @applicationName.
        /// </summary>
        internal static string RemoveUsersFromRoles {
            get {
                return ResourceManager.GetString("RemoveUsersFromRoles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE `users` SET `password` = @password, `lastPasswordChangedDate` = @lastPasswordChangedDate WHERE `login` = @login AND `isLockedOut` = @isLockedOut.
        /// </summary>
        internal static string ResetPassword {
            get {
                return ResourceManager.GetString("ResetPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT COUNT(*) FROM `roles` WHERE `rolename` = @rolename AND `applicationName` = @applicationName.
        /// </summary>
        internal static string RoleExists {
            get {
                return ResourceManager.GetString("RoleExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE `users`  SET `failedPasswordAnswerAttemptCount` = @count WHERE `login` = @login.
        /// </summary>
        internal static string SetFailedPasswordAnswerAttemptCount {
            get {
                return ResourceManager.GetString("SetFailedPasswordAnswerAttemptCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE `users` SET `failedPasswordAttemptCount` = @count WHERE `login` = @login.
        /// </summary>
        internal static string SetFailedPasswordAttemptCount {
            get {
                return ResourceManager.GetString("SetFailedPasswordAttemptCount", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE `users` SET `isLockedOut` = @isLockedOut, `lastLockedOutDate` = @lastLockedOutDate WHERE `login` = @login.
        /// </summary>
        internal static string UnlockUser {
            get {
                return ResourceManager.GetString("UnlockUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE `users` SET `failedPasswordAnswerAttemptCount` = @count, `failedPasswordAnswerAttemptWindowStart` = @windowStart WHERE `login` = @login.
        /// </summary>
        internal static string UpdateFailedPasswordAnswerAttempt {
            get {
                return ResourceManager.GetString("UpdateFailedPasswordAnswerAttempt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE `users` SET `failedPasswordAttemptCount` = @count, `failedPasswordAttemptWindowStart` = @windowStart WHERE `login` = @login.
        /// </summary>
        internal static string UpdateFailedPasswordAttempt {
            get {
                return ResourceManager.GetString("UpdateFailedPasswordAttempt", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE `users` SET `name` = @name, `surname` = @surname, `address` = @address, `townID` = @townID, `country` = @country, `birthdate` = @birthdate, `sex` = @sex, `telephone` = @telephone, `comment` = @comment, `isApproved` = @isApproved  WHERE `login` = @login.
        /// </summary>
        internal static string UpdateUser {
            get {
                return ResourceManager.GetString("UpdateUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE `users` SET `lastActivityDate` = @lastActivityDate WHERE `id` = @id.
        /// </summary>
        internal static string UpdateUserActivityByID {
            get {
                return ResourceManager.GetString("UpdateUserActivityByID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE `users` SET `lastActivityDate` = @lastActivityDate WHERE `login` = @login.
        /// </summary>
        internal static string UpdateUserActivityByLogin {
            get {
                return ResourceManager.GetString("UpdateUserActivityByLogin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE `users` SET `lastLoginDate`= @lastLoginDate WHERE `login` = @login.
        /// </summary>
        internal static string UpdateUserLoginDate {
            get {
                return ResourceManager.GetString("UpdateUserLoginDate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `password`, `isApproved` FROM `users` WHERE `login` = @login AND `isLockedOut` = @isLockedOut.
        /// </summary>
        internal static string ValidateUser {
            get {
                return ResourceManager.GetString("ValidateUser", resourceCulture);
            }
        }
    }
}
