using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using System.Threading;

namespace Erestauracja.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Dotychczasowe hasło")]
        public string OldPassword { get; set; }

        //ustawione 6 na stałe jak sie zdecydujemy to trzeba ustawić
        [Required]
        [StringLength(100, ErrorMessage = "{0} musi zawierać przynajmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nowe hasło")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Powtórz hasło")]
        [Compare("NewPassword", ErrorMessage = "Pola Hasło oraz Powtórz hasło nie są zgodne.")]
        public string ConfirmPassword { get; set; }
    }

    public class LogOnModel
    {
        [Required]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Display(Name = "Zapamiętaj mnie.")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {

        CultureInfo polish = new CultureInfo("pl-PL");
        
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Powtórz email")]
        [Compare("Email", ErrorMessage = "Pola Email oraz Powtórz email nie są zgodne.")]
        public string ConfirmEmail { get; set; }

        //ustawione 6 na stałe jak sie zdecydujemy to trzeba ustawić
        [Required]
        [StringLength(100, ErrorMessage = "{0} musi zawierać przynajmniej {2} znaków.", MinimumLength=6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Powtórz hasło")]
        [Compare("Password", ErrorMessage = "Pola Hasło oraz Powtórz hasło nie są zgodne.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Pytanie do przywracania hasła")]
        public string Question { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Odpowiedz")]
        public string Answer { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Imię")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Adres ")]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Miasto")]
        public string Town { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Kraj")]
        public string Country { get; set; }


        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Data urodzenia")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Birthdate { get; set; }
        
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Płeć")]
        public string Sex { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Numer telefonu")]
        public string Telephone { get; set; }

        
    }

    public class UserDataModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Imię")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Nazwisko")]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Adres ")]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Miasto")]
        public string Town { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Kraj")]
        public string Country { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Data urodzenia")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Birthdate { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Płeć")]
        public string Sex { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Numer telefonu")]
        public string Telephone { get; set; }
    }

    public class UserRoleModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "RoleName")]
        public string RoleName { get; set; }

        [Display(Name = "Roles")]
        public List<string> Roles { get; set; }
    }
}
