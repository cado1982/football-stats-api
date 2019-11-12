CREATE SCHEMA IF NOT EXISTS "stats";

CREATE TABLE "stats"."player_summary" (
    "season_id"                             integer NOT NULL,
    "player_id"                             integer NOT NULL,
    "name"                                  text NOT NULL,
    "games"                                 integer NOT NULL,
    "time"                                  integer NOT NULL,
    "goals"                                 integer NOT NULL,
    "expected_goals"                        real NOT NULL,
    "assists"                               integer NOT NULL,
    "expected_assists"                      real NOT NULL,
    "shots"                                 integer NOT NULL,
    "key_passes"                            integer NOT NULL,
    "yellow_cards"                          integer NOT NULL,
    "red_cards"                             integer NOT NULL,
    "position"                              text NOT NULL,
    "team"                                  text NOT NULL,
    "non_penalty_goals"                     integer NOT NULL,
    "non_penalty_expected_goals"            real NOT NULL,
    "expected_goals_chain"                  real NOT NULL,
    "expected_goals_buildup"                real NOT NULL,
    CONSTRAINT "pk__player_summary" PRIMARY KEY ("season_id", "player_id")
);