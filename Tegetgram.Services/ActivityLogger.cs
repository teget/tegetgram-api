using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Tegetgram.Data;
using Tegetgram.Data.Entities;
using Tegetgram.Services.Interfaces;

namespace Tegetgram.Services
{
    public class ActivityLogger : BaseService, IActivityLogger
    {

        public ActivityLogger(TegetgramDbContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
        }

        public void Log(ILogger logger, string userName, string actionName, string message)
        {
            try
            {
                Guid userId = _dbContext.TegetgramUsers.Where(u => u.Owner.UserName == userName)
                    .Select(u => u.Id)
                    .SingleOrDefault();

                ActivityLog log = new ActivityLog
                {
                    ActionName = actionName,
                    Message = message,
                    TimeStamp = DateTime.Now,
                    UserId = userId
                };

                _dbContext.ActivityLogs.Add(log);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Logging failed for {userName} at action {actionName}. Message: {message}");
            }
        }
    }
}
