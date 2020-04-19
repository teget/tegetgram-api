using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Tegetgram.Data;
using Tegetgram.Data.Entities;
using Tegetgram.Services.DTOs;
using Tegetgram.Services.Interfaces;

namespace Tegetgram.Services
{
    public class MessageService : BaseService, IMessageService
    {
        public MessageService(TegetgramDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task<MessageDTO> GetMessage(string userName, Guid messageId)
        {
            Guid userId = await _dbContext.TegetgramUsers.Where(u => u.Owner.UserName == userName)
                .Select(u => u.Id)
                .SingleOrDefaultAsync();
            if (userId == null)
                throw new ApplicationException($"User {userName} could not be found.");

            Message message = await _dbContext.Messages.SingleOrDefaultAsync(m => m.Id == messageId && (m.RecepientId == userId || m.SenderId == userId));
            if (message == null)
                throw new ApplicationException($"User {userName} does not have a message with the id {messageId}.");

            message.IsNew = false;
            message.ReadOn = DateTime.Now;

            await _dbContext.SaveChangesAsync();

            return _mapper.Map<MessageDTO>(message);
        }

        public async Task<ICollection<MessageItemDTO>> GetMessages(string userName)
        {
            Guid userId = await _dbContext.TegetgramUsers.Where(u => u.Owner.UserName == userName)
                .Select(u => u.Id)
                .SingleOrDefaultAsync();
            if (userId == null)
                throw new ApplicationException($"User {userName} could not be found.");

            ICollection<Message> messages = await _dbContext.Messages.Where(m => m.RecepientId == userId || m.SenderId == userId).ToListAsync();

            return _mapper.Map<ICollection<MessageItemDTO>>(messages);
        }

        public async Task SendMessage(string fromUserName, string toUserName, string message)
        {
            Guid recepientId = await _dbContext.TegetgramUsers.Where(u => u.Owner.UserName == toUserName)
                .Select(u => u.Id)
                .SingleOrDefaultAsync();
            if (recepientId == null)
                throw new ApplicationException($"User {toUserName} could not be found.");

            Guid senderId = await _dbContext.TegetgramUsers.Where(u => u.Owner.UserName == fromUserName)
                .Select(u => u.Id)
                .SingleOrDefaultAsync();
            if (senderId == null)
                throw new ApplicationException($"User {fromUserName} could not be found.");

            if (await _dbContext.UserBlockings.AnyAsync(b => b.BlockerId == recepientId && b.BlockedId == senderId))
                throw new ApplicationException($"User {fromUserName} is blocked by user {toUserName}.");

            var telegraaf = new Message
            {
                IsNew = true,
                RecepientId = recepientId,
                SenderId = senderId,
                SentOn = DateTime.Now,
                Text = message
            };

            await _dbContext.Messages.AddAsync(telegraaf);
            await _dbContext.SaveChangesAsync();
        }
    }
}
