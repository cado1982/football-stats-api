ALTER TABLE "stats"."fixture_shot" 
	ADD COLUMN "team_id" integer NOT NULL REFERENCES "stats"."team"(id) ON DELETE CASCADE,
	DROP COLUMN "home_or_away";

ALTER TABLE "stats"."fixture_player" 
	ADD COLUMN team_id integer NOT NULL REFERENCES "stats"."team"(id) ON DELETE CASCADE,
	DROP COLUMN "home_or_away";