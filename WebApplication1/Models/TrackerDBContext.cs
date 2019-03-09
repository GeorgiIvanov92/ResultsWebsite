using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Tracker.Models;
using WebApi.Models;

namespace WebApplication1.Models
{
    public partial class TrackerDBContext : DbContext
    {
        public TrackerDBContext()
        {
        }

        public TrackerDBContext(DbContextOptions<TrackerDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Results> Results { get; set; }
        public virtual DbSet<Prelive> Prelive { get; set; }
        public virtual DbSet<Player> Player { get; set; }
        public virtual DbSet<Team> Team { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }
      
    }
}
