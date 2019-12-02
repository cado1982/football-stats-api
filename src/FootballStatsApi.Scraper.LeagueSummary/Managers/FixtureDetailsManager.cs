using System.Collections.Generic;
using System.Data;
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

        public FixtureDetailsManager(
            ILogger<FixtureDetailsManager> logger,
            IFixtureRepository fixtureRepository,
            ITeamRepository teamRepository)
        {
            _fixtureRepository = fixtureRepository;
            _teamRepository = teamRepository;
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

            


            /*
            console.log(`Attempting to save match info for fixture ${data.id}. ${data.team_h} vs ${data.team_a} in season ${data.season} and competition ${data.league}`);

            const insertTeamsQuery = `INSERT INTO "stats"."team" (id, name, short_name) VALUES (${data.h},'${data.team_h}',''),(${data.a},'${data.team_a}','') ON CONFLICT(id) DO NOTHING;`;
            const insertMatchInfoQuery = `INSERT INTO "stats"."fixture" 
                (
                    fixture_id,
                    home_team_id,
                    away_team_id,
                    season_id,
                    competition_id,
                    is_result,
                    home_goals,
                    away_goals,
                    expected_home_goals,
                    expected_away_goals,
                    home_win_forecast,
                    home_draw_forecast,
                    home_loss_forecast,
                    home_deep,
                    away_deep,
                    home_ppda,
                    away_ppda,
                    home_shots,
                    away_shots,
                    home_shots_on_target,
                    away_shots_on_target,
                    datetime
                ) 
                VALUES 
                (
                    ${data.id},
                    ${data.h},
                    ${data.a},
                    ${data.season},
                    ${data.league_id},
                    true,
                    ${data.h_goals},
                    ${data.a_goals},
                    ${data.h_xg},
                    ${data.a_xg},
                    ${data.h_w},
                    ${data.h_d},
                    ${data.h_l},
                    ${data.h_deep},
                    ${data.a_deep},
                    ${data.h_ppda},
                    ${data.a_ppda},
                    ${data.h_shot},
                    ${data.a_shot},
                    ${data.h_shotOnTarget},
                    ${data.a_shotOnTarget},
                    '${data.date}'
                )
                ON CONFLICT(fixture_id) DO UPDATE SET 
                is_result = EXCLUDED.is_result,
                home_goals = EXCLUDED.home_goals,
                away_goals = EXCLUDED.away_goals,
                expected_home_goals = EXCLUDED.expected_home_goals,
                expected_away_goals = EXCLUDED.expected_away_goals,
                home_win_forecast = EXCLUDED.home_win_forecast,
                home_draw_forecast = EXCLUDED.home_draw_forecast,
                home_loss_forecast = EXCLUDED.home_loss_forecast,
                home_deep = EXCLUDED.home_deep,
                away_deep = EXCLUDED.away_deep,
                home_ppda = EXCLUDED.home_ppda,
                away_ppda = EXCLUDED.away_ppda,
                home_shots = EXCLUDED.home_shots,
                away_shots = EXCLUDED.away_shots,
                home_shots_on_target = EXCLUDED.home_shots_on_target,
                away_shots_on_target = EXCLUDED.away_shots_on_target,
                datetime = EXCLUDED.datetime;`;

            await pool.query(insertTeamsQuery);
            await pool.query(insertMatchInfoQuery);
            console.log('Saved match info');
            */
        }

        public async Task ProcessRosters(FixtureRosters rosters, IDbConnection conn)
        {
            /*
            console.log(`Attempting to save players for fixture ${matchInfo.id}`);

            let playerValues = '';
            let fixturePlayerValues = '';

            for (const rosterId in data.h) {
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