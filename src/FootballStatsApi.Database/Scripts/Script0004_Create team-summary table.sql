CREATE SCHEMA IF NOT EXISTS "stats";

CREATE TABLE "stats"."team_summary" (
    "team_id"                               integer NOT NULL REFERENCES "stats"."team"(id) ON DELETE CASCADE,
    "season_id"                             integer NOT NULL,
    "competition_id"                        integer NOT NULL REFERENCES "stats"."competition"(id) ON DELETE CASCADE,
    "games"                                 integer NOT NULL,
    "won"                                   integer NOT NULL,
    "drawn"                                 integer NOT NULL,
    "lost"                                  integer NOT NULL,
    "goals_for"                             integer NOT NULL,
    "goals_against"                         integer NOT NULL,
    "points"                                integer NOT NULL,
    "expected_goals"                        real NOT NULL,
    "expected_goals_against"                real NOT NULL,
    "expected_points"                       real NOT NULL,
    "ppda"                                  real NOT NULL,
    "deep_passes"                           integer NOT NULL,
    CONSTRAINT "pk__team_summary" PRIMARY KEY ("season_id", "team_id", "competition_id")
);