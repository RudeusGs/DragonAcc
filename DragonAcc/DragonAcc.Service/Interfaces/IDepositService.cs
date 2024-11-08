using DragonAcc.Service.Models;
using DragonAcc.Service.Models.Deposit;

namespace DragonAcc.Service.Interfaces
{
    public interface IDepositService
    {
        Task<ApiResult> GetAllByUserId(int id);
        Task<ApiResult> GetAll();
        Task<ApiResult> Add(AddDepositModel model);
        Task<ApiResult> UpdateStatus(UpdateStatus_Model model);
    }
}
