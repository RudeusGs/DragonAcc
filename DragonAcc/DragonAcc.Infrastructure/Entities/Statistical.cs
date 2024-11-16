using DragonAcc.Infrastructure.Base;

namespace DragonAcc.Infrastructure.Entities
{
    public class Statistical : EntityBase
    {
        // Tong so tien nap vao website
        public decimal TotalDeposit {  get; set; }
        //Tong so tai khoan
        public int? CountAccount { get; set; }
        // Tong so tai khoan ban duoc
        public int? AccountSold { get; set; }
        // Tong so tai khoan dang ban
        public int? UnSoldAccount { get; set; }
        // Id cua bang thong ke
        public int? UserId { get; set; }
        // Tong so tien da rut
        public decimal? TotalWithDraw { get; set; }

    }
}
