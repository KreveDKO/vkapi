using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Api.Controllers
{
    public class LoginController : Controller
    {
        private ulong AppId;
        public LoginController(IConfiguration configuration)
        {
            ulong.TryParse(configuration["AppId"], out AppId);
        }
        public IActionResult Index()
        {
            var request = HttpContext.Request.Host;
            return Redirect($"https://oauth.vk.com/authorize?client_id={AppId}&scope=friends&response_type=token&v=5.95&redirect_uri={HttpContext.Request.Scheme}://{HttpContext.Request.Host}/login/token");
        }

        public void Token(string access_token)
        {
            SignIn(User, "VkAuth");            
        }
    }
}