using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tegetgram.Data.Entities;
using Tegetgram.Services.DTOs;

namespace Tegetgram.Services.Interfaces
{
    public interface IMessageService
    {
        Task<MessageDTO> GetMessage(string userName, Guid messageId);
        Task<ICollection<MessageItemDTO>> GetMessages(string userName);
        Task SendMessage(string fromUserName, string toUserName, string message);
    }
}
