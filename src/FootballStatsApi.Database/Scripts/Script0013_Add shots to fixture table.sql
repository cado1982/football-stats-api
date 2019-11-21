ALTER TABLE "stats"."fixture" 
	ADD COLUMN "home_shots" integer NOT NULL DEFAULT(0),
	ADD COLUMN "away_shots" integer NOT NULL DEFAULT(0),
	ADD COLUMN "home_shots_on_target" integer NOT NULL DEFAULT(0),
	ADD COLUMN "away_shots_on_target" integer NOT NULL DEFAULT(0);