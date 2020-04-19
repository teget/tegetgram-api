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

        public async Task<TegetgramUserDTO> GetUser(string askingUserName, string userName)
        {
            var userQuery = _dbContext.TegetgramUsers.Where(u => u.Owner.UserName == userName);
            if (userQuery.Any())
                throw new ApplicationException($"User {userName} could not be found");

            if (askingUserName != userName)
                return await _mapper.ProjectTo<TegetgramUserDTO>(userQuery, null, x => x.UserName).SingleOrDefaultAsync();
            else
                return _mapper.Map<TegetgramUserDTO>(await userQuery.SingleOrDefaultAsync());
        }

        public async Task BlockUser(string forUserName, string blockUserName)
        {
            if (String.Equals(forUserName, blockUserName))
                throw new ApplicationException("You shall not block thyself!");

            Guid blockingUserId = await _dbContext.TegetgramUsers.Where(u => u.Owner.UserName == forUserName)
                .Select(u => u.Id)
                .SingleOrDefaultAsync();
            Guid blockedUserId = await _dbContext.TegetgramUsers.Where(u => u.Owner.UserName == blockUserName)
                .Select(u => u.Id)
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

        public async Task UnblockUser(string forUserName, string unblockUserName)
        {
            Guid blockingUserId = await _dbContext.TegetgramUsers.Where(u => u.Owner.UserName == forUserName)
                .Select(u => u.Id)
                .SingleOrDefaultAsync();
            Guid unblockedUserId = await _dbContext.TegetgramUsers.Where(u => u.Owner.UserName == unblockUserName)
                .Select(u => u.Id)
                .SingleOrDefaultAsync();

            if (unblockedUserId == null)
                throw new ApplicationException($"User {unblockUserName} could not be found.");
            if (blockingUserId == null)
                throw new ApplicationException($"User {forUserName} could not be found.");

            if (_dbContext.UserBlockings.Any(b => b.BlockerId == blockingUserId && b.BlockedId == unblockedUserId))
            {
                UserBlocking blocking = new UserBlocking
                {
                    BlockerId = blockingUserId,
                    BlockedId = unblockedUserId
                };

                _dbContext.Remove<UserBlocking>(blocking);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
