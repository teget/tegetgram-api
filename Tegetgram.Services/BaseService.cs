using System;
using AutoMapper;
using Tegetgram.Data;

namespace Tegetgram.Services
{
    public abstract class BaseService
    {
        protected readonly TegetgramDbContext _dbContext;
        protected readonly IMapper _mapper;

        public BaseService(TegetgramDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
    }
}
