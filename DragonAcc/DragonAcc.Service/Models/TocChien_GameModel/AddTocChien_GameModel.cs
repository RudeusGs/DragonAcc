using DragonAcc.Infrastructure.Entities;
using Microsoft.AspNetCore.Http;

namespace DragonAcc.Service.Models.TocChien_GameModel
{
    public class AddTocChien_GameModel
    {
        public string? AccountName { get; set; }
        public string? Password { get; set; }
        public int? ChampionCount { get; set; }
        public int? SkinCount { get; set; }
        public string? Rank { get; set; }
        public decimal? Price { get; set; }
        public List<IFormFile>? Files { get; set; }
    }
}
