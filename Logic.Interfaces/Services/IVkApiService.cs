using System.Collections.Generic;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Utils;

namespace Logic.Interfaces.Services
{
    public interface IVkApiService
    {
        VkCollection<User> GetFriends(long? userId = null);

        User GetUser(long? userId);

        IEnumerable<Message> GetMessages(long userId);

        IEnumerable<long> GetDialogs(ulong offset = 0);

        Group GetGroup(List<string> ids);

        IEnumerable<User> GetGroupMembers(string id, long offset = 0);

        IEnumerable<Group> GetGroups(long userId);

        IEnumerable<Post> GetWallMessage(long userId, ulong offset = 0, int limit = 0);

        long? GetCurrentId();

        IEnumerable<Audio> GetAudios(long userId, ulong offset = 0);
    }
}