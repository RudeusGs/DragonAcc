using DragonAcc.Infrastructure.Base;

namespace DragonAcc.Infrastructure.Entities
{
    public class PurchasedAccount : EntityBase
    {
        public string? GameName { get; set; }
        public string? AccountName { get; set; }
        public string? AccountPasswordChange { get; set; }
        public decimal? Price { get; set; }
        public int? UserId { get; set; }
    }
}
