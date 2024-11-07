using DragonAcc.Infrastructure.Entities;

namespace DragonAcc.Service.Interfaces
{
    public interface IMessageService
    {
        Task<Message> SendMessageAsync(Message message);
        Task<IEnumerable<Message>> GetConversationAsync(string user1, string user2);
        Task<Message> GetMessageByIdAsync(int id);
        Task<bool> DeleteMessageAsync(int id);
    }
}
