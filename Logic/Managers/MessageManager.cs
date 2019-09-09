using Core.DataContext;
using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Managers
{
    public class MessageManager
    {
        public bool CheckTotallCount(long userId, uint count)
        {
            bool result;
            using (var context = new ApplicationContext())
            {
                result = context.Messages.Count(m => m.User.UserId == userId) == count;
            }
            return result;
        }
        
        public void UpdateMessages(List<VkNet.Model.Message> messages, long chatId)
        {
            if (messages.Count == 0)
            {
                return;
            }
            var ids = messages.Select(m => m.Id ?? 0);
            var attachments = messages.Where(m => m.Attachments.Any()).SelectMany(m => m.Attachments.Select(a => new Attachment() { ExternalMessageId = m.Id ?? 0, ExternalId = a.Instance.Id ?? 0, Type = a.Type.Name}));
            using (var context = new ApplicationContext())
            {
                var existsMessagesIds = context.Messages.Where(e => ids.Contains(e.ExternalId)).Select(e => e.ExternalId).ToList();              
                var newMessages = messages.Where(e => !existsMessagesIds.Contains(e.Id ?? 0));
                if (!newMessages.Any())
                {
                    return;
                }
                var user = context.Users.FirstOrDefault(u => u.UserId == chatId);
                if (user == null)
                {
                    user = new User()
                    {
                        UserId = chatId


                    };
                    context.Users.Add(user);
                    context.SaveChanges();
                }

                var insertResult = new List<Message>();
                foreach (var message in newMessages)
                {
                    var mappedMessage = new Message()
                    {
                        ExternalId = message.Id ?? 0,
                        Text = message.Text,
                        User = user,
                        DateTime = message.Date ?? default,
                        Title = message.Title,
                        DialogId = message.FromId ?? 0


                    };
                    insertResult.Add(mappedMessage);
                }
                context.Messages.AddRange(insertResult);
                context.SaveChanges();

            }

        }
    }
}
