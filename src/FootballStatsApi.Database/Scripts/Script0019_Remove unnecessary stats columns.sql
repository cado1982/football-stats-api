ALTER TABLE "stats"."fixture"
    DROP COLUMN "home_goals",
	DROP COLUMN "away_goals",
	DROP COLUMN "expected_home_goals",
	DROP COLUMN "expected_away_goals",
	DROP COLUMN "home_shots",
	DROP COLUMN "away_shots",
	DROP COLUMN "home_shots_on_target",
	DROP COLUMN "away_shots_on_target";

ALTER TABLE "stats"."fixture_player"
    DROP COLUMN "key_passes",
	DROP COLUMN "assists",
	DROP COLUMN "goals",
	DROP COLUMN "own_goals",
	DROP COLUMN "shots",
	DROP COLUMN "expected_goals",
	DROP COLUMN "expected_assists";