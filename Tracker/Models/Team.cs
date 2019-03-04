using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tracker.Models
{
    class Team
    {
        [Key]
        public int TeamId { get; set; }
        public int? SportId { get; set; }
        public string Region { get; set; }

        public DateTime? GameDate { get; set; }
        public int BestOf { get; set; }
    }
}
 