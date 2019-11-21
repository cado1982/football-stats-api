using System;
using System.Collections.Generic;

namespace FootballStatsApi.Domain.Entities
{
    public class FixtureDetails
    {
        public int FixtureId { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public DateTimeOffset DateTime { get; set; }
        public Competition Competition { get; set; }
        public int Season { get; set; }
        public int HomeDeepPasses { get; set; }
        public int AwayDeepPasses { get; set; }
        public double HomePpda { get; set; }
        public double AwayPpda { get; set; }
        public double ForecastHomeWin { get; set; }
        public double ForecastDraw { get; set; }
        public double ForecastAwayWin { get; set; }
    }

    public class FixturePlayer
    {
        public int PlayerId { get; set; }
        public int Minutes { get; set; }
        public string Position { get; set; }
        public int YellowCards { get; set; }
        public int RedCards { get; set; }
        public Player ReplacedBy { get; set; }
        public Player Replaced { get; set; }
        public double ExpectedGoalsChain { get; set; }
        public double ExpectedGoalsBuildup { get; set; }
        public int PositionOrder { get; set; }
    }

    public class FixtureShot
    {
        public int ShotId { get; set; }
        public Player Player { get; set; }
        public Player Assist { get; set; }
        public int Minute { get; set; }
        public string Result { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public double ExpectedGoal { get; set; }
        public string HomeOrAway { get; set; }
        public string Situation { get; set; }
        public string Type { get; set; }
        public string LastAction { get; set; }
    }
}


