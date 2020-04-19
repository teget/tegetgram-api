using System;
using System.Threading.Tasks;
using Tegetgram.Data.Entities;

namespace Tegetgram.Services.Interfaces
{
    public interface IUserService
    {
        //Task<TegetgramUser> GetUser(string userName);
        //Task<Guid> CreateAsync(string userName, string ownerId);
        Task BlockUser(string forUserName, string blockUserName);
        Task UnblockUser(string forUserName, string blockUserName);
    }
}
