using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //optionsBuilder.UseSqlServer("Server=(LocalDb)\\MSSQLLocalDB;Database=TrackerDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Results>(entity =>
            {
                entity.HasKey(e => e.GameId);

                entity.ToTable("results");

                entity.Property(e => e.GameId).HasColumnName("GameID");

                entity.Property(e => e.AwayTeam)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.GameDate).HasColumnType("date");

                entity.Property(e => e.HomeTeam)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LeagueName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.SportId).HasColumnName("SportID");
            });
        }
    }
}
