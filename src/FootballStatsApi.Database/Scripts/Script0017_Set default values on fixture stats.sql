ALTER TABLE "stats"."fixture"
    ALTER COLUMN "home_goals" SET DEFAULT 0,
    ALTER COLUMN "away_goals" SET DEFAULT 0,
    ALTER COLUMN "expected_home_goals" SET DEFAULT 0,
    ALTER COLUMN "expected_away_goals" SET DEFAULT 0,
    ALTER COLUMN "home_win_forecast" SET DEFAULT 0,
    ALTER COLUMN "home_draw_forecast" SET DEFAULT 0,
    ALTER COLUMN "home_loss_forecast" SET DEFAULT 0,
    ALTER COLUMN "home_deep" SET DEFAULT 0,
    ALTER COLUMN "away_deep" SET DEFAULT 0,
    ALTER COLUMN "home_ppda" SET DEFAULT 0,
    ALTER COLUMN "away_ppda" SET DEFAULT 0,
    ALTER COLUMN "details_saved" SET DEFAULT NULL;