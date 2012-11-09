using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
namespace Erestauracja.Models
{
    public class HomeModels
    {
        public string TownName { get; set; }
        public string RestaurantName { get; set; }
        public int RestaurantID { get; set; }
    }

    public class ErrorModels
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Treść zgłoszenia")]
        public string Text { get; set; }
    }
}