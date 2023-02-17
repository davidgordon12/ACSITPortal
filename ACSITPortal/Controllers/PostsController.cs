using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ACSITPortal.Data;
using ACSITPortal.Entities;
using ACSITPortal.Services;
using ACSITPortal.Helpers;

namespace ACSITPortal.Controllers
{
    public class PostsController : Controller
    {
        private readonly PostService _postService;
        private readonly SessionManager _sessionManager;

        public PostsController(PostService postService)
        {
            postService = _postService;
        }

        public IActionResult Index()
        {
            var posts = _postService.GetAllPosts();

            if (posts is null)
            {
                /* This will stop ASP from giving a null-reference error
                   if there are no posts in the database */
                posts = Enumerable.Empty<Post>().ToList();
            }

            return View(posts);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Post post)
        {
            _postService.CreatePost(post);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var post = _postService.GetPostById(id);

            if (post is null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Post post)
        {
            _postService.UpdatePost(post);
            return View();
        }

        public IActionResult Delete(int id) 
        {
            var post = _postService.GetPostById(id);

            if (post is null)
            {
                return NotFound();
            }

            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Post post)
        {
            _postService.DeletePost(post);
            return RedirectToAction("Index");
        }
    }
}
