using System.ComponentModel.DataAnnotations;

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
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage="Wpisz treść zgłoszenia.")]
        [DataType(DataType.Text)]
        [Display(Name = "Treść zgłoszenia")]
        public string Text { get; set; }
    }
}