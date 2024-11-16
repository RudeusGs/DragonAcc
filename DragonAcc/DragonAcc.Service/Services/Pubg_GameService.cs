﻿using DragonAcc.Infrastructure;
using DragonAcc.Infrastructure.Entities;
using DragonAcc.Infrastructure.Entities.GameInfoDetail;
using DragonAcc.Service.Common.IServices;
using DragonAcc.Service.Interfaces;
using DragonAcc.Service.Models;
using DragonAcc.Service.Models.Pubg_GameModel;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DragonAcc.Service.Services
{
    public class Pubg_GameService : BaseService, IPubg_GameService
    {
        private readonly IFtpDirectoryService _ftpDirectoryService;

        public Pubg_GameService(DataContext dataContext, IFtpDirectoryService ftpDirectoryService, IUserService userService)
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

            var accountFolder = $"public/Pubg_Game/{accountId}";

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

        public async Task<ApiResult> AddForUser(AddPubg_GameModel model)
        {
            var gameAccount = await _dataContext.Pubg_Games.FirstOrDefaultAsync(x => x.AccountName == model.AccountName);

            if (gameAccount == null)
            {
                using var tran = await _dataContext.Database.BeginTransactionAsync();
                try
                {
                    var newGameAccount = new Pubg_Game
                    {
                        GameName = "Pubg",
                        AccountName = model.AccountName,
                        Password = model.Password,
                        HumanSkinCount = model.HumanSkinCount,
                        Price = model.Price,
                        GunSkinCount = model.GunSkinCount,
                        Rank = model.Rank,
                        Status = "Đang chờ duyệt",
                        NoYetMoney = false,
                        UserId = _userService.UserId,
                        CreatedDate = DateTime.UtcNow
                    };

                    _dataContext.Pubg_Games.Add(newGameAccount);
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
            var gameAccount = await _dataContext.Pubg_Games.FirstOrDefaultAsync(x => x.Id == id);
            if (gameAccount != null)
            {
                using var tran = await _dataContext.Database.BeginTransactionAsync();
                try
                {
                    _dataContext.Pubg_Games.Remove(gameAccount);
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
            var result = await _dataContext.Pubg_Games.ToListAsync();
            return new ApiResult(result);
        }

        public async Task<ApiResult> GetById(int id)
        {
            var result = await _dataContext.Pubg_Games.FirstOrDefaultAsync(x => x.Id == id);
            return new ApiResult(result);
        }

        public async Task<ApiResult> UpdateForUser(UpdatePubg_GameModel model)
        {
            var gameAccount = await _dataContext.Pubg_Games.FirstOrDefaultAsync(x => x.Id == model.Id);

            if (gameAccount != null)
            {
                using var tran = await _dataContext.Database.BeginTransactionAsync();
                try
                {
                    gameAccount.AccountName = model.AccountName ?? gameAccount.AccountName;
                    gameAccount.Password = model.Password ?? gameAccount.Password;
                    gameAccount.HumanSkinCount = model.HumanSkinCount ?? gameAccount.HumanSkinCount;
                    gameAccount.GunSkinCount = model.GunSkinCount ?? gameAccount.GunSkinCount;
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
            var gameAccount = await _dataContext.Pubg_Games.FirstOrDefaultAsync(x => x.Id == id);

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
                            statistical.CountAccount = (statistical.CountAccount ?? 0) + 1;
                            statistical.UnSoldAccount = (statistical.UnSoldAccount ?? 0) + 1;
                        }
                        else
                        {
                            statistical = new Statistical
                            {
                                UserId = gameAccount.UserId,
                                CountAccount = 1,
                                UnSoldAccount = 1,
                                TotalDeposit = 0m,
                                AccountSold = 0,
                                TotalWithDraw = 0m
                            };
                            _dataContext.Statisticals.Add(statistical);
                        }
                        var notification = new Notification
                        {
                            UserIdSend = _userService.UserId,
                            UserId = gameAccount.UserId,
                            Content = "Tài khoản của bạn đã được duyệt.",
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
            var game = await _dataContext.Pubg_Games.FirstOrDefaultAsync(x => x.Id == id);
            if (game == null)
            {
                return new ApiResult { Message = "Game account not found." };
            }

            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == game.UserId);
            return user != null ? new ApiResult(user.FullName) : new ApiResult { Message = "User not found." };
        }

        public async Task<ApiResult> GetAllByUser(int userId)
        {
            var result = await _dataContext.Pubg_Games
               .Where(x => x.UserId == userId)
               .ToListAsync();
            return new ApiResult(result);
        }

        public async Task<ApiResult> BuyGameAccount(BuyAccountPubg_GameModel model)
        {
            if (model.Id == null)
            {
                return new ApiResult { Message = "Game account ID is required." };
            }

            var gameAccount = await _dataContext.Pubg_Games.FirstOrDefaultAsync(x => x.Id == model.Id);
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
                if (decimal.TryParse(seller.Balance, out decimal sellerBalance))
                {
                    seller.Balance = (sellerBalance + sellerReceiveAmount).ToString();
                }
                else
                {
                    seller.Balance = sellerReceiveAmount.ToString();
                }
                _dataContext.Update(seller);
                buyer.Balance = (buyerBalance - accountPrice).ToString();
                _dataContext.Update(buyer);
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
                gameAccount.Status = "Đã bán";
                gameAccount.NoYetMoney = false;
                _dataContext.Update(gameAccount);

                var sellerStat = await _dataContext.Statisticals.FirstOrDefaultAsync(s => s.UserId == seller.Id);

                if (sellerStat != null)
                {
                    sellerStat.AccountSold = (sellerStat.AccountSold ?? 0) + 1;
                }
                else
                {
                    sellerStat = new Statistical
                    {
                        UserId = seller.Id,
                        CountAccount = 0,
                        UnSoldAccount = 0,
                        AccountSold = 1,
                        TotalDeposit = 0m,
                        TotalWithDraw = 0m
                    };
                    _dataContext.Statisticals.Add(sellerStat);
                }

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
