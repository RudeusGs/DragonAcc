using DragonAcc.Infrastructure.Entities;
using DragonAcc.Service.Common.IServices;
using DragonAcc.Service.Models;
using DragonAcc.Service.Models.LuckyWheel;

namespace DragonAcc.Service.Interfaces
{
    public interface ILuckyWheelService
    {
        public Task<ApiResult> GetAll();
        public Task<ApiResult> Add(AddLuckyWheelModel model);
        public Task<ApiResult> Update(UpdateLuckyWheelModel model);
        public Task<ApiResult> SpinWheel(int cardIndex);
    }
}
