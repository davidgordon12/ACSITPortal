using ACSITPortal.Data;
using ACSITPortal.Entities;

namespace ACSITPortal.Services
{
    public class UserService
    {
        private readonly ACSITPortalContext _context;
        public UserService(ACSITPortalContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets a user by the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The user with the matching id</returns>
        public User GetUserById(int id) 
        {
            try
            {
                return _context.Users.Where(u => u.UserId == id).FirstOrDefault();
            }
            catch
            { return null; }
        }

        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns>True if successful</returns>
        public bool CreateUser(User user)
        {
            // check to see if the username already exists
            if (_context.Users.Any(x => x.UserLogin == user.UserLogin))
                return false;

            try
            {
                user.DateCreated = DateTime.Now;

                _context.Users.Add(user);
                _context.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// Creates a session for the user if given login information matches a record in the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns>True if successful</returns>
        public bool Login(User user)
        {
            // check to see if an entity with matching username exists
            if (_context.Users.Any(x => x.UserLogin == user.UserLogin))
            {
                // if there is a match, check to see if the password in the database
                // matches the user provided password
                var _user = _context.Users.FirstOrDefault(x => x.UserLogin == user.UserLogin);
                if (user.UserPassword == _user.UserPassword)
                    return true;
                else
                    return false;
            }

            return false;
        }
    }
}
