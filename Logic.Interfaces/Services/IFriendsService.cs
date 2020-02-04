using System.Collections.Generic;
using System.Linq;
using Core.DataContext;
using Core.Entity;
using VkNet.Model;

namespace Logic.Interfaces.Services
{
    public interface IFriendsService
    {
        IQueryable<FriendsUserToUser> GetRemovedUsers(ApplicationContext context, long userId,
            List<User> users);

        List<User> GetNewUsers(ApplicationContext context, long userId, List<User> users);
    }
}