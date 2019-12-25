using System;
using System.Threading.Tasks;
using Logic.Dto;
using Logic.Managers;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("/api/[controller]")]
    public class UpdateController : Controller
    {
        private readonly VkApiService _vkApiService;
        private readonly UpdateManager _updateManager;

        public UpdateController(VkApiService vkApiService, UpdateManager updateManager)
        {
            _vkApiService = vkApiService;
            _updateManager = updateManager;
        }

        [HttpPost("friends")]
        public Task Friends([FromBody] FriendsUpdateDto dto) => _updateManager.UpdateFriendList(dto);


        [HttpPost("friendsgroups")]
        public Task FriendsGroups(long id) => _updateManager.UpdateGroupsList(id);

        [HttpPost("userinfo")]
        public Task UpdateUserInfo(long id) => _updateManager.UpdateUserInfo(id);

        [HttpPost("usergroups")]
        public Task UpdateUserGroups(long id) => _updateManager.UpdateGroupsList(id);

        [HttpPost("friendsgroups")]
        public Task UpdateFriendsUserGroup(long id) => _updateManager.UpdateFriendsGroupsList(id);

        [HttpGet("messages")]
        public Task GetMessages()
        {
            var dialogs = _vkApiService.GetDialogs();
            foreach (var dialog in dialogs)
            {
                _vkApiService.GetMessages(dialog);
            }

            return Task.CompletedTask;
        }

        [HttpPost("full")]
        public Task GetAllFriends(long id = 0)
        {
            _updateManager.UpdateFriendList(new FriendsUpdateDto() {Recursive = true, UserId = id});
            _updateManager.UpdateFriendsGroupsList(id);
            return Task.CompletedTask;
        }
    }
}