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

        /// <summary>
        /// Обновляет список друзей
        /// </summary>
        [HttpPost("friends")]
        public Task Friends([FromBody] FriendsUpdateDto dto) => _updateManager.UpdateFriendList(dto);

        /// <summary>
        /// Обновляет группы пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя в ВК</param>
        /// <returns></returns>
        [HttpPost("groups")]
        public Task FriendsGroups(long id) => _updateManager.UpdateGroupsList(id);

        /// <summary>
        /// Обновляет информацию об определённом пользователе
        /// </summary>
        /// <param name="id">Идентификатор пользователя в ВК</param>
        [HttpPost("userinfo")]
        public Task UpdateUserInfo(long id) => _updateManager.UpdateUserInfo(id);

        /// <summary>
        /// Обновляет информацию о группах друзей
        /// </summary>
        /// <param name="id">Идентификатор пользователя в ВК</param>
        [HttpPost("friendsgroups")]
        public Task UpdateFriendsUserGroup(long id) => _updateManager.UpdateFriendsGroupsList(id);
    }
}