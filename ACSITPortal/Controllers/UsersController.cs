using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ACSITPortal.Services;
using ACSITPortal.Helpers;
using ACSITPortal.Entities;
using ACSITPortal.Models;
using System.Xml.Serialization;

namespace ACSITPortal.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserService _userService;
        private readonly PostService _postService;
        private readonly SessionManager _sessionManager;

        public UsersController(UserService userService, PostService postService, SessionManager sessionManager)
        {
            _userService = userService;
            _postService = postService;
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
            if (!_userService.Login(_user))
            {
                ViewBag.LoginError = "Incorrect Username / Password";
                return View();
            }

            // Otherwise, create a session and
            // return the user to the home page
            _sessionManager.CreateUserSession(_user);
            return RedirectToAction("Index", "Home");
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
                UserPassword = user.UserPassword,
                Phone = user.Phone,
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
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Profile()
        {
            List<Post> posts = _postService.GetPostsByUser(_sessionManager.GetUserSession().UserId);

            foreach(var post in posts)
            {
                /* If the post has 0 threads, create an empty list
                 * so .NET doesn't give us a null warning */
                if (post.Threads is null)
                    post.Threads = Enumerable.Empty<Entities.Thread>().ToList();
            }
            return View(posts);
        }

        public IActionResult DeletePost(int id)
        {
            _postService.DeletePost(id);
            return RedirectToAction("Profile");
        }

        public IActionResult EditPost(int id)
        {
            var post = _postService.GetPostById(id);

            if (post is null)
                return NotFound();

            return View(post);
        }

        [HttpPost]
        public IActionResult EditPost(Post post)
        {
            _postService.UpdatePost(post);
            return RedirectToAction("Profile");
        }

        public IActionResult Logout()
        {
            _sessionManager.ClearUserSessions();
            return RedirectToAction("Index", "Home");
        }
    }
}
