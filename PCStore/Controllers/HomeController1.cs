﻿using Microsoft.AspNetCore.Mvc;

namespace PCStore.Controllers
{
    public class HomeController1 : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
