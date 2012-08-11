using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace Erestauracja.Models
{
    //sprawdzić poprawność
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Confirm email address")]
        [Compare("Email", ErrorMessage = "The email and confirmation email do not match.  po PL napisac  ")]
        public string ConfirmEmail { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.  po PL napisac  ", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.   po PL napisac  ")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Question")]
        public string Question { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Answer")]
        public string Answer { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Surname ")]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Address ")]
        public string Address { get; set; }

        //zmienić 
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "TownID")]
        public string TownID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Country  ")]
        public string Country { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Birthdate ")]
        public DateTime Birthdate { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Sex")]
        public string Sex { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telephone ")]
        public string Telephone { get; set; }
    }
}
