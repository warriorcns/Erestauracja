using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace Erestauracja.Models
{
    public class RegisterRestaurantModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Nazwa firmy")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Nazwa wyświetlana")]
        public string DisplayName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Adres lokalu")]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Town ID")]
        public string TownId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Kraj")]
        public string Country { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Numer telefonu")]
        public string Telephone { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Adres email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "NIP")]
        public string Nip { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "REGON")]
        public string Regon { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} musi zawierać przynajmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło terminala")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Powtórz hasło")]
        [Compare("Password", ErrorMessage = "Pola Hasło oraz Powtórz hasło nie są zgodne.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Czas dostawy")]
        public string DeliveryTime { get; set; }
    }

    public class EditRestaurantModel
    {
        [Required]
        [DataType(DataType.Html)]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Nazwa firmy")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Nazwa wyświetlana")]
        public string DisplayName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Adres lokalu")]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Town ID")]
        public string TownId { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Kraj")]
        public string Country { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Numer telefonu")]
        public string Telephone { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Adres email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "NIP")]
        public string Nip { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "REGON")]
        public string Regon { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Czas dostawy")]
        public string DeliveryTime { get; set; }
    }

    public class TestModel
    {
        [DataType(DataType.Html)]
        public string Html { get; set; }
    }
}