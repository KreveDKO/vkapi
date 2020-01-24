using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly DataContextService _contextService;

        public UpdateManager(VkApiService vkApiService, FriendsService friendsService,
            DataContextService contextService)
        {
            _vkApiService = vkApiService;
            _friendsService = friendsService;
            _contextService = contextService;
        }

        public Task UpdateFriendList(FriendsUpdateDto dto, ApplicationContext context = null)
        {
            Console.WriteLine($"{GetType()}.{nameof(UpdateFriendList)} started");
            if (context == null)
            {
                context = new ApplicationContext(_contextService.Options);
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
            Console.WriteLine($"{GetType()}.{nameof(UpdateFriendList)} completed");
            return Task.CompletedTask;
        }

        public Task UpdateFriendsGroupsList(long userId)
        {
            Console.WriteLine($"{GetType()}.{nameof(UpdateFriendsGroupsList)} started");
            using (var context = new ApplicationContext(_contextService.Options))
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

            Console.WriteLine($"{GetType()}.{nameof(UpdateFriendsGroupsList)} completed");
            return Task.CompletedTask;
        }

        public Task UpdateGroupsList(long userId, ApplicationContext context = null)
        {
            Console.WriteLine($"{GetType()}.{nameof(UpdateGroupsList)} completed");
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
                context = new ApplicationContext(_contextService.Options);
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
            Console.WriteLine($"{GetType()}.{nameof(UpdateGroupsList)} completed");
            return Task.CompletedTask;
        }

        public long UpdateUserInfo(long userId, ApplicationContext context = null)
        {
            Console.WriteLine($"{GetType()}.{nameof(UpdateUserInfo)} started");
            User user;
            try
            {
                user = _vkApiService.GetUser(userId);
            }
            catch
            {
                return default;
            }

            if (context == null)
            {
                context = new ApplicationContext(_contextService.Options);
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
            Console.WriteLine($"{GetType()}.{nameof(UpdateUserInfo)} completed");
            return userEntity.Id;
        }


        public long UpdateGroupInfo(GroupUpdateDto dto, ApplicationContext context = null)
        {
            if (context == null)
            {
                context = new ApplicationContext(_contextService.Options);
            }

            Group group;
            try
            {
                group = _vkApiService.GetGroup(new List<string> {dto.Id});
            }
            catch
            {
                return 0;
            }

            var entityGroup = context.UserGroups.FirstOrDefault(g => g.ExternalId == group.Id);
            if (entityGroup == null)
            {
                entityGroup = new VkUserGroup
                {
                    Title = group.Name,
                    ExternalId = group.Id,
                    GroupType = group.Type == GroupType.Group ? 0 : 1,
                    IsDeactivated = false
                };
                context.Add(entityGroup);
                context.SaveChanges();
            }

            
            if (dto.WithUsers)
            {
                Console.WriteLine("Обновление пользователей группы");
                var userCounter = 0;
                var groupUsers = _vkApiService.GetGroupMembers(dto.Id);
                var usersIds = groupUsers.Select(u => u.Id);
                var entityUsers = context.VkUsers.Where(u => usersIds.Contains(u.ExternalId)).Select(u => u.ExternalId).ToList();
                var newUsers = new List<VkUser>();
                foreach (var user in groupUsers)
                {
                    if (entityUsers.Contains(user.Id))
                    {
                        continue;
                    }

                    int? day = null, month = null, year = null;
                    if (!string.IsNullOrEmpty(user.BirthDate))
                    {
                        
                        var splitted = user.BirthDate.Split(".");
                        if (splitted.Length > 0)
                        {
                            day = int.Parse(splitted[0]);
                        }
                        if (splitted.Length > 1)
                        {
                            month = int.Parse(splitted[1]);
                        }
                        if (splitted.Length > 2)
                        {
                            year = int.Parse(splitted[2]);
                        }
                    }
                    var entityUser = new VkUser()
                    {
                        ExternalId = user.Id,
                        FullName = $"{user.FirstName} {user.LastName}",
                        LastCheck = DateTime.Now,
                        PhotoUrl =  user.PhotoMaxOrig?.ToString(),
                        IsDeactivated = user.IsDeactivated,
                        BirthDay = day,
                        BirthMonth = month,
                        BirthYear = year
                        

                    };
                    newUsers.Add(entityUser);
                    if (++userCounter % 200 == 0)
                    {
                        context.AddRange(newUsers);
                        newUsers.Clear();
                        context.SaveChanges();
                        context.Dispose();
                        context = new ApplicationContext(_contextService.Options);
                    }

                }

                context.SaveChanges();
                entityUsers = context.VkUsers.Where(u => usersIds.Contains(u.ExternalId)).Select(u => u.Id).ToList();
                var userToGroups = context.UserToUserGroup.Where(u2g =>
                    u2g.VkUserGroupId == entityGroup.Id && entityUsers.Any(u => u == u2g.VkUserId));
                
                //Находим удаляем всех ушедших
                var deletedUsers = context.UserToUserGroup.Where(u2g =>
                    u2g.VkUserGroupId == entityGroup.Id && !userToGroups.Any(ug => ug.VkUserId == u2g.VkUserId));
                context.RemoveRange(deletedUsers);
                context.SaveChanges();
                context.Dispose();
                context = new ApplicationContext(_contextService.Options);
                
                
                //Добавляем новых
                userCounter = 0;
                foreach (var user in entityUsers)
                {
                    var u2g = new UserToUserGroup()
                    {
                        VkUserId = user,
                        VkUserGroupId = entityGroup.Id
                    };
                    context.Add(u2g);
                    if (++userCounter % 200 == 0)
                    {
                        context.SaveChanges();
                        context.Dispose();
                        context = new ApplicationContext(_contextService.Options);
                    }
                }

                context.SaveChanges();



            }
            


            return entityGroup.Id;
        }

        public Task UpdateMessages()
        {
            Console.WriteLine($"{GetType()}.{nameof(UpdateMessages)} started");
            var dialogs = _vkApiService.GetDialogs();
            foreach (var dialog in dialogs)
            {
                Console.WriteLine($"{GetType()}.{nameof(UpdateMessages)} getting dialog {dialog}");
                _vkApiService.GetMessages(dialog);
            }

            Console.WriteLine($"{GetType()}.{nameof(UpdateMessages)} completed");
            return Task.CompletedTask;
        }

        public Task UpdateAudio(long id, ApplicationContext context = null)
        {
            throw new NotImplementedException();
        }

        public Task UpdateVideo(long id, ApplicationContext context = null)
        {
            throw new NotImplementedException();
        }

        public Task UpdateWall(WallUpdateDto dto)
        {
            Console.WriteLine($"{GetType()}.{nameof(UpdateWall)} started");
            var context = new ApplicationContext(_contextService.Options);
            var wallOwnerId = dto.Id > 0
                ? context.VkUsers.FirstOrDefault(u => u.ExternalId == dto.Id)?.Id
                : context.UserGroups.FirstOrDefault(g => g.ExternalId == dto.Id)?.Id;
            if (wallOwnerId == null)
            {
                wallOwnerId = dto.Id > 0
                    ? UpdateUserInfo(dto.Id, context)
                    : UpdateGroupInfo(new GroupUpdateDto() {Id = Math.Abs(dto.Id).ToString()}, context);
            }

            var wallMessages = _vkApiService.GetWallMessage(dto.Id, dto.Offset, dto.Limit);

            var saveCounter = 0;
            foreach (var wallMessage in wallMessages)
            {
                var entityMessage =
                    context.VkWallMessages.FirstOrDefault(m =>
                        (dto.Id > 0 && m.VkUserId == wallOwnerId || dto.Id < 0 && m.VkGroupId == wallOwnerId) &&
                        m.ExternalId == wallMessage.Id);
                if (entityMessage != null)
                {
                    continue;
                }

                entityMessage = new VkWallMessage
                {
                    Text = wallMessage.Text,
                    ExternalId = wallMessage?.Id ?? 0,
                    LikesCount = wallMessage.Likes.Count,
                    RepostsCount = wallMessage.Reposts.Count,
                    AttachmentsCount = wallMessage.Attachments.Count,
                    CommentsCount = wallMessage.Comments?.Count ?? 0,
                    ViewsCount = wallMessage.Views?.Count ?? 0,
                    VkUserId = dto.Id > 0 ? wallOwnerId : null,
                    VkGroupId = dto.Id < 0 ? wallOwnerId : null,
                    CreationTime = wallMessage.Date ?? DateTime.Now
                };
                context.Add(entityMessage);
                if (++saveCounter % 200 == 0)
                {
                    context.SaveChanges();
                    context.Dispose();
                    context = new ApplicationContext(_contextService.Options);
                }
            }

            context.SaveChanges();
            Console.WriteLine($"{GetType()}.{nameof(UpdateWall)} completed");
            return Task.CompletedTask;
        }
    }
}