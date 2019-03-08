using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Player
    {
        [Key]
        public int? PlayerId { get; set; }
        public string Nickname { get; set; }
        public int SportId { get; set; }
        [ForeignKey("TeamId")]
        public Team Team { get; set; }
        public int? TeamId { get; set; }
    }
}
