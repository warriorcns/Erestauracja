﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Erestauracja.Controllers
{
    [Authorize(Roles = "Klient, Menadżer, PracownikFul, PracownikLow")]
    public class BasketController : Controller
    {
        //
        // GET: /Basket/

        public ActionResult Index()
        {
            return View();
        }

    }
}
