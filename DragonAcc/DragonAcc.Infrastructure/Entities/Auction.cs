using DragonAcc.Infrastructure.Base;

namespace DragonAcc.Infrastructure.Entities
{
    public class Auction : EntityBase
    {
        public string? Prize { get; set; }
        public string? AuctionName { get; set; }
        public string? StartPrice { get; set; }
        public string? CurrentPrice { get; set; }
        public DateTime? StartDateTime { get; set; }
        public string? Image { get; set; }
        public string? TimeAuction { get; set; }
        public bool? Status { get; set; }
        public int? WinnerId { get; set; }
    }
}
