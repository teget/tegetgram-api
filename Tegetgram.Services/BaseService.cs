using System;
using Tegetgram.Data;

namespace Tegetgram.Services
{
public abstract class BaseService
    {
        protected readonly TegetgramDbContext _dbContext;

        public BaseService(TegetgramDbContext dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
