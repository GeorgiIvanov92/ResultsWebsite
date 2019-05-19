using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tracker.Models
{
    [System.Serializable]
    public class Prelive
    {
        [Key]
        public int GameId { get; set; }
        public int? SportId { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string LeagueName { get; set; }
        public DateTime? GameDate { get; set; }
        public int BestOf { get; set; }
    }
}
