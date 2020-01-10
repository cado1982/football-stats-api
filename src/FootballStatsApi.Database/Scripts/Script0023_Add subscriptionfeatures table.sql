CREATE TABLE "public"."subscription_features" (
	"id" serial PRIMARY KEY,
	"display_name" text NOT NULL,
	"subscription_id" integer NOT NULL REFERENCES "public"."subscriptions"("id") ON DELETE CASCADE
);