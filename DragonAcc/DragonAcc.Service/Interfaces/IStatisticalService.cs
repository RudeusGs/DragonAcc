using DragonAcc.Service.Models;

namespace DragonAcc.Service.Interfaces
{
    public interface IStatisticalService
    {
        Task<ApiResult> GetByUserId(int id);
    }
}
