ALTER TABLE "stats"."player_summary" DROP COLUMN "team";
ALTER TABLE "stats"."player_summary" ADD COLUMN "team_id" integer NOT NULL REFERENCES "stats"."team"(id) ON DELETE CASCADE