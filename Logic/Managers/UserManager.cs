using Core.DataContext;
using Core.Entity;
using Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic.Managers
{
    public class UserManager
    {
        VkApiService _vkApiService;
        public UserManager(VkApiService vkApiService)
        {
            _vkApiService = vkApiService;
        }

        void UpdateUser(User entityUser, ApplicationContext context)
        {
            var vkUser = _vkApiService.GetCurrentUser(entityUser.UserId);

            //var entityUser = context.Users.FirstOrDefault(u => u.UserId == userId);
            entityUser.FullName = $"{vkUser?.FirstName} {vkUser?.LastName}";
            entityUser.PhotoUrl = vkUser?.PhotoMaxOrig?.ToString();
            context.SaveChanges();
        }
        public void UpdateUsers()
        {
            using (var context = new ApplicationContext())
            {
                var users = context.Users.Where(u => u.FullName == "" || u.FullName == null);
                foreach (var user in users) 
                {
                    try
                    {
                        UpdateUser(user, context);
                    }
                    catch { }
                    
                }
            }
        }
    }
}
