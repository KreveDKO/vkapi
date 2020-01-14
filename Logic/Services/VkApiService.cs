using System.Collections.Generic;
using System.Linq;
using Core.DataContext;
using Logic.Managers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VkNet;
using VkNet.AudioBypassService.Extensions;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace Logic.Services
{
    public class VkApiService
    {
        private VkApi _vkApi;
        private MessageManager _messageManager;
        private DataContextService _contextService;
        
        public VkApiService(IConfiguration config, MessageManager messageManager, DataContextService contextService)
        {
            _messageManager = messageManager;
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

        public long? GetCurrentId => _vkApi.UserId;

        public User GetUser(long? userId) => _vkApi.Users.Get(new List<long> {userId ?? GetCurrentId ?? 0},
                ProfileFields.FirstName | ProfileFields.PhotoMaxOrig | ProfileFields.LastName | ProfileFields.Photo50)
            .FirstOrDefault();


        public List<Message> GetMessages(long userId)
        {
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
                _messageManager.UpdateMessages(getResult.Messages.ToList(), userId);
                @params.Offset += 200;
                getResult = _vkApi.Messages.GetHistory(@params);
            }

            _messageManager.UpdateMessages(getResult.Messages.ToList(), userId);
            return result;
        }


        public List<long> GetDialogs(ulong offset = 0)
        {
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

            return result;
        }

        public List<Group> GetGroups(long userId)
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
    }
}