using System;
using System.Collections.Generic;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    public class Game
    {
        public int GameId { get; set; }
        public int HomeTeamId { get; set; }
        public int AwayTeamId { get; set; }

        public int HomeTeamGoals { get; set; }
        public Team HomeTeam { get; set; }

        public int AwayTeamGoals { get; set; }
        public Team AwayTeam { get; set; }

        public DateTime DateTime { get; set; }
        public int HomeTeamBatRate { get; set; }
        public int AwayTeamBetRate { get; set; }
        public int DrawBetRate { get; set; }
        public int Result { get; set; }

        public ICollection<PlayerStatistic> PlayerStatistics { get; set; } = new HashSet<PlayerStatistic>();
        public ICollection<Bet> Bets { get; set; } = new HashSet<Bet>();
    }
}
