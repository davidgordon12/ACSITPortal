﻿using ACSITPortal.Data;
using ACSITPortal.Entities;
using ACSITPortal.Helpers;

namespace ACSITPortal.Services
{
    public class PostService
    {
        private readonly ACSITPortalContext _context;
        private readonly SessionManager _sessionManager;

        public PostService(ACSITPortalContext context, SessionManager sessionManager)
        {
            _context = context;
            _sessionManager = sessionManager;
        }

        public List<Post> GetAllPosts()
        {
            return _context.Posts.ToList();
        }

        public List<Post> GetPostsByUser(int userId)
        {
            try
            {
                return _context.Posts
                    .Where(p => p.UserId == userId)
                    .ToList();
            }
            catch
            {
                return null;
            }
        }

        public Post GetPostById(int id)
        {
            try
            {
                return _context.Posts
                    .Where(p => p.PostId == id)
                    .FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public bool CreatePost(Post post)
        {
            try
            {
                post.DateCreated = DateTime.Now;
                _context.Posts.Add(post);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdatePost(Post post)
        {
            try
            {
                _context.Posts.Update(post);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeletePost(int id)
        {
            try
            {
                _context.Posts.Remove(_context.Posts
                    .Where(p => p.PostId == id)
                    .FirstOrDefault());
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
