using DragonAcc.Service.Models;

namespace DragonAcc.Service.Interfaces
{
    public interface INotificationService
    {
        Task<ApiResult> GetAllByUserId(int UserId);
        Task<ApiResult> Read(int id);
    }
}
