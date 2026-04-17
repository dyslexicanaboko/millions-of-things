#!/bin/bash

# Drops and recreates the `schema_compare` database
docker exec -i postgres psql -U postgres -d postgres < create-schema_compare-db.sql

# Run the `build-schema-file.sh` script to create the schema.sql file
docker exec -i postgres psql -U postgres -d schema_compare < schema.sql
