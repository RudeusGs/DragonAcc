
namespace DragonAcc.Service.Models.WithDrawMoeny
{
    public class AddWithDrawMoneyModel
    {
        public string? NumberBank { get; set; }
        public string? NameBank { get; set; }
        public int? UserId { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }
    }
}
