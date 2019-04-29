using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Tracker.Models
{
    public class ChampionStat
    {
        [Key]
        public int ChampionStatId { get; set; }
        public string ChampionName { get; set; }
        [ForeignKey("PlayerId")]
        public Player Player { get; set; }
        public int? PlayerId { get; set; }
        public int GamesPlayed { get; set; }
        public double KDA { get; set; }
        public double WinratePercent { get; set; }
    }
}
