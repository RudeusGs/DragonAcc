using DragonAcc.Infrastructure.Base;

namespace DragonAcc.Infrastructure.Entities.GameInfoDetail
{
    public class Valorant_Game : GameInforBase
    {
        public int? GunSkinCount { get; set; }
        public int? ChampionCount { get; set; }
        public string? Rank { get; set; }
    }
}
