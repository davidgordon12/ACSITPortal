using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ACSITPortal.Services;
using ACSITPortal.Helpers;
using ACSITPortal.Entities;
using ACSITPortal.Models;

namespace ACSITPortal.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserService _userService;
        private readonly SessionManager _sessionManager;

        public UsersController(UserService userService, SessionManager sessionManager)
        {
            _userService = userService;
            _sessionManager = sessionManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel user)
        {
            // Convert our front-end SignupModel to the User model 
            // we want to store in the database
            User _user = new User
            {
                UserLogin = user.UserLogin,
                UserPassword = user.UserPassword
            };

            // If our Login method returned false for any reason,
            // return the view with a generic error
            if (!_userService.CreateUser(_user))
            {
                ViewBag.SignupError = "Incorrect Username / Password";
                return View();
            }

            // Otherwise, create a session and
            // return the user to the home page
            _sessionManager.CreateUserSession(_user);
            return RedirectToAction("Index");
        }

        public IActionResult Signup()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Signup(SignupViewModel user)
        {
            // Convert our front-end SignupModel to the User model 
            // we want to store in the database
            User _user = new User
            {
                UserLogin = user.UserLogin,
                UserPassword = user.UserPassword
            };

            // If our CreateUser method returned false for any reason,
            // return the view with a generic error
            if (!_userService.CreateUser(_user))
            {
                ViewBag.SignupError = "Username is taken";
                return View();
            }

            // Otherwise, create a session and
            // return the user to the home page
            _sessionManager.CreateUserSession(_user);
            return RedirectToAction("Index");
        }
    }
}
