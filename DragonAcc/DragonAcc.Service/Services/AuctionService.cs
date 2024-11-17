using DragonAcc.Infrastructure;
using DragonAcc.Infrastructure.Entities;
using DragonAcc.Service.Common.IServices;
using DragonAcc.Service.Interfaces;
using DragonAcc.Service.Models;
using DragonAcc.Service.Models.Auction;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
////using Microsoft.AspNetCore.SignalR;
//using DragonAcc.Hubs; // Thêm namespace chứa AuctionHub

namespace DragonAcc.Service.Services
{
    public class AuctionService : BaseService, IAuctionService
    {
        private readonly IFtpDirectoryService _ftpDirectoryService;
        //private readonly IHubContext<AuctionHub> _hubContext;

    public AuctionService(
        DataContext dataContext,
        IUserService userService,
        IFtpDirectoryService ftpDirectoryService
        //IHubContext<AuctionHub> hubContext
        )
        : base(dataContext, userService)
    {
        _ftpDirectoryService = ftpDirectoryService;
        //_hubContext = hubContext;
    }

        private async Task<List<string>> UploadFiles(int? accountId, List<IFormFile>? files)
        {
            var uploadedFilePaths = new List<string>();

            if (files == null || !accountId.HasValue)
            {
                return uploadedFilePaths;
            }

            var accountFolder = $"public/GameAccounts/{accountId}";

            foreach (var file in files)
            {
                var fileExt = Path.GetExtension(file.FileName);
                var stream = file.OpenReadStream();


                var fileName = $"{accountId}.{uploadedFilePaths.Count + 1}{fileExt}";

                var result = await _ftpDirectoryService.TransferToFtpDirectoryAsync(stream, accountFolder, fileName);

                if (result.Succeed)
                {
                    uploadedFilePaths.Add($"{accountFolder}/{fileName}");
                }
            }

            return uploadedFilePaths;
        }


        public async Task<ApiResult> Add(AddAuctionModel model)
        {
            var existingAuction = await _dataContext.Auctions.FirstOrDefaultAsync(x => x.AuctionName == model.AuctionName);
            if (existingAuction == null)
            {
                using var tran = await _dataContext.Database.BeginTransactionAsync();
                try
                {
                    var timeAuction = TimeSpan.Parse(model.TimeAuction);
                    var newAuction = new Auction
                    {
                        Prize = model.Prize,
                        AuctionName = model.AuctionName,
                        StartPrice = model.StartPrice,
                        CurrentPrice = model.StartPrice,
                        StartDateTime = model.StartDateTime,
                        Status = true,
                        TimeAuction = timeAuction.ToString(@"hh\:mm\:ss"),
                        WinnerId = null,
                        CreatedDate = DateTime.Now,
                    };

                    _dataContext.Auctions.Add(newAuction);
                    await _dataContext.SaveChangesAsync();

                    if (model.Files != null && model.Files.Count > 0)
                    {
                        var fileUploads = await UploadFiles(newAuction.Id, model.Files);
                        if (fileUploads.Count > 0)
                        {
                            newAuction.Image = string.Join(",", fileUploads);
                        }
                    }

                    await _dataContext.SaveChangesAsync();
                    await tran.CommitAsync();
                    return new(newAuction);
                }
                catch (Exception ex)
                {
                    await tran.RollbackAsync();
                    throw new Exception(ex.Message);
                }
            }

            return new() { Message = "Phiên đấu giá này đã tồn tại." };
        }


        public async Task<ApiResult> Delete(int id)
        {
            var auction = await _dataContext.Auctions.FirstOrDefaultAsync(x => x.Id == id);
            if (auction != null)
            {
                using var tran = await _dataContext.Database.BeginTransactionAsync();
                try
                {
                    _dataContext.Auctions.Remove(auction);
                    await _dataContext.SaveChangesAsync();
                    await tran.CommitAsync();
                    return new();
                }
                catch (Exception e)
                {
                    await tran.RollbackAsync();
                    throw new Exception(e.Message);
                }
            }

            return new() { Message = "Không tìm thấy đấu giá này." };
        }

        public async Task<ApiResult> GetAll()
        {
            var result = await _dataContext.Auctions.ToListAsync();
            return new(result);
        }

        public async Task<ApiResult> GetById(int id)
        {
            var auction = await _dataContext.Auctions.FirstOrDefaultAsync(x => x.Id == id);
            return auction != null ? new(auction) : new() { Message = "Không tìm thấy đấu giá này." };
        }

        public async Task<ApiResult> Update(UpdateAuctionModel model)
        {
            var auction = await _dataContext.Auctions.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (auction != null)
            {
                using var tran = await _dataContext.Database.BeginTransactionAsync();
                try
                {
                    if (string.IsNullOrEmpty(model.AucionName) &&
                        model.StartPrice == null &&
                        model.StartDateTime == null &&
                        string.IsNullOrEmpty(model.TimeAuction) &&
                        (model.Files == null || model.Files.Count == 0) &&
                        model.Prize == null)
                    {
                        return new() { Message = "Không có thông tin nào được cập nhật" };
                    }

                    auction.AuctionName = model.AucionName ?? auction.AuctionName;
                    auction.Prize = model.Prize ?? auction.Prize;
                    auction.StartPrice = model.StartPrice ?? auction.StartPrice;
                    auction.StartDateTime = model.StartDateTime ?? auction.StartDateTime;
                    auction.CurrentPrice = model.StartPrice ?? auction.CurrentPrice;
                    auction.TimeAuction = model.TimeAuction ?? auction.TimeAuction;
                    auction.UpdatedDate = _now;

                    if (model.Files != null && model.Files.Count > 0)
                    {
                        var fileUploads = await UploadFiles(auction.Id, model.Files);
                        if (fileUploads.Count > 0)
                        {
                            auction.Image = string.Join(",", fileUploads);
                        }
                    }

                    await _dataContext.SaveChangesAsync();
                    await tran.CommitAsync();
                    return new() { Message = "Cập nhật thành công!" };
                }
                catch (Exception e)
                {
                    await tran.RollbackAsync();
                    throw new Exception(e.Message);

                }
            }

            return new() { Message = "Không tìm thấy phiên đấu giá này." };
        }

        public async Task<ApiResult> UpdateCurrentPrice(UpdateCurrentPriceModel model)
        {
            var auction = await _dataContext.Auctions.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (auction == null)
            {
                return new ApiResult { Message = "Không tìm thấy phiên đấu giá." };
            }

            if (!decimal.TryParse(model.CurrentPrice, out decimal newPrice))
            {
                return new ApiResult { Message = "Số tiền không hợp lệ." };
            }

            if (!decimal.TryParse(auction.CurrentPrice, out decimal currentAuctionPrice))
            {
                return new ApiResult { Message = "Giá hiện tại của phiên đấu giá không hợp lệ." };
            }

            if (newPrice <= currentAuctionPrice)
            {
                return new ApiResult { Message = "Giá đặt mới phải cao hơn giá hiện tại." };
            }

            using var tran = await _dataContext.Database.BeginTransactionAsync();
            try
            {
                var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == _userService.UserId);
                if (user == null)
                {
                    return new ApiResult { Message = "Người dùng không tồn tại." };
                }

                if (!decimal.TryParse(user.Balance, out decimal userBalance))
                {
                    return new ApiResult { Message = "Số dư của người dùng không hợp lệ." };
                }
                if (userBalance < newPrice)
                {
                    return new ApiResult { Message = "Số dư không đủ để đặt giá thầu này." };
                }
                if (auction.WinnerId.HasValue)
                {
                    var previousWinner = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == auction.WinnerId.Value);
                    if (previousWinner != null)
                    {
                        if (!decimal.TryParse(previousWinner.Balance, out decimal previousUserBalance))
                        {
                            previousUserBalance = 0;
                        }
                        previousWinner.Balance = (previousUserBalance + currentAuctionPrice).ToString();
                        _dataContext.Users.Update(previousWinner);
                    }
                }
                user.Balance = (userBalance - newPrice).ToString();
                _dataContext.Users.Update(user);
                auction.WinnerId = _userService.UserId;
                auction.CurrentPrice = newPrice.ToString();
                await _dataContext.SaveChangesAsync();

                await tran.CommitAsync();
                return new ApiResult { Message = "Giá hiện tại được cập nhật thành công." };
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();
                return new ApiResult { Message = $"Lỗi khi cập nhật giá: {ex.Message}" };
            }
        }
        public async Task<ApiResult> GetWinnerByAuctionId(int auctionId)
        {
            var auction = await _dataContext.Auctions.FirstOrDefaultAsync(a => a.Id == auctionId);
            if (auction == null)
            {
                return new ApiResult { Message = "Không tìm thấy phiên đấu giá." };
            }

            if (!auction.WinnerId.HasValue)
            {
                return new ApiResult { Message = "Phiên đấu giá này chưa có người chiến thắng." };
            }

            var winner = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == auction.WinnerId.Value);
            if (winner == null)
            {
                return new ApiResult { Message = "Không tìm thấy người chiến thắng." };
            }

            var winnerInfo = new
            {
                winner.Id,
                winner.UserName,
                winner.FullName,
                winner.Email,
            };

            return new ApiResult(winnerInfo);
        }

        public Task<ApiResult> GetAllAuctionDetail(int auctionId)
        {
            var auctionDetails = _dataContext.AuctionDetails.Where(x => x.AuctionId == auctionId).ToList();
            return Task.FromResult(new ApiResult(auctionDetails));
        }

        public async Task<ApiResult> AddAuctionDetail(AddAuctionDetailModel model)
        {
            // Tìm phiên đấu giá theo ID
            var auction = await _dataContext.Auctions.FirstOrDefaultAsync(x => x.Id == model.AuctionId);

            if (auction == null)
            {
                return new ApiResult { Message = "Không tìm thấy phiên đấu giá." };
            }

            // Kiểm tra nếu phiên đấu giá đã kết thúc
            if (auction.Status != true)
            {
                return new ApiResult { Message = "Phiên đấu giá này đã kết thúc." };
            }

            // Kiểm tra nếu thời gian hiện tại vẫn chưa tới thời gian bắt đầu của phiên đấu giá
            if (DateTime.Now < auction.StartDateTime)
            {
                return new ApiResult { Message = "Phiên đấu giá này chưa bắt đầu." };
            }

            // Kiểm tra RaisePrice có hợp lệ không (phải lớn hơn CurrentPrice)
            if (!decimal.TryParse(auction.CurrentPrice, out decimal currentAuctionPrice))
            {
                return new ApiResult { Message = "Giá hiện tại của phiên đấu giá không hợp lệ." };
            }

            if (!model.RaisePrice.HasValue || model.RaisePrice.Value <= currentAuctionPrice)
            {
                return new ApiResult { Message = "Giá đặt mới phải cao hơn giá hiện tại." };
            }

            using var tran = await _dataContext.Database.BeginTransactionAsync();
            try
            {
                // Cập nhật thông tin người dùng đặt giá thầu
                var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == model.UserID);
                if (user == null)
                {
                    return new ApiResult { Message = "Người dùng không tồn tại." };
                }

                if (!decimal.TryParse(user.Balance, out decimal userBalance) || userBalance < model.RaisePrice.Value)
                {
                    return new ApiResult { Message = "Số dư không đủ để đặt giá thầu này." };
                }

                // Trả lại tiền cho người chiến thắng trước đó nếu có
                if (auction.WinnerId.HasValue)
                {
                    var previousWinner = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == auction.WinnerId.Value);
                    if (previousWinner != null)
                    {
                        if (!decimal.TryParse(previousWinner.Balance, out decimal previousUserBalance))
                        {
                            previousUserBalance = 0;
                        }
                        previousWinner.Balance = (previousUserBalance + currentAuctionPrice).ToString();
                        _dataContext.Users.Update(previousWinner);
                    }
                }

                // Cập nhật số dư người dùng và thông tin phiên đấu giá
                user.Balance = (userBalance - model.RaisePrice.Value).ToString();
                _dataContext.Users.Update(user);

                auction.WinnerId = model.UserID;
                auction.CurrentPrice = model.RaisePrice.Value.ToString();
                auction.UpdatedDate = DateTime.Now;

                // Thêm bản ghi vào bảng AuctionDetail
                var auctionDetail = new AuctionDetail
                {
                    AuctionId = model.AuctionId.Value,
                    UserID = model.UserID.Value,
                    RaisePrice = model.RaisePrice.Value,
                    CreatedDate = DateTime.Now
                };

                _dataContext.AuctionDetails.Add(auctionDetail);

                // Lưu các thay đổi
                await _dataContext.SaveChangesAsync();
                await tran.CommitAsync();

                return new ApiResult { Message = "Đặt giá thầu thành công.", Data = auctionDetail };
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();
                return new ApiResult { Message = $"Lỗi khi đặt giá thầu: {ex.Message}" };
            }
        }


        public async Task<ApiResult> EndAuction(int auctionId)
        {
            var auction = await _dataContext.Auctions.FirstOrDefaultAsync(x => x.Id == auctionId);

            if (auction == null)
            {
                return new ApiResult { Message = "Không tìm thấy phiên đấu giá." };
            }

            // Cập nhật trạng thái của phiên đấu giá thành false (kết thúc)
            auction.Status = false;
            auction.UpdatedDate = DateTime.Now;

            await _dataContext.SaveChangesAsync();

            return new ApiResult { Message = "Phiên đấu giá đã kết thúc thành công." };
        }

    }
}