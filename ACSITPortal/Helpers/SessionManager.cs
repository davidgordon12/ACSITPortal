using ACSITPortal.Entities;
using ACSITPortal.Services;
using Newtonsoft.Json;

namespace ACSITPortal.Helpers
{
    public class SessionManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserService _userService;

        public SessionManager(IHttpContextAccessor httpContextAccessor, UserService userService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userService = userService;
        }

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
                    JsonConvert.SerializeObject(_userService.GetUserById(user.UserId)));
            }
            catch (Exception)
            {
                _httpContextAccessor.HttpContext.Session.SetString(
                    "Error", "An error occured retrieving your profile");
            }
        }
    }
}
