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
        ///   Looks up a localized string similar to INSERT INTO `category`(`restaurantId`, `categoryName`, `categoryDescription`, `priceOption`, `nonPriceOption`, `nonPriceOption2`) VALUES (@restaurantID,@categoryName,@categoryDescription,@priceOption,@nonPriceOption,@nonPriceOption2).
        /// </summary>
        internal static string AddCategory {
            get {
                return ResourceManager.GetString("AddCategory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO `restaurants`(`name`, `displayName`, `address`, `townId`, `countryId`, `telephone`, `email`, `nip`, `regon`, `creationData`, `inputsCount`, `averageRating`, `password`, `menagerId`, `deliveryTime`, `currentDeliveryTime`, `isApproved`, `lastActivityDate`, `isOnline`, `isLockedOut`, `lastLockedOutDate`) VALUES (@name, @displayName, @address, @townId, (SELECT `id` FROM `countries` WHERE `name` = @country), @telephone, @email, @nip, @regon, @creationData, @inputsCount, @averageRating, @password, (SE [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string AddRestaurant {
            get {
                return ResourceManager.GetString("AddRestaurant", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO `users_in_roles`(`userID`, `roleID`) VALUES ((SELECT `id` FROM `users` WHERE `login`=@login), (SELECT `id` FROM `roles` WHERE `rolename` = @rolename)).
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
        ///   Looks up a localized string similar to INSERT INTO `roles`(`rolename`) VALUES (@rolename).
        /// </summary>
        internal static string CreateRole {
            get {
                return ResourceManager.GetString("CreateRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to INSERT INTO `users`(`login`, `password`, `email`, `name`, `surname`, `address`, `townID`, `countryID`, `birthdate`, `sex`, `telephone`, `comment`, `passwordQuestion`, `passwordAnswer`, `isApproved`, `lastActivityDate`, `lastLoginDate`, `lastPasswordChangedDate`, `creationDate`, `isOnLine`, `isLockedOut`, `lastLockedOutDate`, `failedPasswordAttemptCount`, `failedPasswordAttemptWindowStart`, `failedPasswordAnswerAttemptCount`, `failedPasswordAnswerAttemptWindowStart`) VALUES(@login, @password, @email, @name,  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string CreateUser {
            get {
                return ResourceManager.GetString("CreateUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM `category` WHERE `id`=@id AND `restaurantId`=@restaurantId.
        /// </summary>
        internal static string DeleteCategory {
            get {
                return ResourceManager.GetString("DeleteCategory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to DELETE FROM `roles` WHERE `rolename` = @rolename.
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
        ///   Looks up a localized string similar to DELETE FROM `users_in_roles` WHERE `roleID` = (SELECT `id` FROM `roles` WHERE `rolename` = @rolename).
        /// </summary>
        internal static string DeleteUsersInRole {
            get {
                return ResourceManager.GetString("DeleteUsersInRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE `category` SET `categoryName`=@categoryName,`categoryDescription`=@categoryDescription,`priceOption`=@priceOption,`nonPriceOption`=@nonPriceOption,`nonPriceOption2`=@nonPriceOption2 WHERE `restaurantId`=@restaurantId AND `id` = @id.
        /// </summary>
        internal static string EditCategory {
            get {
                return ResourceManager.GetString("EditCategory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE `rest_page_content` SET `contact`=@contact WHERE `menagerId` = (SELECT `id` FROM `users` WHERE `login` = @managerLogin) AND `restaurantId`=@id.
        /// </summary>
        internal static string EditContactPage {
            get {
                return ResourceManager.GetString("EditContactPage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE `rest_page_content` SET `delivery`=@delivery WHERE `menagerId` = (SELECT `id` FROM `users` WHERE `login` = @managerLogin) AND `restaurantId`=@id.
        /// </summary>
        internal static string EditDeliveryPage {
            get {
                return ResourceManager.GetString("EditDeliveryPage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE `rest_page_content` SET `events`=@events WHERE `menagerId` = (SELECT `id` FROM `users` WHERE `login` = @managerLogin) AND `restaurantId`=@id.
        /// </summary>
        internal static string EditEventsPage {
            get {
                return ResourceManager.GetString("EditEventsPage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE `rest_page_content` SET `description`=@description,`foto`=@foto,`specialOffers`=@specialOffers WHERE `menagerId` = (SELECT `id` FROM `users` WHERE `login` = @managerLogin) AND `restaurantId`=@id.
        /// </summary>
        internal static string EditMainPage {
            get {
                return ResourceManager.GetString("EditMainPage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to UPDATE `restaurants` SET `name`=@name,`displayName`=@displayName,`address`=@address,`townId`=@townId,`countryId`= (SELECT `id` FROM `countries` WHERE `name` = @country),`telephone`=@telephone,`email`=@email,`nip`=@nip,`regon`=@regon,`deliveryTime`=@deliveryTime WHERE `menagerId`= (SELECT `id` FROM `users` WHERE `login` = @menager) AND `id`=@id AND `isLockedOut`=@isLockedOut.
        /// </summary>
        internal static string EditRestaurant {
            get {
                return ResourceManager.GetString("EditRestaurant", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `login`, `email`, `passwordQuestion`, `comment`, `isApproved`, `isLockedOut`, `creationDate`, `lastLoginDate`,`lastActivityDate`, `lastPasswordChangedDate`, `llastLockedOutDate` FROM `users` WHERE `email` LIKE @email ORDER BY `login` Asc.
        /// </summary>
        internal static string FindUsersByEmail {
            get {
                return ResourceManager.GetString("FindUsersByEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `id`, `login`,  `email`, `name`, `surname`, `address`, `townID`, `country`, `birthdate`, `sex`, `telephone`,  `comment`, `passwordQuestion`,  `isApproved`, `lastActivityDate`, `lastLoginDate`, `lastPasswordChangedDate`, `creationDate`,  `isLockedOut`, `lastLockedOutDate`   FROM `users` WHERE `login` LIKE @login ORDER BY `login` Asc.
        /// </summary>
        internal static string FindUsersByName {
            get {
                return ResourceManager.GetString("FindUsersByName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `login` FROM `users` WHERE `id` IN (SELECT `userID` FROM `users_in_roles` WHERE `userID` IN (SELECT `id` FROM `users` WHERE `login` LIKE @login) AND `roleID` = (SELECT `id` FROM `roles` WHERE `rolename` = @rolename)).
        /// </summary>
        internal static string FindUsersInRole {
            get {
                return ResourceManager.GetString("FindUsersInRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `rolename` FROM `roles`.
        /// </summary>
        internal static string GetAllRoles {
            get {
                return ResourceManager.GetString("GetAllRoles", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT u.`id`, u.`login`, u.`email`, u.`name`, u.`surname`, u.`address`, t.`town_name`, t.`postal_code`,c.`name`, u.`birthdate`, u.`sex`, u.`telephone`, u.`comment`, u.`passwordQuestion`, u.`isApproved`, u.`lastActivityDate`, u.`lastLoginDate`, u.`lastPasswordChangedDate`, u.`creationDate`, u.`isLockedOut`, u.`lastLockedOutDate` FROM `users` u JOIN `countries` c ON u.`countryID` = c.`id` JOIN `towns` t ON u.`townID` = t.`id` ORDER BY  u.`login` ASC.
        /// </summary>
        internal static string GetAllUsers {
            get {
                return ResourceManager.GetString("GetAllUsers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `id`, `restaurantId`, `categoryName`, `categoryDescription`, `priceOption`, `nonPriceOption`, `nonPriceOption2` FROM `category` WHERE `restaurantId` = @restaurantId.
        /// </summary>
        internal static string GetCategories {
            get {
                return ResourceManager.GetString("GetCategories", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `id`, `restaurantId`, `categoryName`, `categoryDescription`, `priceOption`, `nonPriceOption`, `nonPriceOption2` FROM `category` WHERE `restaurantId` = @restaurantId AND `id` = @id.
        /// </summary>
        internal static string GetCategory {
            get {
                return ResourceManager.GetString("GetCategory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `contact` FROM `rest_page_content` WHERE `menagerId` = (SELECT `id` FROM `users` WHERE `login` = @managerLogin) AND `restaurantId`=@id.
        /// </summary>
        internal static string GetContactPage {
            get {
                return ResourceManager.GetString("GetContactPage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `name` FROM `countries`.
        /// </summary>
        internal static string GetCountriesList {
            get {
                return ResourceManager.GetString("GetCountriesList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `delivery` FROM `rest_page_content` WHERE `menagerId` = (SELECT `id` FROM `users` WHERE `login` = @managerLogin) AND `restaurantId`=@id.
        /// </summary>
        internal static string GetDeliveryPage {
            get {
                return ResourceManager.GetString("GetDeliveryPage", resourceCulture);
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
        ///   Looks up a localized string similar to SELECT `events` FROM `rest_page_content` WHERE `menagerId` = (SELECT `id` FROM `users` WHERE `login` = @managerLogin) AND `restaurantId`=@id.
        /// </summary>
        internal static string GetEventsPage {
            get {
                return ResourceManager.GetString("GetEventsPage", resourceCulture);
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
        ///   Looks up a localized string similar to SELECT `description`, `foto`, `specialOffers` FROM `rest_page_content` WHERE `menagerId` = (SELECT `id` FROM `users` WHERE `login` = @managerLogin) AND `restaurantId`=@id.
        /// </summary>
        internal static string GetMainPage {
            get {
                return ResourceManager.GetString("GetMainPage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `id`, `postal_code`, `town_name`, `province`, `district`, `community`, `latitude`, `longitude` FROM `towns` WHERE `town_name` REGEXP @townName OR `postal_code` = @postalCode.
        /// </summary>
        internal static string GetMoreMoreTowns {
            get {
                return ResourceManager.GetString("GetMoreMoreTowns", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `id`, `postal_code`, `town_name`, `province`, `district`, `community`, `latitude`, `longitude` FROM `towns` WHERE `town_name` LIKE @townName AND `postal_code` = @postalCode.
        /// </summary>
        internal static string GetMoreTowns {
            get {
                return ResourceManager.GetString("GetMoreTowns", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Count(*) FROM `users` WHERE `lastActivityDate` &gt; @lastActivityDate OR `lastLoginDate` &gt; @lastActivityDate.
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
        ///   Looks up a localized string similar to SELECT x.`id`, x.`name`, x.`displayName`, x.`address`, t.`town_name`, t.`postal_code`, y.`name`, x.`telephone`, x.`email`, x.`nip`, x.`regon`, x.`deliveryTime` FROM `restaurants` x JOIN `countries` y ON x.`countryId` = y.`id` JOIN `towns` t ON x.`townId` = t.`id` WHERE `menagerId` = (SELECT `id` FROM `users` WHERE `login` = @managerLogin) AND x.`id` = @id.
        /// </summary>
        internal static string GetRestaurant {
            get {
                return ResourceManager.GetString("GetRestaurant", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT x.`id`, x.`displayName`, y.`town_name` FROM `restaurants` x JOIN `towns` y ON x.`townId` = y.`id` WHERE y.`town_name` LIKE @townName.
        /// </summary>
        internal static string GetRestaurantByTown {
            get {
                return ResourceManager.GetString("GetRestaurantByTown", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT x.`id`, x.`name`, x.`displayName`, x.`address`, t.`town_name`, t.`postal_code`, y.`name`, x.`telephone`, x.`email`, x.`nip`, x.`regon`, x.`creationData`, x.`inputsCount`, x.`averageRating`, x.`password`, x.`menagerId`, x.`deliveryTime`, x.`currentDeliveryTime`, x.`isApproved`, x.`lastActivityDate`, x.`isLockedOut`, x.`lastLockedOutDate` FROM `restaurants` x JOIN `countries` y ON x.`countryId` = y.`id` JOIN `towns` t ON x.`townId` = t.`id` WHERE `menagerId` = (SELECT `id` FROM `users` WHERE `login` =  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string GetRestaurantsByManagerLogin {
            get {
                return ResourceManager.GetString("GetRestaurantsByManagerLogin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `rolename` FROM `roles` WHERE `id` IN (SELECT `roleID` FROM `users_in_roles` WHERE `userID` = (SELECT `id` FROM `users` WHERE `login` = @login)).
        /// </summary>
        internal static string GetRolesForUser {
            get {
                return ResourceManager.GetString("GetRolesForUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT `id`, `postal_code`, `town_name`, `province`, `district`, `community`, `latitude`, `longitude` FROM `towns` WHERE `town_name` LIKE @townName AND `postal_code` = @postalCode.
        /// </summary>
        internal static string GetTowns {
            get {
                return ResourceManager.GetString("GetTowns", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT u.`id`, u.`login`, u.`email`, u.`name`, u.`surname`, u.`address`, t.`town_name`, t.`postal_code`, c.`name`, u.`birthdate`, u.`sex`, u.`telephone`, u.`comment`, u.`passwordQuestion`, u.`isApproved`, u.`lastActivityDate`, u.`lastLoginDate`, u.`lastPasswordChangedDate`, u.`creationDate`, u.`isLockedOut`, u.`lastLockedOutDate` FROM `users` u JOIN `countries` c
        ///ON u.`countryID` = c.`id` JOIN `towns` t ON u.`townID` = t.`id` WHERE u.`id` = @id.
        /// </summary>
        internal static string GetUserByID {
            get {
                return ResourceManager.GetString("GetUserByID", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT u.`id`, u.`login`, u.`email`, u.`name`, u.`surname`, u.`address`, t.`town_name`, t.`postal_code`, c.`name`, u.`birthdate`, u.`sex`, u.`telephone`, u.`comment`, u.`passwordQuestion`, u.`isApproved`, u.`lastActivityDate`, u.`lastLoginDate`, u.`lastPasswordChangedDate`, u.`creationDate`, u.`isLockedOut`, u.`lastLockedOutDate` FROM `users` u JOIN `countries` c
        ///ON u.`countryID` = c.`id` JOIN `towns` t ON u.`townID` = t.`id` WHERE u.`login` = @login.
        /// </summary>
        internal static string GetUserByLogin {
            get {
                return ResourceManager.GetString("GetUserByLogin", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Count(*) FROM `users` WHERE `email` LIKE @email.
        /// </summary>
        internal static string GetUserCountByEmail {
            get {
                return ResourceManager.GetString("GetUserCountByEmail", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT Count(*) FROM `users` WHERE `login` LIKE @login.
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
        ///   Looks up a localized string similar to SELECT `login` FROM `users` WHERE `id` IN (SELECT `userID` FROM `users_in_roles` WHERE `roleID` = (SELECT `id` FROM `roles` WHERE `rolename` = @rolename)).
        /// </summary>
        internal static string GetUsersInRole {
            get {
                return ResourceManager.GetString("GetUsersInRole", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT 0 FROM `restaurants` WHERE `id` = @restaurantID AND `menagerId` = (SELECT `id` FROM `users` WHERE `login` LIKE @managerLogin).
        /// </summary>
        internal static string IsRestaurantOwner {
            get {
                return ResourceManager.GetString("IsRestaurantOwner", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SELECT COUNT(*) FROM `users_in_roles` WHERE `userID` = (SELECT `id` FROM `users` WHERE `login`=@login) AND `roleID` = (SELECT `id` FROM `roles` WHERE `rolename` = @rolename).
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
        ///   Looks up a localized string similar to DELETE FROM `users_in_roles` WHERE `userID` = (SELECT `id` FROM `users` WHERE `login`=@login) AND `roleID` = (SELECT `id` FROM `roles` WHERE `rolename` = @rolename).
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
        ///   Looks up a localized string similar to SELECT COUNT(*) FROM `roles` WHERE `rolename` = @rolename.
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
        ///   Looks up a localized string similar to UPDATE `users` SET `name` = @name, `surname` = @surname, `address` = @address, `townID` = (SELECT `id` FROM `towns` WHERE `town_name` LIKE @town_name AND `postal_code` LIKE @postal_code), `countryID` = (SELECT `id` FROM `countries` WHERE `name`=@country), `birthdate` = @birthdate, `sex` = @sex, `telephone` = @telephone, `comment` = @comment, `isApproved` = @isApproved  WHERE `login` = @login.
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
