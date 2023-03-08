using ACSITPortal.Entities;
using ACSITPortal.Helpers;
using ACSITPortal.Models;
using ACSITPortal.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ACSITPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly PostService _postService;
        private readonly SessionManager _sessionManager;

        public HomeController(PostService postService, SessionManager sessionManager)
        {
            _postService = postService;
            _sessionManager = sessionManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult CreatePost()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreatePost(Post post)
        {
            post.UserId = _sessionManager.GetUserSession().UserId;

            _postService.CreatePost(post);
            return RedirectToAction("Profile", "Users");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}