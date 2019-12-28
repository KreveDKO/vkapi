using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataContext;
using Core.Entity;
using Logic.Dto;
using Logic.Services;
using VkNet.Enums;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;

namespace Logic.Managers
{
    public class UpdateManager
    {
        private readonly VkApiService _vkApiService;
        private readonly FriendsService _friendsService;


        public UpdateManager(VkApiService vkApiService, FriendsService friendsService)
        {
            _vkApiService = vkApiService;
            _friendsService = friendsService;
        }

        public Task UpdateFriendList(FriendsUpdateDto dto, ApplicationContext context = null)
        {
            if (context == null)
            {
                context = new ApplicationContext();
            }

            if (dto.UserId == null)
            {
                dto.UserId = _vkApiService.GetCurrentId;
            }

            //Находим текущего пользователя и обновляем его связи
            var currentUserEntity = context.VkUsers.FirstOrDefault(u => u.ExternalId == dto.UserId);
            if (currentUserEntity == null)
            {
                User vkUser;
                try
                {
                    vkUser = _vkApiService.GetUser(dto.UserId);
                }
                catch
                {
                    return Task.CompletedTask;
                }

                currentUserEntity = new VkUser
                {
                    ExternalId = vkUser.Id,
                    IsDeactivated = false,
                    PhotoUrl = vkUser.PhotoMaxOrig.ToString(),
                    FullName = $"{vkUser.FirstName} {vkUser.LastName}"
                };
                context.Add(currentUserEntity);
                context.SaveChanges();
            }

            List<User> friends;
            try
            {
                friends = _vkApiService.GetFriends(currentUserEntity.ExternalId).ToList();
            }
            catch
            {
                return Task.CompletedTask;
            }

            context.RemoveRange(_friendsService.GetRemovedUsers(context, currentUserEntity.Id, friends));
            var newFriends = _friendsService.GetNewUsers(context, currentUserEntity.Id, friends);


            foreach (var newFriend in newFriends.Where(f => context.VkUsers.All(u => u.ExternalId != f.Id)))
            {
                context.VkUsers.Add(new VkUser
                {
                    ExternalId = newFriend.Id,
                    FullName = $"{newFriend.FirstName} {newFriend.LastName}",
                    IsDeactivated = newFriend.IsDeactivated,
                    LastCheck = DateTime.Now,
                    PhotoUrl = newFriend.PhotoMaxOrig?.ToString()
                });
            }

            context.SaveChanges();


            foreach (var friend in friends)
            {
                if (dto.Recursive)
                {
                    UpdateFriendList(new FriendsUpdateDto {Recursive = false, UserId = friend.Id}, context);
                }
                var friendEntity = context.VkUsers.First(f => f.ExternalId == friend.Id);
                if (context.FriendsUserToUsers.Any(c =>
                    c.RightUserId == friendEntity.Id && c.LeftUserId == currentUserEntity.Id))
                {
                    continue;
                }

                context.FriendsUserToUsers.Add(new FriendsUserToUser
                {
                    LeftUserId = currentUserEntity.Id,
                    RightUserId = friendEntity.Id
                });
                

            }

            context.SaveChanges();

            return Task.CompletedTask;
        }

        public Task UpdateFriendsGroupsList(long userId)
        {
            using (var context = new ApplicationContext())
            {
                UpdateUserInfo(userId, context);
                var user = context.VkUsers.FirstOrDefault(u => u.ExternalId == userId);
                if (user == null)
                {
                    return Task.CompletedTask;
                }

                var mutualFriend = context.FriendsUserToUsers.Where(u => u.LeftUserId == user.Id)
                    .Select(u => u.RightUser).ToList();
                foreach (var friend in mutualFriend)
                {
                    UpdateGroupsList(friend.ExternalId, context);
                }
            }

            return Task.CompletedTask;
        }

        public Task UpdateGroupsList(long userId, ApplicationContext context = null)
        {
            List<Group> groups;
            try
            {
                groups = _vkApiService.GetGroups(userId);
            }
            catch
            {
                return Task.CompletedTask;
            }

            if (context == null)
            {
                context = new ApplicationContext();
            }

            var user = context.VkUsers.FirstOrDefault(u => u.ExternalId == userId);
            if (user == null)
            {
                return Task.CompletedTask;
            }

            var pages = groups.Where(p => p.Type == GroupType.Page || p.Type == GroupType.Group);
            foreach (var page in pages)
            {
                var groupEntity = context.UserGroups.FirstOrDefault(g => g.ExternalId == page.Id);
                if (groupEntity == null)
                {
                    groupEntity = new VkUserGroup
                    {
                        GroupType = page.Type == GroupType.Group ? 0 : 1,
                        ExternalId = page.Id,
                        Title = page.Name,
                        IsDeactivated = page.IsClosed == GroupPublicity.Closed
                    };
                    context.Add(groupEntity);
                    context.SaveChanges();
                }

                if (context.UserToUserGroup.Any(u2g => u2g.VkUserId == user.Id && u2g.VkUserGroupId == groupEntity.Id))
                {
                    continue;
                }

                context.UserToUserGroup.Add(new UserToUserGroup
                {
                    VkUser = user,
                    VkUserGroup = groupEntity
                });
            }

            context.SaveChanges();

            return Task.CompletedTask;
        }

        public Task UpdateUserInfo(long userId, ApplicationContext context = null)
        {
            User user;
            try
            {
                user = _vkApiService.GetUser(userId);
            }
            catch
            {
                return Task.CompletedTask;
            }

            if (context == null)
            {
                context = new ApplicationContext();
            }

            var userEntity = context.VkUsers.FirstOrDefault(u => u.ExternalId == userId);
            if (userEntity == null)
            {
                userEntity = new VkUser
                {
                    ExternalId = user.Id
                };
                context.VkUsers.Add(userEntity);
            }

            userEntity.FullName = $"{user.FirstName} {user.LastName}";
            userEntity.PhotoUrl = user.PhotoMaxOrig?.ToString();
            userEntity.IsDeactivated = user.IsDeactivated;
            userEntity.LastCheck = DateTime.Now;
            context.SaveChanges();
            return Task.CompletedTask;
        }
    }
}