// File: WithDrawMoneyService.cs

using DragonAcc.Infrastructure;
using DragonAcc.Infrastructure.Entities;
using DragonAcc.Service.Common.IServices;
using DragonAcc.Service.Interfaces;
using DragonAcc.Service.Models;
using DragonAcc.Service.Models.WithDrawMoeny;
using Microsoft.EntityFrameworkCore;

namespace DragonAcc.Service.Services
{
    public class WithDrawMoneyService : BaseService, IWithDrawMoneyService
    {
        public WithDrawMoneyService(DataContext dataContext, IUserService userService)
            : base(dataContext, userService)
        {
        }

        public async Task<ApiResult> Add(AddWithDrawMoneyModel model)
        {
            if (model == null)
            {
                return new ApiResult { Message = "Dữ liệu không hợp lệ." };
            }

            if (string.IsNullOrWhiteSpace(model.NumberBank))
            {
                return new ApiResult { Message = "Số tài khoản ngân hàng không được để trống." };
            }

            if (string.IsNullOrWhiteSpace(model.NameBank))
            {
                return new ApiResult { Message = "Tên ngân hàng không được để trống." };
            }

            if (!model.UserId.HasValue)
            {
                return new ApiResult { Message = "ID người dùng không hợp lệ." };
            }

            if (model.Amount <= 0)
            {
                return new ApiResult { Message = "Số tiền rút phải lớn hơn 0." };
            }

            var user = await _dataContext.Users
                .FirstOrDefaultAsync(u => u.Id == model.UserId.Value);

            if (user == null)
            {
                return new ApiResult { Message = "Người dùng không tồn tại." };
            }

            if (!decimal.TryParse(user.Balance, out decimal currentBalance))
            {
                return new ApiResult { Message = "Số dư người dùng không hợp lệ." };
            }

            if (currentBalance < model.Amount)
            {
                return new ApiResult { Message = "Số dư không đủ để thực hiện giao dịch." };
            }

            using (var transaction = await _dataContext.Database.BeginTransactionAsync())
            {
                try
                {
                    currentBalance -= model.Amount;
                    user.Balance = currentBalance.ToString("F2");

                    _dataContext.Users.Update(user);

                    var withdrawMoney = new WithDrawMoney
                    {
                        NumberBank = model.NumberBank,
                        NameBank = model.NameBank,
                        UserId = model.UserId,
                        Status = "Chờ duyệt",
                        Amount = model.Amount,
                        CreatedDate = DateTime.UtcNow
                    };

                    await _dataContext.WithDrawMoney.AddAsync(withdrawMoney);
                    await _dataContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ApiResult
                    {
                        Data = "Yêu cầu rút tiền đã được tạo thành công."
                    };
                }
                catch (DbUpdateConcurrencyException)
                {
                    await transaction.RollbackAsync();
                    return new ApiResult { Message = "Đã xảy ra xung đột dữ liệu. Vui lòng thử lại." };
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return new ApiResult { Message = "Đã xảy ra lỗi khi tạo yêu cầu rút tiền." };
                }
            }
        }

        public async Task<ApiResult> GetAll()
        {
            var result = await _dataContext.WithDrawMoney.ToListAsync();
            return new(result);
        }

        public async Task<ApiResult> UpdateStatus(int id)
        {
            var withdrawMoney = await _dataContext.WithDrawMoney
                .FirstOrDefaultAsync(w => w.Id == id);

            if (withdrawMoney == null)
            {
                return new ApiResult { Message = "Yêu cầu rút tiền không tồn tại." };
            }

            if (withdrawMoney.Status.Equals("Đã duyệt", StringComparison.OrdinalIgnoreCase))
            {
                return new ApiResult { Message = "Trạng thái đã được cập nhật." };
            }

            var user = await _dataContext.Users
                .FirstOrDefaultAsync(u => u.Id == withdrawMoney.UserId);

            if (user == null)
            {
                return new ApiResult { Message = "Người dùng không tồn tại." };
            }

            using (var transaction = await _dataContext.Database.BeginTransactionAsync())
            {
                try
                {
                    withdrawMoney.Status = "Đã duyệt";

                    var notification = new Notification
                    {
                        UserIdSend = _userService.UserId,
                        UserId = withdrawMoney.UserId,
                        Content = "Yêu cầu rút tiền của bạn đã được duyệt.",
                        IsRead = false,
                        CreatedDate = DateTime.Now,
                    };

                    _dataContext.Notifications.Add(notification);
                    _dataContext.WithDrawMoney.Update(withdrawMoney);

                    // Cập nhật thống kê cho người dùng
                    var userStat = await _dataContext.Statisticals.FirstOrDefaultAsync(s => s.UserId == user.Id);

                    if (userStat != null)
                    {
                        userStat.TotalWithdrawn += withdrawMoney.Amount;
                    }
                    else
                    {
                        userStat = new Statistical
                        {
                            UserId = user.Id,
                            TotalWithdrawn = withdrawMoney.Amount,
                            TotalDeposit = 0m,
                            TotalEarnings = 0m,
                            TotalAccountSales = 0m,
                            TotalAccountPurchases = 0m,
                            CurrentAccountCount = 0,
                            SoldAccountCount = 0,
                            UnsoldAccountCount = 0,
                            CreatedDate = DateTime.Now,
                        };

                        _dataContext.Statisticals.Add(userStat);
                    }

                    await _dataContext.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new ApiResult
                    {
                        Data = "Cập nhật trạng thái yêu cầu rút tiền thành công."
                    };
                }
                catch (DbUpdateConcurrencyException)
                {
                    await transaction.RollbackAsync();
                    return new ApiResult { Message = "Đã xảy ra xung đột dữ liệu. Vui lòng thử lại." };
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    return new ApiResult { Message = "Đã xảy ra lỗi khi cập nhật trạng thái yêu cầu rút tiền." };
                }
            }
        }
    }
}
