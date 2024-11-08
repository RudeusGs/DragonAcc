using DragonAcc.Infrastructure.Base;

namespace DragonAcc.Service.Models.LuckyWheel
{
    public class LuckyWheelListPrize : EntityBase
    {
        public int? UserId { get; set; }
        public string? Prize {  get; set; }
    }
}
