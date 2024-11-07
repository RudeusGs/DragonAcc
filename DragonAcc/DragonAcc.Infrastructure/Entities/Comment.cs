using DragonAcc.Infrastructure.Base;

namespace DragonAcc.Infrastructure.Entities
{
    public class Comment : EntityBase
    {
        public int PostId { get; set; } 
        public int UserId { get; set; }
        public string Content { get; set; }
    }
}
