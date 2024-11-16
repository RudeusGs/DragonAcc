using DragonAcc.Infrastructure.Base;

namespace DragonAcc.Infrastructure.Entities
{
    public class Notification : EntityBase
    {
        public int? UserIdSend {  get; set; }
        public int? UserId { get; set; }
        public string? Content { get; set; }
        public bool IsRead { get; set; }
    }
}
