using DragonAcc.Infrastructure.Entities;
using DragonAcc.Service.Models;

namespace DragonAcc.Service.Interfaces
{
    public interface IPurchasedAccountService
    {
        Task<ApiResult> GetAllByUser(int userId);

    }
}
