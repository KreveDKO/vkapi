﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Core.DataContext;
using Logic.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VkNet;
using VkNet.AudioBypassService.Extensions;
using VkNet.Enums.Filters;
using VkNet.Enums.SafetyEnums;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace Logic.Services
{
    public class VkApiService : IVkApiService
    {
        private VkApi _vkApi;
        private IMessageService _messageService;
        private DataContextService _contextService;

        public VkApiService(IConfiguration config, IMessageService messageService, DataContextService contextService)
        {
            _messageService = messageService;
            ulong.TryParse(config["AppId"], out var appId);
            var login = config["VkAuth:Login"];
            var password = config["VkAuth:Password"];
            var authParams = new ApiAuthParams
            {
                ApplicationId = appId,
                Settings = Settings.All,
                Login = login,
                Password = password
            };
            var services = new ServiceCollection();
            services.AddAudioBypass();
            _vkApi = new VkApi(services);
            _vkApi.Authorize(authParams);
            _contextService = contextService;
        }


        private bool CheckTotalCount(long userId, uint count)
        {
            bool result;
            using (var context = new ApplicationContext(_contextService.Options))
            {
                result = context.VkMessages.Count(m => m.VkUser.ExternalId == userId) == count;
            }

            return result;
        }

        public VkCollection<User> GetFriends(long? userId = null)
        {
            var friendsGetParams = new FriendsGetParams
            {
                UserId = userId,
                Fields = ProfileFields.FirstName | ProfileFields.PhotoMaxOrig | ProfileFields.LastName |
                         ProfileFields.Blacklisted | ProfileFields.Photo50
            };

            return _vkApi.Friends.Get(friendsGetParams);
        }

        public long? GetCurrentId() => _vkApi.UserId;

        public User GetUser(long? userId) => _vkApi.Users.Get(new List<long> {userId ?? GetCurrentId() ?? 0},
                ProfileFields.FirstName | ProfileFields.PhotoMaxOrig | ProfileFields.LastName | ProfileFields.Photo50)
            .FirstOrDefault();


        public IEnumerable<Message> GetMessages(long userId)
        {
            Debug.WriteLine($"{GetType()}.{nameof(GetMessages)} started");
            var result = new List<Message>();
            var @params = new MessagesGetHistoryParams
            {
                UserId = userId,
                Count = 200,
                Offset = 0
            };
            var getResult = _vkApi.Messages.GetHistory(@params);
            if (CheckTotalCount(userId, getResult.TotalCount))
            {
                return result;
            }

            var total = getResult.TotalCount;
            var count = 0;
            while (count < total)
            {
                count += getResult.Messages.Count();
                _messageService.UpdateMessages(getResult.Messages.ToList(), userId);
                @params.Offset += 200;
                getResult = _vkApi.Messages.GetHistory(@params);
            }

            _messageService.UpdateMessages(getResult.Messages.ToList(), userId);
            Debug.WriteLine($"{GetType()}.{nameof(GetMessages)} completed");
            return result;
        }


        public IEnumerable<long> GetDialogs(ulong offset = 0)
        {
            Debug.WriteLine($"{GetType()}.{nameof(GetDialogs)} started");
            var result = new List<long>();
            var @params = new GetConversationsParams
            {
                Count = 200,
                Offset = offset
            };
            var dialogs = _vkApi.Messages.GetConversations(@params);
            var total = dialogs.Count;
            while (result.Count < total)
            {
                result.AddRange(dialogs.Items.Select(d => d.Conversation.Peer.Id).ToList());
                @params.Offset += 200;
                dialogs = _vkApi.Messages.GetConversations(@params);
            }

            Debug.WriteLine($"{GetType()}.{nameof(GetDialogs)} completed");
            return result;
        }

        public Group GetGroup(List<string> ids)
        {
            var result = new Group();
            try
            {
                var response = _vkApi.Groups.GetById(ids, ids.First(), GroupsFields.All);
                result = response.FirstOrDefault();
            }
            catch
            {
            }

            return result;
        }

        public IEnumerable<User> GetGroupMembers(string id, long offset = 0)
        {
            var result = new List<User>();
            var paramas = new GroupsGetMembersParams()
            {
                GroupId = id,
                Offset = offset,
                Fields = UsersFields.All
            };
            var response = _vkApi.Groups.GetMembers(paramas);
            while (response?.Any() ?? false)
            {
                result.AddRange(response.ToList());
                paramas.Offset += 1000;
                try
                {
                    Console.WriteLine(paramas.Offset);
                    response = _vkApi.Groups.GetMembers(paramas);
                }
                catch
                {
                    response = null;
                }
            }

            return result;
        }

        public IEnumerable<Group> GetGroups(long userId)
        {
            var result = new List<Group>();
            try
            {
                var subscriptions = _vkApi.Users.GetSubscriptions(userId, 200, null, GroupsFields.All);
                var count = 0;
                while (subscriptions.Any())
                {
                    count += subscriptions.Count;
                    result.AddRange(subscriptions);
                    subscriptions = _vkApi.Users.GetSubscriptions(userId, 200, count, GroupsFields.All);
                }
            }
            catch
            {
            }

            return result;
        }

        public IEnumerable<Post> GetWallMessage(long userId, ulong offset = 0, int limit = 0)
        {
            ;
            var wallGetParams = new WallGetParams
            {
                OwnerId = userId,
                Count = 100,
                Offset = offset,
                Fields = WallFilter.All
            };
            var response = _vkApi.Wall.Get(wallGetParams);
            var result = new List<Post>();
            while ((response?.WallPosts?.Any() ?? false) && (limit == 0 || result.Count < limit))
            {
                result.AddRange(response.WallPosts);
                wallGetParams.Offset += 100;
                try
                {
                    Console.WriteLine(result.Count);
                    response = _vkApi.Wall.Get(wallGetParams);
                }
                catch
                {
                    response = null;
                }
            }

            return result;
        }

        public IEnumerable<Audio> GetAudios(long userId, long offset = 0)
        {
            var result = new List<Audio>();
            var audioGetParams = new AudioGetParams()
            {
                OwnerId = userId,
                Count = 6000,
                Offset = offset
                
            };
            var collection = _vkApi.Audio.Get(audioGetParams);
            while (collection.Any())
            {
                result.AddRange(collection);
                audioGetParams.Offset += collection.Count;
                collection = _vkApi.Audio.Get(audioGetParams);
            }


            return result;

        }

        public void SendWallMessage(long wallId, string message)
        {
            _vkApi.Wall.Post(new WallPostParams
            {
                OwnerId = wallId,
                Message = message
            });
        }
    }
}