﻿ALTER TABLE "stats"."fixture_shot" ADD COLUMN assisted_by integer NULL REFERENCES "stats"."player"(id);