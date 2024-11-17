using DragonAcc.Infrastructure.Base;
using System;
using System.Collections.Generic;

namespace DragonAcc.Infrastructure.Entities
{
    public class SlotGame : EntityBase
    {
        public decimal SpinCost { get; set; }
        public decimal Reward { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public TimeSpan SpinDuration { get; set; }
        public string? Status { get; set; }
        public int? UserId { get; set; }
        public List<string> Icons { get; set; } = new List<string>();
        public string? SpecialIcon { get; set; }
        public int SpinCount { get; set; }
        public decimal Jackpot {  get; set; }
        public bool IsJackpotTriggered { get; set; }
    }
}
