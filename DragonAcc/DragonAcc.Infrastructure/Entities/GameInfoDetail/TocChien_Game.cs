using DragonAcc.Infrastructure.Base;

namespace DragonAcc.Infrastructure.Entities.GameInfoDetail
{
    public class TocChien_Game : GameInforBase
    {
        public int? ChampionCount { get; set; }
        public int? SkinCount { get; set; }
        public string? Rank { get; set; }
    }
}
