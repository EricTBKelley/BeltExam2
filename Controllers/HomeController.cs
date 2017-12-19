using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BeltExam2.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace BeltExam2.Controllers
{
    public class HomeController : Controller
    {
        private BeltExam2Context _context;
        public HomeController(BeltExam2Context context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("Home/Register")]

        public IActionResult Register(UserViewModel model)
        {
            User CheckUser = _context.Users.SingleOrDefault(user => user.Email == model.Email);
            if (CheckUser != null)
            {
                ViewBag.err = "Email is already in use.";
            }
            else if (ModelState.IsValid)
            {
                User NewUser = new User();
                NewUser.Name = model.Name;
                NewUser.Email = model.Email;
                NewUser.Password = model.Password;
                NewUser.Description = model.Description;
                _context.Users.Add(NewUser);
                _context.SaveChanges();
                User LoggedUser = _context.Users.SingleOrDefault(user => user.Email == model.Email);
                HttpContext.Session.SetInt32("UserID", LoggedUser.UserID);
                return RedirectToAction("Dashboard", "Network");
            }
            return View("Index");
        }
        [HttpPost]
        [Route("Home/LogIn")]

        public IActionResult LogIn(string Email, string Password)
        {
            User CheckUser = _context.Users.SingleOrDefault(user => user.Email == Email);
            if (CheckUser != null)
            {
                if (CheckUser.Password == Password)
                {
                    HttpContext.Session.SetInt32("UserID", CheckUser.UserID);
                    return RedirectToAction("Dashboard", "Network");
                }
            }
            ViewBag.err = "Email and/or Password are incorrect.";
            return View("Index");
        }

        [HttpPost]
        [Route("Home/LogOut")]

        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

    }
}
