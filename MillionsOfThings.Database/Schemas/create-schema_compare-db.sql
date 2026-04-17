-- Important that this is run from the `postgres` database

-- Terminate any existing connections to the `schema_compare` database
SELECT pg_terminate_backend(pid)
FROM pg_stat_activity
WHERE datname = 'schema_compare'
  AND pid <> pg_backend_pid();

-- Drop and recreate the `schema_compare` database
DROP DATABASE IF EXISTS schema_compare;

CREATE DATABASE schema_compare;
