using System;
using Microsoft.Extensions.Configuration;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkNet.Utils;

namespace Logic.Services
{
    public class VkApiService
    {
        private VkApi _vkApi;

        public VkApiService(IConfiguration config)
        {            
            ulong.TryParse(config["AppId"], out var appId);
            var login = config["VkAuth:Login"];
            var password = config["VkAuth:Password"];
            var authParams = new ApiAuthParams()
            {
                ApplicationId = appId,
                Settings = Settings.All,
                Login = login,
                Password = password
            };
            _vkApi = new VkApi();
            _vkApi.Authorize(authParams);

        }

        public VkCollection<User> GetFriends(long? userId = null)
        {
            var friendsGetParams = new FriendsGetParams{UserId = userId,Fields = ProfileFields.All};
           
            return _vkApi.Friends.Get(friendsGetParams);
        }

    }
}
