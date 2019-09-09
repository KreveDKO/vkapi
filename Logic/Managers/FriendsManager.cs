using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DataContext;
using Core.Entity;
using Logic.Dto;
using Logic.Services;
using Remotion.Linq.Clauses.ResultOperators;
using Remotion.Linq.Utilities;
using VkNet;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using User = Core.Entity.User;

namespace Logic.Managers
{
    public class FriendsManager
    {
        private VkApiService _vkApi;
        private long CurrentUser;
        public FriendsManager(VkApiService vkApiService)
        {
            _vkApi = vkApiService;
            CurrentUser = _vkApi.GetCurrentId ?? 0;
        }

        /// <summary>
        /// Обновляет связи между друзьями и записывает их в БД
        /// </summary>
        /// <param name="friends"></param>
        public long UpdateFriendList(long userId)
        {
            if (userId == 0) userId = CurrentUser;
            if (userId == 0) return 0;

            var friends = GetMappedUsersFriends(userId);
            var friendsIds = friends.Select(f => f.Id);
            long entityUserId = 0;
            using (var context = new ApplicationContext())
            {
                //Находим текущего пользователя и удаляем его связи
                var currentUserEntity = context.Users.FirstOrDefault(u => u.UserId == userId);
                if (currentUserEntity == null)
                {
                    var vkUser = _vkApi.GetCurrentUser(userId);
                    currentUserEntity = new User()
                    {
                        UserId = userId,
                        IsDeactivated = false,
                        PhotoUrl = vkUser?.PhotoMaxOrig?.ToString(),
                        FullName = $"{vkUser?.FirstName} {vkUser?.LastName}",
                        MuturalCount = friends.Count
                    };
                    context.Add(currentUserEntity);
                    context.SaveChanges();
                }

                entityUserId = currentUserEntity.Id;
                var needSkip = friends.Count > 1500;
                if (needSkip) Console.WriteLine($"{currentUserEntity.FullName} will skiped");
                Console.WriteLine($"Total: {friends.Count}");

                var removedUsers = RemovedUsers(context, currentUserEntity.Id, friends);
                context.RemoveRange(removedUsers);
                Console.WriteLine($"Removed users: {removedUsers.Count()}");

                var newUsers = NewUsers(context, currentUserEntity.Id, friends);
                Console.WriteLine($"New users: {newUsers.Count}");

                if (needSkip) return entityUserId;
                UpdateEntityUsers(context, newUsers);

                foreach (var user in friends)
                {
                    var entityUser = context.Users.First(u => u.UserId == user.Id);
                    if (entityUser.MuturalCount > 0 && context.FriendsUserToUsers.Count(c => c.RightUserId == entityUser.Id) == entityUser.MuturalCount) continue;
                    if (!context.FriendsUserToUsers.Any(f =>
                        f.RightUserId == entityUser.Id && f.LeftUserId == currentUserEntity.Id))
                    {
                        context.FriendsUserToUsers.Add(new FriendsUserToUser
                        {
                            LeftUserId = currentUserEntity.Id,
                            RightUserId = entityUser.Id
                        });
                        context.SaveChanges();
                    }

                    if (user.IsDeactivated || user.Blacklisted || ((user.IsClosed ?? false) && (!(user.IsFriend ?? false))))
                    {
                        continue;
                    }
                    var mutualFriends = GetMappedUsersFriends(user.Id).ToList();
                    if (entityUser.MuturalCount == mutualFriends.Count) continue;
                    Console.WriteLine($"Update ${user.FirstName} {user.LastName} ({user.Id}) Total: {mutualFriends.Count}");
                    entityUser.MuturalCount = mutualFriends.Count;
                    //var removedMutualUsers = RemovedUsers(context, entityUser.Id, mutualFriends);
                    var newMutualUsers = NewUsers(context, entityUser.Id, mutualFriends);
                    UpdateEntityUsers(context, newMutualUsers);
                    if (newMutualUsers.Count == 0) continue;

                    foreach (var mutualUser in newMutualUsers)
                    {
                        var mutualEntityUser = context.Users.First(u => u.UserId == mutualUser.Id);
                        if (!context.FriendsUserToUsers.Any(e => e.LeftUserId == entityUser.Id && e.RightUserId == mutualEntityUser.Id))
                            context.FriendsUserToUsers.Add(new FriendsUserToUser()
                            {
                                LeftUserId = entityUser.Id,
                                RightUserId = mutualEntityUser.Id
                            });
                        if (!context.FriendsUserToUsers.Any(e => e.RightUserId == entityUser.Id && e.LeftUserId == mutualEntityUser.Id))
                            context.FriendsUserToUsers.Add(new FriendsUserToUser()
                            {
                                RightUserId = entityUser.Id,
                                LeftUserId = mutualEntityUser.Id
                            });

                    }
                    context.SaveChanges();


                }
                context.SaveChanges();
            }

            return entityUserId;
        }
        public void UpdatePublicList()
        {
            using (var context = new ApplicationContext())
            {
                var friends = _vkApi.GetFriends();
                var top = new Dictionary<Group, int>();
                foreach (var friend in friends)
                {
                    var user = context.Users.First(u => u.UserId == friend.Id);
                    var result = _vkApi.GetPublics(friend.Id);
                    var pages = result.Where(p => p.Type == GroupType.Page || p.Type == GroupType.Group);
                    foreach (var page in pages)
                    {
                        var group = context.UserGroups.FirstOrDefault(g => g.GroupId == page.Id);
                        if (group == null)
                        {
                            group = new UserGroup()
                            {
                                GroupType = page.Type ==  GroupType.Group ? 0 : 1,
                                GroupId = page.Id,
                                Title = page.Name,
                                IsDeactivated = page.IsClosed == VkNet.Enums.GroupPublicity.Closed
                            };
                            context.Add(group);
                            context.SaveChanges();
                        }
                        if (context.UserToUserGroup.Any(m2m => m2m.UserId == user.Id && m2m.UserGroupId == group.Id))
                        {
                            continue;
                        }
                        context.UserToUserGroup.Add(new UserToUserGroup()
                        {
                            User = user,
                            UserGroup = group
                        });
                    }
                    context.SaveChanges();
                }
            }

        }
        private void UpdateEntityUsers(ApplicationContext context, List<VkNet.Model.User> newUsers)
        {
            var allUsers = context.Users;
            newUsers = newUsers.Where(nu => allUsers.All(u => u.UserId != nu.Id)).ToList();
            foreach (var user in newUsers)
            {
                context.Users.Add(new User
                {
                    UserId = user.Id,
                    FullName = $"{user.FirstName} {user.LastName}",
                    IsDeactivated = user.IsDeactivated,
                    LastCheck = DateTime.Now,
                    PhotoUrl = user.PhotoMaxOrig?.ToString()
                });

            }
            context.SaveChanges();
        }

        private List<VkNet.Model.User> NewUsers(ApplicationContext context, long userId, List<VkNet.Model.User> users)
        {
            var newUsers = users.Select(e => e.Id);

            var existsM2M = context.FriendsUserToUsers.Where(e => e.LeftUserId == userId/*).ToList().Where(e =>*/ && newUsers.Contains(e.RightUser.UserId)).Select(e => e.RightUserId).ToList();
            var existsUsers = context.Users.Where(e => existsM2M.Contains(e.Id)).Select(e => e.UserId).ToList();
            newUsers = newUsers.Where(e => !existsUsers.Contains(e)).ToList();
            return users.Where(f => newUsers.Contains(f.Id)).ToList();
        }

        private IQueryable<FriendsUserToUser> RemovedUsers(ApplicationContext context, long userId, List<VkNet.Model.User> users)
        {

            var result = new List<long>();

            var newUsersIds = users.Select(e => e.Id);
            var newEntityUsers = context.Users.Where(u => newUsersIds.Contains(u.UserId)).Select(u => u.Id).ToList();
            var removed = context.FriendsUserToUsers.Where(e => e.LeftUserId == userId && !newEntityUsers.Contains(e.RightUserId));

            return removed;

        }

        public int UpdateAllFriendList(long userId)
        {
            var friends = GetMappedUsersFriends(userId);
            var i = 0;
            UpdateFriendList(userId);
            foreach (var friend in friends)
            {
                var startTime = DateTime.Now;

                if (friend.IsDeactivated) continue;
                Console.WriteLine($"Start parsing {friend.FirstName} {friend.LastName} at {startTime}");
                UpdateFriendList(friend.Id);
                i++;
                Console.WriteLine($"Finish parsing {friend.FirstName} {friend.LastName} at {DateTime.Now}. Total time: {DateTime.Now - startTime}");
            }

            return i;

        }

        private List<VkNet.Model.User> GetMappedUsersFriends(long id, IEnumerable<long> friendsIds = null)
        {
            return _vkApi.GetFriends(id)/*.Select(f => new User()
            {
                Id = f.Id,
                FullName = $"{f.FirstName} {f.LastName}",
                IsDeactivated = f.IsDeactivated,
                PhotoUrl = f.PhotoMaxOrig?.ToString(),
                MuturalCount = f.CommonCount ?? 0
            })*/.ToList();
        }

    }
}
