using Microsoft.AspNetCore.Http;

namespace DragonAcc.Service.Models.Valorant_GameModel
{
    public class UpdateValorant_GameModel
    {
        public int? Id { get; set; }
        public string? AccountName { get; set; }
        public string? Password { get; set; }
        public int? GunSkinCount { get; set; }
        public int? ChampionCount { get; set; }
        public string? Rank { get; set; }
        public decimal? Price { get; set; }
        public List<IFormFile>? Files { get; set; }
    }
}
