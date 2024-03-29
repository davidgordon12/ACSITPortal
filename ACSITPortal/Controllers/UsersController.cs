﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ACSITPortal.Services;
using ACSITPortal.Helpers;
using ACSITPortal.Entities;
using ACSITPortal.Models;
using System.Xml.Serialization;
using System.Security.Cryptography;
using NuGet.Common;

namespace ACSITPortal.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserService _userService;
        private readonly PostService _postService;
        private readonly CommentService _commentService;
        private readonly SessionManager _sessionManager;

        public UsersController(UserService userService, PostService postService, CommentService commentService, SessionManager sessionManager)
        {
            _userService = userService;
            _postService = postService;
            _commentService = commentService;
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
                _sessionManager.SetLoginAttempts();
                return View();
            }

            if(_sessionManager.GetLoginAttempts() > 3)
            {
                ViewBag.LoginError = "Too many login attempts! Try again later";
                return View();
            }

            // Otherwise, create a session and
            // return the user to the home page
            _sessionManager.CreateUserSession(_user);
            return RedirectToAction("Profile");
        }

        public IActionResult Signup()
        {
            ViewBag.SignupError = "This username is taken!";
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
                Email = user.Email,
            };

            // If our CreateUser method returned false for any reason,
            // return the view with a generic error
            if (!_userService.CreateUser(_user))
            {
                ViewBag.SignupError = "Username or email is taken";
                return View();
            }

            return RedirectToAction("VerifyUser", new { email = _user.Email });
        }

        public IActionResult VerifyUser(string email)
        {
            _userService.RequestVerification(email);
            return View();
        }

        [HttpPost]
        public IActionResult VerifyUser(VerifyUserViewModel verify)
        {
            var user = _userService.GetUserByEmail(verify.Email);

            if (user is null)
                return NotFound();

            if (user.ActionToken == verify.Token)
                _userService.VerifyUser(user);

            // We need to tell the session that the user is now verified
            _sessionManager.CreateUserSession(user);

            return RedirectToAction("Profile");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            if (email is null)
                return View();

            var user = _userService.GetUserByEmail(email);

            if(user is null)
            {
                ViewBag.ErrMessage = "There is no account that matches this email.";
                return View();
            }

            _userService.RequestResetPassword(user);

            return RedirectToAction("ResetPassword");
        }

        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel resetPasswordViewModel)
        {
            if(resetPasswordViewModel.Token is null)
            {
                ViewBag.ErrMessage = "Please enter a token";
                return View();
            }

            var user = _userService.GetUserByToken(resetPasswordViewModel.Token);

            if (user is null)
            {
                ViewBag.ErrMessage = "Token does not match";
                return View();
            }

            if(DateTime.Now > user.TokenExpires)
            {
                ViewBag.ErrMessage = "Token has expired.";
                return View();
            }

            if(!_userService.ResetPassword(user, resetPasswordViewModel.NewPassword))
            {
                ViewBag.ErrMessage = "Oops! Something went wrong, try again later.";
                return View();
            }

            return RedirectToAction("Login");
        }

        public IActionResult Profile()
        {
            List<Post> posts = _postService.GetPostsByUser(_sessionManager.GetUserSession().UserId);

            foreach(var post in posts)
            {
                post.Threads = _commentService.GetThreadsByPostId(post.PostId);

                /* If the post has 0 threads, create an empty list
                 * so .NET doesn't give us a null warning */
                if (post.Threads is null)
                    post.Threads = Enumerable.Empty<Entities.Thread>().ToList();
            }

            var user = _userService.GetUserById(_sessionManager.GetUserSession().UserId);

            user.Messages = _userService.GetMessages(user.UserId);

            ProfileViewModel profileViewModel = new ProfileViewModel
            {
                Posts = posts,
                Messages = user.Messages
            };

            return View(profileViewModel);
        }

        public IActionResult CreatePost()
        {
            if (_sessionManager.GetUserSession() is null)
            {
                ViewBag.ErrMessage = "";
                return RedirectToAction("Login");
            }
            
            var user = _sessionManager.GetUserSession();

            if(user.Verified == false)
            {
                return RedirectToAction("VerifyUser", new { email = _sessionManager.GetUserSession().Email });
            }

            ViewBag.ErrMessage = "";
            return View();
        }

        [HttpPost]
        public IActionResult CreatePost(Post post)
        {
            if (_sessionManager.GetUserSession() is null)
            {
                ViewBag.ErrMessage = "";
                return RedirectToAction("Login");
            }

            post.UserId = _sessionManager.GetUserSession().UserId;

            if(!_postService.CreatePost(post))
            {
                ViewBag.ErrMessage = "Could not create post. Please note, you may only have 5 posts at a time";
                return View();
            }

            return RedirectToAction("Profile");
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

        public IActionResult ReportPost(int id)
        {
            if (_sessionManager.GetUserSession() is null)
            {
                return View();
            }

            if (_sessionManager.GetUserSession().Verified == false)
            {
                return RedirectToAction("VerifyUser", new { email = _sessionManager.GetUserSession().Email });
            }

            /* We need to get the post here so we can
             * add a report to it */
            var post = _postService.GetPostById(id);

            if (post is null)
                return NotFound();

            if (post.Reports is null)
                post.Reports = 1;
            else
                post.Reports += 1;

            _postService.UpdatePost(post);

            var postModel = new PostViewModel
            {
                Post = post,
                User = _userService.GetUserById(post.UserId),
                Threads = _commentService.GetThreadsByPostId(post.PostId),
            };

            ViewBag.InfoMessage = "The post has been reported";

            return View(postModel);
        }

        [HttpGet]
        public IActionResult CreateThread(int id)
        {
            if (_sessionManager.GetUserSession() is null)
            {
                ViewBag.ErrMessage = "";
                return RedirectToAction("Login");
            }


            if (_sessionManager.GetUserSession().Verified == false)
            {
                return RedirectToAction("VerifyUser", new { email = _sessionManager.GetUserSession().Email });
            }

            Entities.Thread thread = new Entities.Thread();
            thread.PostId = id;
            return View(thread);
        }

        [HttpPost]
        public IActionResult CreateThread(Entities.Thread thread)
        {
            if (!_commentService.CreateThread(thread))
            {
                ViewBag.ErrMessage = "There was an error creating your thread";
                return View(thread);
            }

            return RedirectToAction("ViewPost", "Home", new { id = thread.PostId });
        }

        [HttpGet]
        public IActionResult CreateReply(int id)
        {
            if (_sessionManager.GetUserSession() is null)
            {
                ViewBag.ErrMessage = "";
                return RedirectToAction("Login");
            }

            if (_sessionManager.GetUserSession().Verified == false)
            {
                return RedirectToAction("VerifyUser", new { email = _sessionManager.GetUserSession().Email });
            }

            Reply reply = new Reply();
            reply.ThreadId = id;
            return View(reply);
        }

        [HttpPost]
        public IActionResult CreateReply(Reply reply)
        {
            if (!_commentService.CreateReply(reply))
            {
                ViewBag.ErrMessage = "There was an error creating your reply";
                return View(reply);
            }

            var thread = _commentService.GetThreadById(reply.ThreadId);

            return RedirectToAction("ViewPost", "Home", new { id = thread.PostId } );
        }

        public IActionResult SendMessage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SendMessage(SendMessageViewModel messageViewModel)
        {
            var user = _userService.GetUserByName(messageViewModel.Recepient);

            if(user is null)
            {
                ViewBag.ErrMessage = "Could not find the user specified";
                return View();
            }

            Message message = new Message
            {
                RecepientId = user.UserId,
                SenderId = _sessionManager.GetUserSession().UserId,
                MessageTitle = messageViewModel.Subject,
                MessageContent = messageViewModel.Content,
                MessageSentDate = DateTime.Now,
            };

            _userService.SendMessage(message, user);
            return RedirectToAction("Profile");
        }

        public IActionResult ViewMessage(int id)
        {
            var message = _userService.GetMessage(id);

            if (message is null)
                return NotFound();

            return View(message);
        }

        public IActionResult Logout()
        {
            _sessionManager.ClearUserSessions();
            return RedirectToAction("Index", "Home");
        }
    }
}
