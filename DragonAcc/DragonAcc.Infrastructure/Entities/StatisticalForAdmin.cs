using DragonAcc.Infrastructure.Base;

namespace DragonAcc.Infrastructure.Entities
{
    public class StatisticalForAdmin : EntityBase
    {
        public decimal TotalRevenue { get; set; }
        public decimal TotalAccounts { get; set; }
        public decimal TotalDeposit { get; set; }
        public decimal TotalWithDrawForUser { get; set; }
        public decimal TotalAuction { get; set; }
    }
}
