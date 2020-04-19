using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tegetgram.Data;
using Tegetgram.Data.Entities;
using Tegetgram.Services.Interfaces;

namespace Tegetgram.Services
{
    public class MessageService : BaseService, IMessageService
    {
        public MessageService(TegetgramDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<ICollection<Message>> GetMessages(string userName)
        {
            Guid userId = await _dbContext.TegetgramUsers.Where(u => u.Owner.UserName == userName)
                .Select(u => u.Id)
                .SingleOrDefaultAsync();
            if (userId == null)
                throw new ApplicationException($"User {userName} could not be found.");

            List<Message> messages = await _dbContext.Messages.Where(m => m.RecepientId == userId || m.SenderId == userId).ToListAsync(); // should make get messages read first, maybee -YA

            return messages;
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
