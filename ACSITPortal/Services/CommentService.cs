using ACSITPortal.Data;
using ACSITPortal.Entities;
using ACSITPortal.Helpers;
using Microsoft.EntityFrameworkCore;

namespace ACSITPortal.Services
{
    public class CommentService
    {

        private readonly ACSITPortalContext _context;
        private readonly SessionManager _sessionManager;

        public CommentService(ACSITPortalContext context, SessionManager sessionManager)
        {
            _context = context;
            _sessionManager = sessionManager;
        }

        public List<Entities.Thread> GetThreadsByPostId(int id)
        {
            var threads = _context.Threads.Where(t => t.PostId == id).ToList();

            foreach (var thread in threads)
            {
                thread.Replies = GetRepliesByThread(thread.ThreadId);

                if (thread.Replies is null)
                    thread.Replies = Enumerable.Empty<Reply>().ToList();
            }
            return threads;
        }

        public Entities.Thread GetThreadById(int id)
        {
            return _context.Threads.Where(t => t.ThreadId == id).FirstOrDefault();
        }

        public bool CreateThread(Entities.Thread thread)
        {
            try
            {
                thread.DateCreated = DateTime.Now;
                thread.UserId = _sessionManager.GetUserSession().UserId;
                _context.Threads.Add(thread);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateThread(Entities.Thread thread)
        {
            try
            {
                var _thread = _context.Threads
                    .Where(t => t.ThreadId == thread.ThreadId)
                    .FirstOrDefault();

                _thread.DateUpdated = DateTime.Now;
                _thread.ThreadContent = thread.ThreadContent;

                _context.Threads.Update(_thread);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteThread(int id)
        {
            try
            {
                _context.Threads.Remove(_context.Threads
                    .Where(t => t.ThreadId == id)
                    .FirstOrDefault());
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CreateReply(Reply reply)
        {
            try
            {
                reply.DateCreated = DateTime.Now;
                reply.UserId = _sessionManager.GetUserSession().UserId;
                _context.Replies.Add(reply);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<Reply> GetRepliesByThread(int threadId)
        {
            return _context.Replies.Where(r => r.ThreadId == threadId).ToList();
        }
    }
}
