// File: DepositService.cs

using DragonAcc.Infrastructure;
using DragonAcc.Infrastructure.Entities;
using DragonAcc.Service.Common.IServices;
using DragonAcc.Service.Interfaces;
using DragonAcc.Service.Models;
using DragonAcc.Service.Models.Deposit;
using Microsoft.EntityFrameworkCore;

namespace DragonAcc.Service.Services
{
    public class DepositService : BaseService, IDepositService
    {
        public DepositService(DataContext dataContext, IUserService userService) : base(dataContext, userService)
        {
        }

        public async Task<ApiResult> Add(AddDepositModel model)
        {
            if (model.DepositAmount == null || string.IsNullOrEmpty(model.NumberCard))
            {
                return new ApiResult { Message = "Invalid deposit details." };
            }

            var newDeposit = new Deposit
            {
                UserId = _userService.UserId,
                TelecomO = model.TelecomO,
                DepositAmount = model.DepositAmount,
                NumberCard = model.NumberCard,
                NumberSeri = model.NumberSeri,
                Status = "Đang duyệt",
                CreatedDate = DateTime.UtcNow
            };

            _dataContext.Deposits.Add(newDeposit);
            await _dataContext.SaveChangesAsync();

            return new ApiResult(newDeposit) { Message = "Deposit request added successfully." };
        }

        public async Task<ApiResult> GetAllByUserId(int id)
        {
            var deposits = await _dataContext.Deposits
                .Where(d => d.UserId == id)
                .ToListAsync();

            return new ApiResult(deposits);
        }

        public async Task<ApiResult> GetAll()
        {
            var deposits = await _dataContext.Deposits.ToListAsync();
            return new ApiResult(deposits);
        }

        public async Task<ApiResult> UpdateStatus(UpdateStatus_Model model)
        {
            if (model.Id == null)
            {
                return new ApiResult { Message = "Missing required information for updating status." };
            }

            var deposit = await _dataContext.Deposits.FirstOrDefaultAsync(d => d.Id == model.Id);
            if (deposit == null)
            {
                return new ApiResult { Message = "Deposit not found." };
            }

            if (deposit.Status == "Thành công")
            {
                return new ApiResult { Message = "This deposit has already been approved." };
            }

            var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.Id == deposit.UserId);
            if (user == null)
            {
                return new ApiResult { Message = "User not found." };
            }

            if (deposit.DepositAmount == null)
            {
                return new ApiResult { Message = "Deposit amount is not set." };
            }

            decimal depositAmount = deposit.DepositAmount.Value;
            decimal userReceiveAmount = depositAmount * 0.8M;

            using var transaction = await _dataContext.Database.BeginTransactionAsync();
            try
            {
                if (decimal.TryParse(user.Balance, out decimal userBalance))
                {
                    user.Balance = (userBalance + userReceiveAmount).ToString();
                }
                else
                {
                    user.Balance = userReceiveAmount.ToString();
                }

                // Cập nhật số xu dựa trên số tiền nạp
                if (depositAmount <= 50000)
                {
                    user.Coin += 2;
                }
                else if (depositAmount > 50000 && depositAmount <= 200000)
                {
                    user.Coin += 4;
                }
                else if (depositAmount > 200000 && depositAmount <= 1000000)
                {
                    user.Coin += 8;
                }

                deposit.Status = "Thành công";
                deposit.UpdatedDate = DateTime.UtcNow;

                var notification = new Notification
                {
                    UserIdSend = _userService.UserId,
                    UserId = deposit.UserId,
                    Content = "Thẻ cào của bạn đã được duyệt.",
                    IsRead = false,
                    CreatedDate = DateTime.Now,
                };

                _dataContext.Notifications.Add(notification);
                _dataContext.Users.Update(user);
                _dataContext.Deposits.Update(deposit);

 
                var userStat = await _dataContext.Statisticals.FirstOrDefaultAsync(s => s.UserId == user.Id);

                if (userStat != null)
                {
                    userStat.TotalDeposit += depositAmount;
                }
                else
                {
                    userStat = new Statistical
                    {
                        UserId = user.Id,
                        TotalDeposit = depositAmount,
                        TotalEarnings = 0m,
                        TotalAccountSales = 0m,
                        TotalAccountPurchases = 0m,
                        CurrentAccountCount = 0,
                        SoldAccountCount = 0,
                        UnsoldAccountCount = 0,
                        TotalWithdrawn = 0m,
                        CreatedDate = DateTime.Now,
                    };

                    _dataContext.Statisticals.Add(userStat);
                }

                await _dataContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ApiResult { Message = "Thành công!" };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ApiResult { Message = $"Failed to update deposit status: {ex.Message}" };
            }
        }
    }
}
