using DragonAcc.Infrastructure;
using DragonAcc.Service.Common.IServices;
using DragonAcc.Service.Interfaces;
using DragonAcc.Service.Models;
using DragonAcc.Service.Models.LuckyWheel;
using Microsoft.EntityFrameworkCore;

namespace DragonAcc.Service.Services
{
    public class LuckyWheelListPrizeService : BaseService, ILuckyWheelListPrizeService
    {
        public LuckyWheelListPrizeService(DataContext dataContext, IUserService userService) : base(dataContext, userService)
        {
        }

        public async Task<ApiResult> GetAll()
        {
            var result = await _dataContext.LuckyWheelListPrizes.ToListAsync();
            return new(result);
        }


        public async Task<ApiResult> GetAllByUser(int userId)
        {
            var result = await _dataContext.LuckyWheelListPrizes
                .Where(x => x.UserId == userId)
                .ToListAsync();
            return new(result);
        }
    }
}
