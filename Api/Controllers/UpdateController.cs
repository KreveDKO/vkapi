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
        public JsonResult UpdateUserInfo(long id) => new JsonResult(_updateManager.UpdateUserInfo(id));

        /// <summary>
        /// Обновляет информацию о группах друзей
        /// </summary>
        /// <param name="id">Идентификатор пользователя в ВК</param>
        [HttpPost("friendsgroups")]
        public Task UpdateFriendsUserGroup(long id) => _updateManager.UpdateFriendsGroupsList(id);

        /// <summary>
        /// Обновляет сообщения пользователя
        /// </summary>
        [HttpPost("messages")]
        public Task UpdateMessages() => _updateManager.UpdateMessages();

        
        /// <summary>
        /// Обновляет аудиозаписи пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя в ВК</param>
        [HttpPost("audio")]
        public Task UpdateAudio(long id) => _updateManager.UpdateAudio(id);

        /// <summary>
        /// Обновляет видеозаписи пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя в ВК</param>
        [HttpPost("video")]
        public Task UpdateVideo(long id) => _updateManager.UpdateVideo(id);

        /// <summary>
        /// Обновляет информацию о группе
        /// </summary>
        /// <param name="id">Идентификатор группы в ВК</param>
        [HttpPost("group")]
        public JsonResult UpdateGroup([FromBody] GroupUpdateDto dto) => new JsonResult(_updateManager.UpdateGroupInfo(dto));
        
        [HttpPost("wall")]
        public Task UpdateWall([FromBody] WallUpdateDto dto) => _updateManager.UpdateWall(dto);
    }
}