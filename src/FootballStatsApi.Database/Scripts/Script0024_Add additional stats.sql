ALTER TABLE "stats"."fixture"
	ADD COLUMN home_goals integer NOT NULL DEFAULT(0),
	ADD COLUMN away_goals integer NOT NULL DEFAULT(0),
	ADD COLUMN home_shots integer NOT NULL DEFAULT(0),
    ADD COLUMN away_shots integer NOT NULL DEFAULT(0),
    ADD COLUMN home_shots_on_target integer NOT NULL DEFAULT(0),
    ADD COLUMN away_shots_on_target integer NOT NULL DEFAULT(0),
    ADD COLUMN home_expected_goals real NOT NULL DEFAULT(0),
    ADD COLUMN away_expected_goals real NOT NULL DEFAULT(0),
    ADD COLUMN home_expected_points real NOT NULL DEFAULT(0),
    ADD COLUMN away_expected_points real NOT NULL DEFAULT(0);

ALTER TABLE "stats"."fixture_player"
    ADD COLUMN shots integer NOT NULL DEFAULT(0),
    ADD COLUMN shots_on_target integer NOT NULL DEFAULT(0),
    ADD COLUMN key_passes integer NOT NULL DEFAULT(0),
    ADD COLUMN assists integer NOT NULL DEFAULT(0),
    ADD COLUMN goals integer NOT NULL DEFAULT(0),
    ADD COLUMN own_goals integer NOT NULL DEFAULT(0),
    ADD COLUMN expected_goals real NOT NULL DEFAULT(0),
    ADD COLUMN expected_assists real NOT NULL DEFAULT(0);