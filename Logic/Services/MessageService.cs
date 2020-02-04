using System.Collections.Generic;
using System.Linq;
using Core.DataContext;
using Core.Entity;
using Logic.Interfaces.Services;
using VkNet.Model;

namespace Logic.Services
{
    public class MessageService : IMessageService
    {
        private readonly DataContextService _contextService;

        public MessageService(DataContextService contextService)
        {
            _contextService = contextService;
        }
        
        public void UpdateMessages(List<Message> messages, long chatId)
        {
            if ((messages?.Count ?? 0) == 0)
            {
                return;
            }
            var ids = messages.Select(m => m.Id ?? 0);
            var attachments = messages.Where(m => m.Attachments.Any()).SelectMany(m =>
                m.Attachments.Select(a => new MessageAttachment
                    {ExternalMessageId = m.Id ?? 0, ExternalId = a.Instance.Id ?? 0, Type = a.Type.Name}));
            using (var context = new  ApplicationContext(_contextService.Options))
            {
                var existsMessagesIds = context.VkMessages.Where(e => ids.Contains(e.ExternalId)).Select(e => e.ExternalId).ToList();              
                var newMessages = messages.Where(e => !existsMessagesIds.Contains(e.Id ?? 0));
                if (!newMessages.Any())
                {
                    return;
                }
                var user = context.VkUsers.FirstOrDefault(u => u.ExternalId == chatId);
                if (user == null)
                {
                    user = new VkUser
                    {
                        ExternalId = chatId


                    };
                    context.VkUsers.Add(user);
                    context.SaveChanges();
                }

                var insertResult = new List<VkMessage>();
                foreach (var message in newMessages)
                {
                    var mappedMessage = new VkMessage
                    {
                        ExternalId = message.Id ?? 0,
                        Text = message.Text,
                        VkUser = user,
                        DateTime = message.Date ?? default,
                        Title = message.Title,
                        DialogId = message.FromId ?? 0


                    };
                    insertResult.Add(mappedMessage);
                }
                context.VkMessages.AddRange(insertResult);
                context.SaveChanges();

            }

        }
    }
}