using DragonAcc.Service.Models;
using DragonAcc.Service.Models.TocChien_GameModel;

namespace DragonAcc.Service.Interfaces
{
    public interface ITocChien_GameService
    {
        Task<ApiResult> AddForUser(AddTocChien_GameModel model);
        Task<ApiResult> UpdateForUser(UpdateTocChien_GameModel model);
        Task<ApiResult> UpdateForAdmin(int id);
        Task<ApiResult> DeleteForUserAndAdmin(int id);
        Task<ApiResult> GetById(int id);
        Task<ApiResult> GetAll();
        Task<ApiResult> BuyGameAccount(BuyAccountTocChien_GameModel model);
        Task<ApiResult> GetFullName(int id);
        Task<ApiResult> GetAllByUser(int userId);
    }
}
