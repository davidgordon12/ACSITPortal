using ACSITPortal.Data;
using ACSITPortal.Entities;
using ACSITPortal.Helpers;
using ACSITPortal.Models;
using Microsoft.EntityFrameworkCore;

namespace ACSITPortal.Services
{
    public class UserService
    {
        private readonly ACSITPortalContext _context;
        private readonly Mailer _mailer;
        public UserService(ACSITPortalContext context, Mailer mailer)
        {
            _context = context;
            _mailer = mailer;
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
        /// Gets a user by the given name
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The user with the matching name</returns>
        public User GetUserByName(string name)
        {
            try
            {
                return _context.Users
                    .Where(u => u.UserLogin == name)
                    .FirstOrDefault();
            }
            catch
            { return null; }
        }

        /// <summary>
        /// Gets a user by the given email
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The user with the matching email</returns>
        public User GetUserByEmail(string email)
        {
            try
            {
                return _context.Users.Where(u => u.Email == email).FirstOrDefault();
            }
            catch
            { return null; }
        }

        public User GetUserByToken(string token)
        {
            try
            {
                return _context.Users.Where(u => u.ActionToken == token).FirstOrDefault();
            }
            catch
            { return null; }
        }

        public List<Post> GetUsersPosts(int id)
        {
            try
            {
                return _context.Posts
                    .Where(p => p.UserId == id)
                    .ToList();
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
            // check to see if the username or email already exists
            if (_context.Users.Any(x => x.UserLogin == user.UserLogin || x.Email == user.Email))
                return false;

            try
            {
                // get the salted password and salt from HashPassword method
                var strings = Encryption.HashPassword(user.UserPassword);

                // and update the model with the data
                user.UserPassword = strings[0];
                user.UserSalt = strings[1];

                _context.Users.Add(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool VerifyUser(User user)
        {
            try
            {
                user.Verified = true;
                _context.Users.Update(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// Creates a session for the user if given login information matches a record in the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns>True if successful</returns>
        public bool Login(User user)
        {
            // check to see if an entity with matching username and password exists
            if (_context.Users.Any(x => x.UserLogin == user.UserLogin))
            {
                // if there is a match, check to see if the password in the database
                // matches the user provided password
                var _user = _context.Users.FirstOrDefault(x => x.UserLogin == user.UserLogin);
                if (Encryption.HashPassword(user.UserPassword, _user.UserSalt) == _user.UserPassword)
                    return true;
                else
                    return false;
            }

            return false;
        }

        public void RequestResetPassword(User user)
        {
            // Creates a random token that the user will use to authenticate
            user.ActionToken = Encryption.GenerateRandomToken();
            user.TokenExpires = DateTime.Now.AddMinutes(10);
            _context.SaveChanges();

            _mailer.SendEmail(user.Email, "Password reset token", "We have received a request for a password reset for this account. If this was not you, ignore this email. If this was you, paste this code into the form \n" + user.ActionToken);
        }

        public void RequestVerification(string email)
        {
            var user = GetUserByEmail(email);

            // Creates a random token that the user will use to authenticate
            user.ActionToken = Encryption.GenerateRandomToken();
            user.TokenExpires = DateTime.Now.AddMinutes(10);
            user.VerificationExpires = DateTime.Now.AddDays(7);
            _context.SaveChanges();

            _mailer.SendEmail(user.Email, "Account Verification Token", "Thank you for signing up on ACSIT Portal! To continue your account creation, paste this code into the verification form \n" + user.ActionToken);
        }

        public bool ResetPassword(User user, string newPassword)
        {
            try
            {
                // Hash the new password before updating it in the database
                newPassword = Encryption.HashPassword(newPassword, user.UserSalt);
                user.UserPassword = newPassword;
                _context.Users.Update(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception) { return false; }
        }

        public bool SendMessage(Message message, User user)
        {
            try
            {
                message.MessageSentDate = DateTime.Now;
                user.Messages.Add(message);
                _context.Users.Update(user);
                _context.SaveChanges();

                return true;
            }
            catch (Exception) { return false; }
        }

        public Message GetMessage(int id)
        {
            var message = _context.Messages.Where(m => m.MessageId == id).FirstOrDefault();

            if (message is not null)
                return message;
            else
                return null;
        }

        public List<Message> GetMessages (int id)
        {
            var messages = _context.Messages.Where(m=>m.RecepientId == id).ToList();

            if (messages is not null)
                return messages;
            else
                return Enumerable.Empty<Message>().ToList();
        }
    }
}
