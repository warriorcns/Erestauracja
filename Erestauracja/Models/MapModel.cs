using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

namespace Erestauracja.Models
{
    public class MapModel
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string Title { get; set; }
        public int zIndex { get; set; }
        public string InfoWindowContent { get; set; }
    }


}