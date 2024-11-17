// File: TocChien_GameService.cs

using DragonAcc.Infrastructure;
using DragonAcc.Infrastructure.Entities;
using DragonAcc.Infrastructure.Entities.GameInfoDetail;
using DragonAcc.Service.Common.IServices;
using DragonAcc.Service.Interfaces;
using DragonAcc.Service.Models;
using DragonAcc.Service.Models.TocChien_GameModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonAcc.Service.Services
{
    public class TocChien_GameService : BaseService, ITocChien_GameService
    {
        private readonly IFtpDirectoryService _ftpDirectoryService;

        public TocChien_GameService(DataContext dataContext, IFtpDirectoryService ftpDirectoryService, IUserService userService)
            : base(dataContext, userService)
        {
            _ftpDirectoryService = ftpDirectoryService;
        }

        private async Task<List<string>> UploadFiles(int? accountId, List<IFormFile>? files)
        {
            var uploadedFilePaths = new List<string>();

            if (files == null || !accountId.HasValue)
            {
                return uploadedFilePaths;
            }

            var accountFolder = $"public/TocChien_Game/{accountId}";

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

        public async Task<ApiResult> AddForUser(AddTocChien_GameModel model)
        {
            var gameAccount = await _dataContext.TocChien_Games.FirstOrDefaultAsync(x => x.AccountName == model.AccountName);

            if (gameAccount == null)
            {
                using var tran = await _dataContext.Database.BeginTransactionAsync();
                try
                {
                    var newGameAccount = new TocChien_Game
                    {
                        GameName = "Tốc chiến",
                        AccountName = model.AccountName,
                        Password = model.Password,
                        ChampionCount = model.ChampionCount,
                        SkinCount = model.SkinCount,
                        Rank = model.Rank,
                        Price = model.Price,
                        Status = "Đang chờ duyệt",
                        NoYetMoney = false,
                        UserId = _userService.UserId,
                        CreatedDate = DateTime.UtcNow
                    };

                    _dataContext.TocChien_Games.Add(newGameAccount);
                    await _dataContext.SaveChangesAsync();

                    if (model.Files != null && model.Files.Any())
                    {
                        var fileUploads = await UploadFiles(newGameAccount.Id, model.Files);
                        if (fileUploads.Any())
                        {
                            newGameAccount.Image = string.Join(";", fileUploads);
                        }
                    }

                    await _dataContext.SaveChangesAsync();
                    await tran.CommitAsync();
                    return new ApiResult(newGameAccount);
                }
                catch (Exception e)
                {
                    await tran.RollbackAsync();
                    return new ApiResult { Message = $"Error: {e.Message}" };
                }
            }

            return new ApiResult
            {
                Message = "Tài khoản này đã tồn tại! Vui lòng nhập thêm tài khoản khác."
            };
        }

        public async Task<ApiResult> DeleteForUserAndAdmin(int id)
        {
            var gameAccount = await _dataContext.TocChien_Games.FirstOrDefaultAsync(x => x.Id == id);
            if (gameAccount != null)
            {
                using var tran = await _dataContext.Database.BeginTransactionAsync();
                try
                {
                    _dataContext.TocChien_Games.Remove(gameAccount);
                    await _dataContext.SaveChangesAsync();
                    await tran.CommitAsync();
                    return new ApiResult();
                }
                catch (Exception e)
                {
                    await tran.RollbackAsync();
                    return new ApiResult { Message = $"Error: {e.Message}" };
                }
            }
            return new ApiResult { Message = "Không tìm thấy tài khoản này." };
        }

        public async Task<ApiResult> GetAll()
        {
            var result = await _dataContext.TocChien_Games.ToListAsync();
            return new ApiResult(result);
        }

        public async Task<ApiResult> GetById(int id)
        {
            var result = await _dataContext.TocChien_Games.FirstOrDefaultAsync(x => x.Id == id);
            return new ApiResult(result);
        }

        public async Task<ApiResult> UpdateForUser(UpdateTocChien_GameModel model)
        {
            var gameAccount = await _dataContext.TocChien_Games.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (gameAccount != null)
            {
                using var tran = await _dataContext.Database.BeginTransactionAsync();
                try
                {
                    gameAccount.AccountName = model.AccountName ?? gameAccount.AccountName;
                    gameAccount.Password = model.Password ?? gameAccount.Password;
                    gameAccount.ChampionCount = model.ChampionCount ?? gameAccount.ChampionCount;
                    gameAccount.SkinCount = model.SkinCount ?? gameAccount.SkinCount;
                    gameAccount.Rank = model.Rank ?? gameAccount.Rank;
                    gameAccount.Price = model.Price ?? gameAccount.Price;
                    gameAccount.UpdatedDate = DateTime.UtcNow;

                    if (model.Files != null && model.Files.Any())
                    {
                        var fileUploads = await UploadFiles(gameAccount.Id, model.Files);
                        if (fileUploads.Any())
                        {
                            gameAccount.Image = string.IsNullOrEmpty(gameAccount.Image)
                                ? string.Join(";", fileUploads)
                                : $"{gameAccount.Image};{string.Join(";", fileUploads)}";
                        }
                    }

                    await _dataContext.SaveChangesAsync();
                    await tran.CommitAsync();

                    return new ApiResult { Message = "Cập nhật thành công!" };
                }
                catch (Exception e)
                {
                    await tran.RollbackAsync();
                    return new ApiResult { Message = $"Error: {e.Message}" };
                }
            }

            return new ApiResult { Message = "Không tìm thấy tài khoản game này." };
        }

        public async Task<ApiResult> UpdateForAdmin(int id)
        {
            var gameAccount = await _dataContext.TocChien_Games.FirstOrDefaultAsync(x => x.Id == id);

            if (gameAccount != null)
            {
                using var tran = await _dataContext.Database.BeginTransactionAsync();
                try
                {
                    if (gameAccount.Status == "Đang chờ duyệt")
                    {
                        gameAccount.Status = "Đang bán";
                        gameAccount.PasswordChanged = RandomPasswordChangeService.GenerateRandomString();
                        gameAccount.AdminUpdate = _userService.UserId;

                        var statistical = await _dataContext.Statisticals.FirstOrDefaultAsync(s => s.UserId == gameAccount.UserId);

                        if (statistical != null)
                        {
                            statistical.CurrentAccountCount += 1;
                            statistical.UnsoldAccountCount += 1;
                        }
                        else
                        {
                            statistical = new Statistical
                            {
                                UserId = gameAccount.UserId,
                                CurrentAccountCount = 1,
                                UnsoldAccountCount = 1,
                                TotalDeposit = 0m,
                                SoldAccountCount = 0,
                                TotalWithdrawn = 0m,
                                TotalEarnings = 0m,
                                TotalAccountSales = 0m,
                                TotalAccountPurchases = 0m,
                                CreatedDate = DateTime.Now,
                            };
                            _dataContext.Statisticals.Add(statistical);
                        }
                        var notification = new Notification
                        {
                            UserIdSend = _userService.UserId,
                            UserId = gameAccount.UserId,
                            Content = "Tài khoản tốc chiến của bạn đã được duyệt.",
                            IsRead = false,
                            CreatedDate = DateTime.Now,
                        };

                        _dataContext.Notifications.Add(notification);
                    }
                    else
                    {
                        return new ApiResult { Message = "Lỗi trạng thái không đúng." };
                    }

                    await _dataContext.SaveChangesAsync();
                    await tran.CommitAsync();

                    return new ApiResult
                    {
                        Data = new { Message = "Duyệt tài khoản thành công." }
                    };
                }
                catch (Exception e)
                {
                    await tran.RollbackAsync();
                    return new ApiResult { Message = $"Error: {e.Message}" };
                }
            }

            return new ApiResult { Message = "Không tìm thấy tài khoản game này." };
        }

        public async Task<ApiResult> GetFullName(int id)
        {
            var game = await _dataContext.TocChien_Games.FirstOrDefaultAsync(x => x.Id == id);
            if (game == null)
            {
                return new ApiResult { Message = "Game account not found." };
            }

            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == game.UserId);
            return user != null ? new ApiResult(user.FullName) : new ApiResult { Message = "User not found." };
        }

        public async Task<ApiResult> GetAllByUser(int userId)
        {
            var result = await _dataContext.TocChien_Games
               .Where(x => x.UserId == userId)
               .ToListAsync();
            return new ApiResult(result);
        }

        public async Task<ApiResult> BuyGameAccount(BuyAccountTocChien_GameModel model)
        {
            if (model.Id == null)
            {
                return new ApiResult { Message = "Game account ID is required." };
            }

            var gameAccount = await _dataContext.TocChien_Games.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (gameAccount == null)
            {
                return new ApiResult { Message = "Game account not found." };
            }

            if (gameAccount.Status == "Đã bán")
            {
                return new ApiResult { Message = "Tài khoản này đã bán." };
            }

            var buyer = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
            if (buyer == null)
            {
                return new ApiResult { Message = "User not found." };
            }

            if (!decimal.TryParse(buyer.Balance, out decimal buyerBalance))
            {
                return new ApiResult { Message = "Số dư của người dùng không hợp lệ." };
            }

            if (!gameAccount.Price.HasValue || gameAccount.Price <= 0)
            {
                return new ApiResult { Message = "Giá của tài khoản game không hợp lệ." };
            }

            decimal accountPrice = gameAccount.Price.Value;

            if (buyerBalance < accountPrice)
            {
                return new ApiResult { Message = "Số dư không đủ để mua tài khoản này." };
            }

            if (gameAccount.UserId == _userService.UserId)
            {
                return new ApiResult { Message = "Bạn không thể mua tài khoản của chính mình." };
            }

            if (gameAccount.Status == "Đang chờ duyệt")
            {
                return new ApiResult { Message = "Tài khoản này chưa duyệt không thể mua." };
            }

            var seller = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == gameAccount.UserId);
            if (seller == null)
            {
                return new ApiResult { Message = "Người bán không tồn tại." };
            }

            decimal sellerReceiveAmount = accountPrice * 0.9M;

            using var tran = await _dataContext.Database.BeginTransactionAsync();
            try
            {
                // Cập nhật số dư cho người bán
                if (decimal.TryParse(seller.Balance, out decimal sellerBalance))
                {
                    seller.Balance = (sellerBalance + sellerReceiveAmount).ToString();
                }
                else
                {
                    seller.Balance = sellerReceiveAmount.ToString();
                }

                // Cập nhật xu cho người mua
                if (accountPrice > 50000)
                {
                    buyer.Coin += 1;
                }

                _dataContext.Update(seller);

                // Cập nhật số dư cho người mua
                buyer.Balance = (buyerBalance - accountPrice).ToString();
                _dataContext.Update(buyer);

                // Thêm vào bảng PurchasedAccount
                var purchasedAccount = new PurchasedAccount
                {
                    UserId = buyer.Id,
                    GameName = gameAccount.GameName,
                    AccountName = gameAccount.AccountName,
                    AccountPasswordChange = gameAccount.PasswordChanged,
                    Price = accountPrice,
                    CreatedDate = DateTime.UtcNow,
                };

                _dataContext.PurchasedAccounts.Add(purchasedAccount);

                // Cập nhật trạng thái tài khoản game
                gameAccount.Status = "Đã bán";
                gameAccount.NoYetMoney = false;
                _dataContext.Update(gameAccount);

                // Cập nhật thống kê cho người bán
                var sellerStat = await _dataContext.Statisticals.FirstOrDefaultAsync(s => s.UserId == seller.Id);

                if (sellerStat != null)
                {
                    sellerStat.SoldAccountCount += 1;
                    sellerStat.UnsoldAccountCount -= 1;
                    sellerStat.TotalAccountSales += sellerReceiveAmount;
                    sellerStat.TotalEarnings += sellerReceiveAmount;
                }
                else
                {
                    sellerStat = new Statistical
                    {
                        UserId = seller.Id,
                        SoldAccountCount = 1,
                        UnsoldAccountCount = 0,
                        TotalDeposit = 0m,
                        TotalWithdrawn = 0m,
                        CurrentAccountCount = 0,
                        TotalAccountSales = sellerReceiveAmount,
                        TotalEarnings = sellerReceiveAmount,
                        TotalAccountPurchases = 0m,
                        CreatedDate = DateTime.Now,
                    };
                    _dataContext.Statisticals.Add(sellerStat);
                }

                // Cập nhật thống kê cho người mua
                var buyerStat = await _dataContext.Statisticals.FirstOrDefaultAsync(s => s.UserId == buyer.Id);

                if (buyerStat != null)
                {
                    buyerStat.CurrentAccountCount += 1;
                    buyerStat.TotalAccountPurchases += accountPrice;
                    buyerStat.TotalEarnings -= accountPrice;
                }
                else
                {
                    buyerStat = new Statistical
                    {
                        UserId = buyer.Id,
                        CurrentAccountCount = 1,
                        SoldAccountCount = 0,
                        UnsoldAccountCount = 0,
                        TotalDeposit = 0m,
                        TotalWithdrawn = 0m,
                        TotalAccountSales = 0m,
                        TotalEarnings = -accountPrice,
                        TotalAccountPurchases = accountPrice,
                        CreatedDate = DateTime.Now,
                    };
                    _dataContext.Statisticals.Add(buyerStat);
                }

                var notification = new Notification
                {
                    UserIdSend = model.UserId,
                    UserId = seller.Id,
                    Content = $"{buyer.FullName ?? buyer.UserName} đã mua tài khoản tốc chiến của bạn.",
                    IsRead = false,
                    CreatedDate = DateTime.Now,
                };
                _dataContext.Notifications.Add(notification);
                await _dataContext.SaveChangesAsync();
                await tran.CommitAsync();

                return new ApiResult
                {
                    Data = new { Message = "Mua tài khoản thành công." }
                };
            }
            catch (Exception ex)
            {
                await tran.RollbackAsync();
                return new ApiResult { Message = $"Error during purchase: {ex.InnerException?.Message ?? ex.Message}" };
            }
        }
    }
}
