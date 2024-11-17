using DragonAcc.Infrastructure;
using DragonAcc.Infrastructure.Entities;
using DragonAcc.Service.Common.IServices;
using DragonAcc.Service.Interfaces;
using DragonAcc.Service.Models;
using Microsoft.EntityFrameworkCore;

namespace DragonAcc.Service.Services
{
    public class NotificationService : BaseService, INotificationService
    {
        public NotificationService(DataContext dataContext, IUserService userService) : base(dataContext, userService)
        {
        }

        public async Task<ApiResult> GetAllByUserId(int UserId)
        {
            var result = await _dataContext.Notifications
              .Where(x => x.UserId == UserId)
              .ToListAsync();
            return new(result);
        }

        public async Task<ApiResult> Read(int id)
        {
            var currentUserId = _userService.UserId;
            var notification = await _dataContext.Notifications
                .FirstOrDefaultAsync(x => x.Id == id && x.UserId == currentUserId);

            if (notification == null)
            {
                return new ApiResult
                {

                    Message = "Thông báo không tồn tại hoặc không thuộc quyền của bạn."
                };
            }
            if (!notification.IsRead)
            {
                notification.IsRead = true;
                await _dataContext.SaveChangesAsync();
            }

            return new ApiResult
            {
                Data = notification,
                Message = "Đánh dấu thông báo là đã đọc thành công."
            };
        }

    }
}
