using DragonAcc.Service.Models;

namespace DragonAcc.Service.Interfaces 
{
    public interface ILuckyWheelListPrizeService
    {
        public Task<ApiResult> GetAll();
        public Task<ApiResult> GetAllByUser(int userId);
    }
}
