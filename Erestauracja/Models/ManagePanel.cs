using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erestauracja.Models
{
    public class RegisterRestaurantModel
    {
        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Login")]
        public string Login { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Adres email (powiązany z PayPal)")]
        public string Email { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Powtórz email")]
        [Compare("Email", ErrorMessage = "Pola Email oraz Powtórz email nie są zgodne.")]
        public string ConfirmEmail { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [StringLength(100, ErrorMessage = "{0} musi zawierać przynajmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Powtórz hasło")]
        [Compare("Password", ErrorMessage = "Pola Hasło oraz Powtórz hasło nie są zgodne.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Pytanie do przywracania hasła")]
        public string Question { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Odpowiedz")]
        public string Answer { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Nazwa firmy")]
        public string Name { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Nazwa wyświetlana")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Adres lokalu")]
        public string Address { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Miasto")]
        public string Town { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Kraj")]
        public string Country { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Numer telefonu")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "NIP")]
        public string Nip { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "REGON")]
        public string Regon { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Time)]
        [Display(Name = "Czas dostawy")]
        public string DeliveryTime { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Cena dostawy")]
        public string DeliveryPrice { get; set; }

    }

    public class EditRestaurantModel
    {
        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Html)]
        public int Id { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Nazwa firmy")]
        public string Name { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Nazwa wyświetlana")]
        public string DisplayName { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Adres lokalu")]
        public string Address { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Miasto")]
        public string Town { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Kod pocztowy")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Kraj")]
        public string Country { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Numer telefonu")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "NIP")]
        public string Nip { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "REGON")]
        public string Regon { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Czas dostawy")]
        public string DeliveryTime { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Cena dostawy")]
        public string DeliveryPrice { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [Display(Name = "Widoczna dla klientów")]
        public bool IsEnabled { get; set; }
    }

    public class ChangeResPasswordModel
    {
        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Password)]
        [Display(Name = "Dotychczasowe hasło")]
        public string OldPassword { get; set; }

        //ustawione 6 na stałe jak sie zdecydujemy to trzeba ustawić
        [Required(ErrorMessage = "To pole jest wymagane.")]
        [StringLength(100, ErrorMessage = "{0} musi zawierać przynajmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nowe hasło")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Powtórz hasło")]
        [Compare("NewPassword", ErrorMessage = "Pola Hasło oraz Powtórz hasło nie są zgodne.")]
        public string ConfirmPassword { get; set; }

        [HiddenInput]
        public string Login { get; set; }
    }

    public class MainPageModel
    {
        [HiddenInput]
        public int RestaurantID { get; set; }

        [AllowHtml]
        [DataType(DataType.Html)]
        public string Description { get; set; }

        [AllowHtml]
        [DataType(DataType.Html)]
        public string Foto { get; set; }

        [AllowHtml]
        [DataType(DataType.Html)]
        public string SpecialOffers { get; set; }

        //[AllowHtml]
        //[DataType(DataType.Html)]
        public images File { get; set; }

        public List<images> Files { get; set; }
        
        
    }

    public class images
    {
        public String name { get; set; }
        public Uri link { get; set; }

    }

    public class DeliveryPageModel
    {
        [HiddenInput]
        public int RestaurantID { get; set; }

        [AllowHtml]
        [DataType(DataType.Html)]
        public string Delivery { get; set; }
    }

    public class EventsPageModel
    {
        [HiddenInput]
        public int RestaurantID { get; set; }

        [AllowHtml]
        [DataType(DataType.Html)]
        public string Events { get; set; }
    }

    public class ContactPageModel
    {
        [HiddenInput]
        public int RestaurantID { get; set; }

        [AllowHtml]
        [DataType(DataType.Html)]
        public string Contact { get; set; }
    }

    public class CategoryModel
    {
        [HiddenInput]
        public int RestaurantID { get; set; }

        [HiddenInput]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Nazwa kategori")]
        public string CategoryName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Opis")]
        public string CategoryDescription { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Opcja wpływająca na cene")]
        public string PriceOption { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Opcja niewpływająca na cene")]
        public string NonPriceOption { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Opcja niewpływająca na cene")]
        public string NonPriceOption2 { get; set; }
    }

    public class AddProductModel
    {
        [HiddenInput]
        public int RestaurantID { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Nazwa produktu")]
        public string ProductName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Opis")]
        public string ProductDescription { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Kategoria")]
        public int Category { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Cena")]
        public string Price { get; set; }
    }

    public class MenuModel
    {
        [HiddenInput]
        public int RestaurantID { get; set; }

        public List<Erestauracja.ServiceReference.Category> Kategorie { get; set; }
        public List<Erestauracja.ServiceReference.Menu> Menu { get; set; }
    }

    public class ClientMenuModel
    {
        [HiddenInput]
        public int RestaurantID { get; set; }

        public List<Erestauracja.ServiceReference.Menu> Menu { get; set; }
    }

    public class ProductModel
    {
        [HiddenInput]
        public int ProductId { get; set; }

        [HiddenInput]
        public int RestaurantID { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Nazwa produktu")]
        public string ProductName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Opis")]
        public string ProductDescription { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Kategoria")]
        public int Category { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [DataType(DataType.Text)]
        [Display(Name = "Cena")]
        public string Price { get; set; }

        [Display(Name = "Dostępny")]
        public bool isAvailable { get; set; }
    }

    public class EmployeePasswordModel
    {
        [HiddenInput]
        public string EmployeeLogin { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [StringLength(100, ErrorMessage = "{0} musi zawierać przynajmniej {2} znaków.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Powtórz hasło")]
        [Compare("Password", ErrorMessage = "Pola Hasło oraz Powtórz hasło nie są zgodne.")]
        public string ConfirmPassword { get; set; }
    }

    public class ReportCommentModel
    {
        [HiddenInput]
        public int RestaurantId { get; set; }

        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public string UserLogin { get; set; }

        [HiddenInput]
        public string DisplayName { get; set; }

        [HiddenInput]
        public string Address { get; set; }

        [HiddenInput]
        public string Town { get; set; }

        [HiddenInput]
        public string Postal { get; set; }

        [HiddenInput]
        public double Rating { get; set; }

        [HiddenInput]
        public string Comment { get; set; }

        [HiddenInput]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane.")]
        [Display(Name = "Powód")]
        public string Report { get; set; }
    }
}