ALTER TABLE "stats"."fixture" 
    ADD COLUMN "home_passes" int NOT NULL DEFAULT(0),
    ADD COLUMN "home_defensive_actions" int NOT NULL DEFAULT(0),
    ADD COLUMN "away_passes" int NOT NULL DEFAULT(0),
    ADD COLUMN "away_defensive_actions" int NOT NULL DEFAULT(0),
    DROP COLUMN "home_ppda",
    DROP COLUMN "away_ppda";