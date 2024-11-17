using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DragonAcc.Service.Models.Auction
{
    public class AddAuctionDetailModel
    {
        public int? AuctionId { get; set; }
        public int? UserID { get; set; }
        public int? RaisePrice { get; set; }
    }
}
