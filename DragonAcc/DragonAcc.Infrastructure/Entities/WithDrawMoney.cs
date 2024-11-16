using DragonAcc.Infrastructure.Base;

namespace DragonAcc.Infrastructure.Entities
{
    public class WithDrawMoney : EntityBase
    {
        public string? NumberBank { get; set; }
        public string? NameBank { get; set; }
        public int? UserId { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }
    }
}
