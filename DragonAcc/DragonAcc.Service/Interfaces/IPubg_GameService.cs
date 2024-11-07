using DragonAcc.Service.Models.Lol_GameModel;
using DragonAcc.Service.Models;
using DragonAcc.Service.Models.Pubg_GameModel;

namespace DragonAcc.Service.Interfaces
{
    public interface IPubg_GameService
    {
        Task<ApiResult> AddForUser(AddPubg_GameModel model);
        Task<ApiResult> UpdateForUser(UpdatePubg_GameModel model);
        Task<ApiResult> UpdateForAdmin(int id);
        Task<ApiResult> DeleteForUserAndAdmin(int id);
        Task<ApiResult> GetById(int id);
        Task<ApiResult> GetAll();
        Task<ApiResult> BuyGameAccount(BuyAccountPubg_GameModel model);
        Task<ApiResult> GetFullName(int id);
        Task<ApiResult> GetAllByUser(int userId);
    }
}
