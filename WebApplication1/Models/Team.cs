using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
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
        public int GoldPerMinute { get; set; }
        public int GoldDifferencePerMinute { get; set; }
        public int GoldDifferenceAt15 { get; set; }
        public float CSPerMinute { get; set; }
        public float CSDifferenceAt15 { get; set; }
        public float TowerDifferenceAt15 { get; set; }
        public int FirstTowerPercent { get; set; }
        public float DragonsPerGame { get; set; }
        public float DragonsAt15 { get; set; }
        public float HeraldPerGame { get; set; }
        public float NashorsPerGame { get; set; }
        public int DamagePerMinute { get; set; }
        public int FirstBloodPercent { get; set; }
        public float KillsPerGame { get; set; }
        public float DeathsPerGame { get; set; }
        public float KDRatio { get; set; }
        public float WardsPerMinute { get; set; }
        public float VisionPerMinute { get; set; }
        public float WardsClearedPerMinute { get; set; }
        public float WardsClearedPercent { get; set; }
        public int CloudDrakesKilled { get; set; }
        public int CloudDrakesLost { get; set; }
        public int OceanDrakesKilled { get; set; }
        public int OceanDrakesLost { get; set; }
        public int InfernalDrakesKilled { get; set; }
        public int InfernalDrakesLost { get; set; }
        public int MountainDrakesKilled { get; set; }
        public int MountainDrakesLost { get; set; }
        public int ElderDrakesKilled { get; set; }
        public int ElderDrakesLost { get; set; }
        public int BlueSideWins { get; set; }
        public int BlueSideLosses { get; set; }
        public int RedSideWins { get; set; }
        public int RedSideLosses { get; set; }

    }
}
