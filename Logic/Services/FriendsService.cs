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
            var newEntityUsers = context.Users.Where(u => newUsersIds.Contains(u.UserId)).Select(u => u.Id).ToList();
            var removed = context.FriendsUserToUsers.Where(e => e.LeftUserId == userId && !newEntityUsers.Contains(e.RightUserId));

            return removed;
        }
        
        public List<User> GetNewUsers(ApplicationContext context, long userId, List<User> users)
        {
            var newUsers = users.Select(e => e.Id);

            var existsM2M = context.FriendsUserToUsers.Where(e => e.LeftUserId == userId/*).ToList().Where(e =>*/ && newUsers.Contains(e.RightUser.UserId)).Select(e => e.RightUserId).ToList();
            var existsUsers = context.Users.Where(e => existsM2M.Contains(e.Id)).Select(e => e.UserId).ToList();
            newUsers = newUsers.Where(e => !existsUsers.Contains(e)).ToList();
            return users.Where(f => newUsers.Contains(f.Id)).ToList();
        }

    }
}