using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Core.Dto;
using Logic.Interfaces.Managers;
using Logic.Interfaces.Services;
using Logic.Services;

namespace Logic.Managers
{
    public class ActionManager : IActionManager
    {
        private readonly IVkApiService _vkApiService;

        public ActionManager(IVkApiService vkApiService)
        {
            _vkApiService = vkApiService;
        }
        
        public async Task SendWallMessages(SendWallMessageDto dto)
        {
            foreach (var message in dto.Messages)
            {
                _vkApiService.SendWallMessage(dto.WallId,message);
                await Task.Delay(dto.Delay  * 1000);
            }
        }
    }
}