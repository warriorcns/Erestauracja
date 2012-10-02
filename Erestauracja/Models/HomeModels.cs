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
}