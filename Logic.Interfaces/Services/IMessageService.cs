using System.Collections.Generic;
using VkNet.Model;

namespace Logic.Interfaces.Services
{
    public interface IMessageService
    {
        void UpdateMessages(List<Message> messages, long chatId);
    }
}