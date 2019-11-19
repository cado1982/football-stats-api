CREATE SCHEMA IF NOT EXISTS "stats";

CREATE TABLE "stats"."fixture" (
    "fixture_id"                            integer PRIMARY KEY,
    "home_team_id"                          integer NOT NULL REFERENCES "stats"."team"(id) ON DELETE CASCADE,
    "away_team_id"                          integer NOT NULL REFERENCES "stats"."team"(id) ON DELETE CASCADE,
    "season_id"                             integer NOT NULL,
    "competition_id"                        integer NOT NULL REFERENCES "stats"."competition"(id) ON DELETE CASCADE,
    "is_result"                             boolean NOT NULL,
    "home_goals"                            integer NULL,
    "away_goals"                            integer NULL,
    "expected_home_goals"                   real NULL,
    "expected_away_goals"                   real NULL,
    "home_win_forecast"                     real NULL,
    "home_draw_forecast"                    real NULL,
    "home_loss_forecast"                    real NULL,
    "datetime"                              timestamp NOT NULL
);