CREATE SCHEMA IF NOT EXISTS "stats";

CREATE TABLE "stats"."fixture_player" (
    "player_id"                 integer NOT NULL REFERENCES "stats"."player"(id) ON DELETE CASCADE,
    "fixture_id"                integer NOT NULL REFERENCES "stats"."fixture"(fixture_id) ON DELETE CASCADE,
    "time"                      integer NOT NULL,
    "home_or_away"              text NOT NULL,
    "position"                  text NOT NULL,
    "yellow_cards"              integer NOT NULL,
    "red_cards"                 integer NOT NULL,
    "replaced_by_id"            integer NULL REFERENCES "stats"."player"(id) ON DELETE CASCADE, -- Player was subbed off and replaced by this player id
    "replaced_id"               integer NULL REFERENCES "stats"."player"(id) ON DELETE CASCADE, -- Player was brought on and replaced this player id
    "key_passes"                integer NOT NULL,
    "assists"                   integer NOT NULL,
    "expected_goals_chain"      real NOT NULL,
    "expected_goals_buildup"    real NOT NULL,
    "position_order"            integer NOT NULL,
    "goals"                     integer NOT NULL,
    "own_goals"                 integer NOT NULL,
    "shots"                     integer NOT NULL,
    "expected_goals"            real NOT NULL,
    "expected_assists"          real NOT NULL,
    CONSTRAINT "pk__fixture_player" PRIMARY KEY ("player_id", "fixture_id")
);