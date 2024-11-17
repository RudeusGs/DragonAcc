using DragonAcc.Infrastructure.Base;

namespace DragonAcc.Infrastructure.Entities
{
    public class Statistical : EntityBase
    {
        // Tổng số tiền kiếm được, có thể âm nếu người dùng lỗ
        public decimal TotalEarnings { get; set; }

        // Tổng số tiền từ bán tài khoản
        public decimal TotalAccountSales { get; set; }

        // Tổng số tiền đã chi cho việc mua tài khoản
        public decimal TotalAccountPurchases { get; set; }

        // Tổng số tiền nạp vào website
        public decimal TotalDeposit { get; set; }

        // Tổng số tài khoản hiện có của người dùng
        public int CurrentAccountCount { get; set; }

        // Tổng số tài khoản đã bán được
        public int SoldAccountCount { get; set; }

        // Tổng số tài khoản đang rao bán nhưng chưa bán được
        public int UnsoldAccountCount { get; set; }

        // Tổng số giao dịch bán tài khoản thành công
        public int SuccessfulSalesCount { get; set; }

        // Tổng số giao dịch mua tài khoản thành công
        public int SuccessfulPurchasesCount { get; set; }

        // Tổng số tiền đã rút khỏi website
        public decimal TotalWithdrawn { get; set; }

        // Tổng số tài khoản đã thêm vào trang bán hàng nhưng chưa được đăng bán
        public int DraftAccountsCount { get; set; }

        // Id của người dùng (người bán) trong bảng thống kê
        public int? UserId { get; set; }
    }
}
