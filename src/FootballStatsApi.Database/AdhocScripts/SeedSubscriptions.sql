INSERT INTO "public"."subscriptions" ("id", "internal_name", "display_name", "hourly_call_limit", "cost", "is_active", "is_internal") VALUES
    (1, 'hobby',		'Hobby',		60,			0,		true,		false),
    (2, 'amateur',		'Amateur',		3600,		-1,		false,		false),
    (3, 'pro',			'Pro',			100000,		-1,		false,		false),
	(4, 'enterprise',	'Enterprise',	-1,			-2,		false,		false);

INSERT INTO "public"."subscription_features" ("subscription_id", "display_name") VALUES
	(1, 'All 6 leagues*'),
	(1, 'Basic team stats'),
	(1, 'Basic player stats'),
	(1, 'Basic fixture stats'),
	(1, 'Historic stats*'),
	(2, 'All Hobby features'),
	(2, 'Detailed team stats'),
	(2, 'Detailed player stats'),
	(2, 'Detailed fixture stats'),
	(3, 'All Amateur features'),
	(3, 'Expert team stats'),
	(3, 'Expert player stats'),
	(3, 'Expert fixture stats'),
	(4, 'All Pro features'),
	(4, 'Custom stats'),
	(4, 'Custom endpoints'),
	(4, 'Priority support channel'),
	(4, 'Service level agreements');