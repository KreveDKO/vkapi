using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.Managers;
using Logic.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class MessageController : Controller
    {
        VkApiService _vkApiService;
        UserManager _userManager;
        public MessageController(VkApiService vkApiService, UserManager userManager)
        {
            _vkApiService = vkApiService;
            _userManager = userManager;
        }

        [HttpGet("messages")]
        public IActionResult GetMessages()
        {
            var result = new List<VkNet.Model.Message>();
            var dialogs = _vkApiService.GetDialogs();
            foreach (var dialog in dialogs)
            {
                Console.WriteLine(dialog);
                result.AddRange(_vkApiService.GetMessages(dialog));
            }
            
            return new JsonResult(result);
        }
        [HttpGet("update")]
        public IActionResult UpdateUsers()
        {
            _userManager.UpdateUsers();
            return new JsonResult("");
        }
    }
}