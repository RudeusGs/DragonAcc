using DragonAcc.Service.Models;
using DragonAcc.Service.Models.WithDrawMoeny;

namespace DragonAcc.Service.Interfaces
{
    public interface IWithDrawMoneyService
    {
        Task<ApiResult> GetAll();
        Task<ApiResult> Add(AddWithDrawMoneyModel model);
        Task<ApiResult> UpdateStatus(int id);
    }
}
