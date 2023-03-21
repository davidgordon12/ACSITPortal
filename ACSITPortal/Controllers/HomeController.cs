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
        private readonly UserService _userService;
        private readonly SessionManager _sessionManager;

        public HomeController(PostService postService, SessionManager sessionManager, UserService userService)
        {
            _postService = postService;
            _userService = userService;
            _sessionManager = sessionManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PostList()
        {
            var posts = _postService.GetAllPosts();

            foreach (var post in posts)
            {
                post.Threads = _postService.GetThreadsByPostId(post.PostId);

                /* If the post has 0 threads, create an empty list
                 * so .NET doesn't give us a null warning */
                if (post.Threads is null)
                    post.Threads = Enumerable.Empty<Entities.Thread>().ToList();
            }

            return View(posts);
        }

        public IActionResult ViewPost(int id)
        {
            /* We need to get the post here so we can
             * get the User by id in the postModel */
            var post = _postService.GetPostById(id);

            if (post is null)
                return NotFound();

            var postModel = new PostViewModel
            {
                Post = post,
                User = _userService.GetUserById(post.UserId),
                Threads = _postService.GetThreadsByPostId(post.PostId),
            };

            return View(postModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}