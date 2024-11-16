using Microsoft.AspNetCore.Http;

namespace DragonAcc.Service.Models.NgocRong_GameModel
{
    public class AddNgocRong_GameModel
    {
        public string? AccountName { get; set; }
        public string? Password { get; set; }
        public int? Server { get; set; }
        public string? Planet { get; set; }
        public decimal? Price { get; set; }
        public List<IFormFile>? Files { get; set; }
    }
}
