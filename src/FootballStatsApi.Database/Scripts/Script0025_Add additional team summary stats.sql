ALTER TABLE "stats"."team_summary"
	ADD COLUMN non_penalty_expected_goals real NOT NULL DEFAULT(0),
	ADD COLUMN non_penalty_expected_goals_against real NOT NULL DEFAULT(0),
	ADD COLUMN opposition_ppda real NOT NULL DEFAULT(0),
    ADD COLUMN opposition_deep_passes integer NOT NULL DEFAULT(0);