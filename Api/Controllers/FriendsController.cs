using System;
using System.Collections.Generic;
using System.Linq;
using Core.DataContext;
using Core.Entity;
using Logic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("/api/[controller]")]
    public class FriendsController : Controller
    {
        private VkApiService _vkApi { get; set; }
        public FriendsController(VkApiService vkApiService)
        {
            _vkApi = vkApiService;
        }

        [HttpPost("init")]
        public IActionResult GetFriends([FromBody] long? id)
        {
            var friends = _vkApi.GetFriends(id).Select(f => new User()
            {
                Id = f.Id,
                FullName = $"{f.FirstName} {f.LastName}",
                IsDeactivated = f.IsDeactivated,
                PhotoUrl = f.PhotoMaxOrig?.ToString(),
                MuturalCount = f.CommonCount ?? 0, 
                MuturalFriend =  new List<User>()
            }).ToList();
            var friendsIds = friends.Select(f => f.Id);
            using (var context = new ApplicationContext())
            {                
                foreach (var user in friends)
                {
                    var entityUser = context.Users.FirstOrDefault(u => u.UserId == user.Id);
                    if (entityUser == null)
                    {
                        entityUser = new User
                        {
                            UserId = user.Id,
                            FullName = user.FullName,
                            IsDeactivated = user.IsDeactivated,
                            LastCheck = DateTime.Now,
                            MuturalCount = user.MuturalCount,
                            PhotoUrl = user.PhotoUrl
                        };
                        context.Users.Add(entityUser);
                        context.SaveChanges();
                    }

                    if (user.IsDeactivated || user.MuturalCount == 0)
                    {
                        continue;
                    }

                    var manyToMany = context.FriendsUserToUsers.Where(f => f.LeftUserId == entityUser.Id);
                    if (manyToMany.Count() != entityUser.MuturalCount)
                    {

                        context.FriendsUserToUsers.RemoveRange(manyToMany);
                        entityUser.MuturalCount = user.MuturalCount;
                        context.SaveChanges();
                        user.MuturalFriend = _vkApi.GetFriends(user.Id).Select(f => new User()
                        {
                            Id = f.Id,
                            FullName = $"{f.FirstName} {f.LastName}",
                            IsDeactivated = f.IsDeactivated,
                            PhotoUrl = f.PhotoMaxOrig?.ToString(),
                            MuturalCount = f.CommonCount?? 0
                        }).Where(f => friendsIds.Contains(f.Id)).ToList();

                        foreach (var muturalUser in user.MuturalFriend)
                        {
                            var muturalEntityUser = context.Users.FirstOrDefault(u => u.UserId == muturalUser.Id);
                            if (muturalEntityUser == null)
                            {
                                muturalEntityUser = new User
                                {
                                    UserId = muturalUser.Id,
                                    FullName = muturalUser.FullName,
                                    IsDeactivated = muturalUser.IsDeactivated,
                                    LastCheck = DateTime.Now,
                                    MuturalCount = muturalUser.MuturalCount,
                                    PhotoUrl = muturalUser.PhotoUrl
                                };
                                context.Users.Add(muturalEntityUser);
                                context.SaveChanges();
                            }
                            if (context.FriendsUserToUsers.Any(e => e.LeftUserId == entityUser.Id && e.RightUserId == muturalEntityUser.Id)) continue;
                            context.Add(new FriendsUserToUser()
                            {
                                LeftUserId = entityUser.Id,
                                RightUserId = muturalEntityUser.Id
                            });
                            
                        }
                        context.SaveChanges();
                    }
                    else
                    {
                        var mutural = context.FriendsUserToUsers.Where(e => e.LeftUserId == entityUser.Id)
                            .Select(e => e.RightUserId);
                        user.MuturalFriend = context.Users.Where(e => mutural.Contains(e.Id)).ToList();
                    }
                }
            }
            
            return new JsonResult(friends);
        }

        [HttpGet("getmatrix")]
        public IActionResult GetMatrix()
        {
            int[][] result = null;
            using (var context = new ApplicationContext())
            {
                var users = context.Users.Where(e => !e.IsDeactivated).ToArray();
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

                var names = string.Join("\n", users.Select(e => e.FullName));
                var strResult = string.Join("\n", result.Select(e => string.Join(",", e)));
            }

            
            return new JsonResult(result);
        }
    }
}