using DragonAcc.Infrastructure;
using DragonAcc.Infrastructure.Entities;
using DragonAcc.Service.Common.IServices;
using DragonAcc.Service.Interfaces;
using DragonAcc.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace DragonAcc.Service.Services
{
    public class StatisticalService : BaseService, IStatisticalService
    {
        public StatisticalService(DataContext dataContext, IUserService userService) : base(dataContext, userService)
        {
        }

        public async Task<ApiResult> GetByUserId(int id)
        {
            var result = await _dataContext.Statisticals
              .Where(x => x.UserId == id)
              .ToListAsync();
            return new(result);
        }
    }
}
