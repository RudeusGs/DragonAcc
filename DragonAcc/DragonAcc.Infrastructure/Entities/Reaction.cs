using DragonAcc.Infrastructure.Base;

namespace DragonAcc.Infrastructure.Entities
{
    public class Reaction : EntityBase
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Type { get; set; }
    }
}
