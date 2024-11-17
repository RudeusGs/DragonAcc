using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;
using DragonAcc.Infrastructure.Entities;
using DragonAcc.Infrastructure.Entities.GameInfoDetail;
using DragonAcc.Infrastructure.Base;
using DragonAcc.Service.Models.LuckyWheel;

namespace DragonAcc.Infrastructure
{
    public class DataContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Valorant_Game> Valorant_Games { get; set; }
        public DbSet<TocChien_Game> TocChien_Games { get; set; }
        public DbSet<Pubg_Game> Pubg_Games { get; set; }
        public DbSet<NgocRong_Game> NgocRong_Games { get; set; }
        public DbSet<Lol_Game> Lol_Games { get; set; }
        public DbSet<PurchasedAccount> PurchasedAccounts { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<LuckyWheel> LuckyWheels { get; set; }
        public DbSet<LuckyWheelListPrize> LuckyWheelListPrizes { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Statistical> Statisticals { get; set; }
        public DbSet<Notification>  Notifications { get; set; }
        public DbSet<StatisticalForAdmin> StatisticalForAdmins { get; set; }
        public DbSet<WithDrawMoney> WithDrawMoney { get; set; }
        public DbSet<AuctionDetail> AuctionDetails { get; set; }

    }
}
