using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tegetgram.Data.Entities;

namespace Tegetgram.Services.Interfaces
{
    public interface IMessageService
    {
        Task<ICollection<Message>> GetMessages(string userName);
        Task SendMessage(string fromUserName, string toUserName, string message);
    }
}
