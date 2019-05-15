using Core.DataContext;
using Core.Entity;
using Logic.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Managers
{
    public class GraphManager
    {
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

        public List<Link> GetUserLinks(long userId)
        {
            var result = new List<Link>();
            using (var context = new ApplicationContext())
            {
                var friends = context.FriendsUserToUsers.Where(e => e.LeftUserId == userId).Select(e => e.RightUserId);
                result = context.FriendsUserToUsers.Where(e => friends.Contains(e.LeftUserId) && e.LeftUserId != userId).Select(e => new Link() { Source = e.LeftUserId, Target = e.RightUserId }).Where(e => context.Users.Any(u => (u.Id == e.Source || u.Id == e.Target) && !u.IsDeactivated)).ToList();

            }

            return result;
        }

        public List<Node> GetUserNodes(List<Link> links)
        {
            var result = new List<Node>();
            using (var context = new ApplicationContext())
            {

                result = context.Users.Where(e => links.Any(n => n.Source == e.Id || n.Target == e.Id )).Select(u => new Node() { Id = u.Id, User = u.FullName, Description = u.PhotoUrl, UserId = u.UserId }).ToList();

            }
            return result;
        }

    }
}
