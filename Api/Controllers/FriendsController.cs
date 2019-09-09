using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataContext;
using Core.Entity;
using Logic.Dto;
using Logic.Managers;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("/api/[controller]")]
    public class FriendsController : Controller
    {
        private VkApiService _vkApi { get; set; }
        private FriendsManager _friendsManager;

        public FriendsController(VkApiService vkApiService, FriendsManager friendsManager)
        {
            _vkApi = vkApiService;
            _friendsManager = friendsManager;
        }

        [HttpGet("getself")]
        public IActionResult GetSelf()
        {
            var self = _vkApi.GetCurrentUser(null);
            return new JsonResult(new Node() { User = $"{self.LastName} {self.FirstName}", Id = self.Id,Description = self.Photo50.ToString(),IsActive = true });
        }

        [HttpGet("vkfirends")]
        public IActionResult GetFriends(long id)
        {
            var friends = _vkApi.GetFriends(id);
            var links = friends.Select(e => new Link { Source = id, Target = e.Id});
            var nodes = friends.Select(f => new Node() { Id = f.Id, User = $"{f.LastName} {f.FirstName}", Description = f.Photo50?.ToString(), IsActive = !(f.IsDeactivated  || (f.IsClosed ?? false) || f.Blacklisted)});
            return new JsonResult(new { links , nodes });
            
        }

        [HttpPost("init")]
        public IActionResult InitFriends(long? id)
        {
            var userId = _friendsManager.UpdateFriendList(id ?? 0);
            _friendsManager.UpdatePublicList(id);
            //var result = _friendsManager.GetFriendsList(userId);



            return new JsonResult(userId);
        }

        [HttpPost("initgroups")]
        public IActionResult InitFriendsPublics(long? id)
        {
            _friendsManager.UpdatePublicList(id);
            return new JsonResult("");
        }

        [HttpPost("fullinit")]
        public IActionResult GetAllFriends(long id = 0)
        {
            var result = _friendsManager.UpdateAllFriendList(id);
            return new JsonResult(result);
        }

        [HttpGet("getmatrix")]
        public IActionResult GetMatrix(long id = 0)
        {
            int[][] result = null;
            var names = "";
            var graph = "";
            using (var context = new ApplicationContext())
            {
                var friendsUserIds = context.FriendsUserToUsers.Where(f => f.LeftUserId == id).Select(e => e.RightUserId);
                var users = context.Users.Where(e => !e.IsDeactivated && friendsUserIds.Contains(e.Id)).ToArray();
                var count = users.Count();
                result = new int[count][];
                for (var i = 0; i < count; i++)
                {
                    result[i] = new int[count];
                    for (var j = 0; j < count; j++)
                    {
                        if (context.FriendsUserToUsers.Any(f => f.LeftUserId == users[i].Id && f.RightUserId == users[j].Id))
                            result[i][j] = 1;
                        else
                            result[i][j] = 0;
                    }
                }

                names = string.Join("\n", users.Select(e => e.FullName));
                graph = string.Join("\n", result.Select(e => string.Join(",", e)));
            }


            return new JsonResult(new { names, graph });
        }

    }
}