using System;
using System.Collections.Generic;
using Logic.Managers;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/message")]
    [ApiController]
    public class MessageController : Controller
    {
        private readonly VkApiService _vkApiService;
        
        public MessageController(VkApiService vkApiService)
        {
            _vkApiService = vkApiService; ;
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
    }
}