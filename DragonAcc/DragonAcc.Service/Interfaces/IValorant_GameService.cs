using DragonAcc.Service.Models.Lol_GameModel;
using DragonAcc.Service.Models;
using DragonAcc.Service.Models.Valorant_GameModel;

namespace DragonAcc.Service.Interfaces
{
    public interface IValorant_GameService
    {
        Task<ApiResult> AddForUser(AddValorant_GameModel model);
        Task<ApiResult> UpdateForUser(UpdateValorant_GameModel model);
        Task<ApiResult> UpdateForAdmin(int id);
        Task<ApiResult> DeleteForUserAndAdmin(int id);
        Task<ApiResult> GetById(int id);
        Task<ApiResult> GetAll();
        Task<ApiResult> BuyGameAccount(BuyAccountValorant_GameModel model);
        Task<ApiResult> GetFullName(int id);
        Task<ApiResult> GetAllByUser(int userId);
    }
}
