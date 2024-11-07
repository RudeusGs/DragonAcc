using DragonAcc.Service.Models.Lol_GameModel;
using DragonAcc.Service.Models;
using DragonAcc.Service.Models.NgocRong_GameModel;

namespace DragonAcc.Service.Interfaces
{
    public interface INgocRong_GameService
    {
        Task<ApiResult> AddForUser(AddNgocRong_GameModel model);
        Task<ApiResult> UpdateForUser(UpdateNgocRong_GameModel model);
        Task<ApiResult> UpdateForAdmin(int id);
        Task<ApiResult> DeleteForUserAndAdmin(int id);
        Task<ApiResult> GetById(int id);
        Task<ApiResult> GetAll();
        Task<ApiResult> BuyGameAccount(BuyAccountNgocRong_GameModel model);
        Task<ApiResult> GetFullName(int id);
        Task<ApiResult> GetAllByUser(int userId);
    }
}
