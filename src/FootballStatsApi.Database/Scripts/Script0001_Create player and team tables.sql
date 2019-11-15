CREATE SCHEMA IF NOT EXISTS "stats";

CREATE TABLE "stats"."player" (
    "id" integer PRIMARY KEY,
    "name" text NOT NULL
);

CREATE TABLE "stats"."team" (
    "id" integer PRIMARY KEY,
    "name" text NOT NULL,
    "short_name" text NOT NULL
);



