CREATE SCHEMA IF NOT EXISTS "stats";

CREATE TABLE "stats"."fixture_shot" (
    "shot_id"                               integer PRIMARY KEY,
    "player_id"                             integer NOT NULL REFERENCES "stats"."player"(id) ON DELETE CASCADE,
    "fixture_id"                            integer NOT NULL REFERENCES "stats"."fixture"(fixture_id) ON DELETE CASCADE,
    "minute"                                integer NOT NULL,
    "result"                                text NOT NULL,
    "x"                                     real NOT NULL,
    "y"                                     real NOT NULL,
    "expected_goal"                         real NOT NULL,
    "home_or_away"                          text NOT NULL,
    "situation"                             text NOT NULL,
    "shot_type"                             text NOT NULL,
    "last_action"                           text NOT NULL
);