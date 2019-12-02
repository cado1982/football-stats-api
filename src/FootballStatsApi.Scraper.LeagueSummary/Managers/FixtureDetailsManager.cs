using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using FootballStatsApi.Domain.Repositories;
using FootballStatsApi.Scraper.LeagueSummary.Models;
using Microsoft.Extensions.Logging;
using Entities = FootballStatsApi.Domain.Entities;

namespace FootballStatsApi.Scraper.LeagueSummary
{
    public class FixtureDetailsManager : IFixtureDetailsManager
    {
        private readonly ILogger<FixtureDetailsManager> _logger;
        private readonly IFixtureRepository _fixtureRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IPlayerRepository _playerRepository;

        public FixtureDetailsManager(
            ILogger<FixtureDetailsManager> logger,
            IFixtureRepository fixtureRepository,
            ITeamRepository teamRepository,
            IPlayerRepository playerRepository)
        {
            _fixtureRepository = fixtureRepository;
            _teamRepository = teamRepository;
            _playerRepository = playerRepository;
            _logger = logger;
        }

        public async Task ProcessMatchInfo(FixtureMatchInfo matchInfo, IDbConnection conn)
        {
            var teams = new List<Entities.Team>
            {
                new Entities.Team
                {
                    Id = matchInfo.HomeTeamId,
                    Name = matchInfo.HomeTeamName,
                    ShortName = string.Empty
                },
                new Entities.Team
                {
                    Id = matchInfo.AwayTeamId,
                    Name = matchInfo.AwayTeamName,
                    ShortName = string.Empty
                }
            };
            await _teamRepository.InsertMultipleAsync(teams, conn);

            var details = new Entities.FixtureDetails
            {
                FixtureId = matchInfo.Id,
                HomeDeepPasses = matchInfo.HomeDeepPasses,
                AwayDeepPasses = matchInfo.AwayDeepPasses,
                HomeExpectedGoals = matchInfo.HomeExpectedGoals,
                AwayExpectedGoals = matchInfo.AwayExpectedGoals,
                HomeGoals = matchInfo.HomeGoals,
                AwayGoals = matchInfo.AwayGoals,
                HomeShots = matchInfo.HomeShots,
                AwayShots = matchInfo.AwayShots,
                ForecastHomeWin = matchInfo.HomeWinForecast,
                ForecastDraw = matchInfo.DrawForecast,
                ForecastAwayWin = matchInfo.AwayWinForecast,
                HomeShotsOnTarget = matchInfo.HomeShotsOnTarget,
                AwayShotsOnTarget = matchInfo.AwayShotsOnTarget,
                HomePpda = matchInfo.HomePpda,
                AwayPpda = matchInfo.AwayPpda
            };

            await _fixtureRepository.Update(details, conn);
        }

        public async Task ProcessRosters(FixtureRosters rosters, IDbConnection conn)
        {
            var homePlayers = rosters.Home.Values.ToList();
            var awayPlayers = rosters.Away.Values.ToList();
            var players = homePlayers.Union(awayPlayers).ToList();
            
            var playerEntities = players.Select(p => new Entities.Player 
            {
                Id = p.PlayerId,
                Name = p.PlayerName
            }).ToList();

            await _playerRepository.InsertPlayersAsync(playerEntities, conn);

            var fixturePlayers = players.Select(p => new Entities.FixturePlayer
            {
                ExpectedGoalsBuildup = p.ExpectedGoalsBuildup,
                ExpectedGoalsChain = p.ExpectedGoalsChain,
                Minutes = p.Time,
                Player = new Entities.Player { Id = p.PlayerId },
                Finish making this object to pass to the fixture repository
            }).ToList();

            

            /*for (const rosterId in data.h) {
                if (data.h.hasOwnProperty(rosterId)) {
                    const player = data.h[rosterId];

                    const rosterInEntry = player.roster_in === 0 ? null : data.h[player.roster_in];
                    const rosterOutEntry = player.roster_out === 0 ? null : data.h[player.roster_out];

                    fixturePlayerValues = fixturePlayerValues.concat(`(${player.player_id},${matchInfo.id},${matchInfo.h},${player.time},
                        '${player.position}',${player.yellow_card},${player.red_card},${!rosterInEntry ? 'NULL' : rosterInEntry.player_id},
                        ${!rosterOutEntry ? 'NULL' : rosterOutEntry.player_id},${player.key_passes},${player.assists},
                        ${player.xGChain},${player.xGBuildup},${player.positionOrder},${player.goals},${player.own_goals},
                        ${player.shots},${player.xG},${player.xA}),`);

                    playerValues = playerValues.concat(`(${player.player_id},'${player.player}'),`);
                }
            }

            for (const rosterId in data.a) {
                if (data.a.hasOwnProperty(rosterId)) {
                    const player = data.a[rosterId];

                    const rosterInEntry = player.roster_in === 0 ? null : data.a[player.roster_in];
                    const rosterOutEntry = player.roster_out === 0 ? null : data.a[player.roster_out];

                    fixturePlayerValues = fixturePlayerValues.concat(`(${player.player_id},${matchInfo.id},${matchInfo.a},${player.time},
                        '${player.position}',${player.yellow_card},${player.red_card},${!rosterInEntry ? 'NULL' : rosterInEntry.player_id},
                        ${!rosterOutEntry ? 'NULL' : rosterOutEntry.player_id},${player.key_passes},${player.assists},
                        ${player.xGChain},${player.xGBuildup},${player.positionOrder},${player.goals},${player.own_goals},
                        ${player.shots},${player.xG},${player.xA}),`);

                    playerValues = playerValues.concat(`(${player.player_id},'${player.player}'),`);
                }
            }

            // Remove the final commas
            playerValues = playerValues.slice(0, playerValues.length - 1);
            fixturePlayerValues = fixturePlayerValues.slice(0, fixturePlayerValues.length - 1);

            const insertPlayersQuery = `INSERT INTO "stats"."player" (id,name) VALUES ${playerValues} ON CONFLICT DO NOTHING;`;
            const insertFixturePlayersQuery = `INSERT INTO "stats"."fixture_player" 
            (
                player_id,
                fixture_id,
                team_id,
                time,
                position,
                yellow_cards,
                red_cards,
                replaced_by_id,
                replaced_id,
                key_passes,
                assists,
                expected_goals_chain,
                expected_goals_buildup,
                position_order,
                goals,
                own_goals,
                shots,
                expected_goals,
                expected_assists
            ) VALUES ${fixturePlayerValues}
            ON CONFLICT(player_id, fixture_id) DO UPDATE SET 
                time = EXCLUDED.time,
                team_id = EXCLUDED.team_id,
                position = EXCLUDED.position,
                yellow_cards = EXCLUDED.yellow_cards,
                red_cards = EXCLUDED.red_cards,
                replaced_by_id = EXCLUDED.replaced_by_id,
                replaced_id = EXCLUDED.replaced_id,
                key_passes = EXCLUDED.key_passes,
                assists = EXCLUDED.assists,
                expected_goals_chain = EXCLUDED.expected_goals_chain,
                expected_goals_buildup = EXCLUDED.expected_goals_buildup,
                position_order = EXCLUDED.position_order,
                goals = EXCLUDED.goals,
                own_goals = EXCLUDED.own_goals,
                shots = EXCLUDED.shots,
                expected_goals = EXCLUDED.expected_goals,
                expected_assists = EXCLUDED.expected_assists`;

            console.log(insertFixturePlayersQuery);

            await pool.query(insertPlayersQuery);
            await pool.query(insertFixturePlayersQuery);
            console.log(`Saved players for fixture ${matchInfo.id}`);
            */
        }

        public async Task ProcessShots(FixtureShots shots, IDbConnection conn)
        {
            /*
            console.log(`Attempting to save shots for fixture ${matchInfo.id}. ${data.h.length} home shots and ${data.a.length} away shots.`);

                let shotValues: string = '';

                for (const shot of data.h) {
                    let assistedById = 0;
                    console.log(shot.player_assisted);
                    if (shot.player_assisted) {
                        for (const id in rosters.h) {
                            if (rosters.h.hasOwnProperty(id) && rosters.h[id].player === shot.player_assisted) {
                                assistedById = rosters.h[id].player_id
                            }
                        }
                    }

                    shotValues = shotValues.concat(`(${shot.id},${shot.player_id},${matchInfo.id},${matchInfo.h},${shot.minute},'${shot.result}',
                        ${shot.X},${shot.Y},${shot.xG},'${shot.situation}','${shot.shotType}',
                        '${shot.lastAction}',${!assistedById ? 'NULL' : assistedById}),`);
                }

                for (const shot of data.a) {
                    let assistedById = 0;

                    if (shot.player_assisted) {
                        for (const id in rosters.a) {
                            if (rosters.a.hasOwnProperty(id) && rosters.a[id].player === shot.player_assisted) {
                                assistedById = rosters.a[id].player_id
                            }
                        }
                    }

                    shotValues = shotValues.concat(`(${shot.id},${shot.player_id},${matchInfo.id},${matchInfo.a},${shot.minute},'${shot.result}',
                        ${shot.X},${shot.Y},${shot.xG},'${shot.situation}','${shot.shotType}',
                        '${shot.lastAction}',${!assistedById ? 'NULL' : assistedById}),`);
                }

                // Remove the final commas
                shotValues = shotValues.slice(0, shotValues.length - 1);

                const insertShotsQuery = `INSERT INTO "stats"."fixture_shot" 
                (
                    shot_id,
                    player_id,
                    fixture_id,
                    team_id,
                    minute,
                    result,
                    x,
                    y,
                    expected_goal,
                    situation,
                    shot_type,
                    last_action,
                    assisted_by
                ) VALUES ${shotValues}
                ON CONFLICT(shot_id) DO NOTHING;`;

                await pool.query(insertShotsQuery);
            */
        }

        public async Task ConfirmDetailsSaved(int fixtureId, IDbConnection conn)
        {
            await _fixtureRepository.UpdateDetailsSavedAsync(fixtureId, conn);
        }
    }

}