using System.Threading.Tasks;
using Core.Dto;
using Logic.Interfaces.Managers;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    
    [Route("/api/[controller]")]
    public class ActionController  : Controller
    {
        private readonly IActionManager _actionManager;

        public ActionController(IActionManager actionManager)
        {
            _actionManager = actionManager;
        }
        
        /// <summary>
        /// Отправка сообщений на стену с указанной задержкой
        /// </summary>
        /// <param name="dto">Настройки для отправки</param>
        /// <returns></returns>
        [HttpPost("sendwallmessages")]
        public Task SendWallMessages([FromBody]SendWallMessageDto dto) => _actionManager.SendWallMessages(dto);
    }
}