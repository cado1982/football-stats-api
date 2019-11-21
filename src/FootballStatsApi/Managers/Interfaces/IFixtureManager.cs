using FootballStatsApi.Domain.Entities.Identity;
using FootballStatsApi.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace FootballStatsApi.Managers
{
    public interface IFixtureManager
    {
        Task<FixtureDetailed> GetDetailsAsync(int fixtureId);
    }
}
