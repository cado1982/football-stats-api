CREATE SCHEMA IF NOT EXISTS identity;

CREATE TABLE identity.users (
    "id" integer PRIMARY KEY NOT NULL,
    "user_name" character varying(256),
    "normalized_user_name" character varying(256),
    "email" character varying(256),
    "normalized_email" character varying(256),
    "email_confirmed" boolean NOT NULL,
    "password_hash" text,
    "security_stamp" text,
    "concurrency_stamp" text,
    "phone_number" text,
    "phone_number_confirmed" boolean NOT NULL,
    "two_factor_enabled" boolean NOT NULL,
    "lockout_end" timestamp with time zone,
    "lockout_enabled" boolean NOT NULL,
    "access_failed_count" integer NOT NULL,
    "api_key" uuid NULL
);

CREATE INDEX ix_users__normalized_email ON identity.users (normalized_email);
CREATE UNIQUE INDEX ix_users__normalized_user_name ON identity.users (normalized_user_name);

CREATE TABLE identity.user_tokens (
    "user_id" integer NOT NULL,
    "login_provider" character varying(450) NOT NULL,
    "name" character varying(450) NOT NULL,
    "value" text,
    CONSTRAINT "pk__user_tokens" PRIMARY KEY ("user_id", "login_provider", "name"),
    CONSTRAINT "fk__user_tokens__users__user_id" FOREIGN KEY ("user_id")
        REFERENCES identity."users" ("id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);

CREATE TABLE identity.user_claims (
    "id" integer PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    "user_id" integer NOT NULL,
    "claim_type" text,
    "claim_value" text,
    CONSTRAINT "fk__user_claims__users__user_id" FOREIGN KEY ("user_id")
        REFERENCES identity."users" ("id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);

CREATE INDEX "ix__user_claims__user_id" ON identity."user_claims" ("user_id");

CREATE TABLE identity.roles (
    "id" integer PRIMARY KEY NOT NULL,
    "name" character varying(256),
    "normalized_name" character varying(256),
    "concurrency_stamp" text
);
CREATE UNIQUE INDEX ix_roles__name ON identity.roles (name);

CREATE TABLE identity.user_roles (
    "user_id" integer  NOT NULL references identity.users(id) ON DELETE CASCADE,
    "role_id" integer NOT NULL references identity.roles(id) ON DELETE CASCADE,
    CONSTRAINT "pk__user_roles" PRIMARY KEY ("user_id", "role_id")
);
CREATE INDEX ix_user_roles__role_id ON identity.user_roles (role_id);

CREATE TABLE identity.user_logins (
    "login_provider" character varying(450) NOT NULL,
    "provider_key" character varying(450) NOT NULL,
    "provider_display_name" text,
    "user_id" integer NOT NULL references identity.users(id) ON DELETE CASCADE,
    CONSTRAINT "pk__user_logins" PRIMARY KEY ("login_provider", "provider_key")
);
CREATE INDEX ix_user_logins__user_id ON identity.user_logins (user_id);

CREATE TABLE identity.role_claims (
    "id" integer PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    "role_id" integer NOT NULL references identity.roles(id) ON DELETE CASCADE,
    "claim_type" text,
    "claim_value" text
);
CREATE INDEX ix_role_claims__role_id ON identity.role_claims (role_id);