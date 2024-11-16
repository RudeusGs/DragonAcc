using Microsoft.AspNetCore.Http;

namespace DragonAcc.Service.Models.Pubg_GameModel
{
    public class AddPubg_GameModel
    {
        public string? AccountName { get; set; }
        public string? Password { get; set; }
        public int? GunSkinCount { get; set; }
        public int? HumanSkinCount { get; set; }
        public string? Rank { get; set; }
        public decimal? Price { get; set; }
        public List<IFormFile>? Files { get; set; }
    }
}
