using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Erestauracja.Models
{
    public class UserLogin
    {
        public string Token { get; set; }
        public string Secret { get; set; }
    }
}