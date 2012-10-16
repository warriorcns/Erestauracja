﻿using System;
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

        [Required]
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

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Nazwa produktu")]
        public string ProductName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Opis")]
        public string ProductDescription { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Kategoria")]
        public int Category { get; set; }

        [Required]
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

    public class ProductModel
    {
       // string priceOption = null;
      //  DateTime creationDate = new DateTime();
      //  bool isAvailable = false;
      //  bool isEnabled = false;

        [HiddenInput]
        public int ProductId { get; set; }

        [HiddenInput]
        public int RestaurantID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Nazwa produktu")]
        public string ProductName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Opis")]
        public string ProductDescription { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Kategoria")]
        public int Category { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Cena")]
        public string Price { get; set; }

        [Display(Name = "Dostępny")]
        public bool isAvailable { get; set; }
    }
}