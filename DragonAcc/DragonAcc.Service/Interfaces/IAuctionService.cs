using DragonAcc.Infrastructure.Entities;
using DragonAcc.Service.Models;
using DragonAcc.Service.Models.Auction;

namespace DragonAcc.Service.Interfaces
{
    public interface IAuctionService
    {
        public Task<ApiResult> GetAll();
        public Task<ApiResult> GetById(int id);
        public Task<ApiResult> Delete(int id);
        public Task<ApiResult> Add(AddAuctionModel model);
        public Task<ApiResult> Update(UpdateAuctionModel model);
        public Task<ApiResult> UpdateCurrentPrice(UpdateCurrentPriceModel model);
        Task<ApiResult> GetWinnerByAuctionId(int auctionId);

        public Task<ApiResult> GetAllAuctionDetail(int auctionId);
        public Task<ApiResult> AddAuctionDetail(AddAuctionDetailModel model);
        public Task<ApiResult> EndAuction(int id);
    }
}