using DragonAcc.Infrastructure.Base;

namespace DragonAcc.Infrastructure.Entities
{
    public class CheckinDay : EntityBase
    {
        public int? StreakCount { get; set; }
        public string? Prize {  get; set; }
        public int? UserId { get; set; }
    }
}
