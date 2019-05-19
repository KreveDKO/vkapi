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
            var users = new List<User>();
            using (var context = new ApplicationContext())
            {
                //Список друзей пользователя и друзей его друзей
                var friends = context.FriendsUserToUsers.Where(e => e.LeftUserId == userId);
                var friendsOfFriends = context.FriendsUserToUsers.Where(e => friends.Select(f => f.RightUserId).Contains(e.LeftUserId) && (!(e.LeftUser.IsDeactivated && e.RightUser.IsDeactivated)));

                result.InsertRange(0, friends.Select(e => new Link { Source = e.LeftUserId, Target = e.RightUserId }));
                result.InsertRange(0, friendsOfFriends.Select(e => new Link { Source = e.LeftUserId, Target = e.RightUserId }));

                //var targetIds = result.Select(e => e.Source).Distinct();
                //var counts = targetIds.Select(e => new { Id = e, Count = result.Count(r => e == r.Source) }).Where(r => r.Count >= 2).ToList();
                //result = result.Where(r => counts.Select(c => c.Id).Contains(r.Target)).ToList();


            }

            return result;
        }

        public List<Node> GetUserNodes(List<Link> links)
        {
            var result = new List<Node>();
            using (var context = new ApplicationContext())
            {
                var usersIds = new List<long>();
                usersIds.InsertRange(0, links.Select(e => e.Source));
                usersIds.InsertRange(0, links.Select(e => e.Target));
                usersIds = usersIds.Distinct().ToList();
                result = context.Users
                    .Where(e => usersIds.Contains(e.Id))
                    .Select(u => new Node() { Id = u.Id, User = u.FullName, Description = u.PhotoUrl, UserId = u.UserId })
                    .ToList();
            }
            return result;
        }

    }
}
