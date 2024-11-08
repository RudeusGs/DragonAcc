using DragonAcc.Infrastructure;
using DragonAcc.Infrastructure.Entities;
using DragonAcc.Service.Common.IServices;
using DragonAcc.Service.Interfaces;
using DragonAcc.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace DragonAcc.Service.Services
{
    public class PurchasedAccountService : BaseService, IPurchasedAccountService
    {
        public PurchasedAccountService(DataContext dataContext, IUserService userService)
            : base(dataContext, userService)
        {
        }

        public async Task<ApiResult> GetAllByUser(int userId)
        {
            var result = await _dataContext.PurchasedAccounts
                .Where(x => x.UserId == userId)
                .ToListAsync();
            return new(result);
        }

    }
}
