using DragonAcc.Service.Models;
using DragonAcc.Service.Models.Lol_GameModel;

namespace DragonAcc.Service.Interfaces
{
    public interface ILol_GameService
    {
        Task<ApiResult> AddForUser(AddLol_GameModel model);
        Task<ApiResult> UpdateForUser(UpdateLol_GameModel model);
        Task<ApiResult> UpdateForAdmin(int id);
        Task<ApiResult> DeleteForUserAndAdmin(int id);
        Task<ApiResult> GetById(int id);
        Task<ApiResult> GetAll();
        Task<ApiResult> BuyGameAccount(BuyAccountLol_GameModel model);
        Task<ApiResult> GetFullName(int id);
        Task<ApiResult> GetAllByUser(int userId);
    }
}
