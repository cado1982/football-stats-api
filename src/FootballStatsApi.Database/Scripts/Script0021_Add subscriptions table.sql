CREATE TABLE "public"."subscriptions" (
    "id" integer PRIMARY KEY,
    "internal_name" text NOT NULL,
    "display_name" text NOT NULL,
    "hourly_call_limit" integer NOT NULL,
    "cost" integer NOT NULL,
    "is_active" boolean NOT NULL DEFAULT false
);

ALTER TABLE "identity"."users" 
    ADD COLUMN "subscription_id" integer NOT NULL REFERENCES "public"."subscriptions"("id") ON DELETE RESTRICT;