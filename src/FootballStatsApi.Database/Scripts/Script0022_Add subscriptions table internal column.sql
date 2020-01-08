ALTER TABLE "public"."subscriptions"
	ADD COLUMN is_internal boolean NOT NULL DEFAULT(false);