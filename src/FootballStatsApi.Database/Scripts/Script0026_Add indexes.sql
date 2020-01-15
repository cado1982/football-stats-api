CREATE INDEX ix_fixture__competition_season_detailssaved ON stats.fixture (competition_id, season_id, details_saved);
CREATE INDEX ix_fixture_player__fixture_id ON stats.fixture_player (fixture_id);
CREATE INDEX ix_fixture_shot__fixture_id ON stats.fixture_shot (fixture_id);
CREATE INDEX ix_player_summary__competition_season ON stats.player_summary (season_id, competition_id);
CREATE INDEX ix_team_summary__competition_season ON stats.team_summary (season_id, competition_id);
CREATE INDEX ix_team_summary__teamid_seasonid ON stats.team_summary (team_id, season_id);
CREATE INDEX ix_request_log__userid_timestamp ON public.request_log (user_id, timestamp);