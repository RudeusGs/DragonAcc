using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonAcc.Infrastructure.Entities
{
    public class AuctionDetail
    {
        public int? Id { get; set; }
        public int? AuctionId { get; set; }
        public int? UserID { get; set; }
        public decimal? RaisePrice { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
