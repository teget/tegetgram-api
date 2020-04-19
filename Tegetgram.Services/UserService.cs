using System;
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
    public class UserService : BaseService, IUserService
    {
        public UserService(TegetgramDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public async Task BlockUser(string forUserName, string blockUserName)
        {
            if (String.Equals(forUserName, blockUserName))
                throw new ApplicationException("You shall not block thyself!");

            Guid blockingUserId = await _dbContext.TegetgramUsers.Where(u => u.Owner.UserName == forUserName)
                .Select(u => u.Id)
                .SingleOrDefaultAsync();
            (Guid blockedUserId, string blockedUserName) = await _dbContext.TegetgramUsers.Where(u => u.Owner.UserName == blockUserName)
                .Select(u => new Tuple<Guid, string>(u.Id, u.Owner.UserName))
                .SingleOrDefaultAsync();

            if (blockedUserId == null)
                throw new ApplicationException($"User {blockUserName} could not be found.");
            if (blockingUserId == null)
                throw new ApplicationException($"User {forUserName} could not be found.");
            if (_dbContext.UserBlockings.Any(ub => ub.BlockerId == blockingUserId && ub.BlockedId == blockedUserId))
                throw new ApplicationException($"User {blockUserName} is already blocked.");

            await _dbContext.UserBlockings.AddAsync(new UserBlocking
            {
                BlockerId = blockingUserId,
                BlockedId = blockedUserId
            });
            await _dbContext.SaveChangesAsync();
        }

        public Task<TegetgramUserDTO> GetUser(string userName)
        {
            throw new NotImplementedException();
        }

        public Task UnblockUser(string forUserName, string blockUserName)
        {
            throw new NotImplementedException();
        }
    }
}
