using System;
using System.Collections.Generic;
using System.Text;

namespace P03_FootballBetting.Data.Models
{
    public class PlayerStatistic
    {
        public int GameId { get; set; }
        public Game Game { get; set; }
        
        public int PlayerId { get; set; }
        public Player Player { get; set; }

        public int ScoreGoals { get; set; }
        public string Assists { get; set; }
        public DateTime MinutesPlayed { get; set; }


    }
}
