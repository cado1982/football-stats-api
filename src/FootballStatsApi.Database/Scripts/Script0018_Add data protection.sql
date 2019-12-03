CREATE SCHEMA IF NOT EXISTS data_protection;

CREATE TABLE data_protection.keys (
    "id" integer PRIMARY KEY GENERATED ALWAYS AS IDENTITY,
    "friendly_name" text,
    "xml" text
);