ALTER TABLE "stats"."fixture"
    ALTER COLUMN "home_goals" SET NOT NULL,
    ALTER COLUMN "away_goals" SET NOT NULL,
    ALTER COLUMN "expected_home_goals" SET NOT NULL,
    ALTER COLUMN "expected_away_goals" SET NOT NULL,
    ALTER COLUMN "home_win_forecast" SET NOT NULL,
    ALTER COLUMN "home_draw_forecast" SET NOT NULL,
    ALTER COLUMN "home_loss_forecast" SET NOT NULL,
    ALTER COLUMN "home_deep" SET NOT NULL,
    ALTER COLUMN "away_deep" SET NOT NULL,
    ALTER COLUMN "home_ppda" SET NOT NULL,
    ALTER COLUMN "away_ppda" SET NOT NULL;