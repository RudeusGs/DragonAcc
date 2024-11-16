using DragonAcc.Infrastructure.Entities;
using DragonAcc.Infrastructure.Entities.GameInfoDetail;

namespace DragonAcc.Infrastructure.Base
{
    public class GameInforBase
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public int? AdminUpdate {  get; set; }
        public string? GameName { get; set; }
        public string? AccountName { get; set; }
        public string? Password { get; set; }
        public string? PasswordChanged { get; set; }
        public decimal? Price { get; set; }
        public string? Image { get; set; }
        public string? Status { get; set; }
        public bool? NoYetMoney { get; set; }
        public virtual DateTime? CreatedDate { get; set; }
        public virtual DateTime? UpdatedDate { get; set; }
    }
}
