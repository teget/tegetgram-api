using System;
using System.Threading.Tasks;
using Tegetgram.Services.DTOs;

namespace Tegetgram.Services.Interfaces
{
    public interface IUserService
    {
        Task<TegetgramUserDTO> GetUser(string askingUserName, string userName);
        //Task<Guid> CreateAsync(string userName, string ownerId);
        Task BlockUser(string forUserName, string blockUserName);
        Task UnblockUser(string forUserName, string unblockUserName);
    }
}
