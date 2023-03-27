using ACSITPortal.Entities;
using ACSITPortal.Services;
using Newtonsoft.Json;

namespace ACSITPortal.Helpers
{
    public class SessionManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserService _userService;
        public int LoginAttempts { get; set; } = 0;

        public SessionManager(IHttpContextAccessor httpContextAccessor, UserService userService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

        public void SetLoginAttempts()
        {
            LoginAttempts = GetLoginAttempts() + 1;
            _httpContextAccessor.HttpContext.Session.SetString("LoginAttempts",
                    LoginAttempts.ToString());
        }

        public int GetLoginAttempts()
        {
            return Convert.ToInt32(_httpContextAccessor
                .HttpContext.Session.GetString("LoginAttempts"));
        }

        /// <summary>
        /// Deserializes the object stored in the HttpContext session "User"
        /// </summary>
        /// <returns>The User that was stored in the session</returns>
        public User GetUserSession()
        {
            try
            {
                return JsonConvert.DeserializeObject<User>(
                    _httpContextAccessor.HttpContext.Session.GetString("User"));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void CreateUserSession(User user)
        {
            try
            {
                _httpContextAccessor.HttpContext.Session.SetString("User",
                    JsonConvert.SerializeObject(_userService.GetUserByName(user.UserLogin)));
            }
            catch (Exception)
            {
                _httpContextAccessor.HttpContext.Session.SetString(
                    "Error", "An error occured retrieving your profile");
            }
        }

        public void ClearUserSessions()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
        }
    }
}
