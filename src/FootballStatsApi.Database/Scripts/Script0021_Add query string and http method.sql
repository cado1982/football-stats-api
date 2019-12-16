ALTER TABLE "stats"."request_log"
    ADD COLUMN "http_method" text NOT NULL DEFAULT '',
    ADD COLUMN "query_string" text NULL,
    ADD CONSTRAINT "fk_request_log__user" FOREIGN KEY (user_id) REFERENCES "identity"."users"(id);

CREATE INDEX ix_request_log__user_timestamp ON "stats"."request_log"("user_id", "timestamp");