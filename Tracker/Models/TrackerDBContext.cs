using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Configuration;

namespace Tracker.Models
{
    public partial class TrackerDBContext : DbContext
    {

        public TrackerDBContext()
        {
        }

        public TrackerDBContext(DbContextOptions<TrackerDBContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        public virtual DbSet<Results> Results { get; set; }
        public virtual DbSet<Prelive> Prelive { get; set; }
        public virtual DbSet<GeneralPlayerStats> GeneralPlayerStats { get; set; }
        public virtual DbSet<Player> Player { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["SQL"].ConnectionString);
        }
      
    }
}
