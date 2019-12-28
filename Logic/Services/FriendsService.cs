using System.Collections.Generic;
using System.Linq;
using Core.DataContext;
using Core.Entity;
using VkNet.Model;

namespace Logic.Services
{
    public class FriendsService
    {
        public IQueryable<FriendsUserToUser> GetRemovedUsers(ApplicationContext context, long userId,
            List<User> users)
        {
            var newUsersIds = users.Select(e => e.Id);
            var newEntityUsers = context.VkUsers.Where(u => newUsersIds.Contains(u.ExternalId)).Select(u => u.Id).ToList();
            var removed = context.FriendsUserToUsers.Where(e => e.LeftUserId == userId && !newEntityUsers.Contains(e.RightUserId));

            return removed;
        }
        
        public List<User> GetNewUsers(ApplicationContext context, long userId, List<User> users)
        {
            var newUsers = users.Select(e => e.Id);

            var existsM2M = context.FriendsUserToUsers.Where(e => e.LeftUserId == userId/*).ToList().Where(e =>*/ && newUsers.Contains(e.RightUser.ExternalId)).Select(e => e.RightUserId).ToList();
            var existsUsers = context.VkUsers.Where(e => existsM2M.Contains(e.Id)).Select(e => e.ExternalId).ToList();
            newUsers = newUsers.Where(e => !existsUsers.Contains(e)).ToList();
            return users.Where(f => newUsers.Contains(f.Id)).ToList();
        }

    }
}