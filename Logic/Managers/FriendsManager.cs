using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DataContext;
using Core.Entity;
using Logic.Services;
using Remotion.Linq.Clauses.ResultOperators;
using Remotion.Linq.Utilities;
using VkNet;
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
                        FullName = $"{vkUser?.FirstName} {vkUser?.LastName}"
                    };
                    context.Add(currentUserEntity);
                    context.SaveChanges();
                }

                entityUserId = currentUserEntity.Id;
                var currentManyToMany = context.FriendsUserToUsers.Where(e => e.LeftUserId == currentUserEntity.Id);
                var needSkip = false;
                //context.RemoveRange(currentManyToMany);
                //context.SaveChanges();
                if (currentManyToMany.Count() == friends.Count || needSkip) return entityUserId;

                foreach (var user in friends)
                {
                    var entityUser = context.Users.FirstOrDefault(u => u.UserId == user.Id);
                    if (entityUser == null)
                    {
                        entityUser = new User
                        {
                            UserId = user.Id,
                            FullName = $"{user.FirstName} {user.LastName}",
                            IsDeactivated = user.IsDeactivated,
                            LastCheck = DateTime.Now,
                            PhotoUrl = user.PhotoMaxOrig?.ToString()
                        };
                        context.Users.Add(entityUser);
                        context.SaveChanges();
                    }

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

                    if (user.IsDeactivated || ((user.IsClosed ?? false) && (!(user.IsFriend ?? false))))
                    {
                        continue;
                    }

                    //Todo: обработать сценарий удаления пользователя, кроме как парсить все связи
                    //var manyToMany = context.FriendsUserToUsers.Where(f => f.LeftUserId == entityUser.Id);
                    //context.FriendsUserToUsers.RemoveRange(manyToMany);
                    context.SaveChanges();
                    var mutuaList = GetMappedUsersFriends(user.Id).Where(e => friendsIds.Contains(e.Id)).ToList();
                    foreach (var mutualUser in mutuaList)
                    {
                        var mutualEntityUser = context.Users.FirstOrDefault(u => u.UserId == mutualUser.Id);
                        if (mutualEntityUser == null)
                        {
                            mutualEntityUser = new User
                            {
                                UserId = mutualUser.Id,
                                FullName = $"{mutualUser.FirstName} {mutualUser.LastName}",
                                IsDeactivated = mutualUser.IsDeactivated,
                                LastCheck = DateTime.Now,
                                PhotoUrl = mutualUser.PhotoMaxOrig?.ToString()
                            };
                            context.Users.Add(mutualEntityUser);
                            context.SaveChanges();
                        }
                        else
                        {
                            mutualEntityUser.FullName = $"{mutualUser.FirstName} {mutualUser.LastName}";
                            context.SaveChanges();
                        }
                        if (context.FriendsUserToUsers.Any(e => e.LeftUserId == entityUser.Id && e.RightUserId == mutualEntityUser.Id)) continue;
                        context.Add(new FriendsUserToUser()
                        {
                            LeftUserId = entityUser.Id,
                            RightUserId = mutualEntityUser.Id
                        });

                    }
                    context.SaveChanges();
                    
                }
            }

            return entityUserId;
        }

        public int UpdateAllFriendList(long userId)
        {
            var friends = GetMappedUsersFriends(userId);
            var i = 0;
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

        public List<User> GetFriendsList(long userId)
        {
            
            var result = new List<User>();
            using (var context = new ApplicationContext())
            {
                var friends = context.FriendsUserToUsers.Where(f => f.LeftUserId == userId);
                foreach (var friendsUserToUser in friends)
                {
                    var user = context.Users.FirstOrDefault(u => u.Id == friendsUserToUser.RightUserId);
                    result.Add(user);
                }

                foreach (var user in result)
                {
                    var mutual = context.FriendsUserToUsers.Where(e => e.LeftUserId == user.Id).Select(e => e.RightUserId);
                    user.MuturalFriend = context.Users.Where(u => mutual.Contains(u.Id)).ToList();
                }
                
            }
            return result;
        }

        private List<VkNet.Model.User> GetMappedUsersFriends(long id, IEnumerable<long> friendsIds =  null)
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
