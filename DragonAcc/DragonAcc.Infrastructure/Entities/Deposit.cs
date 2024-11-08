using DragonAcc.Infrastructure.Base;

namespace DragonAcc.Infrastructure.Entities
{
    public class Deposit : EntityBase
    {
        public int? UserId { get; set; }
        public string? TelecomO {  get; set; }
        public decimal? DepositAmount { get; set; }
        public string? NumberCard {  get; set; }
        public string? NumberSeri { get; set; }
        public string? Status { get; set; }
    }
}
