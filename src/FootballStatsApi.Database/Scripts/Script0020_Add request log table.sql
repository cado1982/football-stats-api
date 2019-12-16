CREATE TABLE "public"."request_log" (
    "id" integer PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
	"user_id" integer NULL REFERENCES "identity"."users"(id) ON DELETE SET NULL,
	"ip_address" inet NOT NULL,
	"response_ms" integer NOT NULL,
	"endpoint" text NOT NULL,
    "timestamp" timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
    "http_method" text NOT NULL,
    "query_string" text NOT NULL,
    "status_code" integer NOT NULL
);

CREATE INDEX ix_request_log__user_timestamp ON "public"."request_log"("user_id", "timestamp", "status_code");