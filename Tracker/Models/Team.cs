using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Tracker.Models
{
    public class Team
    {
        [Key]
        public int? Id { get; set; }
        public string Name { get; set; }
        public int? SportId { get; set; }
        public string Region { get; set; }
        public float Winrate { get; set; }
        public float AverageGameTime { get; set; }

    }
}
 