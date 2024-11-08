﻿    using DragonAcc.Infrastructure;
using DragonAcc.Infrastructure.Entities;
using DragonAcc.Infrastructure.Entities.GameInfoDetail;
    using DragonAcc.Service.Common.IServices;
    using DragonAcc.Service.Interfaces;
    using DragonAcc.Service.Models;
    using DragonAcc.Service.Models.Lol_GameModel;
    using Microsoft.AspNetCore.Http;
    using Microsoft.EntityFrameworkCore;

    namespace DragonAcc.Service.Services
    {
        public class Lol_GameService : BaseService, ILol_GameService
        {
            private readonly IFtpDirectoryService _ftpDirectoryService;
            public Lol_GameService(DataContext dataContext, IFtpDirectoryService ftpDirectoryService, IUserService userService) : base(dataContext, userService)
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

                var accountFolder = $"public/Lol_Game/{accountId}";

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
            public async Task<ApiResult> AddForUser(AddLol_GameModel model)
            {
                var gameAccount = await _dataContext.Lol_Games.FirstOrDefaultAsync(x => x.AccountName == model.AccountName);

                if (gameAccount == null)
                {
                    using var tran = _dataContext.Database.BeginTransaction();
                    try
                    {
                        var newGameAccount = new Lol_Game
                        {
                            GameName = "Liên minh huyền thoại",
                            AccountName = model.AccountName,
                            Password = model.Password,
                            ChampionCount = model.ChampionCount,
                            SkinCount = model.SkinCount,
                            Rank = model.Rank,
                            Price = model.Price,
                            Description = model.Description,
                            Status = "Đang chờ duyệt",
                            NoYetMoney = false,
                            UserId = _userService.UserId,                      
                            CreatedDate = _now
                        };

                        _dataContext.Lol_Games.Add(newGameAccount);
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
                        return new(newGameAccount);
                    }
                    catch (Exception e)
                    {
                        await tran.RollbackAsync();
                        throw new Exception(e.Message);
                    }
                }

                return new()
                {
                    Message = "Tài khoản này đã tồn tại! Vui lòng nhập thêm tài khoản khác."
                };
            }

            public async Task<ApiResult> DeleteForUserAndAdmin(int id)
            {
                var gameaccount = await _dataContext.Lol_Games.FirstOrDefaultAsync(x => x.Id == id);
                if (gameaccount != null)
                {
                    using var tran = await _dataContext.Database.BeginTransactionAsync();
                    try
                    {
                        _dataContext.Lol_Games.Remove(gameaccount);
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
                return new ApiResult() { Message = "Không tìm thấy tài khoản này." };
            }

            public async Task<ApiResult> GetAll()
            {
                var result = await _dataContext.Lol_Games.ToListAsync();
                return new(result);
            }

            public async Task<ApiResult> GetById(int id)
            {
                var result = await _dataContext.Lol_Games.FirstOrDefaultAsync(x => x.Id == id);
                return new(result);
            }

            public async Task<ApiResult> UpdateForUser(UpdateLol_GameModel model)
            {
                var gameAccount = await _dataContext.Lol_Games.FirstOrDefaultAsync(x => x.Id == model.Id);

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
                        gameAccount.Description = model.Description ?? gameAccount.Description;
                        gameAccount.Price = model.Price ?? gameAccount.Price;
                        gameAccount.UpdatedDate = _now;
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

                        return new() { Message = "Cập nhật thành công!" };
                    }
                    catch (Exception e)
                    {
                        await tran.RollbackAsync();
                        throw new Exception(e.Message);
                    }
                }

                return new() { Message = "Không tìm thấy tài khoản game này." };
            }

            public async Task<ApiResult> UpdateForAdmin(int id)
            {
                var gameAccount = await _dataContext.Lol_Games.FirstOrDefaultAsync(x => x.Id == id);

                if (gameAccount != null)
                {
                    using var tran = await _dataContext.Database.BeginTransactionAsync();
                    try
                    {
                        if(gameAccount.Status == "Đang chờ duyệt")
                        {
                            gameAccount.Status = "Đang bán";
                            gameAccount.PasswordChanged = RandomPasswordChangeService.GenerateRandomString();
                            gameAccount.AdminUpdate = _userService.UserId;
                        }
                        else
                        {
                            return new() { Message = "Lỗi trạng thái không dúng" };
                        }
                        await _dataContext.SaveChangesAsync();
                        await tran.CommitAsync();

                        return new() { Message = "Duyệt thành công" };
                    }
                    catch (Exception e)
                    {
                        await tran.RollbackAsync();
                        throw new Exception(e.Message);
                    }
                }

                return new() { Message = "Không tìm thấy tài khoản game này." };
            }
        public async Task<ApiResult> GetFullName(int id)
        {
            var game = await _dataContext.Lol_Games.FirstOrDefaultAsync(x => x.Id == id);

            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == game.UserId);
            return new(user.FullName);
        }
        public async Task<ApiResult> GetAllByUser(int userId)
        {
            var result = await _dataContext.Lol_Games
               .Where(x => x.UserId == userId)
               .ToListAsync();
            return new(result);
        }
        public async Task<ApiResult> BuyGameAccount(BuyAccountLol_GameModel model)
        {

            Console.WriteLine("Received Game Account ID: " + model.Id);
            if (model.Id == null)
            {
                return new ApiResult { Message = "Game account ID is required." };
            }

            var gameAccount = await _dataContext.Lol_Games.FirstOrDefaultAsync(x => x.Id == model.Id);
            if (gameAccount == null)
            {
                return new ApiResult { Message = "Game account not found." };
            }

            if (gameAccount.Status == "Đã bán")
            {
                return new ApiResult { Message = "Tài khoản này đã bán." };
            }

            var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == model.UserId);
            if (user == null)
            {
                return new ApiResult { Message = "User not found." };
            }

            if (!decimal.TryParse(user.Balance, out decimal userBalance))
            {
                return new ApiResult { Message = "Số dư của người dùng không hợp lệ." };
            }

            if (!gameAccount.Price.HasValue || gameAccount.Price <= 0)
            {
                return new ApiResult { Message = "Giá của tài khoản game không hợp lệ." };
            }

            decimal accountPrice = gameAccount.Price.Value;

            if (userBalance < accountPrice)
            {
                return new ApiResult { Message = "Số dư không đủ để mua tài khoản này." };
            }      
            if (gameAccount.UserId == _userService.UserId)
            {
                return new ApiResult { Message = "Bạn không thể mua tài khoản của chính mình" };
            }
            if(gameAccount.Status == "Đang chờ duyệt")
            {
                return new ApiResult { Message = "Tài khoản này chưa duyệt không thể mua" };
            }
            var seller = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == gameAccount.UserId);
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
                user.Balance = (userBalance - accountPrice).ToString();
                _dataContext.Update(user);

                var purchasedAccount = new PurchasedAccount
                {
                    UserId = _userService.UserId,
                    GameName = gameAccount.GameName,
                    AccountName = gameAccount.AccountName,
                    AccountPasswordChange = gameAccount.PasswordChanged,
                    Price = accountPrice,
                    CreatedDate = DateTime.UtcNow,
                };

                _dataContext.PurchasedAccounts.Add(purchasedAccount);
                gameAccount.Status = "Đã bán";
                gameAccount.NoYetMoney = false;
                await _dataContext.SaveChangesAsync();
                await tran.CommitAsync();

                return new ApiResult
                {
                    Message = "Mua tài khoản thành công.",
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

