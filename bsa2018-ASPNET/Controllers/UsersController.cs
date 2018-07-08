using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using bsa2018_ASPNET.Services;

namespace bsa2018_ASPNET.Controllers
{
    public class UsersController : Controller
    {
        private UsersService users;

        public UsersController(UsersService users)
        {
            this.users = users;
        }

        public IActionResult Index()
        {
            return View(users.Users);
        }

        public new IActionResult User(int id)
        {
            try
            {
                return View(users.Users.First(u => u.Id == id));
            }
            catch (Exception)
            {
                return Content("Incorrect id user");
            }
        }

        public IActionResult Post(int id)
        {
            try
            {
                return View(users.Users.SelectMany(u => u.Posts).First(p => p.Id == id));
            }
            catch (Exception)
            {
                return Content("Incorrect id post");
            }

        }

        public IActionResult ToDo(int id)
        {
            try
            {
                return View(users.Users.SelectMany(u => u.ToDos).First(td => td.Id == id));
            }
            catch (Exception)
            {
                return Content("Incorrect id toDo");
            }
        }
    }
}