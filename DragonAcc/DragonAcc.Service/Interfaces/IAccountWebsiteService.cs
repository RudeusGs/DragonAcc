using DragonAcc.Service.Models;

namespace DragonAcc.Service.Interfaces
{
    public interface IAccountWebsiteService
    {
        Task<ApiResult> GetAll();
        Task<ApiResult> GetById(int id);
    }
}
