ALTER TABLE "stats"."fixture"
    ALTER COLUMN "home_goals" SET DATA TYPE integer NOT NULL,
    ALTER COLUMN "away_goals" SET DATA TYPE integer NOT NULL,
    ALTER COLUMN "expected_home_goals" SET DATA TYPE real NOT NULL,
    ALTER COLUMN "expected_away_goals" SET DATA TYPE real NOT NULL,
    ALTER COLUMN "home_win_forecast" SET DATA TYPE real NOT NULL,
    ALTER COLUMN "home_draw_forecast" SET DATA TYPE real NOT NULL,
    ALTER COLUMN "home_loss_forecast" SET DATA TYPE real NOT NULL,
    ALTER COLUMN "home_deep" SET DATA TYPE integer NOT NULL,
    ALTER COLUMN "away_deep" SET DATA TYPE integer NOT NULL;