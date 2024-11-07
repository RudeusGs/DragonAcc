using DragonAcc.Infrastructure.Base;

namespace DragonAcc.Infrastructure.Entities.GameInfoDetail
{
    public class Pubg_Game : GameInforBase
    {
        public int? GunSkinCount { get; set; }
        public int? HumanSkinCount { get; set; }
        public string? Rank { get; set; }
    }
}
