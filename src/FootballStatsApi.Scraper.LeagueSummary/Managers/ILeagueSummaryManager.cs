using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using FootballStatsApi.Domain.Entities;

namespace FootballStatsApi.Scraper.LeagueSummary
{
    public interface ILeagueSummaryManager
    {
        Task ProcessFixtures(List<Models.Fixture> fixtures, int season, Competition competition, IDbConnection connection);
        Task ProcessTeams(List<Models.Team> teams, int season, Competition competition, IDbConnection connection);
        Task ProcessPlayers(List<Models.Player> players, int season, Competition competition, IDbConnection conn);
    }
}