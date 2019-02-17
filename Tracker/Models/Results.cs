using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tracker.Models
{
    public class Results
    {
        [Key]
        public int GameId { get; set; }
        public int? SportId { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string LeagueName { get; set; }
        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }
        public int? GamePart { get; set; }
        public DateTime? GameDate { get; set; }
    }
}
