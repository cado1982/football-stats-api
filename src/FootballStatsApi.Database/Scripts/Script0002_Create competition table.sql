CREATE SCHEMA IF NOT EXISTS "stats";

CREATE TABLE "stats"."competition" (
    "id" integer PRIMARY KEY,
    "name" text NOT NULL,
    "internal_name" text NOT NULL
);

INSERT INTO "stats"."competition" ("id", "name", "internal_name") VALUES
    (1, 'English Premier League', 'EPL'),
    (2, 'Serie A', 'Serie_A'),
    (3, 'Bundesliga', 'Bundesliga'),
    (4, 'La Liga', 'La_Liga'),
    (5, 'Ligue 1', 'Ligue_1'),
    (6, 'Russian Premier League', 'RFPL');
    