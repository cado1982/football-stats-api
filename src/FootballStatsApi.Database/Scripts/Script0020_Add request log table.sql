CREATE TABLE stats.request_log (
    "id" integer PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
	"user_id" integer NOT NULL,
	"ip_address" inet NOT NULL,
	"response_ms" integer NOT NULL,
	"endpoint" text NOT NULL,
    "timestamp" timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP
);