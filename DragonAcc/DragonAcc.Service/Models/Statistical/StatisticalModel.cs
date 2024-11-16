

namespace DragonAcc.Service.Models.Statistical
{
    public class StatisticalModel
    {
        // Tong thu nhap
        public decimal TotalPrice { get; set; }
        // Tong so tien ban duoc tu tai khoan
        public decimal TotalAccount { get; set; }
        // Tong so tien mua tai khoan
        public decimal TotalPurchasedAccount { get; set; }
        // Tong so tien nap vao website
        public decimal TotalDeposit { get; set; }
        //Tong so tai khoan
        public int? CountAccount { get; set; }
        // Tong so tai khoan ban duoc
        public int? AccountSold { get; set; }
        // Tong so tai khoan dang ban
        public int? UnSoldAccount { get; set; }
        public int? UserId { get; set; }
    }
}
