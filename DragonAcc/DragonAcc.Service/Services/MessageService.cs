using DragonAcc.Infrastructure;
using DragonAcc.Infrastructure.Entities;
using DragonAcc.Service.Common.IServices;
using DragonAcc.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DragonAcc.Service.Services
{
    public class MessageService : BaseService, IMessageService
    {
        public MessageService(DataContext dataContext, IUserService userService) : base(dataContext, userService)
        {
        }

        public async Task<Message> SendMessageAsync(Message message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            message.CreatedDate = DateTime.UtcNow;
            await _dataContext.Messages.AddAsync(message);
            await _dataContext.SaveChangesAsync();
            return message;
        }

        public async Task<IEnumerable<Message>> GetConversationAsync(string user1, string user2)
        {
            if (string.IsNullOrEmpty(user1) || string.IsNullOrEmpty(user2))
                throw new ArgumentException("User identifiers cannot be null or empty.");

            return await _dataContext.Messages
                .Where(m => (m.Sender == user1 && m.Receiver == user2) || (m.Sender == user2 && m.Receiver == user1))
                .OrderBy(m => m.CreatedDate)
                .ToListAsync();
        }

        public async Task<Message> GetMessageByIdAsync(int id)
        {
            return await _dataContext.Messages.FindAsync(id);
        }

        public async Task<bool> DeleteMessageAsync(int id)
        {
            var message = await _dataContext.Messages.FindAsync(id);
            if (message != null)
            {
                _dataContext.Messages.Remove(message);
                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
