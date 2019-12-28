using System.Threading.Tasks;
using Logic.Dto;
using Logic.Managers;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("/api/[controller]")]
    public class UpdateController : Controller
    {
        private readonly UpdateManager _updateManager;

        public UpdateController(UpdateManager updateManager)
        {
            _updateManager = updateManager;
        }

        [HttpPost("friends")]
        public Task Friends([FromBody] FriendsUpdateDto dto) => _updateManager.UpdateFriendList(dto);


        [HttpPost("groups")]
        public Task FriendsGroups(long id) => _updateManager.UpdateGroupsList(id);

        [HttpPost("userinfo")]
        public Task UpdateUserInfo(long id) => _updateManager.UpdateUserInfo(id);

        [HttpPost("usergroups")]
        public Task UpdateUserGroups(long id) => _updateManager.UpdateGroupsList(id);

        [HttpPost("friendsgroups")]
        public Task UpdateFriendsUserGroup(long id) => _updateManager.UpdateFriendsGroupsList(id);
    }
}