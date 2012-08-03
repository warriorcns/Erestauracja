using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Erestauracja.Models
{
    public class modeltest
    {
        [Display(Name = "User name")]
        public string Name { get; set; }
    }
}