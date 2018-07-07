﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bsa2018_ASPNET.Models;
using bsa2018_ASPNET.Services;

namespace bsa2018_ASPNET.Controllers
{
    public class HomeController : Controller
    {
        private UsersService users;

        public HomeController(UsersService users)
        {
            this.users = users;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult First()
        {
            return View();
        }

        [HttpPost]
        public IActionResult First(int userId)
        {
            try
            {
                var queryResult = users.FirstQuery(userId);
                return View(queryResult);
            }
            catch (Exception)
            {
                return Content("Id is not correct!");
            }
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
