using Microsoft.EntityFrameworkCore;
using P03_FootballBetting.Data.Configuration;
using P03_FootballBetting.Data.Models;
using System;

namespace P03_FootballBetting.Data
{
    public class FootbalBettingContext : DbContext
    {
        public FootbalBettingContext()
        {
        }

        public FootbalBettingContext(DbContextOptions options) 
            : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Town> Towns { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<PlayerStatistic> PlayerStatistics { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Bet> Bets { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(ConfigurationString.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Team>(entity =>
            {
                entity.HasKey(e => e.TownId);

                entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode();

                entity.Property(e => e.LogoUrl)
                 .HasMaxLength(250)
                 .IsRequired(false)
                 .IsUnicode();

                entity.Property(e => e.Initials)
                .HasMaxLength(3)
                .IsRequired()
                .IsUnicode();

                entity.Property(e => e.Budget)
                .IsRequired();

                entity.HasOne(t => t.PrimaryKitColor)
                .WithMany(c => c.PrimaryKitTeams)
                .IsRequired(false)
                .HasForeignKey(t => t.PrimaryKitColorId)
                .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(t => t.SecondaryKitColor)
                .WithMany(c => c.SecondaryKitTeams)
                .IsRequired(false)
                .HasForeignKey(t => t.SecondaryKitColorId)
                .OnDelete(DeleteBehavior.NoAction);
               

                entity.HasOne(t => t.Town)
                .WithMany(t => t.Teams)
                .HasForeignKey(t => t.TownId);
            });
            builder.Entity<Color>(entity => 
            {
                entity.HasKey(c => c.ColorId);
                entity.Property(c => c.Name)
                .HasMaxLength(30)
                .IsRequired()
                .IsUnicode();
                
                 
            });
            builder.Entity<Town>(entity => 
            {
                entity.HasKey(t => t.TownId);

                entity.Property(t => t.Name)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode();

                entity.HasOne(t => t.Country)
                .WithMany(c => c.Towns)
                .HasForeignKey(t => t.CountryId);

            });
            builder.Entity<Country>(entity => 
            {
                entity.HasKey(c => c.CountryId);

                entity.Property(c => c.Name)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode();

            });
            builder.Entity<Player>(entity => 
            {
                entity.HasKey(p => p.PlayerId);

                entity.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired()
                .IsUnicode();

                entity.Property(p => p.SquadNumber)
                .HasMaxLength(3)
                .IsRequired()
                .IsUnicode(false);

                entity.Property(p => p.isInjured)
                .IsRequired();

                entity.HasOne(p => p.Team)
                .WithMany(p => p.Players)
                .HasForeignKey(p => p.TeamId);

                entity.HasOne(p => p.Position)
                .WithMany(po => po.Players)
                .HasForeignKey(p => p.PositionId);

            });
            builder.Entity<Position>(eniity =>
            {
            eniity.HasKey(p => p.PositionId);

            eniity.Property(p => p.Name)
            .HasMaxLength(20)
            .IsRequired()
            .IsUnicode();
            });
            builder.Entity<PlayerStatistic> (entity=> 
            {
                entity.HasKey(ps => new { ps.GameId, ps.PlayerId });

                entity.Property(ps => ps.Assists)
                .IsRequired();

                entity.Property(ps => ps.MinutesPlayed)
                .IsRequired();

                entity.HasOne(ps => ps.Game)
                .WithMany(g => g.PlayerStatistics)
                .HasForeignKey(ps => ps.GameId);

                entity.HasOne(ps => ps.Player)
                .WithMany(p => p.PlayerStatistics)
                .HasForeignKey(ps => ps.PlayerId);
            });
            builder.Entity<Game>(entity => 
            {
                entity.HasKey(g => g.GameId);

                entity.Property(g => g.HomeTeamGoals)
                .IsRequired();

                entity.Property(g => g.AwayTeamGoals)
                .IsRequired();

                entity.Property(g => g.DateTime)
                .IsRequired();

                entity.Property(g => g.HomeTeamBatRate)
                .IsRequired();
                
                entity.Property(d => d.DrawBetRate)
                .IsRequired();

                entity.Property(r => r.Result)
                .HasMaxLength(7)
                .IsRequired();

                entity.HasOne(g => g.HomeTeam)
                .WithMany(g => g.HomeGames)
                .HasForeignKey(g => g.HomeTeamId);

                entity.HasOne(g => g.AwayTeam)
                .WithMany(g => g.AwayGames)
                .HasForeignKey(g => g.AwayTeamId);
                
            });
            builder.Entity<Bet>(entity => 
            {
                entity.HasKey(b => b.BetId);

                entity.Property(b => b.Amount)
                .IsRequired();

                entity.Property(b => b.Prediction)
                .IsRequired();

                entity.Property(b => b.DateTime)
                .IsRequired();

                entity.HasOne(b => b.User)
                .WithMany(b => b.Bets)
                .HasForeignKey(b => b.UserId);

                entity.HasOne(g => g.Game)
                .WithMany(b => b.Bets)
                .HasForeignKey(g => g.GameId);


            });
            builder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserId);

                entity.Property(u => u.Username)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode(false);

                entity.Property(u => u.Password)
                .HasMaxLength(30)
                .IsRequired()
                .IsUnicode(false);

                entity.Property(u => u.Email)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode(false);

                entity.Property(u => u.Name)
                .HasMaxLength(100)
                .IsRequired(false)
                .IsUnicode(true);

                entity.Property(u => u.Balance)
                .IsRequired();

            });
        }
    }
}
